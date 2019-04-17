using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FGA.Core_Classes;

namespace FGA.GameClasses
{
    class LocalMultiplayerGame : GameClass
    {
        PlayerController playerOneController, playerTwoController;
        Level playerOneLevel, playerTwoLevel;

        public LocalMultiplayerGame()
        {
            playerOneController = new PlayerController(PlayerIndex.One);
            playerTwoController = new PlayerController(PlayerIndex.Two);
        }

        protected override void UpdateInGame(GameTime gameTime)
        {
            playerOneController.Update(gameTime);
            playerTwoController.Update(gameTime);

            playerOneLevel.Update(gameTime);
            playerTwoLevel.Update(gameTime);

            base.UpdateInGame(gameTime);
        }

        protected override void UpdateGameOver(GameTime gameTime)
        {
            playerOneController.Update(gameTime);
            playerTwoController.Update(gameTime);

            playerOneLevel.Update(gameTime);
            playerTwoLevel.Update(gameTime);

            if (playerOneLevel.returnToMenu && playerTwoLevel.returnToMenu)
                overlordReference.ReturnToMenu();
        }

        protected override void DrawInGame(GameTime gameTime, SpriteBatch spriteBatch)
        {
            playerOneController.Draw(gameTime, spriteBatch);
            playerTwoController.Draw(gameTime, spriteBatch);

            playerOneLevel.Draw(gameTime, spriteBatch);
            playerTwoLevel.Draw(gameTime, spriteBatch);
            
            base.DrawInGame(gameTime, spriteBatch);
        }

        protected override void DrawPause(GameTime gameTime, SpriteBatch spriteBatch)
        {
            playerOneController.Draw(gameTime, spriteBatch);
            playerTwoController.Draw(gameTime, spriteBatch);

            playerOneLevel.Draw(gameTime, spriteBatch);
            playerTwoLevel.Draw(gameTime, spriteBatch);

            base.DrawPause(gameTime, spriteBatch);
        }

        protected override void DrawGameOver(GameTime gameTime, SpriteBatch spriteBatch)
        {
            playerOneController.Draw(gameTime, spriteBatch);
            playerTwoController.Draw(gameTime, spriteBatch);

            playerOneLevel.Draw(gameTime, spriteBatch);
            playerTwoLevel.Draw(gameTime, spriteBatch);
        }

        public override void CreateGame(Games gameMode)
        {
            switch (gameMode)
            {
                case Games.Classic:
                    playerOneLevel = new Level(PlayerIndex.One);
                    playerTwoLevel = new Level(PlayerIndex.Two);
                    break;
                case Games.Tug:
                    playerOneLevel = new Tug_Of_War(PlayerIndex.One);
                    playerTwoLevel = new Tug_Of_War(PlayerIndex.Two);
                    break;
                case Games.Line:
                    playerOneLevel = new LineEmUp(PlayerIndex.One);
                    playerTwoLevel = new LineEmUp(PlayerIndex.Two);
                    break;
                case Games.Pick:
                    playerOneLevel = new PicknDrop(PlayerIndex.One);
                    playerTwoLevel = new PicknDrop(PlayerIndex.Two);
                    break;
                case Games.Switch:
                    playerOneLevel = new SwitcharooBuckaroo(PlayerIndex.One);
                    playerTwoLevel = new SwitcharooBuckaroo(PlayerIndex.Two);
                    break;
                case Games.Nightmare:
                    playerOneLevel = new Nightmare(PlayerIndex.One);
                    playerTwoLevel = new Nightmare(PlayerIndex.Two);
                    break;
                default:
                    Console.WriteLine("The gamemode " + gameMode + " isn't supported in local multiplayer.");
                    overlordReference.ReturnToMenu();
                    break;
            }

            playerOneController.SetLevelReference(playerOneLevel);
            playerOneController.SetGameMode(gameMode);
            playerOneLevel.gameClassReference = this;

            playerTwoController.SetLevelReference(playerTwoLevel);
            playerTwoController.SetGameMode(gameMode);
            playerTwoLevel.gameClassReference = this;
            gameState = GameState.InGame;

            base.CreateGame(gameMode);
        }

        public override void OnCountdownFinish(PlayerIndex player)
        {
            if (player == PlayerIndex.One)
                playerOneController.StartGame();
            else if (player == PlayerIndex.Two)
                playerTwoController.StartGame();
        }

        public override void OnLinesCompleted(PlayerIndex player, int linesCompleted)
        {
            switch (gameMode)
            {
                case Games.Tug:
                    if(player == PlayerIndex.One)
                        ((Tug_Of_War)playerTwoLevel).AddFrozenBlocks(linesCompleted);
                    else if(player == PlayerIndex.Two)
                        ((Tug_Of_War)playerOneLevel).AddFrozenBlocks(linesCompleted);
                    break;
                case Games.Switch:
                    if (player == PlayerIndex.One)
                        ((SwitcharooBuckaroo)playerTwoLevel).OnEnemyCompletedLines(linesCompleted);
                    else if (player == PlayerIndex.Two)
                        ((SwitcharooBuckaroo)playerOneLevel).OnEnemyCompletedLines(linesCompleted);
                    break;
                case Games.Nightmare:
                    if (player == PlayerIndex.One)
                        ((Nightmare)playerTwoLevel).IncreaseSpookFactor(linesCompleted);
                    else if (player == PlayerIndex.Two)
                        ((Nightmare)playerOneLevel).IncreaseSpookFactor(linesCompleted);
                    break;
            }
        }

        public override void OnEnemyShapeChosen(PlayerIndex player, Shapes.Shape shape)
        {
            if (gameMode == Games.Pick)
            {
                if (player == PlayerIndex.One)
                    playerTwoLevel.shapeQueue.Add(shape);
                else if (player == PlayerIndex.Two)
                    playerOneLevel.shapeQueue.Add(shape);
            }
        }

        public override void OnAttackEnemy(PlayerIndex player, Action action)
        {
            if (gameMode == Games.Line)
            {
                if (player == PlayerIndex.One)
                    ((LineEmUp)playerTwoLevel).TakeDamage(action);
                else if (player == PlayerIndex.Two)
                    ((LineEmUp)playerOneLevel).TakeDamage(action);
            }
        }

        public override void OnLose(PlayerIndex player)
        {
            if (player == PlayerIndex.One)
                playerTwoLevel.Win();
            else if (player == PlayerIndex.Two)
                playerOneLevel.Win();

            playerOneController.EndGame();
            playerTwoController.EndGame();

            base.OnLose(player);
        }
    }
}
