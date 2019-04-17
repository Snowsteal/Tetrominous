using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace FGA
{
    class Animation
    {
        Texture2D texture;
        Vector2 position;
        int numFrames;
        int frameIndex;
        int fps;
        int timeSinceLastFrame;

        Vector2 frameSize
        {
            get
            {
                return new Vector2(texture.Width / numFrames, texture.Height);
            }
        }

        int frameTime
        {
            get
            {
                return 1000 / fps;
            }
        }

        public Animation(Texture2D texture, Vector2 position, int numFrames, int fps)
        {
            this.texture = texture;
            this.position = position;
            this.numFrames = numFrames;
            this.fps = fps;
        }

        public void Update(GameTime gameTime)
        {
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;

            if (timeSinceLastFrame > frameTime)
            {
                timeSinceLastFrame -= frameTime;
                frameIndex++;
                if (frameIndex >= numFrames)
                    frameIndex = 0;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, 
                new Rectangle((int)(position.X * Game1.displayRatio.X), 
                (int)(position.Y * Game1.displayRatio.Y), 
                (int)(frameSize.X * Game1.displayRatio.X), 
                (int)(frameSize.Y * Game1.displayRatio.Y)),
                new Rectangle((int)(frameSize.X * frameIndex), 0, (int)frameSize.X, (int)frameSize.Y),
                Color.White);
        }
    }
}
