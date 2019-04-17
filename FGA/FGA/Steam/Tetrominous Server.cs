using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Steamworks;
using FGA.Online;

namespace FGA.Steam
{
    public struct ClientConnectionData
    {
        public bool bActive;                       // Wether the slot is in use or available
        public CSteamID SteamIDUser;               // Steam id of the client
        public UInt64 TickCountSinceLastData;      // Last time since we got data from this client
    }

    class Tetrominous_Server
    {


        // Constants
        public const string TETROMINOUS_SERVER_VERSION = "1.0.0.0";
        public const string GAMEDIR = "Tetrominous";
        public const ushort TETROMINOUS_AUTHENTICATION_PORT = 8766;
        public const ushort TETROMINOUS_SERVER_PORT = 27015;
        public const ushort TETROMINOUS_MASTER_SERVER_UPDATER_PORT = 27016;
        public const ushort MAX_PLAYERS_PER_SERVER = 2;

        // Variables
        ClientConnectionData ClientData;
        private bool bConnectedToSteam = false;

        // Accessors
        public bool BConnectedToSteam { get { return bConnectedToSteam; } }

        public Tetrominous_Server()
        {
            if (!SteamGameServer.InitGameServer(0, TETROMINOUS_SERVER_PORT, TETROMINOUS_MASTER_SERVER_UPDATER_PORT, 0, Game1.APPID, TETROMINOUS_SERVER_VERSION))
            {
                Console.WriteLine("Failed to Initiate Game Server.");
            }

            // Set the "game dir".
            // This is currently required for all games.  However, soon we will be
            // using the AppID for most purposes, and this string will only be needed
            // for mods.  it may not be changed after the server has logged on
            SteamGameServer.SetModDir(GAMEDIR);

            // These fields are currently required, but will go away soon.
            // See their documentation for more info
            SteamGameServer.SetProduct("Tetronimous");
            SteamGameServer.SetGameDescription("Tetris minigames");

            // Initiate Anonymous login
            SteamGameServer.LogOnAnonymous();

            // We want to actively update the master server with our presence so players can
            // find us via the steam matchmaking/server browser interfaces
            SteamGameServer.EnableHeartbeats(true);
        }

        ~Tetrominous_Server()
        {
            // Notify steam we are going offline
            SteamGameServer.EnableHeartbeats(false);

            // Disconnect from steam servers
            SteamGameServer.LogOff();

        }

        /// <summary>
        /// Handle clients connecting to the server
        /// </summary>
        /// <param name="callback"> Callback variable sent to server containing steam id of connecting client </param>
        void OnP2PSessionRequest(P2PSessionRequest_t callback)
        {
            // accept connection request to anyone
            SteamGameServerNetworking.AcceptP2PSessionWithUser(callback.m_steamIDRemote);
        }

        /// <summary>
        /// Handle clients disconnecting
        /// </summary>
        /// <param name="callback"> Struct containing client info </param>
        void OnP2PSessionConnectFail(P2PSessionConnectFail_t callback)
        {
            // Handle user disconnecting from server
            // RemovePlayerFromServer(i);
        }

        /// <summary>
        /// Sends data to the client but is not reliable
        /// </summary>
        /// <param name="clientID"> ID of client to send to </param>
        /// <param name="data"> Data to send to client </param>
        /// <param name="sizeOfData"> Size of data </param>
        /// <returns> Whether the send suceeded or not </returns>
        bool BSendDataToClient(CSteamID clientID, byte[] data, uint sizeOfData)
        {
            if (SteamGameServerNetworking.SendP2PPacket(clientID, data, sizeOfData, EP2PSend.k_EP2PSendUnreliable))
            {
                Console.WriteLine("Failed to send data to client " + SteamFriends.GetFriendPersonaName(clientID));
                return false;
            }

            return true;
        }

        void OnClientBeginAuthentication(CSteamID clientID, byte[] pToken, int uTokenLen)
        {
            // If the clientdata is already in use, refuse anymore connections
            if (ClientData.bActive)
            {
                // CallbackMsg_t msg;

                // Tell the client that authentication failed or server is full
                // BSendDataToClient(clientID, &msg, sizeof(msg));
                MsgServerFailAuthentication msg = new MsgServerFailAuthentication();
                byte[] msg_array = Converter.ObjectToByteArray(msg);
                uint size = (uint)(msg_array.Length * sizeof(byte));    // Another way to calculate size, dont know if it works

                SteamGameServerNetworking.SendP2PPacket(clientID, msg_array, (uint)Marshal.SizeOf(msg_array), EP2PSend.k_EP2PSendReliable);
                return;
            }
            else
            {
                // ClientData[i].TickCountSinceLastData = Current Game Tick Time

                // Authenticate User With Steam Back-End Servers
                if (SteamGameServer.BeginAuthSession(pToken, uTokenLen, clientID) != EBeginAuthSessionResult.k_EBeginAuthSessionResultOK)
                {
                    MsgServerFailAuthentication msg = new MsgServerFailAuthentication();
                    byte[] msg_array = Converter.ObjectToByteArray(msg);
                    uint size = (uint)(msg_array.Length * sizeof(byte));    // Another way to calculate size, dont know if it works

                    SteamGameServerNetworking.SendP2PPacket(clientID, msg_array, (uint)Marshal.SizeOf(msg_array), EP2PSend.k_EP2PSendReliable);
                }

                ClientData.SteamIDUser = clientID;
                ClientData.bActive = true;
            }

        }

        void OnAuthCompleted(bool bAuthSeuccessful)
        {
            if (!ClientData.bActive)
            {
                Console.WriteLine("Goth authentication completed callback for non-active slot");
                return;
            }

            if (!bAuthSeuccessful)
            {
                // Tell GS that user is leaving
                SteamGameServer.EndAuthSession(ClientData.SteamIDUser);

                // Send out deny to client
                MsgServerFailAuthentication msg = new MsgServerFailAuthentication();
                byte[] msg_array = Converter.ObjectToByteArray(msg);
                uint size = (uint)(msg_array.Length * sizeof(byte));    // Another way to calculate size, dont know if it works

                SteamGameServerNetworking.SendP2PPacket(ClientData.SteamIDUser, msg_array, (uint)Marshal.SizeOf(msg_array), EP2PSend.k_EP2PSendReliable);

                // Clear out client data
                ClientData.bActive = false;
                return;
            }

            // The client has passes authentication, integrate it into game
            // For example setting up name and score and maybe even level
            // Also tell them they've joined
            MsgServerPassAuthentication pass_msg = new MsgServerPassAuthentication();
            byte[] pass_array = Converter.ObjectToByteArray(pass_msg);

            SteamGameServerNetworking.SendP2PPacket(ClientData.SteamIDUser, pass_array, (uint)Marshal.SizeOf(pass_array), EP2PSend.k_EP2PSendReliable);



            // Check if there are two players, if there are we just need them to be reeady
            // and then the game will start. players will have a button to allow them to
            // denote if they are ready or not. need to add server states and change lobby
            // states to be able to accept server incoming messages
            // Basically Start the Game
            

        }

        void RemovePlayerFromServer()
        {
            if (!ClientData.bActive)
            {
                Console.WriteLine("Trying to remove a player that does not exist.");
                return;
            }

            ClientData.bActive = false;
            SteamGameServer.EndAuthSession(ClientData.SteamIDUser);
        }

        void ReceiveNetworkData()
        {
            byte[] recvBuf;
            uint msgSize;
            CSteamID steamIDRemote;

            // Read through all packets on the steam network
            while (SteamGameServerNetworking.IsP2PPacketAvailable(out msgSize))
            {
                // Allocate room for the packet
                recvBuf = new byte[msgSize];

                // Read the packet
                if (!SteamGameServerNetworking.ReadP2PPacket(recvBuf, msgSize, out msgSize, out steamIDRemote))
                {
                    // Returns false if no data is available, done reading messages if somehow while statement doesn't catch it
                    break;
                }

                Message msg = (Message)Converter.ByteArrayToObject(recvBuf);

                switch (msg.GetMessageType())
                {
                    case EMessage.EMsg_ClientInitiateConnection:
                        break;
                    case EMessage.EMsg_ClientBeginAuthentication:
                        break;
                    case EMessage.EMsg_ClientSendLocalUpdate:
                        break;
                    case EMessage.EMsg_ClientPing:
                        break;
                    case EMessage.EMsg_ClientLeavingServer:
                        break;
                    default:
                        Console.WriteLine("Got an invalid message on server. MessageType: " + msg.GetMessageType());
                        break;
                }
            }
        }
    }
}
