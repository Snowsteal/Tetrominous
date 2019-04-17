using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FGA.Backgrounds
{
    class DoubleTroubleBackground : Background
    {
        public DoubleTroubleBackground()
        {
            backgroundTexture = Game1.GlobalContent.Load<Texture2D>(@"Background Screens/Dual Handjobs Screen/Dual_Handjobs_Screen");
        }
    }
}
