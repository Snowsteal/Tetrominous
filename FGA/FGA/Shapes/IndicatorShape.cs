using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FGA.Shapes
{
    class IndicatorShape
    {
        public static Texture2D indicatorTexture;
        public List<IndicatorBlocks> shapes = new List<IndicatorBlocks>();
        public Vector2 position = new Vector2(6, 19);
        public int indicatorShapeIndex = 0;
        public int maxIndicatorIndex;

        public IndicatorShape(Shape parent)
        {
            int i;
            int j;
            int q;
            maxIndicatorIndex = parent.maxIndex;

            //This transfers all of the rotation data from the parent shape into the indicator. This will be used to determine which indicators to use
            for (i = 0; i < parent.stateRelativePositions.Count; i++)
            {
                shapes.Add(new IndicatorBlocks());
                for (int Q = 0; Q < parent.stateRelativePositions[i].Count; Q++)
                {
                    this.shapes[i].stateRelativePositions.Add(new Vector2(parent.stateRelativePositions[i][Q].X, parent.stateRelativePositions[i][Q].Y));
                }
            }

            //Use blocks to hold the location and the texture, but don't enable it so that it doesn't interfere with the falling block
            for (i = 0; i < shapes.Count; i++)
            {
                for (j = 0; j < shapes[i].stateRelativePositions.Count; j++)
                    shapes[i].blocks.Add(new Block(false));
            }

            //These are used to determine where there are surrounding blocks in relation to a specific block. Useful for determining which indicator texture we need
            //True on the bools means that there is a block immediately in that direction
            bool left, right, up, down;
            Vector2 checkUp, checkRight, checkLeft, checkDown;
            Vector2 check;
            int sumIndex = 0;

            for (i = 0; i < shapes.Count; i++)
            {
                for (j = 0; j < shapes[i].blocks.Count; j++)
                {
                    left = false;
                    right = false;
                    up = false;
                    down = false;
                    sumIndex = 0;

                    //Checks will be used to see if a block in the shape exists in that spot. This will determine the texture
                    checkUp = new Vector2(shapes[i].stateRelativePositions[j].X, shapes[i].stateRelativePositions[j].Y + 1);
                    checkLeft = new Vector2(shapes[i].stateRelativePositions[j].X-1, shapes[i].stateRelativePositions[j].Y);
                    checkRight = new Vector2(shapes[i].stateRelativePositions[j].X+1, shapes[i].stateRelativePositions[j].Y);
                    checkDown = new Vector2(shapes[i].stateRelativePositions[j].X, shapes[i].stateRelativePositions[j].Y -1);

                    for (q = 0; q < shapes[i].stateRelativePositions.Count; q++)
                    {
                        check = new Vector2(shapes[i].stateRelativePositions[q].X, shapes[i].stateRelativePositions[q].Y);
                        if (check == checkLeft)
                        {
                            left = true;
                            sumIndex += 4;
                        }
                        if (check == checkRight)
                        {
                            right = true;
                            sumIndex += 1;
                        }
                        if (check == checkUp)
                        {
                            up = true;
                            sumIndex += 2;
                        }
                        if (check == checkDown)
                        {
                            down = true;
                            sumIndex += 8;
                        }
                    }

                    //Determine which texture is needed
                    if (up)
                    {
                        if (down)
                        {
                            if(left && right)
                                shapes[i].blocks[j].blockColor.texture = Game1.GlobalContent.Load<Texture2D>("Indicators\\Empty");
                            else if (left)
                                shapes[i].blocks[j].blockColor.texture = Game1.GlobalContent.Load<Texture2D>("Indicators\\)");
                            else if (right)
                                shapes[i].blocks[j].blockColor.texture = Game1.GlobalContent.Load<Texture2D>("Indicators\\(");
                            else
                                shapes[i].blocks[j].blockColor.texture = Game1.GlobalContent.Load<Texture2D>("Indicators\\ll");
                        }
                        else
                        {
                            if (left && right)
                                shapes[i].blocks[j].blockColor.texture = Game1.GlobalContent.Load<Texture2D>("Indicators\\_");
                            else if (left)
                                shapes[i].blocks[j].blockColor.texture = Game1.GlobalContent.Load<Texture2D>("Indicators\\J");
                            else if (right)
                                shapes[i].blocks[j].blockColor.texture = Game1.GlobalContent.Load<Texture2D>("Indicators\\L");
                            else
                                shapes[i].blocks[j].blockColor.texture = Game1.GlobalContent.Load<Texture2D>("Indicators\\U");
                        }
                    }
                    else
                    {
                        if (left)
                        {
                            if (right && down)
                                shapes[i].blocks[j].blockColor.texture = Game1.GlobalContent.Load<Texture2D>("Indicators\\T-1");
                            else if (down)
                                shapes[i].blocks[j].blockColor.texture = Game1.GlobalContent.Load<Texture2D>("Indicators\\7");
                            else if (right)
                                shapes[i].blocks[j].blockColor.texture = Game1.GlobalContent.Load<Texture2D>("Indicators\\=");
                            else
                                shapes[i].blocks[j].blockColor.texture = Game1.GlobalContent.Load<Texture2D>("Indicators\\]");
                        }
                        else
                        {
                            if(right && down)
                                shapes[i].blocks[j].blockColor.texture = Game1.GlobalContent.Load<Texture2D>("Indicators\\l'''");
                            else if (right)
                                shapes[i].blocks[j].blockColor.texture = Game1.GlobalContent.Load<Texture2D>("Indicators\\[");
                            else if (down)
                                shapes[i].blocks[j].blockColor.texture = Game1.GlobalContent.Load<Texture2D>("Indicators\\n");
                            else
                                shapes[i].blocks[j].blockColor.texture = Game1.GlobalContent.Load<Texture2D>("Indicators\\Square");
                            
                        }
                    }

                }
            }
        }

        public void DeterminePosition(Shape parent, List<List<Block>> grid)
        {
            position = parent.gridPosition;
            indicatorShapeIndex = parent.currentShapeIndex;
            bool stop = false;

            do
            {
                //Check for each block in the shape
                for (int i = 0; i < shapes[indicatorShapeIndex].stateRelativePositions.Count; i++)
                {
                    //determine where the block will be after transforming
                    int xPos = (int)position.X + (int)shapes[indicatorShapeIndex].stateRelativePositions[i].X;
                    int yPos = (int)position.Y + (int)shapes[indicatorShapeIndex].stateRelativePositions[i].Y;

                    //this will check if the index goes out of range
                    try
                    {
                        //grid is rows first then cols
                        if (grid[yPos][xPos].enabled && !stop) //check that there isnt an enabled block blocking the direction
                        {
                            position.Y++;
                            stop = true;
                        }
                        
                            
                    }
                    catch
                    {
                        position.Y++;
                        stop = true;
                    }
                  
                }
                
                if (!stop)
                        position.Y--;
            } while (!stop);

            for (int i = 0; i < shapes[indicatorShapeIndex].stateRelativePositions.Count; i++)
            {
                shapes[indicatorShapeIndex].blocks[i].position = new Vector2(position.X + shapes[indicatorShapeIndex].stateRelativePositions[i].X, position.Y + shapes[indicatorShapeIndex].stateRelativePositions[i].Y);
            }
        }

        public IndicatorShape(Shape parent, bool loose)
        {
            int i;
            int j;
            int q;
            int x = 0;
            Vector2 Temp;
            Texture2D SuperTemp;
            maxIndicatorIndex = 3;

            //There will always be 4 rotations of the indicator, needed for color swapping even though position might not change
            //so we need to add the same state relative positions until we have 4 of them
            for (i = 0; i < 4 / (parent.maxIndex + 1); i++)
            {

                for (j = 0; j < parent.stateRelativePositions.Count; j++)
                {
                    shapes.Add(new IndicatorBlocks());

                    for (q = 0; q < parent.blocks.Count; q++)
                    {
                        this.shapes[x].stateRelativePositions.Add(new Vector2(parent.stateRelativePositions[j][q].X, parent.stateRelativePositions[j][q].Y));
                    }
                    x++;
                }

            }

            //Use blocks to hold the location and the texture, but don't enable it so that it doesn't interfere with the falling block
            for (i = 0; i < shapes.Count; i++)
            {
                for (j = 0; j < 4; j++)
                    shapes[i].blocks.Add(new Block(false));
            }

            //Determine the color of the indicator based off of the parent block's color
            for (i = 0; i < shapes.Count; i++)
            {
                for (j = 0; j < 4; j++)
                {

                    switch (parent.blocks[j].blockColor.color)
                    {
                        case Colors.Red: shapes[i].blocks[j].blockColor.texture = Game1.GlobalContent.Load<Texture2D>("Indicators\\RedIndicator");
                            break;
                        case Colors.Blue: shapes[i].blocks[j].blockColor.texture = Game1.GlobalContent.Load<Texture2D>("Indicators\\BlueIndicator");
                            break;
                        case Colors.Green: shapes[i].blocks[j].blockColor.texture = Game1.GlobalContent.Load<Texture2D>("Indicators\\GreenIndicator");
                            break;
                        case Colors.Purple: shapes[i].blocks[j].blockColor.texture = Game1.GlobalContent.Load<Texture2D>("Indicators\\PurpleIndicator");
                            break;
                        case Colors.Rainbow: shapes[i].blocks[j].blockColor.texture = Game1.GlobalContent.Load<Texture2D>("Indicators\\RainbowIndicator");
                            break;
                        default: shapes[i].blocks[j].blockColor.texture = Game1.GlobalContent.Load<Texture2D>("Indicators\\Square");
                            break;

                    }

                }

                //If that was the last rotation state but we still have more indicator states for the rotation of color, move the colors of the blocks so that the indicator can have the right color
                //Add one so that we dont divide by 0
                if ((i + 1) % (parent.maxIndex + 1) == 0 && parent.maxIndex != 3)
                    parent.SpecialRotation(parent);

                //Sort the relative position vectors from least to greatest. This will be used when determining the position of the indicator, so that we don't accidentally have blocks overlap.
                for (j = 0; j < parent.blocks.Count; j++)
                    for (q = j + 1; q < parent.blocks.Count; q++)
                    {
                        if (shapes[i].stateRelativePositions[j].Y > shapes[i].stateRelativePositions[q].Y)
                        {
                            SuperTemp = shapes[i].blocks[j].blockColor.texture;
                            shapes[i].blocks[j].blockColor.texture = shapes[i].blocks[q].blockColor.texture;
                            shapes[i].blocks[q].blockColor.texture = SuperTemp;

                            Temp = shapes[i].stateRelativePositions[j];
                            shapes[i].stateRelativePositions[j] = shapes[i].stateRelativePositions[q];
                            shapes[i].stateRelativePositions[q] = Temp;
                        }
                    }

            }


        }

        public void DetermineLoosePosition(Shape parent, List<List<Block>> grid)
        {
            int i;
            bool stop;
            position = parent.gridPosition;

            for (i = 0; i < parent.blocks.Count; i++)
            {
                shapes[indicatorShapeIndex].blocks[i].position = position + shapes[indicatorShapeIndex].stateRelativePositions[i];
                stop = false;
                do
                {
                    //Check to see if the next space down is available. If not, move it back up and stop the loop
                    shapes[indicatorShapeIndex].blocks[i].position.Y--;
                    int xPos = (int)shapes[indicatorShapeIndex].blocks[i].position.X;
                    int yPos = (int)shapes[indicatorShapeIndex].blocks[i].position.Y;

                    //this will check if the index goes out of range
                    try
                    {
                        //grid is rows first then cols
                        if (grid[yPos][xPos].enabled && !stop) //check that there isnt an enabled block blocking the direction
                        {
                            shapes[indicatorShapeIndex].blocks[i].position.Y++;
                            stop = true;
                        }

                    }
                    catch
                    {
                        shapes[indicatorShapeIndex].blocks[i].position.Y++;
                        stop = true;
                    }

                } while (!stop);

                //Temporarily enable the space so that the other indicators don't go into that spot
                grid[(int)shapes[indicatorShapeIndex].blocks[i].position.Y][(int)shapes[indicatorShapeIndex].blocks[i].position.X].enabled = true;


            }

            //Re-disable the spaces that the indicators are in
            for (i = 0; i < parent.blocks.Count; i++)
            {
                grid[(int)shapes[indicatorShapeIndex].blocks[i].position.Y][(int)shapes[indicatorShapeIndex].blocks[i].position.X].enabled = false;
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, Shape parent, Vector2 positionOfGrid)
        {
            for (int i = 0; i < shapes[indicatorShapeIndex].blocks.Count; i++)
            {
                Vector2 drawPosition = new Vector2(positionOfGrid.X + ((shapes[indicatorShapeIndex].blocks[i].position.X) * 40 * Game1.displayRatio.X), positionOfGrid.Y - ((shapes[indicatorShapeIndex].blocks[i].position.Y) * 40 * Game1.displayRatio.Y));
                shapes[indicatorShapeIndex].blocks[i].Draw(gameTime, spriteBatch, drawPosition);
            }
        }
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, Shape parent, Vector2 positionOfGrid, Texture2D effect, float spookFactor)
        {
            Color spookopacity;
            spookFactor *= 8;
            for (int i = 0; i < shapes[indicatorShapeIndex].blocks.Count; i++)
            {
                float alpha = Math.Abs(parent.gridPosition.X - shapes[indicatorShapeIndex].blocks[i].position.X) + Math.Abs(parent.gridPosition.Y - shapes[indicatorShapeIndex].blocks[i].position.Y);
                alpha = Math.Max(0, (spookFactor * alpha));
                spookopacity = new Color(0, 0, 0, alpha);
                Vector2 drawPosition = new Vector2(positionOfGrid.X + ((shapes[indicatorShapeIndex].blocks[i].position.X) * 40 * Game1.displayRatio.X), positionOfGrid.Y - ((shapes[indicatorShapeIndex].blocks[i].position.Y) * 40 * Game1.displayRatio.Y));
                shapes[indicatorShapeIndex].blocks[i].Draw(gameTime, spriteBatch, drawPosition);
                spriteBatch.Draw(effect, new Rectangle((int)drawPosition.X, (int)drawPosition.Y, (int)(40 * Game1.displayRatio.X), (int)(40 * Game1.displayRatio.Y)), spookopacity);
            }
        }
    }
}
