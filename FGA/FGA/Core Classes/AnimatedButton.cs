using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace FGA
{
    class AnimatedButton : Button
    {
        //Animation values for each button state
        int frameIndex = 0;
        int timeSinceLastFrame = 0;
        int[] milliSecondsPerFrame;
        int[] maxFrames;

        //Returns max frames of current animation
        int MaxFrames
        {
            get
            {
                return maxFrames[(int)buttonState];
            }
        }

        //creates button without resizing it
        public AnimatedButton(Texture2D texture, Vector2 size)
            : this(texture, new Vector2(0, 0), size, 0.0f)
        {
        }

        //constructor for a basic button and resizes it to the specified size
        public AnimatedButton(Texture2D texture, Vector2 position, Vector2 size)
            : this(texture, position, size, 0.0f)
        {
        }

        //constructor for a button with rotational value
        public AnimatedButton(Texture2D texture, Vector2 position, Vector2 size, float rotation)
            : base(texture, position, size, rotation)
        {
            //Set max frames
            maxFrames = new int[3];
            setMaxFrames(texture.Width / (int)size.X, texture.Width / (int)size.X, texture.Width / (int)size.X);

            //Set milliseconds per frame
            milliSecondsPerFrame = new int[3];
            setMillisecondsPerFrame(200, 200, 200);
        }

        public override void Update(GameTime gameTime)
        {
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;

            if (timeSinceLastFrame > milliSecondsPerFrame[(int)buttonState])
            {
                timeSinceLastFrame = 0;
                frameIndex++;

                if (frameIndex >= maxFrames[(int)buttonState])
                    frameIndex = 0;
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (enabled)
                spriteBatch.Draw(texture, buttonBounds,
                    new Rectangle(frameIndex * (int)size.X, (int)buttonState * texture.Height / 3, (int)size.X, texture.Height / 3),
                        Color.White, rotation, Vector2.Zero,
                        SpriteEffects.None, 1.0f);
        }

        public void setMaxFrames(int maxIdleFrames, int maxSelectedFrames, int maxClickedFrames)
        {
            maxFrames[0] = maxIdleFrames;
            maxFrames[1] = maxSelectedFrames;
            maxFrames[2] = maxClickedFrames;
        }

        public void setMillisecondsPerFrame(int time1, int time2, int time3)
        {
            milliSecondsPerFrame[0] = time1;
            milliSecondsPerFrame[1] = time2;
            milliSecondsPerFrame[2] = time3;
        }
    }
}
