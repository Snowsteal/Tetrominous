using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FGA.Backgrounds.Background_Objects
{
    class BackgroundObject_PointToPointTeleport : BackgroundObject_PointToPoint
    {
        public BackgroundObject_PointToPointTeleport(Texture2D texture, Vector2 p1, Vector2 p2, float speed)
            : base(texture, p1, p2, speed)
        {
        }

        protected override void EndReached()
        {
            //reset position in case it was off by a little
            if (Vector2.DistanceSquared(position, p1) < Vector2.DistanceSquared(position, p2))
                position += p2 - p1;
            else
                position += p1 - p2;
        }
    }
}
