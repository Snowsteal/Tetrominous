using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FGA.Shapes;


namespace FGA.Online
{
    public enum EMessage
    {
        // Server Messages
        EMsg_ServerBegin = 0,
        EMsg_ServerSendInfo = EMsg_ServerBegin + 1,
        EMsg_ServerFailAuthentication = EMsg_ServerBegin + 2,
        EMsg_ServerPassAuthentication = EMsg_ServerBegin + 3,
        EMsg_ServerUpdateWorld = EMsg_ServerBegin + 4,
        EMsg_ServerExiting = EMsg_ServerBegin + 5,
        EMsg_ServerPingResponse = EMsg_ServerBegin + 6,
        EMsg_ServerFull = EMsg_ServerBegin + 7,
        EMsg_ServerStartGame = EMsg_ServerBegin + 8,

        // Client Messages
        EMsg_ClientBegin = 100,
        EMsg_ClientInitiateConnection = EMsg_ClientBegin + 1,
        EMsg_ClientBeginAuthentication = EMsg_ClientBegin + 2,
        EMsg_ClientSendLocalUpdate = EMsg_ClientBegin + 3,
        EMsg_ClientLeavingServer = EMsg_ClientBegin + 4,
        EMsg_ClientPing = EMsg_ClientBegin + 5,

        // P2P Authentication Messages
        EMsg_P2PBegin = 200,
        EMsg_P2PSendingTicket = EMsg_P2PBegin + 1,

        // Chat Messages
        EMsg_ChatBegin = 300,
        EMsg_ChatData = EMsg_ClientBegin + 1,

        // Voice Chat Messages
        EMsg_VoiceChatBegin = 400,
        EMsg_VoiceChatPing = EMsg_VoiceChatBegin + 1,   // just a keep alive message
        EMsg_VoiceChatData = EMsg_VoiceChatBegin + 2,   // Voice data from another player

        // Game Messages
        EMsg_GameBegin = 500,
        EMsg_GameReady = EMsg_GameBegin + 1,
        EMsg_GameLevelUpdate = EMsg_GameBegin + 2,
        EMsg_GameLinesCompleted = EMsg_GameBegin + 3,
        EMsg_GameEnemyShapeChosen = EMsg_GameBegin + 4,
        EMsg_GameAttackEnemy = EMsg_GameBegin + 5,
        EMsg_GameLose = EMsg_GameBegin + 6,
    }

    [Serializable]
    public abstract class Message
    {
        protected EMessage messageType;
        public EMessage GetMessageType() { return messageType; }
    }

    [Serializable]
    public class MsgServerSendInfo : Message
    {
        protected UInt64 SteamIDServer;
        protected string ServerName;
        public MsgServerSendInfo()
        {
            messageType = EMessage.EMsg_ServerSendInfo;
        }

        public void SetSteamIDServer(UInt64 SteamID) { SteamIDServer = SteamID; }
        public UInt64 GetSteamIDServer() { return SteamIDServer; }

        public void SetServerName(string ServerName) { this.ServerName = ServerName; }
        public string GetServerName() { return ServerName; }
    }

    [Serializable]
    public class MsgServerFailAuthentication : Message
    {
        public MsgServerFailAuthentication()
        {
            messageType = EMessage.EMsg_ServerFailAuthentication;
        }
    }

    [Serializable]
    public class MsgServerPassAuthentication : Message
    {
        public MsgServerPassAuthentication()
        {
            messageType = EMessage.EMsg_ServerPassAuthentication;
        }
    }

    [Serializable]
    public class MsgServerUpdateWorld : Message
    {
        private Converter.DataLevelSimple LevelInfo;

        public MsgServerUpdateWorld()
        {
            messageType = EMessage.EMsg_ServerUpdateWorld;
        }

        public void SetLevelInfo(Converter.DataLevelSimple LevelInfo) { this.LevelInfo = LevelInfo; }
        public Converter.DataLevelSimple GetLevelInfo() { return LevelInfo; }
    }

    [Serializable]
    public class MsgServerExiting : Message
    {
        public MsgServerExiting()
        {
            messageType = EMessage.EMsg_ServerExiting;
        }
    }

    [Serializable]
    public class MsgServerPingResponse : Message
    {
        public MsgServerPingResponse()
        {
            messageType = EMessage.EMsg_ServerPingResponse;
        }
    }

    [Serializable]
    public class MsgServerFull : Message
    {
        public MsgServerFull()
        {
            messageType = EMessage.EMsg_ServerFull;
        }
    }

    [Serializable]
    public class MsgServerStartGame : Message
    {
        private Games gameMode;

        public MsgServerStartGame()
        {
            messageType = EMessage.EMsg_ServerStartGame;
        }

        public void SetGameMode(Games gameMode) { this.gameMode = gameMode; }
        public Games GetGameMode() { return gameMode; }
    }

    [Serializable]
    public class MsgClientInitiateConnection : Message
    {
        public MsgClientInitiateConnection()
        {
            messageType = EMessage.EMsg_ClientInitiateConnection;
        }
    }

    [Serializable]
    public class MsgClientBeginAuthentication : Message
    {
        private UInt32 tokenLength;
        private char[] token;
        private UInt64 SteamID;

        public MsgClientBeginAuthentication()
        {
            messageType = EMessage.EMsg_ClientBeginAuthentication;
        }

        public void SetTokenLength(UInt32 tokenLength) { this.tokenLength = tokenLength; }
        public UInt32 GetTokenLength() { return tokenLength; }

        public void SetToken(char[] token) { this.token = token; }
        public char[] GetToken() { return token; }

        public void SetSteamID(UInt64 SteamID) { this.SteamID = SteamID; }
        public UInt64 GetSteamID() { return SteamID; }

    }

    [Serializable]
    public class MsgClientSendLocalUpdate : Message
    {
        private Converter.DataLevelSimple LevelInfo;

        public MsgClientSendLocalUpdate()
        {
            messageType = EMessage.EMsg_ClientSendLocalUpdate;
        }

        public void SetLevelInfo(Converter.DataLevelSimple LevelInfo) { this.LevelInfo = LevelInfo; }
        public Converter.DataLevelSimple GetLevelInfo() { return LevelInfo; }
    }

    [Serializable]
    public class MsgClientLeaveingServer : Message
    {
        public MsgClientLeaveingServer()
        {
            messageType = EMessage.EMsg_ClientLeavingServer;
        }
    }

    [Serializable]
    public class MsgClientPing : Message
    {
        public MsgClientPing()
        {
            messageType = EMessage.EMsg_ClientPing;
        }
    }

    [Serializable]
    public class MsgP2PSendingticket : Message
    {
        private UInt32 tokenLength;
        private char[] token;
        private UInt64 SteamID;

        public MsgP2PSendingticket()
        {
            messageType = EMessage.EMsg_P2PSendingTicket;
        }

        public void SetTokenLength(UInt32 tokenLength) { this.tokenLength = tokenLength; }
        public UInt32 GetTokenLength() { return tokenLength; }

        public void SetToken(char[] token) { this.token = token; }
        public char[] GetToken() { return token; }

        public void SetSteamID(UInt64 SteamID) { this.SteamID = SteamID; }
        public UInt64 GetSteamID() { return SteamID; }

    }

    [Serializable]
    public class MsgVoiceChatPing : Message
    {
        public MsgVoiceChatPing()
        {
            messageType = EMessage.EMsg_VoiceChatPing;
        }
    }

    [Serializable]
    public class MsgVoiceChatData : Message
    {
        private UInt32 DataLength;

        public MsgVoiceChatData()
        {
            messageType = EMessage.EMsg_VoiceChatData;
        }

        public void SetDataLength(UInt32 DataLength) { this.DataLength = DataLength; }
        public UInt32 GetDataLength() { return DataLength; }
    }

    [Serializable]
    public class MsgGameReady : Message
    {
        private bool isReady;

        public MsgGameReady()
        {
            messageType = EMessage.EMsg_GameReady;
        }

        public void SetIsReady(bool isReady) { this.isReady = isReady; }
        public bool GetIsReady() { return isReady; }
    }

    [Serializable]
    public class MsgGameLevelUpdate : Message
    {
        private List<List<Converter.DataBlock>> dataGrid;
        private string fallingShapeData;
        private string shapeQueueData;

        public MsgGameLevelUpdate()
        {
            messageType = EMessage.EMsg_GameLevelUpdate;
        }

        public void SetGrid(List<List<Block>> grid) { this.dataGrid = Converter.ConvertToDataGrid(grid); }
        public List<List<Block>> GetGrid() { return Converter.ConvertToGrid(dataGrid); }

        public void SetShapeQueueShape(Shape shape) { this.shapeQueueData = Converter.ConvertToString(shape); }
        public Shape GetShapeQueueShape() { return Converter.ConvertToShape(shapeQueueData); }

        public void SetFallingShape(Shape shape) { this.fallingShapeData = Converter.ConvertToString(shape); }
        public Shape GetFallingShape() { return Converter.ConvertToShape(fallingShapeData); }
    }

    [Serializable]
    public class MsgGameLinesCompleted : Message
    {
        int linesCompleted;

        public MsgGameLinesCompleted()
        {
            messageType = EMessage.EMsg_GameLinesCompleted;
        }

        public void SetLinesCompleted(int linesCompleted) { this.linesCompleted = linesCompleted; }
        public int GetLinesCompleted() { return linesCompleted; }
    }

    [Serializable]
    public class MsgGameEnemyShapeChosen : Message
    {
        string shapeData;

        public MsgGameEnemyShapeChosen()
        {
            messageType = EMessage.EMsg_GameEnemyShapeChosen;
        }

        public void SetShape(Shape shape) { this.shapeData = Converter.ConvertToString(shape); }
        public Shape GetShape() { return Converter.ConvertToShape(shapeData); }
    }

    [Serializable]
    public class MsgGameAttackEnemy : Message
    {
        Action action;

        public MsgGameAttackEnemy()
        {
            messageType = EMessage.EMsg_GameAttackEnemy;
        }

        public void SetAction(Action action) { this.action = action; }
        public Action GetAction() { return action; }
    }

    [Serializable]
    public class MsgGameLose : Message
    {
        public MsgGameLose()
        {
            messageType = EMessage.EMsg_GameLose;
        }
    }

}
