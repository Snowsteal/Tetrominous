using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FGA.TetrisFactory;

namespace FGA.TetrisFactory.Upgrades
{
    class NewBlockFactory : Upgrade
    {
        public NewBlockFactory(Texture2D texture, string name, int index, ulong cost)
            : base(texture, name, index, cost)
        {
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
                tetrisFactory.factoryList.Add(new Block_Factory());
                tetrisFactory.AddEmptyLayer();
                tetrisFactory.AddAutoGridRow();
                button.enabled = false;
                tetrisFactory.showNextBlockFactoryUpgrade = true;
                purchased = true;
            }
            else
                tetrisFactory.showNextBlockFactoryUpgrade = false;
        }

    }
}
