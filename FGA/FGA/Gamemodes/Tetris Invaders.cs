using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FGA.Shapes;

namespace FGA
{
    class Tetris_Invaders : Level
    {
        static Vector2 position2 = new Vector2(400, 200);
        SpriteFont font;

        public Tetris_Invaders(PlayerIndex player)
            : base(player)
        {
            font = Game1.GlobalContent.Load<SpriteFont>("Textures/ourFont");
        }

        protected override void UpdateInGame(GameTime gameTime)
        {
            
        }

        protected override void DrawInGame(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, "abcdefghijklmnopqrstuvwxyz1234567890!#$ ", new Vector2(500, 600), Color.White);
        }
    }
}
