using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace FGA.Backgrounds
{
    public class BackgroundObject : Object
    {
        protected float rotation;
        public SpriteEffects spriteEffect;

        public BackgroundObject(Texture2D texture, Vector2 position)
            : this(texture, position, 0f)
        {
        }

        public BackgroundObject(Texture2D texture, Vector2 position, float rotation)
        {
            this.texture = texture;
            this.position = position;
            this.rotation = rotation;
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Update(GameTime gameTime, Vector2 position, float rotation)
        {
            this.position = position;
            this.rotation = rotation;
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(texture, position, null, Color.White, rotation, new Vector2(0, 0), 1.0f, spriteEffects, 0);
            spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, (int)(texture.Width * Game1.displayRatio.X), (int)(texture.Height * Game1.displayRatio.Y)), null, Color.White, rotation, Vector2.Zero, spriteEffect, 0f);
        }
    }
}
