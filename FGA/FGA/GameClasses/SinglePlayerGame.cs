using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FGA.Core_Classes;

namespace FGA.GameClasses
{
    class SinglePlayerGame : GameClass
    {
        PlayerController playerController;
        Level level;

        public SinglePlayerGame()
        {
            playerController = new PlayerController(PlayerIndex.One);
        }

        protected override void UpdateInGame(GameTime gameTime)
        {
            base.UpdateInGame(gameTime);

            playerController.Update(gameTime);
            level.Update(gameTime);
        }

        protected override void UpdateGameOver(GameTime gameTime)
        {
            playerController.Update(gameTime);
            level.Update(gameTime);

            if (level.returnToMenu)
                overlordReference.ReturnToMenu();
        }

        protected override void DrawInGame(GameTime gameTime, SpriteBatch spriteBatch)
        {
            playerController.Draw(gameTime, spriteBatch);
            level.Draw(gameTime, spriteBatch);

            base.DrawInGame(gameTime, spriteBatch);
        }

        protected override void DrawPause(GameTime gameTime, SpriteBatch spriteBatch)
        {
            playerController.Draw(gameTime, spriteBatch);
            level.Draw(gameTime, spriteBatch);
            
            base.DrawPause(gameTime, spriteBatch);
        }

        protected override void DrawGameOver(GameTime gameTime, SpriteBatch spriteBatch)
        {
            playerController.Draw(gameTime, spriteBatch);
            level.Draw(gameTime, spriteBatch);
        }

        public override void CreateGame(Games gameMode)
        {
            switch (gameMode)
            {
                case Games.Classic: level = new Level(PlayerIndex.One);
                    break;
                case Games.Factory: level = new Tetris_Factory();
                    break;
                case Games.DoubleTrouble: level = new DoubleTrouble();
                    break;
                default: Console.WriteLine("The gamemode " + gameMode + " isn't supported in singleplayer.");
                    break;
            }

            playerController.SetLevelReference(level);
            playerController.SetGameMode(gameMode);
            level.gameClassReference = this;
            gameState = GameState.InGame;
        }

        public override void OnCountdownFinish(PlayerIndex player)
        {
            playerController.StartGame();
        }

        public override void OnLose(PlayerIndex player)
        {
            playerController.EndGame();

            base.OnLose(player);
        }
    }
}
