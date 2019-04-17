using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FGA.Core_Classes
{
    class PlayerController
    {
        protected enum ControllerState { Waiting, InGame, EndGame };

        protected Level levelReference;
        protected PlayerIndex player;
        protected Games gameMode;
        protected ControllerState controllerState;
        protected bool isReady;

        public PlayerController(PlayerIndex player)
        {
            this.player = player;
            controllerState = ControllerState.Waiting;
            isReady = false;
        }

        public virtual void Update(GameTime gameTime)
        {
            switch (controllerState)
            {
                case ControllerState.Waiting:
                    UpdateWaiting(gameTime);
                    break;
                case ControllerState.InGame:
                    UpdateInGame(gameTime);
                    break;
                case ControllerState.EndGame:
                    UpdateEndGame(gameTime);
                    break;
            }
        }

        protected virtual void UpdateWaiting(GameTime gameTime)
        {
            
        }

        protected virtual void UpdateInGame(GameTime gameTime)
        {
            if (player == PlayerIndex.One)
            {
                //Move left
                if (Input.P1Left)
                    levelReference.MoveLeft();

                //Move right
                if (Input.P1Right)
                    levelReference.MoveRight();

                //Rotate
                if (Input.P1Rotate)
                    levelReference.Rotate();

                //Drop down 1 block
                if (Input.P1Drop1)
                    levelReference.DropOne();


                //Drop completely down
                if (Input.P1DropAll)
                    levelReference.DropAll();

                if (gameMode == Games.Tug || gameMode == Games.OnlineTug)
                {
                    if (Input.P1Hold)
                        ((Tug_Of_War)levelReference).HoldShape();
                }

                if (gameMode == Games.DoubleTrouble)
                {
                    if (Input.P2Left)
                        ((DoubleTrouble)levelReference).MoveLeftP2();

                    if (Input.P2Right)
                        ((DoubleTrouble)levelReference).MoveRightP2();

                    if (Input.P2Rotate)
                        ((DoubleTrouble)levelReference).RotateP2();

                    if (Input.P2Drop1)
                        ((DoubleTrouble)levelReference).DropOneP2();

                    if (Input.P2DropAll)
                        ((DoubleTrouble)levelReference).DropAllP2();
                }

                if (Input.IsKeyPress(Microsoft.Xna.Framework.Input.Keys.T))
                {
                    FGA.Online.MsgGameLevelUpdate msg = new FGA.Online.MsgGameLevelUpdate();
                    msg.SetGrid(levelReference.Grid);
                    msg.SetShapeQueueShape(levelReference.shapeQueue[0]);
                    msg.SetFallingShape(levelReference.FallingShape);

                    byte[] tempArray = FGA.Online.Converter.ObjectToByteArray(msg);
                    
                    Console.WriteLine("Level data is " + tempArray.Length + " long and " + tempArray.Length * sizeof(byte) + " bytes");
                    
                }
            }
            else if (player == PlayerIndex.Two)
            {
                //Move left
                if (Input.P2Left)
                    levelReference.MoveLeft();

                //Move right
                if (Input.P2Right)
                    levelReference.MoveRight();

                //Rotate
                if (Input.P2Rotate)
                    levelReference.Rotate();

                //Drop down 1 block
                if (Input.P2Drop1)
                    levelReference.DropOne();

                //Drop completely down
                if (Input.P2DropAll)
                    levelReference.DropAll();

                if (gameMode == Games.Tug || gameMode == Games.OnlineTug)
                {
                    if (Input.P2Hold)
                        ((Tug_Of_War)levelReference).HoldShape();
                }
            }

            levelReference.Update(gameTime);
        }

        protected virtual void UpdateEndGame(GameTime gameTime)
        {
            if (Input.Pause || Input.IsKeyPress(Microsoft.Xna.Framework.Input.Keys.Space))
                levelReference.FinishEverything();
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            switch (controllerState)
            {
                case ControllerState.Waiting:
                    DrawWaiting(gameTime, spriteBatch);
                    break;
                case ControllerState.InGame:
                    DrawInGame(gameTime, spriteBatch);
                    break;
                case ControllerState.EndGame:
                    DrawEndGame(gameTime, spriteBatch);
                    break;
            }
        }

        protected virtual void DrawWaiting(GameTime gameTime, SpriteBatch spriteBatch)
        {

        }

        protected virtual void DrawInGame(GameTime gameTime, SpriteBatch spriteBatch)
        {
            levelReference.Draw(gameTime, spriteBatch);
        }

        protected virtual void DrawEndGame(GameTime gameTime, SpriteBatch spriteBatch)
        {

        }

        public void SetLevelReference(Level levelReference)
        {
            this.levelReference = levelReference;
        }

        public Level GetLevelReference()
        {
            return this.levelReference;
        }

        public void SetGameMode(Games gameMode)
        {
            this.gameMode = gameMode;
        }

        public void EndGame()
        {
            controllerState = ControllerState.EndGame;
        }

        public void StartGame()
        {
            if(levelReference != null)
                controllerState = ControllerState.InGame;
        }

        public bool IsReady
        {
            get { return isReady; }
        }

    }
}
