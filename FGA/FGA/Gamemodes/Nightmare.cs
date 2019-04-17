using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using FGA.Shapes;

namespace FGA
{
    class Nightmare : Level
    {
        float spookFactor;
        Texture2D spookeffect;

        public Nightmare(PlayerIndex player)
            : base(player)
        {
            if (player == PlayerIndex.One)
            {
                position = new Vector2(289, 954) * Game1.displayRatio;
                scoreDisplayPosition = new Vector2(705, 470) * Game1.displayRatio;
                nextBlockDisplayPosition = new Vector2(520, -703) * Game1.displayRatio;
            }
            else if (player == PlayerIndex.Two)
            {
                position = new Vector2(1238, 954) * Game1.displayRatio;
                scoreDisplayPosition = new Vector2(960, 470) * Game1.displayRatio;
                nextBlockDisplayPosition = new Vector2(-175, -703) * Game1.displayRatio;
            }

            spookFactor = 0;
            spookeffect = Game1.GlobalContent.Load<Texture2D>(@"Background Screens/Black_Square");
        }

        protected override void DrawInGame(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Color spookopacity;

            for (int i = 0; i < grid.Count; i++)
            {
                for (int j = 0; j < grid[i].Count; j++)
                {
                    if (grid[i][j].enabled)
                    {
                        grid[i][j].position = new Vector2(position.X + (j * 40 * Game1.displayRatio.X), position.Y - (i * 40 * Game1.displayRatio.Y));
                        float alpha = Math.Abs(currentFallingShape.gridPosition.X - j) + Math.Abs(currentFallingShape.gridPosition.Y - i);
                        alpha = Math.Max(0, (spookFactor * alpha));
                        grid[i][j].Draw(gameTime, spriteBatch, grid[i][j].position, alpha);
                        spookopacity = new Color(0, 0, 0, alpha);
                        spriteBatch.Draw(spookeffect, new Rectangle((int)Math.Round(grid[i][j].position.X), (int)Math.Round(grid[i][j].position.Y), (int)(40 * Game1.displayRatio.X), (int)(40 * Game1.displayRatio.Y)), spookopacity);

                    }
                }
            }

            DrawIndicator(gameTime, spriteBatch);
            currentFallingShape.DrawToGrid(gameTime, spriteBatch, position);

            DrawNextBlock(gameTime, spriteBatch);
        }

        protected override void DrawIndicator(GameTime gameTime, SpriteBatch spriteBatch)
        {
            indicatorShape.Draw(gameTime, spriteBatch, currentFallingShape, position, spookeffect, spookFactor);
        }

        protected override void DrawScore(GameTime gameTime, SpriteBatch spriteBatch)
        {

        }

        protected override Shape GenerateRandomShape()
        {
            Shape Temp = base.GenerateRandomShape();
            Temp.SetBlockColors(BlockColors.NightmareColor);

            return Temp;
        }

        public void IncreaseSpookFactor(int linesCompleted)
        {
            spookFactor += ((float)linesCompleted) / 250f;
        }

        protected override void AddShapeToGrid()
        {
            currentFallingShape.AddShapeToGrid(grid);
            NextFallingShape();
            int linesss = CheckCompletedRows();

            gameClassReference.OnLinesCompleted(player, linesss);
        }
    }
}
