using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FGA.Core_Classes;
using FGA.Online;
using FGA.Steam;
using Steamworks;

namespace FGA.GameClasses
{
    class OnlineMultiplayerGame : GameClass
    {
        PlayerController playerController;
        Level player, otherPlayer;
        bool isReady, isOtherReady;
        double countdown = 4;

        public OnlineMultiplayerGame()
        {
            playerController = new PlayerController(PlayerIndex.One);
            isReady = false;
            isOtherReady = false;
        }

        protected override void UpdateLobby(GameTime gameTime)
        {
            ReceiveNetworkData();

            // Wait for player to be ready and also other player to be ready before
            if (Input.IsKeyPress(Keys.Space) || Input.IsKeyPress(Keys.Enter))
            {
                isReady = !isReady;
                if (!isReady)
                    ResetCountdown();

                // Send message on state of readyness
                MsgGameReady msg = new MsgGameReady();
                msg.SetIsReady(isReady);
                Console.WriteLine("Sending Ready message: " + msg.GetMessageType());
                Steam_Manager.SendP2PMessage(Steam_Manager.ConnectedPlayer, msg, EP2PSend.k_EP2PSendReliable);
            }

            // Pressing escape means cancel ready status
            if (Input.IsKeyPress(Keys.Escape))
            {
                isReady = false;
                ResetCountdown();

                // Send message on state of readyness
                MsgGameReady msg = new MsgGameReady();
                msg.SetIsReady(isReady);
                Console.WriteLine("Sending Ready message: " + msg.GetMessageType());
                Steam_Manager.SendP2PMessage(Steam_Manager.ConnectedPlayer, msg, EP2PSend.k_EP2PSendReliable);
            }

            if (isReady && isOtherReady)
                gameState = GameState.InGame;
        }

        protected override void UpdateInGame(GameTime gameTime)
        {
            ReceiveNetworkData();
            playerController.Update(gameTime);
            player.Update(gameTime);
            otherPlayer.Update(gameTime);

            MsgGameLevelUpdate msg = new MsgGameLevelUpdate();
            msg.SetGrid(player.Grid);
            msg.SetShapeQueueShape(player.shapeQueue[0]);
            msg.SetFallingShape(player.FallingShape);
            Steam_Manager.SendP2PMessage(Steam_Manager.ConnectedPlayer, msg, EP2PSend.k_EP2PSendReliable);

            base.UpdateInGame(gameTime);
        }

        protected override void UpdatePause(GameTime gameTime)
        {
            ReceiveNetworkData();
            player.Update(gameTime);
            otherPlayer.Update(gameTime);

            base.UpdatePause(gameTime);
        }

        protected override void UpdateGameOver(GameTime gameTime)
        {
            playerController.Update(gameTime);

            player.Update(gameTime);
            otherPlayer.Update(gameTime);

            if (player.returnToMenu)
                overlordReference.ReturnToMenu();
        }

        protected override void DrawLobby(GameTime gameTime, SpriteBatch spriteBatch)
        {
            playerController.Draw(gameTime, spriteBatch);

            player.Draw(gameTime, spriteBatch);
            otherPlayer.Draw(gameTime, spriteBatch);

            base.DrawLobby(gameTime, spriteBatch);
        }

        protected override void DrawInGame(GameTime gameTime, SpriteBatch spriteBatch)
        {
            playerController.Draw(gameTime, spriteBatch);

            player.Draw(gameTime, spriteBatch);
            otherPlayer.Draw(gameTime, spriteBatch);

            base.DrawInGame(gameTime, spriteBatch);
        }

        protected override void DrawGameOver(GameTime gameTime, SpriteBatch spriteBatch)
        {
            playerController.Draw(gameTime, spriteBatch);

            player.Draw(gameTime, spriteBatch);
            otherPlayer.Draw(gameTime, spriteBatch);

            base.DrawGameOver(gameTime, spriteBatch);
        }

        public override void CreateGame(Games gameMode)
        {
            switch (gameMode)
            {
                case Games.OnlineClassic:
                    player = new Level(PlayerIndex.One);
                    otherPlayer = new Level(PlayerIndex.Two);
                    break;
                case Games.OnlineTug:
                    player = new Tug_Of_War(PlayerIndex.One);
                    otherPlayer = new Tug_Of_War(PlayerIndex.Two);
                    break;
                case Games.OnlineLine:
                    player = new LineEmUp(PlayerIndex.One);
                    otherPlayer = new LineEmUp(PlayerIndex.Two);
                    break;
                case Games.OnlinePick:
                    player = new PicknDrop(PlayerIndex.One);
                    otherPlayer = new PicknDrop(PlayerIndex.Two);
                    break;
                case Games.OnlineSwitch:
                    player = new SwitcharooBuckaroo(PlayerIndex.One);
                    otherPlayer = new SwitcharooBuckaroo(PlayerIndex.Two);
                    break;
                case Games.OnlineNightmare:
                    player = new Nightmare(PlayerIndex.One);
                    otherPlayer = new Nightmare(PlayerIndex.Two);
                    break;
                case Games.OnlineDoubleTrouble:
                    player = new DoubleTrouble();
                    break;
                default: Console.WriteLine("The gamemode " + gameMode + " isn't supported in online multiplayer.");
                    break;
            }

            playerController.SetLevelReference(player);
            playerController.SetGameMode(gameMode);
            player.gameClassReference = this;
            otherPlayer.gameClassReference = this;
            gameState = GameState.InGame;
        }

        void ReceiveNetworkData()
        {
            byte[] recvBuf;
            uint msgSize;
            CSteamID steamIDRemote;

            // Read through all packets on the steam network
            while (SteamNetworking.IsP2PPacketAvailable(out msgSize))
            {
                // Allocate room for the packet
                recvBuf = new byte[msgSize];

                // Read the packet
                if (!SteamNetworking.ReadP2PPacket(recvBuf, msgSize, out msgSize, out steamIDRemote))
                {
                    // Returns false if no data is available, done reading messages if somehow while statement doesn't catch it
                    break;
                }

                Message msg = (Message)Converter.ByteArrayToObject(recvBuf);
                Console.WriteLine("Got a message:" + msg.GetMessageType());

                switch (msg.GetMessageType())
                {
                    case EMessage.EMsg_GameReady:
                        isOtherReady = ((MsgGameReady)msg).GetIsReady();
                        break;

                    case EMessage.EMsg_GameLinesCompleted:
                        int linesCompleted = ((MsgGameLinesCompleted)msg).GetLinesCompleted();

                        switch (gameMode)
                        {
                            case Games.OnlineTug: ((Tug_Of_War)player).AddFrozenBlocks(linesCompleted);
                                break;
                            case Games.Switch: ((SwitcharooBuckaroo)player).OnEnemyCompletedLines(linesCompleted);
                                break;
                            case Games.Nightmare: ((Nightmare)player).IncreaseSpookFactor(linesCompleted);
                                break;
                        }
                        break;

                    case EMessage.EMsg_GameEnemyShapeChosen:
                        if (gameMode == Games.OnlinePick)
                        {
                            player.shapeQueue.Add(((MsgGameEnemyShapeChosen)msg).GetShape());
                        }
                        break;

                    case EMessage.EMsg_GameAttackEnemy:
                        ((LineEmUp)player).TakeDamage(((MsgGameAttackEnemy)msg).GetAction());
                        break;

                    case EMessage.EMsg_GameLose:
                        player.Win();
                        playerController.EndGame();
                        break;

                    case EMessage.EMsg_GameLevelUpdate:
                        otherPlayer.Grid = ((MsgGameLevelUpdate)msg).GetGrid();
                        otherPlayer.shapeQueue[0] = ((MsgGameLevelUpdate)msg).GetShapeQueueShape();
                        otherPlayer.FallingShape = ((MsgGameLevelUpdate)msg).GetFallingShape();
                        break;

                    case EMessage.EMsg_ClientLeavingServer:
                        break;

                    default:
                        Console.WriteLine("Got an invalid message on server. MessageType: " + msg.GetMessageType());
                        break;
                }
            }
        }

        public override void OnCountdownFinish(PlayerIndex player)
        {
            if (player == PlayerIndex.One)
                playerController.StartGame();
        }

        public override void OnLinesCompleted(PlayerIndex player, int linesCompleted)
        {
            if (player == PlayerIndex.One)
            {
                MsgGameLinesCompleted msg = new MsgGameLinesCompleted();
                msg.SetLinesCompleted(linesCompleted);
                Steam_Manager.SendP2PMessage(Steam_Manager.ConnectedPlayer, msg, EP2PSend.k_EP2PSendReliable);
            }
        }

        public override void OnEnemyShapeChosen(PlayerIndex player, Shapes.Shape shape)
        {
            if (player == PlayerIndex.One)
            {
                MsgGameEnemyShapeChosen msg = new MsgGameEnemyShapeChosen();
                msg.SetShape(shape);
                Steam_Manager.SendP2PMessage(Steam_Manager.ConnectedPlayer, msg, EP2PSend.k_EP2PSendReliable);
            }
        }

        public override void OnAttackEnemy(PlayerIndex player, Action action)
        {
            
        }

        public override void OnLose(PlayerIndex player)
        {
            if (player == PlayerIndex.One)
            {
                MsgGameLose msg = new MsgGameLose();
                Steam_Manager.SendP2PMessage(Steam_Manager.ConnectedPlayer, msg, EP2PSend.k_EP2PSendReliable);
                playerController.EndGame();

                base.OnLose(player);
            }
        }

        protected void ResetCountdown()
        {
            countdown = 4;
        }
    }
}
