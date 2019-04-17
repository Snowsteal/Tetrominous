using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace FGA.TetrisFactory.Upgrades
{
    class Increase_Block_Tier : Upgrade
    {
        public int looktoFactoryList;
        public Increase_Block_Tier(Texture2D texture, string name, int index, ulong cost, int factoryListIndex)
            : base(texture, name, index, cost)
        {
            looktoFactoryList = factoryListIndex;
            purchased = false;
        }
        public override void Update(GameTime gameTime, Tetris_Factory tetrisFactory)
        {
            base.Update(gameTime, tetrisFactory);
        }
        protected override void Bought(Tetris_Factory tetrisFactory)
        {
            if (tetrisFactory.cash >= cost)
            {
                base.Bought(tetrisFactory);
                if (tetrisFactory.factoryList[looktoFactoryList].blockTier < 7)
                {
                    cost += (ulong)(150 + cost * .25);
                    tetrisFactory.factoryList[looktoFactoryList].blockTier++;
                }
                if (tetrisFactory.factoryList[looktoFactoryList].blockTier == 7)
                    button.enabled = false;
            }
        }
    }
}
