using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace FGA.Backgrounds
{
    class MainMenuBackground : Background
    {

        //Side Tiangles
        Texture2D triangle;
        float maxSpeed;
        float rightYPos, leftYPos;
        float rightVel, leftVel;
        float rightAccel, leftAccel;
        float triangleSpacing = 15f * Game1.displayRatio.Y;       //15 pixels of spacing between each triangle

        public MainMenuBackground()
        {
            //Load in fingers
            backgroundTexture = Game1.GlobalContent.Load<Texture2D>(@"Background Screens/Title Screen/TitleScreen");
            BackgroundObject_Oscillation redFinger = new BackgroundObject_Oscillation(Game1.GlobalContent.Load<Texture2D>(@"Background Screens/Title Screen/Red Middle Finger"), new Vector2(250, 610) * Game1.displayRatio, true, 2000d, 0d, 5f * Game1.displayRatio.Y);
            BackgroundObject_Oscillation redFingerUpsideDown = new BackgroundObject_Oscillation(Game1.GlobalContent.Load<Texture2D>(@"Background Screens/Title Screen/Red Middle Finger"), new Vector2(250, -30) * Game1.displayRatio, true, 2000d, 200d, 5f * Game1.displayRatio.Y);
            redFingerUpsideDown.spriteEffect = SpriteEffects.FlipVertically;
            BackgroundObject_Oscillation blueFinger = new BackgroundObject_Oscillation(Game1.GlobalContent.Load<Texture2D>(@"Background Screens/Title Screen/Blue Middle Finger"), new Vector2(1200, 610) * Game1.displayRatio, true, 2000d, 600d, 5f * Game1.displayRatio.Y);
            BackgroundObject_Oscillation blueFingerUpsideDown = new BackgroundObject_Oscillation(Game1.GlobalContent.Load<Texture2D>(@"Background Screens/Title Screen/Blue Middle Finger"), new Vector2(1200, -30) * Game1.displayRatio, true, 2000d, 1500d, 5f * Game1.displayRatio.Y);
            blueFingerUpsideDown.spriteEffect = SpriteEffects.FlipVertically;
            backgroundObjects.Add(redFinger);
            backgroundObjects.Add(redFingerUpsideDown);
            backgroundObjects.Add(blueFinger);
            backgroundObjects.Add(blueFingerUpsideDown);

            //Load in triangle
            triangle = Game1.GlobalContent.Load<Texture2D>(@"Background Screens/Title Screen/Triangle");
            maxSpeed = 1000f * Game1.displayRatio.Y;
            rightAccel = 200 * Game1.displayRatio.Y;
            leftAccel = -200 * Game1.displayRatio.Y;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            UpdateSideTriangles(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            //Draw each sides triangles
            //Left
            for (float yPos = leftYPos; yPos < 1080f * Game1.displayRatio.Y; yPos += triangle.Height * Game1.displayRatio.Y + triangleSpacing)
            {
                spriteBatch.Draw(triangle, new Rectangle(0, (int)yPos, (int)(triangle.Width * Game1.displayRatio.X), (int)(triangle.Height * Game1.displayRatio.Y)), Color.White);
            }
            //Right
            for (float yPos = rightYPos; yPos < 1080f * Game1.displayRatio.Y; yPos += triangle.Height * Game1.displayRatio.Y + triangleSpacing)
            {
                //spriteBatch.Draw(triangle, new Vector2((1920 - triangle.Width) * Game1.displayRatio.X, yPos), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.FlipHorizontally, 0);
                spriteBatch.Draw(triangle, new Rectangle((int)((1920 - triangle.Width) * Game1.displayRatio.X), (int)(yPos), (int)(triangle.Width * Game1.displayRatio.X), (int)(triangle.Height * Game1.displayRatio.Y)), null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
            }
        }

        private void UpdateSideTriangles(GameTime gameTime)
        {
            //Right
            //Update Acceleration
            if (Math.Abs(rightVel) == maxSpeed && Game1.rnd.NextDouble() < 0.01)
                rightAccel *= -1;
            //Update velocity then position
            rightVel = MathHelper.Clamp(rightVel += rightAccel * gameTime.ElapsedGameTime.Milliseconds / 1000f, -1 * maxSpeed, maxSpeed);
            rightYPos += rightVel * gameTime.ElapsedGameTime.Milliseconds / 1000f;
            //Contain the position within drawing limits
            if (rightYPos > 0)
                rightYPos -= triangle.Height * Game1.displayRatio.Y + triangleSpacing;
            if (rightYPos <= triangle.Height * -1)
                rightYPos += triangle.Height * Game1.displayRatio.Y + triangleSpacing;

            //Left
            //Update Acceleration
            if (Math.Abs(leftVel) == maxSpeed && Game1.rnd.NextDouble() < 0.1)
                leftAccel *= -1;
            //Update velocity then position
            leftVel = MathHelper.Clamp(leftVel += leftAccel * gameTime.ElapsedGameTime.Milliseconds / 1000f, -1 * maxSpeed,maxSpeed);
            leftYPos += leftVel * gameTime.ElapsedGameTime.Milliseconds / 1000f;
            //Contain the position within drawing limits
            if (leftYPos > 0)
                leftYPos -= triangle.Height * Game1.displayRatio.Y + triangleSpacing;
            if (leftYPos <= triangle.Height * -1)
                leftYPos += triangle.Height * Game1.displayRatio.Y + triangleSpacing;
        }
    }
}
