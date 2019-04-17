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
    public class Block
    {
        public bool enabled;
        public Vector2 position, gridPosition;
        public BlockColor blockColor;

        public int BlockIndex
        {
            get { return (int)blockColor.color; }
        }

        public Block(bool enabled)
        {
            this.enabled = enabled;
            blockColor = BlockColors.EmptyColor;
        }

        public void UpdateGridPosition(GameTime gameTime, Vector2 levelPosition, Vector2 gridPosition)
        {
            this.gridPosition = gridPosition;
            position = new Vector2(levelPosition.X + (gridPosition.X * 40 * Game1.displayRatio.X), levelPosition.Y - (gridPosition.Y * 40 * Game1.displayRatio.Y));
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position)
        {
            float spacingx = 40 * Game1.displayRatio.X;
            if (spacingx != Math.Truncate(spacingx))
            {
                spacingx++;
                spacingx = (float)Math.Truncate(spacingx);
            }

            float spacingy = 40 * Game1.displayRatio.Y;
            if (spacingy != Math.Truncate(spacingy))
            {
                spacingy++;
                spacingy = (float)Math.Truncate(spacingy);
            }

            spriteBatch.Draw(blockColor.texture, new Rectangle((int)Math.Round(position.X), (int)Math.Round(position.Y), (int)spacingx, (int)spacingy), Color.White);
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Rectangle position)
        {
            spriteBatch.Draw(blockColor.texture, position, Color.White);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position, float alpha)
        {
            float spacingx = 40 * Game1.displayRatio.X;
            if (spacingx != Math.Truncate(spacingx))
            {
                spacingx++;
                spacingx = (float)Math.Truncate(spacingx);
            }

            float spacingy = 40 * Game1.displayRatio.Y;
            if (spacingy != Math.Truncate(spacingy))
            {
                spacingy++;
                spacingy = (float)Math.Truncate(spacingy);
            }

            spriteBatch.Draw(blockColor.texture, new Rectangle((int)Math.Round(position.X), (int)Math.Round(position.Y), (int)spacingx, (int)spacingy), Color.White);
        }
        public void SetColor(BlockColor blockColor)
        {
            this.blockColor = blockColor;
        }

        public void SetColor(int colorIndex)
        {
            this.blockColor = BlockColors.AllBlocks[colorIndex];
        }
    }
}
