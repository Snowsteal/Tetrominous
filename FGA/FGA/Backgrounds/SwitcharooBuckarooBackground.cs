using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace FGA.Backgrounds
{
    class SwitcharooBuckarooBackground : Background
    {
        public SwitcharooBuckarooBackground()
        {
            backgroundTexture = Game1.GlobalContent.Load<Texture2D>(@"Background Screens/Switcharoo_Buckaroo_Screen");
        }
    }
}
