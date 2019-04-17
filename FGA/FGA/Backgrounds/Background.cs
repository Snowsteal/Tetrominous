using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FGA.Backgrounds
{
    public class Background
    {
        protected Texture2D backgroundTexture;
        protected List<BackgroundObject> backgroundObjects = new List<BackgroundObject>();

        public Background()
        {

        }

        public virtual void Update(GameTime gameTime)
        {
            for (int i = 0; i < backgroundObjects.Count; i++)
            {
                backgroundObjects[i].Update(gameTime);
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backgroundTexture, Game1.GameGraphicsDevice.Viewport.Bounds, Color.White);

            for (int i = 0; i < backgroundObjects.Count; i++)
            {
                backgroundObjects[i].Draw(gameTime, spriteBatch);
            }
        }
    }
}
