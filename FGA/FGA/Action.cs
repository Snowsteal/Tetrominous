using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace FGA
{
    [Serializable]
    public class Action
    {
        public int damage;
        public int againstArmor;
        public int defense;
        public List<Texture2D> animations;

    }
}
