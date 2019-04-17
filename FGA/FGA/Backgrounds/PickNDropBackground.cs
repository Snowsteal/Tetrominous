using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace FGA.Backgrounds
{
    class PickNDropBackground : Background
    {
        public PickNDropBackground()
        {
            backgroundTexture = Game1.GlobalContent.Load<Texture2D>(@"Background Screens/Pick_n_drop_screen");
        }
    }
}
