using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace FGA.Backgrounds
{
    class SplashBackground : Background
    {
        Texture2D opacity_Effect;
        int splashTimer;

        public SplashBackground()
        {
            backgroundTexture = Game1.GlobalContent.Load<Texture2D>(@"Background Screens/SplashScreen/SplashScreen");
            opacity_Effect = Game1.GlobalContent.Load<Texture2D>(@"Background Screens/SplashScreen/Black_Square");
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);

            splashTimer += gameTime.ElapsedGameTime.Milliseconds;
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            float alpha = 1 - ((float)splashTimer / 1000f);
            spriteBatch.Draw(opacity_Effect, new Rectangle(0, 0, (int)(1920 * Game1.displayRatio.X), (int)(1080 * Game1.displayRatio.Y)), new Color(255, 255, 255, alpha));
        }
    }
}
