using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FGA.Shapes;
using System.Net.Sockets;
using System.Threading;
using FGA.Online;

namespace FGA
{
    class Tug_Of_War : Level
    {
        //Amount of rows the oponent has sent to this side
        public int frozenBlocks = 0;

        //Hold
        Shape holdShape = null;
        bool switched = false;      // Can only switch once, this variable keep track of that

        public Tug_Of_War(PlayerIndex player)
            : base(player)
        {
            if (player == PlayerIndex.One)
            {
                position = new Vector2(289, 954) * Game1.displayRatio;
                scoreDisplayPosition = new Vector2(705, 470) * Game1.displayRatio;
                nextBlockDisplayPosition = new Vector2(520, -703) * Game1.displayRatio;
            }
            else if (player == PlayerIndex.Two)
            {
                position = new Vector2(1250, 954) * Game1.displayRatio;
                scoreDisplayPosition = new Vector2(960, 470) * Game1.displayRatio;
                nextBlockDisplayPosition = new Vector2(-175, -703) * Game1.displayRatio;
            }

        }

        public Tug_Of_War(NetworkStream netStream)
            : base(netStream)
        {
            /*
             * 
                    levelP1.scoreDisplayPosition = new Vector2(655, 470) * Game1.displayRatio;
                    levelP1.nextBlockDisplayPosition = new Vector2(685, -673) * Game1.displayRatio;
             * 
             * 
            enemyLevel.scoreDisplayPosition = new Vector2(1060, 470) * Game1.displayRatio;
            enemyLevel.nextBlockDisplayPosition = new Vector2(-325, -673) * Game1.displayRatio;
             * */
        }

        protected override void DrawInGame(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.DrawInGame(gameTime, spriteBatch);

            if (holdShape != null)
            {
                holdShape.Draw_Next_Block(gameTime, spriteBatch, position + nextBlockDisplayPosition + new Vector2(0, 555) * Game1.displayRatio);
            }
        }

        protected override void AddShapeToGrid()
        {
            switched = false;

            base.AddShapeToGrid();
        }

        protected override int CheckCompletedRows()
        {
            int linesCompletedTogether = 0;
            bool isCompleted;

            for (int i = frozenBlocks; i < grid.Count; i++)
            {
                isCompleted = true;

                //Go through each block and make sure they are all enabled
                for (int j = 0; j < grid[i].Count; j++)
                {
                    if (!grid[i][j].enabled)
                    {
                        isCompleted = false;
                    }
                }

                //Remove row and add new one if row is completed
                if (isCompleted)
                {
                    linesCompleted++;
                    linesCompletedTogether++;
                    grid.RemoveAt(i);
                    GenerateNewRow();
                    i--;
                }
            }

            //Calculate Score
            score += (int)Math.Max(0.0, 1000 * linesCompletedTogether + (500 * (linesCompletedTogether - 1)));

            if (linesCompletedTogether != 0)
            {
                gameClassReference.OnLinesCompleted(player, linesCompletedTogether);

                RemoveFrozenBlocks(1 + linesCompletedTogether / 3);

                UpdateIndicator();
            }

            return linesCompletedTogether;
        }

        protected override void ResetLevel()
        {
            base.ResetLevel();
            frozenBlocks = 0;
        }

        public void HoldShape()
        {
            if (!switched)
            {
                if (holdShape == null)
                {
                    holdShape = currentFallingShape;
                    NextFallingShape();
                }
                else
                {
                    Shape tempShape = currentFallingShape;
                    currentFallingShape = holdShape;
                    holdShape = tempShape;
                    tempShape = null;
                }

                switched = true;
                holdShape.gridPosition = new Vector2(6, 19);
                holdShape.currentShapeIndex = 0;
                SetIndicator();
            }
        }

        //Adds new number of frozen blocks to current level
        public void AddFrozenBlocks(int numNewFrozenBlocks)
        {
            //numNewFrozenBlocks = 10;

            //Add new blocks to counting number
            frozenBlocks += numNewFrozenBlocks;

            //Check enemy for anyblocks that will be outside of bounds if grid is moved up
            for (int i = grid.Count - numNewFrozenBlocks - 1; i < grid.Count; i++)
            {
                for (int j = 0; j < grid[i].Count; j++)
                {
                    //set loss to true if blocks will be moved out
                    if (grid[i][j].enabled)
                        Lose();
                }
            }

            grid.RemoveRange(grid.Count - numNewFrozenBlocks - 1, numNewFrozenBlocks);

            //Reverse order of all elements to make adding new rows easier
            grid.Reverse(0, grid.Count);

            //Add new frozen blocks
            for (int i = 0; i < numNewFrozenBlocks; i++)
            {
                grid.Add(new List<Block>());
                for (int j = 0; j < grid[i].Count; j++)
                {
                    int index1 = grid.Count - 1;
                    grid[index1].Add(new Block(true));
                    grid[index1][grid[index1].Count - 1].SetColor(BlockColors.GreyColor);
                }
            }

            //Unreverse everything
            grid.Reverse(0, grid.Count);

            //Check the current falling shape and see if it'll be stuck inside the new frozen rows
            float minY = currentFallingShape.stateRelativePositions[currentFallingShape.currentShapeIndex].Min(Vector2 => Vector2.Y);
            if (currentFallingShape.CheckLoss(grid))
            {
                int yTransformation = 1;
                //keep trying to move the falling shape up until there's nothing on it anymore
                while(!currentFallingShape.Transform(new Vector2(0, yTransformation), grid))
                {
                    yTransformation++;
                    //if the current falling shape moves above the grid, the player loses
                    if (currentFallingShape.gridPosition.Y + yTransformation > grid.Count)
                        Lose();
                }
            }

            UpdateIndicator();
        }

        //Removes number of frozen blocks tocurrent level
        public void RemoveFrozenBlocks(int numFrozenBlocksToRemove)
        {
            if (frozenBlocks <= numFrozenBlocksToRemove)
            {
                numFrozenBlocksToRemove = frozenBlocks;
            }

            //Remove blocks from counting number
            frozenBlocks -= numFrozenBlocksToRemove;

            for (int i = 0; i < numFrozenBlocksToRemove; i++)
            {
                grid.RemoveAt(0);
                GenerateNewRow();
            }

            UpdateIndicator();
        }
    }
}
