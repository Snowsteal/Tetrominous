using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FGA.Shapes
{
    class LShape : Shape
    {
        public override string Name
        {
            get
            {
                return "LShape";
            }
        }

        public LShape()
            : base(4)
        {
            size = new Vector2(2, 3);

            //1st state pointed down
            stateRelativePositions[0].Add(new Vector2(1, 0));
            stateRelativePositions[0].Add(new Vector2(-1, 0));
            stateRelativePositions[0].Add(new Vector2(-1, -1));

            //2nd state, pointed right
            stateRelativePositions.Add(new List<Vector2>());
            stateRelativePositions[1].Add(new Vector2(0, 0));
            stateRelativePositions[1].Add(new Vector2(0, 1));
            stateRelativePositions[1].Add(new Vector2(0, -1));
            stateRelativePositions[1].Add(new Vector2(1, -1));

            //3rd state, pointed up
            stateRelativePositions.Add(new List<Vector2>());
            stateRelativePositions[2].Add(new Vector2(0, 0));
            stateRelativePositions[2].Add(new Vector2(-1, 0));
            stateRelativePositions[2].Add(new Vector2(1, 0));
            stateRelativePositions[2].Add(new Vector2(1, 1));

            //4th state, pointed left
            stateRelativePositions.Add(new List<Vector2>());
            stateRelativePositions[3].Add(new Vector2(0, 0));
            stateRelativePositions[3].Add(new Vector2(0, -1));
            stateRelativePositions[3].Add(new Vector2(0, 1));
            stateRelativePositions[3].Add(new Vector2(-1, 1));

            maxIndex = 3;
        }

    }
}
