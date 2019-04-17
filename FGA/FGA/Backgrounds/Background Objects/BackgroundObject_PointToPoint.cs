using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FGA.Backgrounds.Background_Objects
{
    /// <summary>
    /// A an object that moves back and forth between two points
    /// </summary>
    class BackgroundObject_PointToPoint : BackgroundObject_Line
    {
        protected Vector2 p1, p2;
        protected float speed;

        //Override velocity so that it's calculated from the points and speed
        //this way the object will always travel between the two points
        new public Vector2 velocity
        {
            get
            {
                Vector2 normal = p1 - p2;
                normal.Normalize();

                return normal * speed;
            }
        }

        public BackgroundObject_PointToPoint(Texture2D texture, Vector2 p1, Vector2 p2, float speed)
            : base(texture, p1)
        {
            this.p1 = p1;
            this.p2 = p2;
            this.speed = speed;
            UpdateVelocity();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //determine if the position is outside of the two points 
            bool xBound = (position.X >= p1.X && position.X >= p2.X) || (position.X <= p1.X && position.X <= p2.X);
            bool yBound = (position.Y >= p1.Y && position.Y >= p2.Y) || (position.Y <= p1.Y && position.Y <= p2.Y);
            if (xBound && yBound)
                EndReached();
        }

        protected virtual void EndReached()
        {
            //reverse direction
            speed *= -1;
            UpdateVelocity();

            //reset position in case it was off by a little
            if (Vector2.DistanceSquared(position, p1) < Vector2.DistanceSquared(position, p2))
                position = p1;
            else
                position = p2;
        }

        private void UpdateVelocity()
        {
            base.velocity = this.velocity;
        }
    }
}
