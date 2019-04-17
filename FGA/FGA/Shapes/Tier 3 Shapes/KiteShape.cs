using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FGA.Shapes;
using Microsoft.Xna.Framework;

namespace FGA.Shapes
{
    class KiteShape : Shape
    {
        public override string Name
        {
            get
            {
                return "Kite Shape";
            }
        }

        public KiteShape()
            :base(6)
        {
            //
            //    []
            //    [][]
            //  [][]
            //    []
            //

            size = new Vector2(3, 4);

            //1st state, pointed up/down
            stateRelativePositions[0].Add(new Vector2(1, 0));
            stateRelativePositions[0].Add(new Vector2(0, 1));
            stateRelativePositions[0].Add(new Vector2(0, -1));
            stateRelativePositions[0].Add(new Vector2(0, -2));
            stateRelativePositions[0].Add(new Vector2(-1, -1));

            //2nd state, pointed left/right
            stateRelativePositions.Add(new List<Vector2>());
            stateRelativePositions[1].Add(new Vector2(0, 0));
            stateRelativePositions[1].Add(new Vector2(1, 0));
            stateRelativePositions[1].Add(new Vector2(-1, 0));
            stateRelativePositions[1].Add(new Vector2(-2, 0));
            stateRelativePositions[1].Add(new Vector2(0, -1));
            stateRelativePositions[1].Add(new Vector2(-1, 1));

            maxIndex = 1;
            gridPosition = new Vector2(6, 20);


        }
    }
}
