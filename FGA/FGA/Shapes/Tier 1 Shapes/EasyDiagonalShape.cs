using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace FGA.Shapes
{
    class EasyDiagonalShape : Shape
    {
        public override string Name
        {
            get
            {
                return "Easy Diagonal Shape";
            }
        }

        public EasyDiagonalShape()//2 diagonal
            : base(2)
        {
            //
            //    []
            //  []
            //

            size = new Vector2(2, 2);

            //1st state pointed up/right
            stateRelativePositions[0].Add(new Vector2(-1, -1));

            //2nd state, pointed down/left
            stateRelativePositions.Add(new List<Vector2>());
            stateRelativePositions[1].Add(new Vector2(0, 0));
            stateRelativePositions[1].Add(new Vector2(1, -1));

            maxIndex = 1;
        }

    }
}
