using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FGA.Shapes;
using Microsoft.Xna.Framework.Graphics;

namespace FGA
{
    //You and another player work together to prevent the board from filling
    //up with useless crap, blocks will constantly spawn from the bottom and
    //you guys have to make sure the board doesn't overflow
    //The name dual hand jobs is optional, i'm just super bored right now
    class DoubleTrouble : Level
    {
        //variables for second player
        protected Shape currentFallingShapeP2;
        protected IndicatorShape indicatorShapeP2;
        protected int fallingTimerP2;

        public DoubleTrouble()
            : base(PlayerIndex.One)
        {
            //load in default values
            //scoreDisplayPosition = new Vector2(705, 470) * Game1.displayRatio;
            //nextBlockDisplayPosition = new Vector2(520, -703) * Game1.displayRatio;
            position = new Vector2(361, 1008);

            //Generate additional rows for the bigger field
            for (int i = 0; i < 0; i++)
            {
                GenerateNewRow();
            }

            //Initialize P2 settings
            //Set falling peice and queue peices
            currentFallingShapeP2 = GenerateRandomShape();
            currentFallingShapeP2.gridPosition = new Vector2(24, 21);

            //Setup indicators for the falling shape
            SetIndicatorP2();

            fallingTimerP2 = TimeDelayMilliseconds;
        }

        //protected override void UpdateInput(GameTime gameTime)
        //{
        //    //P1 Inputs
        //    base.UpdateInput(gameTime);

        //    //P2 inputs
        //    //Move left
        //    if (Input.P2Left)
        //    {
        //        if (currentFallingShapeP2.Transform(new Vector2(-1, 0), grid))
        //            UpdateIndicatorP2();
        //    }

        //    //Move right
        //    if (Input.P2Right)
        //    {
        //        if (currentFallingShapeP2.Transform(new Vector2(1, 0), grid))
        //            UpdateIndicatorP2();
        //    }

        //    //Rotate
        //    if (Input.P2Rotate)
        //    {
        //        if (currentFallingShapeP2.Rotate(grid))
        //        {

        //            if (indicatorShapeP2.indicatorShapeIndex == indicatorShape.maxIndicatorIndex)
        //                indicatorShapeP2.indicatorShapeIndex = 0;
        //            else
        //                indicatorShapeP2.indicatorShapeIndex++;

        //            UpdateIndicatorP2();
        //        }
        //    }

        //    //Drop down 1 block
        //    if (Input.P2Drop1)
        //    {
        //        //check movement was valid and don't need to AddShapeToGrid
        //        if (!currentFallingShapeP2.Transform(new Vector2(0, -1), grid))
        //        {
        //            //reset auto drop timer
        //            fallingTimerP2 = TimeDelayMilliseconds;
        //        }
        //    }

        //    //Drop completely down
        //    if (Input.P2DropAll)
        //    {
        //        bool success;

        //        //keep trying to drop down one block until it can't anymore
        //        do
        //        {
        //            success = currentFallingShapeP2.Transform(new Vector2(0, -1), grid);
        //            score += 5;
        //        }
        //        while (success);

        //        AddShapeToGridP2();
        //    }
        //}

        //P2 inputs
        //Move left
        public void MoveLeftP2()
        {
            if (currentFallingShapeP2.Transform(new Vector2(-1, 0), grid))
                UpdateIndicatorP2();
        }

        //Move right
        public void MoveRightP2()
        {
            if (currentFallingShapeP2.Transform(new Vector2(1, 0), grid))
                UpdateIndicatorP2();
        }

        //Rotate
        public void RotateP2()
        {
            if (currentFallingShapeP2.Rotate(grid))
            {

                if (indicatorShapeP2.indicatorShapeIndex == indicatorShape.maxIndicatorIndex)
                    indicatorShapeP2.indicatorShapeIndex = 0;
                else
                    indicatorShapeP2.indicatorShapeIndex++;

                UpdateIndicatorP2();
            }
        }

        //Drop down 1 block
        public void DropOneP2()
        {
            //check movement was valid and don't need to AddShapeToGrid
            if (!currentFallingShapeP2.Transform(new Vector2(0, -1), grid))
            {
                //reset auto drop timer
                fallingTimerP2 = TimeDelayMilliseconds;
            }
        }

        //Drop completely down
        public void DropAllP2()
        {
            bool success;

            //keep trying to drop down one block until it can't anymore
            do
            {
                success = currentFallingShapeP2.Transform(new Vector2(0, -1), grid);
                score += 5;
            }
            while (success);

            AddShapeToGridP2();
        }

        protected override void UpdateFallTimer(GameTime gameTime)
        {
            //P1 Timer
            base.UpdateFallTimer(gameTime);

            //P2 timer
            fallingTimerP2 -= gameTime.ElapsedGameTime.Milliseconds;
            if (fallingTimerP2 <= 0)
            {
                //drop shape down and reset autofall timer
                //check if the shape has stuck the bottom
                if (!currentFallingShapeP2.Transform(new Vector2(0, -1), grid))
                {
                    AddShapeToGridP2();
                }
                fallingTimerP2 = TimeDelayMilliseconds;
            }
        }

        protected override void DrawInGame(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.DrawInGame(gameTime, spriteBatch);

            //Draw player two
            currentFallingShapeP2.DrawToGrid(gameTime, spriteBatch, position);
            //indicator is already drawn by the overwritten draw indicator function
        }

        protected override void DrawIndicator(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.DrawIndicator(gameTime, spriteBatch);
            //Draw P2 indicator
            indicatorShapeP2.Draw(gameTime, spriteBatch, currentFallingShape, position);
        }

        protected override void DrawNextBlock(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //Draw all 4 parts of the queue
            Vector2 offset;

            switch (shapeQueue[0].Name)
            {
                case "IShape":
                    offset = new Vector2(-7, 20) * Game1.displayRatio;
                    break;
                case "OShape":
                    offset = new Vector2(20, 0) * Game1.displayRatio;
                    break;
                default:
                    offset = Vector2.Zero;
                    break;
            }

            shapeQueue[0].Draw_Next_Block(gameTime, spriteBatch, new Vector2(962, 33) * Game1.displayRatio + offset);
        }

        protected override void GenerateNewRow()
        {
            base.GenerateNewRow();

            //Additional blocks for the longer row
            for (int j = 0; j < 20; j++)
                grid[grid.Count - 1].Add(new Block(false));
        }

        protected override Shape GenerateRandomShape()
        {
            Shape randomShape = base.GenerateRandomShape();
            randomShape.gridPosition = new Vector2(6, 21);

            return randomShape;
        }

        protected override void NextFallingShape()
        {
            base.NextFallingShape();

            while (shapeQueue.Count < 4)
            {
                shapeQueue.Add(GenerateRandomShape());
            }
        }

        protected override int CheckCompletedRows()
        {
            int completedRows = base.CheckCompletedRows();

            if (completedRows > 0)
                UpdateIndicatorP2();

            return completedRows;
        }

        public override void SetIndicator()
        {
            base.SetIndicator();
            UpdateIndicatorP2();
        }

        protected virtual void AddShapeToGridP2()
        {
            currentFallingShapeP2.AddShapeToGrid(grid);
            NextFallingShapeP2();
            CheckCompletedRows();
        }

        public virtual void SetIndicatorP2()
        {
            indicatorShapeP2 = new IndicatorShape(currentFallingShapeP2);
            UpdateIndicator();
            UpdateIndicatorP2();
        }

        protected virtual void UpdateIndicatorP2()
        {
            if (indicatorShapeP2 != null)
                indicatorShapeP2.DeterminePosition(currentFallingShapeP2, grid);
        }

        protected virtual void NextFallingShapeP2()
        {
            //Pull next shape from queue and generate new shape for queue
            currentFallingShapeP2 = shapeQueue[0];
            currentFallingShapeP2.gridPosition = new Vector2(24, 21);
            if (currentFallingShapeP2.CheckLoss(grid))
                Lose();
            //loss = currentFallingShapeP2.CheckLoss(grid);
            SetIndicatorP2();
            shapeQueue.RemoveAt(0);
            while (shapeQueue.Count < 4)
            {
                shapeQueue.Add(GenerateRandomShape());
            }

            fallingTimer = TimeDelayMilliseconds;
        }
    }
}
