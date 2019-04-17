using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace FGA.TetrisFactory.Upgrades
{
    class IncreaseBlockFactorySpeed : Upgrade
    {
        public int lookToFactory;
        public IncreaseBlockFactorySpeed(Texture2D texture, string name, int index, ulong cost, int factoryindex)
            : base(texture, name, index, cost)
        {
            purchased = false;
            lookToFactory = factoryindex;
        }

        protected override void Bought(Tetris_Factory tetrisFactory)
        {
            if (tetrisFactory.cash >= cost)
            {
                base.Bought(tetrisFactory);
                tetrisFactory.factoryList[lookToFactory].Upgrade();
                cost += (ulong)(10 + .05 * cost);
            }
        }
    }
}
