using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FGA.Shapes
{
    class ShapeShiftingShape : Shape
    {
        public override string Name
        {
            get
            {
                return "Shape Shifting Shape";
            }
        }

        public ShapeShiftingShape()
            : base(8)
        {
            size = new Vector2(4, 4);
            //Make some shapes
            int Num;
            Shape Temp;
            int key;

            //I've changed it so that instead of creating 4 different shapes and keeping track of which rotation state that shape
            //for a form, it now just keeps that states inside the shape shifting shape's relative state positions
            //since it is a dynamic array, we're allowed to have different amounts of state positions for each rotation state
            //this also means that we can use it with the default indicator functions
            stateRelativePositions.Clear();
            for (int i = 0; i < 4; i++)
            {
                Temp = null;
                Num = Game1.rnd.Next(0, 4);

                switch (Num)
                {
                    case 0: Temp = new KiteShape();
                        break;
                    case 1: Temp = new HookShape();
                        break;
                    case 2: Temp = new TonfuShape();
                        break;
                    case 3: Temp = new HardDiagonalShape();
                        break;
                }

                key = Game1.rnd.Next(0, Temp.stateRelativePositions.Count);
                stateRelativePositions.Add(Temp.stateRelativePositions[key]);
            }

            //If the first rotation state is out of bounds, push it back in
            int max = (int)stateRelativePositions[currentShapeIndex].Max(Vector2 => Vector2.Y);
            if(gridPosition.Y + max > 21)
                gridPosition.Y += 21 - (gridPosition.Y + max);

            SetBlockColors(BlockColors.ShapeShiftingColor);
            maxIndex = 3;
        }

        public override bool Rotate(List<List<Block>> grid)
        {
            return base.Rotate(grid);
        }
    }
}
