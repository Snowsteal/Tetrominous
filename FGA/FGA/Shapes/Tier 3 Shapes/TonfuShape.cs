using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace FGA.Shapes
{
    class TonfuShape : Shape
    {
        public override string Name
        {
            get
            {
                return "Tonfu Shape";
            }
        }

        public TonfuShape()
            : base(6)
        {
            //
            //       []
            // [][][][]
            //   []
            //

            size = new Vector2(4, 3);

            //1st state, pointed up
            stateRelativePositions[0].Add(new Vector2(-2, 0));
            stateRelativePositions[0].Add(new Vector2(-1, 0));
            stateRelativePositions[0].Add(new Vector2(1, 0));
            stateRelativePositions[0].Add(new Vector2(1, 1));
            stateRelativePositions[0].Add(new Vector2(-1, -1));

            //2nd state, pointed right
            stateRelativePositions.Add(new List<Vector2>());
            stateRelativePositions[1].Add(new Vector2(-1, 0));
            stateRelativePositions[1].Add(new Vector2(-1, 1));
            stateRelativePositions[1].Add(new Vector2(-1, -1));
            stateRelativePositions[1].Add(new Vector2(-1, 2));
            stateRelativePositions[1].Add(new Vector2(-2, 2));
            stateRelativePositions[1].Add(new Vector2(0, 0));

            //2nd state, pointed down
            stateRelativePositions.Add(new List<Vector2>());
            stateRelativePositions[2].Add(new Vector2(-1, 0));
            stateRelativePositions[2].Add(new Vector2(-1, 1));
            stateRelativePositions[2].Add(new Vector2(0, 0));
            stateRelativePositions[2].Add(new Vector2(-2, 0));
            stateRelativePositions[2].Add(new Vector2(-3, 0));
            stateRelativePositions[2].Add(new Vector2(-3, -1));

            //2nd state, pointed left
            stateRelativePositions.Add(new List<Vector2>());
            stateRelativePositions[3].Add(new Vector2(-1, 0));
            stateRelativePositions[3].Add(new Vector2(-1, 1));
            stateRelativePositions[3].Add(new Vector2(-1, -1));
            stateRelativePositions[3].Add(new Vector2(-1, -2));
            stateRelativePositions[3].Add(new Vector2(0, -2));
            stateRelativePositions[3].Add(new Vector2(-2, 0));

            maxIndex = 3;
            gridPosition = new Vector2(6, 20);


        }
    }
}
