using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace FGA.Backgrounds
{
    class LineEmUpBackground : Background
    {
        public LineEmUpBackground()
        {
            backgroundTexture = Game1.GlobalContent.Load<Texture2D>(@"Background Screens/LineEmUpScreen");
        }
    }
}
