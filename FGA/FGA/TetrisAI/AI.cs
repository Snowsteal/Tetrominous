using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FGA.Shapes;

namespace FGA.TetrisAI
{
    class AI : Level
    {

        public float holeMultiplier, bumpinessMultiplier, linesMultiplier, heightMultiplier;
        public class Multipliers
        {

            public float HoleMultiplier, BumpinessMultiplier, LinesMultiplier, HeightMultiplier, score;

            public Multipliers(Multipliers Copy_Class)
            {
                this.BumpinessMultiplier = Copy_Class.BumpinessMultiplier;
                this.HeightMultiplier = Copy_Class.HeightMultiplier;
                this.HoleMultiplier = Copy_Class.HoleMultiplier;
                this.LinesMultiplier = Copy_Class.LinesMultiplier;
                this.score = Copy_Class.score;
            }
            public Multipliers()
            {

            }

        }
        List<Multipliers> Mult_Values = new List<Multipliers>(0);
        Random RNG = new Random();
        
        public struct DecisionBlock
        {
            public bool determined; //True if a grid location has already been checked for its possibleRotations
            public bool accessable; //True if a surrounding block that is deemed accessable can transfer a block of the same state over to this one

            /*Clarification on viable and possible rotations:
             * 
             *For a given origin point, a block can exist in none to all of its rotation states, denoted by possibleRotations. If the block is deemed accessable, then at least some of these are viable
             *However, if the block has 4 possible rotation states, it is possible for there to be a gap between possible rotation states, e.g. State 1 and 3 are possible, but 2 isn't.
             *Since you have to go through state 2 to get to state 3, state 3 may not be viable. If it is accessible from state 3 of another position, then it is. However if the block is
             *accessible through state one only, then state 3 is not a viable rotation and should not be considered when deciding places that the block can be placed.
             *If a rotation state is accessible, then all of the following consecutive rotation states become viable as well.
             *
             */
            public List<int> viableRotations;
            public List<int> possibleRotations;
        }

        protected List<List<DecisionBlock>> decisionGrid = new List<List<DecisionBlock>>();

        public AI(int startingLevel, PlayerIndex player, Vector2 position)
            : base(player)
        {
            for (int i = 0; i < grid.Count; i++)
                AddNewDecisionRow();

            for (int Q = 0; Q < 8; Q++)
            {
                Mult_Values.Add(new Multipliers());
                Mult_Values[Q].BumpinessMultiplier = ((float)(RNG.NextDouble() * 2 - 1));
                Mult_Values[Q].HeightMultiplier = ((float)(RNG.NextDouble() * 2 - 1));
                Mult_Values[Q].HoleMultiplier = ((float)(RNG.NextDouble() * 2 - 1));
                Mult_Values[Q].LinesMultiplier = ((float)(RNG.NextDouble() * 2 - 1));
            }

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Evaluate(currentFallingShape);
            Mult_Values = Generate_New_Generation(Mult_Values);
        }

        public virtual float Evaluate(Shape tempPlacementShape)
        {
            //aggregate height and Bumpiness
            int Previous_Row_Height = 0;
            int Row_Height = 0;
            int Aggregate_Height = 0;
            int Bumpiness = 0;
            int Holes = 0;
            for (int Q = 0; Q < currentFallingShape.blocks.Count; Q++)
            {
                grid[(int)(currentFallingShape.gridPosition.Y + currentFallingShape.stateRelativePositions[currentFallingShape.currentShapeIndex][Q].Y)]
                    [(int)(currentFallingShape.stateRelativePositions[currentFallingShape.currentShapeIndex][Q].X + currentFallingShape.gridPosition.X)].enabled = true;
            }
            for (int Q = 0; Q < grid[0].Count; Q++)
            {
                //start from the top, at (A, Q) and move down and find the first block which will be the height of that row
                for (int A = grid.Count - 1; A > -1; A--)
                {
                    if (Q != 0)
                        Previous_Row_Height = Row_Height;

                    if (Grid[A][Q].enabled)
                    {
                        Row_Height = A+1;
                        for (int Z = A-1; Z > -1; Z--)//starts from the next block down of the row height, and go down to count the holes
                        {
                            if (!Grid[Z][Q].enabled)//if they aint no block therr, add a hole to it
                                Holes++;
                        }
                        break;
                    }
                }
                Aggregate_Height += Row_Height;//calc agg height
                Bumpiness += Math.Abs(Previous_Row_Height - Row_Height);//calc bump
            }

            //now for the heartache, lines completed
            int Lines_Completed = 0;

            //so tha logic that we use goes somethin like this
            //these coords, + the grid's position, will be used to see if that line is complete
            //do so by setting the block "there" by setting the grid positions active
            //if so, add that to a variable
            bool isCompleted;

            for (int i = 0; i < grid.Count; i++)
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
                    Lines_Completed++;
                }
            }
            //set them all back to false so the temp block dissapears
            for (int Q = 0; Q < currentFallingShape.blocks.Count; Q++)
            {
                grid[(int)(currentFallingShape.gridPosition.Y + currentFallingShape.stateRelativePositions[currentFallingShape.currentShapeIndex][Q].Y)]
                    [(int)(currentFallingShape.stateRelativePositions[currentFallingShape.currentShapeIndex][Q].X + currentFallingShape.gridPosition.X)].enabled = false;
            }

            float Key_to_the_Kingdom;
            Key_to_the_Kingdom = Aggregate_Height * heightMultiplier + bumpinessMultiplier * Bumpiness + holeMultiplier * Holes + linesCompleted * linesMultiplier;

            return Key_to_the_Kingdom;
        }
        //creates and returns a new generation of values based on the 4 highest scores of the "parent"
        protected List<Multipliers> Generate_New_Generation(List<Multipliers> Mult_Values)//syntax of calling this should be Mult_Values = Generate_New_Generation(Mult_Values);
        {
            List<Multipliers> Children = new List<Multipliers>();
            Multipliers Temp = new Multipliers();
            //sort the Mult_Values by highest score
            for (int Q = 0; Q < Mult_Values.Count; Q++)
            {
                for (int A = Q + 1; A < Mult_Values.Count; A++)
                {
                    if (Mult_Values[Q].score < Mult_Values[A].score)
                    {
                        Temp = new Multipliers(Mult_Values[Q]);
                        Mult_Values[Q] = new Multipliers(Mult_Values[A]);
                        Mult_Values[A] = new Multipliers(Temp);
                    }
                }
            }//tested, works!

            //A is 0, B is 1, C is 2, D is 3 (elements)
            //So pairing will go AB CD AC BD so that each parent is used twice
            //element wise, the pairs go as 01 23 02 13
            for (int Q = 0; Q < 8; Q++)
                Children.Add(new Multipliers());
            //child 1 and 2, pairing with AB
            for (int Q = 0; Q < 2; Q++)
            {
                Children[Q].HoleMultiplier = setChildValue(Mult_Values[0].HoleMultiplier, Mult_Values[1].HoleMultiplier);
                Children[Q].LinesMultiplier = setChildValue(Mult_Values[0].LinesMultiplier, Mult_Values[1].LinesMultiplier);
                Children[Q].BumpinessMultiplier = setChildValue(Mult_Values[0].BumpinessMultiplier, Mult_Values[1].BumpinessMultiplier);
                Children[Q].HeightMultiplier = setChildValue(Mult_Values[0].HeightMultiplier, Mult_Values[1].HeightMultiplier);
            }
            //child 3 and 4, pairing with CD
            for (int Q = 2; Q < 4; Q++)
            {
                Children[Q].HoleMultiplier = setChildValue(Mult_Values[2].HoleMultiplier, Mult_Values[3].HoleMultiplier);
                Children[Q].LinesMultiplier = setChildValue(Mult_Values[2].LinesMultiplier, Mult_Values[3].LinesMultiplier);
                Children[Q].BumpinessMultiplier = setChildValue(Mult_Values[2].BumpinessMultiplier, Mult_Values[3].BumpinessMultiplier);
                Children[Q].HeightMultiplier = setChildValue(Mult_Values[2].HeightMultiplier, Mult_Values[3].HeightMultiplier);
            }
            //child 5 and 6, pairing with AC
            for (int Q = 4; Q < 6; Q++)
            {
                Children[Q].HoleMultiplier = setChildValue(Mult_Values[0].HoleMultiplier, Mult_Values[2].HoleMultiplier);
                Children[Q].LinesMultiplier = setChildValue(Mult_Values[0].LinesMultiplier, Mult_Values[2].LinesMultiplier);
                Children[Q].BumpinessMultiplier = setChildValue(Mult_Values[0].BumpinessMultiplier, Mult_Values[2].BumpinessMultiplier);
                Children[Q].HeightMultiplier = setChildValue(Mult_Values[0].HeightMultiplier, Mult_Values[2].HeightMultiplier);
            }
            //child 7 and 8, pairing with BD
            for (int Q = 4; Q < 6; Q++)
            {
                Children[Q].HoleMultiplier = setChildValue(Mult_Values[1].HoleMultiplier, Mult_Values[3].HoleMultiplier);
                Children[Q].LinesMultiplier = setChildValue(Mult_Values[1].LinesMultiplier, Mult_Values[3].LinesMultiplier);
                Children[Q].BumpinessMultiplier = setChildValue(Mult_Values[1].BumpinessMultiplier, Mult_Values[3].BumpinessMultiplier);
                Children[Q].HeightMultiplier = setChildValue(Mult_Values[1].HeightMultiplier, Mult_Values[3].HeightMultiplier);
            }
            return Children;
        }

        protected float setChildValue(float Parent1val, float Parent2Val)
        {
            double rand = RNG.NextDouble();
            float value;

            if (rand <= .475)
                value = Parent1val;
            else if (rand <= .95)
                value = Parent2Val;
            else
                value = (float)RNG.NextDouble() * 2 - 1;

            return value;
        }

        //Function for determine and check viability

        //Function for if already determined and recheck viability (only if new rotation states become viable)

        protected virtual void AddNewDecisionRow()
        {
            decisionGrid.Add(new List<DecisionBlock>());
            for (int j = 0; j < 10; j++)
                decisionGrid[decisionGrid.Count - 1].Add(new DecisionBlock());
        }
    }
}
