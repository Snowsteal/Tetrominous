using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FGA.Shapes
{
    class SShape : Shape
    {
        public override string Name
        {
            get
            {
                return "SShape";
            }
        }

        public SShape()
            : base(4)
        {
            size = new Vector2(3, 2);

            //1st state, horizontal, origin point has already been added
            stateRelativePositions[0].Add(new Vector2(1, 0));
            stateRelativePositions[0].Add(new Vector2(0, -1));
            stateRelativePositions[0].Add(new Vector2(-1, -1));

            //2nd state, vertical
            stateRelativePositions.Add(new List<Vector2>());
            stateRelativePositions[1].Add(new Vector2(0, 0));
            stateRelativePositions[1].Add(new Vector2(0, 1));
            stateRelativePositions[1].Add(new Vector2(1, 0));
            stateRelativePositions[1].Add(new Vector2(1, -1));

            maxIndex = 1;
        }

        public override bool Rotate(List<List<Block>> grid)
        {
            //Determine whether the shape rotated or not
            bool check = base.Rotate(grid);

            if (check && currentShapeIndex % 2 == 0)
            {
                SpecialRotation(this);
            }

            return check;
        }

        public override void SpecialRotation(Shape shape)
        {
            Block temp;

            //swap the colors of the blocks around
            for (int i = 0; i < shape.blocks.Count / 2; i++)
            {
                temp = shape.blocks[i];
                shape.blocks[i] = shape.blocks[i + 2];
                shape.blocks[i + 2] = temp;
            }
        }
    }
}
