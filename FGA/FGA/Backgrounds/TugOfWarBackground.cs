using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace FGA.Backgrounds
{
    class TugOfWarBackground : Background
    {
        Texture2D ropeTexture, redBoxTexture, blueBoxTexture;

        public TugOfWarBackground()
        {
            backgroundTexture = Game1.GlobalContent.Load<Texture2D>(@"Background Screens/Tug Of War Screen/TugOfWarScreen");
            ropeTexture = Game1.GlobalContent.Load<Texture2D>(@"Background Screens/Tug Of War Screen/Rope");
            redBoxTexture = Game1.GlobalContent.Load<Texture2D>(@"Background Screens/Tug Of War Screen/Red Box");
            blueBoxTexture = Game1.GlobalContent.Load<Texture2D>(@"Background Screens/Tug Of War Screen/Blue Box");

            BackgroundObject_Oscillation rope = new BackgroundObject_Oscillation(Game1.GlobalContent.Load<Texture2D>(@"Background Screens/Tug Of War Screen/Rope"), new Vector2(0, 474) * Game1.displayRatio, 4000d, 0d, 20f * Game1.displayRatio.X);
            backgroundObjects.Add(rope);
            backgroundObjects.Add(new BackgroundObject(Game1.GlobalContent.Load<Texture2D>(@"Background Screens/Tug Of War Screen/Red Box"), new Vector2(275, 100) * Game1.displayRatio));
            backgroundObjects.Add(new BackgroundObject(Game1.GlobalContent.Load<Texture2D>(@"Background Screens/Tug Of War Screen/Blue Box"), new Vector2(1235, 100) * Game1.displayRatio));
        }
    }
}
