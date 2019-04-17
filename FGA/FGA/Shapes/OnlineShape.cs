using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace FGA.Shapes
{
    class OnlineShape : Shape
    {
        public OnlineShape(Vector2 gridPosition, List<Block> blocks, List<Vector2> stateRelativePositions)
            : base(blocks.Count)
        {
            float maxX = stateRelativePositions.Max(Vector2 => Vector2.X);
            float minX = stateRelativePositions.Min(Vector2 => Vector2.X);
            float maxY = stateRelativePositions.Max(Vector2 => Vector2.Y);
            float minY = stateRelativePositions.Min(Vector2 => Vector2.Y);

            size = new Vector2(maxX - minX, maxY - minY);

            //set position and block textures
            this.gridPosition = gridPosition;
            this.blocks = blocks;

            //set the relative positions of the blocks
            this.stateRelativePositions[0] = new List<Vector2>();
            for (int i = 0; i < stateRelativePositions.Count; i++)
            {
                this.stateRelativePositions[0].Add(stateRelativePositions[i]);
            }

            maxIndex = 0;
        }
    }
}
