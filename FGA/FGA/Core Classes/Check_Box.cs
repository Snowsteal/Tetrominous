using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace FGA.Core_Classes
{
    public class Check_Box
    {
        Rectangle Position;
        Texture2D Texture;
        int Index;
        bool freeze_index;
        public bool clicked = false;
        public bool justclick;

        public Check_Box(Rectangle position)
            : this(position, Game1.GlobalContent.Load<Texture2D>(@"Textures/CheckBox"))
        {

        }

        public Check_Box(Rectangle position, Texture2D texture)
        {
            Position = position;
            Texture = texture;
            Index = 0;
        }

        public void IsMouseOver()
        {
            if (Position.Contains(Input.mouseState.X, Input.mouseState.Y))
            {
                if(!freeze_index)
                    Index = 1;
            }
            else
            {
                if(!freeze_index)
                    Index = 0;
            }
        }
        public void CheckClicked()
        {
            if (Position.Contains(Input.mouseState.X, Input.mouseState.Y) && Input.mouseState.LeftButton == ButtonState.Pressed && Input.prevMouseState.LeftButton == ButtonState.Released)
            {
                clicked = !clicked;
                justclick = true;
            }
            if (clicked)
            {
                freeze_index = true;
                Index = 2;
            }
            else if (!clicked)
            {
                freeze_index = false;
                Index = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, new Rectangle(0, ((Texture.Height/3)*Index), Texture.Width, (Texture.Height/3)), Color.White);
        }
    }
}
