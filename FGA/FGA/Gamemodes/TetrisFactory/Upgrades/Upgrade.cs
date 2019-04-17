using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FGA.TetrisFactory
{
    class Upgrade : Object
    {
        public Button button;
        protected string name;
        protected int index;
        public ulong cost;
        public bool purchased;

        public Upgrade(Texture2D texture, string name, int index, ulong cost)
        {
            button = new Button(texture);
            this.name = name;
            this.index = index;
            this.cost = cost;
        }

        public virtual void Update(GameTime gameTime, Tetris_Factory tetrisFactory)
        {
            button.Update(gameTime);

            if (button.IsClicked)
                Bought(tetrisFactory);
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            button.Draw(spriteBatch);
        }

        protected virtual void Bought(Tetris_Factory tetrisFactory)
        {
            if(tetrisFactory.cash >= cost)
                tetrisFactory.cash -= cost;
        }
    }
}
