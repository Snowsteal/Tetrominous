using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace FGA.Backgrounds
{
    class BackgroundObject_Line : BackgroundObject
    {
        public Vector2 velocity;
        public float angularSpeed;

        public BackgroundObject_Line(Texture2D texture, Vector2 position)
            : this(texture, position, Vector2.Zero, 0f, 0f)
        {
        }

        public BackgroundObject_Line(Texture2D texture, Vector2 position, Vector2 velocity, float rotation, float angularSpeed)
            : base(texture, position, rotation)
        {
            this.velocity = velocity;
            this.angularSpeed = angularSpeed;
        }

        public override void Update(GameTime gameTime)
        {
            position += (velocity * gameTime.ElapsedGameTime.Milliseconds / 1000);
            rotation += (angularSpeed * gameTime.ElapsedGameTime.Milliseconds / 1000);
        }

        public static Vector2 CalculateChangeInPosition(GameTime gameTime, Vector2 velocity)
        {
            return velocity / 1000 * (float)gameTime.ElapsedGameTime.Milliseconds;
        }
    }
}
