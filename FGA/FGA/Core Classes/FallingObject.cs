using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace FGA
{
    class FallingObject : Object
    {
        Vector2 velocity;

        public FallingObject(Texture2D texture, Vector2 position, Vector2 size, Vector2 velocity)
            : base()
        {
            this.texture = texture;
            this.position = position;
            this.size = size;
            this.velocity = velocity;
        }

        public void Update(GameTime gameTime)
        {
            velocity.Y += 10.0f * gameTime.ElapsedGameTime.Milliseconds / 1000;
            position += velocity;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y), Color.White);
        }
    }
}
