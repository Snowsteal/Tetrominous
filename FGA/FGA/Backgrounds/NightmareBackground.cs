using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace FGA.Backgrounds
{
    class NightmareBackground : Background
    {
        public NightmareBackground()
        {
            backgroundTexture = Game1.GlobalContent.Load<Texture2D>(@"Background Screens/SpookyScreen");
        }
    }
}
