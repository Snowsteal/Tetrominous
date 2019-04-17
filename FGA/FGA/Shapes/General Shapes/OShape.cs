using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FGA.Shapes
{
    class OShape : Shape
    {
        public override string Name
        {
            get
            {
                return "OShape";
            }
        }

        public OShape()
            : base(4)
        {
            size = new Vector2(2, 2);

            //only state
            stateRelativePositions[0].Add(new Vector2(0, -1));
            stateRelativePositions[0].Add(new Vector2(-1, -1));
            stateRelativePositions[0].Add(new Vector2(-1, 0));

            maxIndex = 0;
        }

        public override bool Rotate(List<List<Block>> grid)
        {
            //Rotate the colors in the block
            SpecialRotation(this);
            return true;
        }

        public override void SpecialRotation(Shape shape)
        {
            Block temp = shape.blocks[0];
            shape.blocks.RemoveAt(0);
            shape.blocks.Add(temp);
        }

    }
}
