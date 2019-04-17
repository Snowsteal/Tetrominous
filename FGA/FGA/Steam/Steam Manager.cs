using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Steamworks;
using FGA.Online;

namespace FGA.Steam
{
    class Steam_Manager
    {
        // Constants
        static AppId_t APP_ID = ((AppId_t)509150);

        // Overlord reference
        static Game1 overlordReference;

        // Private Variables
        private static bool isInitialized = false;
        private static string username = "Player";
        private static CSteamID userID;
        private static CSteamID connectedPlayer;

        // Callbacks and results
        static Callback<GameOverlayActivated_t> m_GameOverlayActivated;
        static Callback<LobbyCreated_t> m_LobbyCreated;
        static Callback<GameLobbyJoinRequested_t> m_GameLobbyJoinRequested;
        static Callback<LobbyEnter_t> m_LobbyEntered;
        static Callback<P2PSessionRequest_t> m_P2PSessionRequest;

        // Public acessors
        public static bool IsInitialized
        {
            get { return isInitialized; }
        }

        public static string Username
        {
            get { return username; }
        }

        public static CSteamID ConnectedPlayer
        {
            get { return connectedPlayer; }
            set { connectedPlayer = value; }
        }

        public static CSteamID UserID
        {
            get { return userID; }
        }

        public static void SetOverlordReference(Game1 overlordReferences)
        {
            overlordReference = overlordReferences;
        }

        // Returns false if any initialization fails, it would be best to close the game afterwards, otherwise this return true so it's safe to continue game initialization
        public static bool Initialize()
        {
            // Make sure the game is being run through steam
            if (SteamAPI.RestartAppIfNecessary(APP_ID))
                return false;

            // Steam initialization
            try
            {
                if (!SteamAPI.Init())
                {
                    Console.WriteLine("SteamAPI.Init() failed!");
                    return false;
                }
            }
            catch (DllNotFoundException e)
            { // We check this here as it will be the first instance of it.
                Console.WriteLine(e);
                return false;
            }

            // Makes sure steamworks.net is running under the correct platform
            if (!Packsize.Test())
            {
                Console.WriteLine("You're using the wrong Steamworks.NET Assembly for this platform!");
                return false;
            }

            // Makes sure steamworks redistributable binaries are the correct version
            if (!DllCheck.Test())
            {
                Console.WriteLine("You're using the wrong dlls for this platform!");
                return false;
            }

            // Check that user is logged onto steam, mainly for when the game is launched from outside of steam in visual studio
            if (!SteamUser.BLoggedOn())
            {
                Console.WriteLine("Steam user is not logged on, pls log on and try launching again.");
                return false;
            }

            // Grab the name of player
            username = SteamFriends.GetPersonaName();
            userID = SteamUser.GetSteamID();

            Console.WriteLine("Hello " + Username + ". Welcome to Tetronminous.");

            isInitialized = true;

            InitializeCallbacks();

            return true;
        }

        // Run after Initialize
        private static void InitializeCallbacks()
        {
            if (isInitialized)
            {
                m_GameOverlayActivated = Callback<GameOverlayActivated_t>.Create(OnGameOverlayActivated);
                m_LobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
                m_GameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
                m_LobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
                m_P2PSessionRequest = Callback<P2PSessionRequest_t>.Create(OnP2PSessionRequest);
            }
        }

        public static void Unload()
        {
            if (!isInitialized)
                return;

            SteamAPI.Shutdown();
        }

        // Update the steam manager every tick, mostly used for callbacks
        public static void Update()
        {
            // Steam should be initialized before updating
            if (!isInitialized)
                return;

            SteamAPI.RunCallbacks();
        }

        static void OnGameOverlayActivated(GameOverlayActivated_t pCallback)
        {
            if (pCallback.m_bActive == 1)
            {
                if(overlordReference.getGameState() == Game1.GameState.Game)
                    overlordReference.getGameClass().Pause();
            }
        }

        static void OnLobbyCreated(LobbyCreated_t pCallback)
        {
            if (pCallback.m_eResult == EResult.k_EResultOK)
                overlordReference.getMenu().OnLobbyCreated((CSteamID)pCallback.m_ulSteamIDLobby);
            else
                overlordReference.getMenu().OnLobbyCreationFailure();
        }

        static void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t pCallback)
        {
            overlordReference.getMenu().OnLobbyInvite(pCallback.m_steamIDFriend, pCallback.m_steamIDLobby);
        }

        static void OnLobbyEntered(LobbyEnter_t pCallback)
        {
            overlordReference.getMenu().OnLobbyEntered((CSteamID)pCallback.m_ulSteamIDLobby);
        }

        static void OnP2PSessionRequest(P2PSessionRequest_t callback)
        {
            // accept connection request to anyone
            if (connectedPlayer == callback.m_steamIDRemote)
                SteamGameServerNetworking.AcceptP2PSessionWithUser(callback.m_steamIDRemote);
            else
                Console.WriteLine("Rejecting connection request.");
        }

        static void OnP2PSessionConnectFail(P2PSessionConnectFail_t callback)
        {
            // Handle user disconnecting from other user
            overlordReference.OnDisconnect();
        }

        public static void SendP2PMessage(CSteamID clientID, Message msg, EP2PSend sendType)
        {
            byte[] msg_array = Converter.ObjectToByteArray(msg);
            uint size = (uint)msg_array.Length;    // Another way to calculate size, dont know if it works

            // SteamGameServerNetworking.SendP2PPacket(clientID, msg_array, (uint)Marshal.SizeOf(msg_array), sendType);
            SteamNetworking.SendP2PPacket(clientID, msg_array, size, sendType);
        }
    }
}
