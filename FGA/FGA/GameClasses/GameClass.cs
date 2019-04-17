using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FGA.Shapes;

namespace FGA.GameClasses
{
    public abstract class GameClass
    {
        protected enum GameState { Lobby, InGame, Pause, EndGame };

        protected GameState gameState;
        protected Games gameMode;

        protected Game1 overlordReference;

        //GameState Pause varibles
        protected Texture2D PauseMenuTexture;
        protected Rectangle PauseMenuLocation;
        protected Button exitButton, resumeButton;

        public GameClass()
        {
            PauseMenuTexture = Game1.GlobalContent.Load<Texture2D>(@"Background Screens/Pause_Screen");
            PauseMenuLocation = new Rectangle((int)(480 * Game1.displayRatio.X), (int)(270 * Game1.displayRatio.Y), (int)(960 * Game1.displayRatio.X), (int)(540 * Game1.displayRatio.Y));

            //Initialize Buttons
            resumeButton = new Button(Game1.GlobalContent.Load<Texture2D>(@"Buttons & Cursor/Silver_Resume_Button"), new Vector2(610, 555) * Game1.displayRatio, new Vector2(300, 150) * Game1.displayRatio);
            exitButton = new Button(Game1.GlobalContent.Load<Texture2D>(@"Buttons & Cursor/Silver_Exit_Button"), new Vector2(1010, 555) * Game1.displayRatio, new Vector2(300, 150) * Game1.displayRatio);
        }

        public void SetOverlordReference(Game1 overlord) { this.overlordReference = overlord; }

        public virtual void Update(GameTime gameTime)
        {
            switch (gameState)
            {
                case GameState.Lobby: UpdateLobby(gameTime);
                    break;
                case GameState.InGame: UpdateInGame(gameTime);
                    break;
                case GameState.Pause: UpdatePause(gameTime);
                    break;
                case GameState.EndGame: UpdateGameOver(gameTime);
                    break;
            }
        }

        protected virtual void UpdateLobby(GameTime gameTime) { }

        protected virtual void UpdateInGame(GameTime gameTime)
        {
            if (Input.Pause)
                Pause();
        }

        protected virtual void UpdatePause(GameTime gameTime)
        {
            if (Input.Pause)
            {
                UnPause();
            }

            resumeButton.Update(gameTime);
            exitButton.Update(gameTime);

            if (resumeButton.IsClicked)
            {
                UnPause();
            }

            if (exitButton.IsClicked)
            {
                // Need something to tell game to exit back to menu
                overlordReference.ReturnToMenu();
            }
        }

        protected virtual void UpdateGameOver(GameTime gameTime) { }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            switch (gameState)
            {
                case GameState.Lobby: DrawLobby(gameTime, spriteBatch);
                    break;
                case GameState.InGame: DrawInGame(gameTime, spriteBatch);
                    break;
                case GameState.Pause: DrawPause(gameTime, spriteBatch);
                    break;
                case GameState.EndGame: DrawGameOver(gameTime, spriteBatch);
                    break;
            }
        }

        protected virtual void DrawLobby(GameTime gameTime, SpriteBatch spriteBatch) { }

        protected virtual void DrawInGame(GameTime gameTime, SpriteBatch spriteBatch) { }

        protected virtual void DrawPause(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //Draw Menu
            spriteBatch.Draw(PauseMenuTexture, PauseMenuLocation, Color.White);

            //Draw Menu and Buttons
            exitButton.Draw(spriteBatch);
            resumeButton.Draw(spriteBatch);
        }

        protected virtual void DrawGameOver(GameTime gameTime, SpriteBatch spriteBatch) {  }

        public virtual void PauseOrUnpause()
        {
            if (gameState == GameState.Pause)
                gameState = GameState.InGame;
            else
                gameState = GameState.Pause;
        }

        public virtual void UnPause()
        {
            gameState = GameState.InGame;
        }

        public virtual void Pause()
        {
            gameState = GameState.Pause;
        }

        public virtual void OnLose(PlayerIndex player) 
        {
            gameState = GameState.EndGame;
        }

        public virtual void CreateGame(Games gameType)
        {
            this.gameMode = gameType;   
        }

        public virtual void OnLinesCompleted(PlayerIndex player, int linesCompleted) { }

        public virtual void OnCountdownFinish(PlayerIndex player) { }

        public virtual void OnEnemyShapeChosen(PlayerIndex player, Shape shape) { }

        public virtual void OnAttackEnemy(PlayerIndex player, Action action) { }
    }
}
