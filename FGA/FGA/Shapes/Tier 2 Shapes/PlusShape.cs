using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace FGA.Shapes
{
    class PlusShape : Shape
    {
        public override string Name
        {
            get
            {
                return "Plus Shape";
            }
        }

        public PlusShape()//plus sign
            : base(5)
        {
            //
            //    []
            //  [][][]
            //    []
            //

            size = new Vector2(3, 3);

            //only state
            stateRelativePositions[0].Add(new Vector2(-1, 0));
            stateRelativePositions[0].Add(new Vector2(0, -1));
            stateRelativePositions[0].Add(new Vector2(0, 1));
            stateRelativePositions[0].Add(new Vector2(1, 0));

            maxIndex = 0;
            gridPosition = new Vector2(6, 18);//move it down 1 block in the grid to prevent index out of range errors.
        }

        public override bool Rotate(List<List<Block>> grid)
        {
            //This block does not rotate, so this method does nothing.
            return true;
        }
    }
}
