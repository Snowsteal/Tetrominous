using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FGA.Shapes
{
    public abstract class Shape
    {
        //Relative Vector Positions for each state and each block in each state
        public List<List<Vector2>> stateRelativePositions = new List<List<Vector2>>();
        public int currentShapeIndex = 0;
        public bool loose;

        //List of blocks the shape has
        public List<Block> blocks = new List<Block>();

        public Vector2 gridPosition = new Vector2(6, 21);

        //Max rotation states
        public int maxIndex;

        public Vector2 size;

        public virtual string Name
        {
            get { return "Shape"; }
        }

        public Shape(int blockCount)
        {
            //Adds a new state
            stateRelativePositions.Add(new List<Vector2>());

            //Adds an origin to the state
            stateRelativePositions[0].Add(Vector2.Zero);

            //Initialize New Blocks
            for (int i = 0; i < blockCount; i++)
            {
                //adds blocks that are enabled
                blocks.Add(new Block(true));
            }

            //Set color of blocks
            SetBlockColors(BlockColors.VanillaColors);
        }

        public void DrawToGrid(GameTime gameTime, SpriteBatch spriteBatch, Vector2 positionOfGrid)
        {
            for (int i = 0; i < stateRelativePositions[currentShapeIndex].Count; i++)
            {
                blocks[i].Draw(gameTime, spriteBatch, new Vector2(positionOfGrid.X + ((gridPosition.X + stateRelativePositions[currentShapeIndex][i].X) * 40 * Game1.displayRatio.X), positionOfGrid.Y - ((gridPosition.Y + stateRelativePositions[currentShapeIndex][i].Y) * 40 * Game1.displayRatio.Y)));
            }
        }

        public void Draw_Next_Block(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position)
        {
            float scaleX;
            scaleX = 0;
            for (int i = 0; i < stateRelativePositions[currentShapeIndex].Count; i++)
            {
                if (size.X >= 4)
                    scaleX = 20;

                blocks[i].Draw(gameTime, spriteBatch, new Vector2(position.X + scaleX + stateRelativePositions[0][i].X * 40 * Game1.displayRatio.X, position.Y - stateRelativePositions[0][i].Y * 40 * Game1.displayRatio.Y));
            }
        }

        public void SetBlockColors(BlockColor blockColor)
        {
            for (int i = 0; i < blocks.Count; i++)
                blocks[i].SetColor(blockColor);
        }

        public void SetBlockColors(int blockIndex, BlockColor blockColor)
        {
            if (blockIndex >= 0 && blockIndex < blocks.Count)
            {
                blocks[blockIndex].SetColor(blockColor);
            }
        }

        public void SetBlockColors(BlockColor[] blockColors)
        {
            for (int i = 0; i < blocks.Count; i++)
            {
                int Element = Game1.rnd.Next(0, blockColors.Length);
                blocks[i].SetColor(blockColors[Element]);
            }
        }

        public void SetBlockColors(BlockColor[] blockColors, double[] colorChances)
        {
            if (blockColors.Length == colorChances.Length)
            {
                double chance = Game1.rnd.NextDouble();
                double chanceTotal = 0;

                for (int i = 0; i < blocks.Count; i++)
                {
                    for (int j = 0; j < colorChances.Length; j++)
                    {
                        chanceTotal += colorChances[j];
                        if (chance < chanceTotal)
                        {
                            blocks[i].SetColor(blockColors[j]);
                            break;
                        }
                    }
                }
            }
        }

        public bool Transform(Vector2 translation, List<List<Block>> grid)
        {
            //flag for making sure everything is okay to move
            bool isMovable = true;

            //Check for each block in the shape
            for (int i = 0; i < stateRelativePositions[currentShapeIndex].Count; i++)
            {
                //determine where the block will be after transforming
                int xPos = (int)gridPosition.X + (int)stateRelativePositions[currentShapeIndex][i].X + (int)translation.X;
                int yPos = (int)gridPosition.Y + (int)stateRelativePositions[currentShapeIndex][i].Y + (int)translation.Y;

                //this will check if the index goes out of range
                try
                {
                    //grid is rows first then cols
                    if (grid[yPos][xPos].enabled) //check that there isnt an enabled block blocking the diretion
                    {
                        isMovable = false;
                    }
                }
                catch
                {
                    isMovable = false;
                }
            }

            if (isMovable)
            {
                //Move the block to its new location, and move the indicator to match the transformation
                gridPosition += translation;
            }

            return isMovable;
        }

        public void AddShapeToGrid(List<List<Block>> grid)
        {

            for (int i = 0; i < stateRelativePositions[currentShapeIndex].Count; i++)
            {
                //determines position plus relative position of each block and adds it to the master grid
                 grid[(int)gridPosition.Y + (int)stateRelativePositions[currentShapeIndex][i].Y][(int)gridPosition.X + (int)stateRelativePositions[currentShapeIndex][i].X] = blocks[i];
            }
        }

        //Increases the currentShapeIndex to change which state the shape is in
        public virtual bool Rotate(List<List<Block>> grid)
        {
            bool isRotateable = true;

            //This will check if the index goes out of bounds
            try
            {
                //Move the shape to the next rotation state
                if (currentShapeIndex < maxIndex)
                {
                    currentShapeIndex++;
                }
                else
                {
                    currentShapeIndex = 0;
                }

                //Check to see if the new space for the block is open
                for (int i = 0; i < stateRelativePositions[currentShapeIndex].Count; i++)
                {
                    int xPos = (int)gridPosition.X + (int)stateRelativePositions[currentShapeIndex][i].X;
                    int yPos = (int)gridPosition.Y + (int)stateRelativePositions[currentShapeIndex][i].Y;

                    if (grid[yPos][xPos].enabled) //check that there isnt an enabled block blocking the direction
                    {
                        isRotateable = false;
                    }

                }
            }
            catch
            {
                //In the event that a block goes out of bounds, we need to see how far and what direction it went out
                //The ranges of the shape
                int xMin = 0;
                int xMax = 0;
                int yMin = 0;
                int yMax = 0;
                int xTranslation = 0;

                for (int i = 0; i < stateRelativePositions[currentShapeIndex].Count; i++)
                {
                    //Determine the range of the shape
                    int xPos = (int)gridPosition.X + (int)stateRelativePositions[currentShapeIndex][i].X;
                    int yPos = (int)gridPosition.Y + (int)stateRelativePositions[currentShapeIndex][i].Y;

                    if (xPos < xMin)
                        xMin = xPos;
                    if (xPos > xMax)
                        xMax = xPos;
                    if (yPos < yMin)
                        yMin = yPos;
                    if (yPos > yMax)
                        yMax = yPos;

                }

                //If the out of bounds were in the y direction, the shape doesn't rotate. If in the x, we need to determine how far to move it over
                if (yMax > grid.Count - 1 || yMin < 0)
                    isRotateable = false;
                else if (xMax > grid[0].Count - 1)
                    xTranslation = (grid[0].Count - 1) - xMax;
                else if (xMin < 0)
                    xTranslation = -xMin;

                //If in the x direction, check the new position after the block is moved over
                if (isRotateable)
                {
                    for (int i = 0; i < stateRelativePositions[currentShapeIndex].Count; i++)
                    {
                        int xPos = (int)gridPosition.X + (int)stateRelativePositions[currentShapeIndex][i].X + xTranslation;
                        int yPos = (int)gridPosition.Y + (int)stateRelativePositions[currentShapeIndex][i].Y;

                        if (grid[yPos][xPos].enabled) //check that there isnt an enabled block blocking the direction
                        {
                            isRotateable = false;
                        }

                    }

                    //If that space is open, move the center block over
                    if (isRotateable)
                        gridPosition.X += xTranslation;
                }

            }

            //Return the shape to its previous rotation state if it wasn't able to rotate
            if (!isRotateable)
            {
                if (currentShapeIndex == 0)
                    currentShapeIndex = maxIndex;
                else
                    currentShapeIndex--;
            }

            return isRotateable;
        }

        public virtual void SpecialRotation(Shape shape)
        {
            //This function will be used for shapes that do not have 4 rotation states, but we still want to swap the colors around
        }

        public bool CheckLoss(List<List<Block>> grid)
        {
            for (int i = 0; i < stateRelativePositions[currentShapeIndex].Count; i++)
            {
                int xPos = (int)gridPosition.X + (int)stateRelativePositions[currentShapeIndex][i].X;
                int yPos = (int)gridPosition.Y + (int)stateRelativePositions[currentShapeIndex][i].Y;

                if (grid[yPos][xPos].enabled) //check that there isnt an enabled block blocking the newly made block
                {
                    //If there is, then the player has lost the game
                    return true;
                }

            }
            //If the space is open, the game is still on!
            return false;
        }
    }

    public enum Colors {
        Blue = 0, 
        Cyan = 1, 
        Green = 2, 
        Orange = 3, 
        Purple = 4, 
        Red = 5,
        Gold = 6, 
        Rainbow = 7, 
        Black = 8,
        DirtBlock = 9, 
        WoodBlock = 10,
        StoneBlock = 11, 
        CopperBlock = 12, 
        IronBlock = 13,
        GoldBlock = 14,
        DiamondBlock = 15, 
        IndustrialBlock = 16,
        SpookyBlock = 17,
        GreyBlock = 18,
        Empty };

    public struct BlockColor
    {
        public Texture2D texture;
        public Colors color;

        public BlockColor(Texture2D texture, Colors color)
        {
            this.texture = texture;
            this.color = color;
        }
    };

    public static class BlockColors
    {
        public static BlockColor[] AllBlocks;

        public static void Initialize()
        {
            //Initialize Block Textures
            AllBlocks = new BlockColor[20];
            AllBlocks[0] = new BlockColor(Game1.GlobalContent.Load<Texture2D>(@"Blocks/Blue_Block_L"), Colors.Blue);
            AllBlocks[1] = new BlockColor(Game1.GlobalContent.Load<Texture2D>(@"Blocks/Cyan_Block_I"), Colors.Cyan);
            AllBlocks[2] = new BlockColor(Game1.GlobalContent.Load<Texture2D>(@"Blocks/Green_Block_inverseZ"), Colors.Green);
            AllBlocks[3] = new BlockColor(Game1.GlobalContent.Load<Texture2D>(@"Blocks/Orange_Block_inverseL"), Colors.Orange);
            AllBlocks[4] = new BlockColor(Game1.GlobalContent.Load<Texture2D>(@"Blocks/Purple_Block_T"), Colors.Purple);
            AllBlocks[5] = new BlockColor(Game1.GlobalContent.Load<Texture2D>(@"Blocks/Red_Block_Z"), Colors.Red);
            AllBlocks[6] = new BlockColor(Game1.GlobalContent.Load<Texture2D>(@"Blocks/Yellow_Block_Sq"), Colors.Gold);
            AllBlocks[7] = new BlockColor(Game1.GlobalContent.Load<Texture2D>(@"Blocks/Rainbowblock"), Colors.Rainbow);
            AllBlocks[8] = new BlockColor(Game1.GlobalContent.Load<Texture2D>(@"Blocks/BlackBlock"), Colors.Black);
            AllBlocks[9] = new BlockColor(Game1.GlobalContent.Load<Texture2D>(@"Blocks/Dirt_Block"), Colors.DirtBlock);
            AllBlocks[10] = new BlockColor(Game1.GlobalContent.Load<Texture2D>(@"Blocks/Wood_Block"), Colors.WoodBlock);
            AllBlocks[11] = new BlockColor(Game1.GlobalContent.Load<Texture2D>(@"Blocks/Stone_Block"), Colors.StoneBlock);
            AllBlocks[12] = new BlockColor(Game1.GlobalContent.Load<Texture2D>(@"Blocks/Copper_Block"), Colors.CopperBlock);
            AllBlocks[13] = new BlockColor(Game1.GlobalContent.Load<Texture2D>(@"Blocks/Iron_Block"), Colors.IronBlock);
            AllBlocks[14] = new BlockColor(Game1.GlobalContent.Load<Texture2D>(@"Blocks/Gold_Block"), Colors.GoldBlock);
            AllBlocks[15] = new BlockColor(Game1.GlobalContent.Load<Texture2D>(@"Blocks/Diamond_Block"), Colors.DiamondBlock);
            AllBlocks[16] = new BlockColor(Game1.GlobalContent.Load<Texture2D>(@"Blocks/Industrial_Block"), Colors.IndustrialBlock);
            AllBlocks[17] = new BlockColor(Game1.GlobalContent.Load<Texture2D>(@"Blocks/spooky_Block"), Colors.SpookyBlock);
            AllBlocks[18] = new BlockColor(Game1.GlobalContent.Load<Texture2D>(@"Blocks/Grey_Block"), Colors.GreyBlock);
            AllBlocks[19] = new BlockColor(Game1.GlobalContent.Load<Texture2D>(@"Blocks/InvisibleBlock"), Colors.Empty);
        }

        public static BlockColor[] VanillaColors
        {
            get { return SelectColors(0, 1, 2, 3, 4, 5, 6); }
        }

        public static BlockColor[] LineEmUpColors
        {
            get { return SelectColors(0, 2, 4, 5, 6, 7); }
        }

        public static BlockColor[] FactoryColors
        {
            get { return SelectColors(9, 10, 11, 12, 13, 14, 15, 16); }
        }

        public static BlockColor NightmareColor
        {
            get { return AllBlocks[17]; }
        }

        public static BlockColor ShapeShiftingColor
        {
            get { return AllBlocks[8]; }
        }

        public static BlockColor GreyColor
        {
            get { return AllBlocks[18]; }
        }

        public static BlockColor[] SelectColors(params int[] colorIndexes)
        {
            BlockColor[] tempColors = new BlockColor[colorIndexes.Length];
            for (int i = 0; i < colorIndexes.Length; i++)
            {
                tempColors[i] = AllBlocks[colorIndexes[i]];
            }

            return tempColors;
        }

        public static BlockColor EmptyColor
        {
            get { return AllBlocks[19]; }
        }
    }
}
