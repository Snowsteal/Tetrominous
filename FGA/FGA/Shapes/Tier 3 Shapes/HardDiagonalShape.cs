using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace FGA.Shapes
{
    class HardDiagonalShape : Shape
    {
        public override string Name
        {
            get
            {
                return "Hard Diagonal Shape";
            }
        }

        public HardDiagonalShape()
            : base(4)
        {
            //
            //        []
            //      []
            //    []
            //  []
            //

            size = new Vector2(4, 4);

            //1st state, pointed left
            stateRelativePositions[0].Add(new Vector2(1, 1));
            stateRelativePositions[0].Add(new Vector2(-1, -1));
            stateRelativePositions[0].Add(new Vector2(-2, -2));

            //2nd state, pointed right
            stateRelativePositions.Add(new List<Vector2>());
            stateRelativePositions[1].Add(new Vector2(0, 0));
            stateRelativePositions[1].Add(new Vector2(-1, 1));
            stateRelativePositions[1].Add(new Vector2(1, -1));
            stateRelativePositions[1].Add(new Vector2(2, -2));

            maxIndex = 1;
            gridPosition = new Vector2(6, 20);


        }
    }
}
