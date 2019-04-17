using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FGA.Shapes;
using FGA.Backgrounds;

namespace FGA.TetrisFactory
{
    class Block_Factory
    {
        public struct Produced_Blocks                           //A produced block that will contain the texture and how much has been produced
        {
            public Block block;
            public int numBlocks;
        };

        //public static Texture2D[] BlockTierTextures = new Texture2D[8];

        public const float STARTING_BLOCKS_PER_SECOND = 0.1f;   //Produces 1 block every 10 seconds, can be changed for balancing
        public const float BASE_COST = 10.0f;                   //Original cost
        public const float COST_MULTIPLIER = 1.1f;              //Increase in cost each time it is upgraded
        float addiplier, multiplier;                            //things that increase blocks per second
        float blocksProduced;                                   //Amount of blocks produced
        int numUpgrades;                                        //Amount of times the factory has been upgrdeas
        public int blockTier;                                          //Value of the blocks produced

        public float BlocksPerSecond
        {   //Blocks per second due to multipliers and addipliers
            get
            {
                return (STARTING_BLOCKS_PER_SECOND + addiplier) * multiplier;
            }
        }

        public float Cost   //Upgades increase costs exponentially
        {
            get
            {
                return BASE_COST * (float)Math.Pow(COST_MULTIPLIER, numUpgrades);
            }
        }

        public Block_Factory()
        {
            multiplier = 1;
            numUpgrades = 1;
            blockTier = 0;
        }

        //Two updates to determine if it's building in the autogrid or the playing grid
        public void Update(GameTime gameTime)
        {   
            //Playing grid
            blocksProduced += (BlocksPerSecond * gameTime.ElapsedGameTime.Milliseconds / 1000);
        }

        public void Update(GameTime gameTime, List<Block> row, ref ulong cash)
        {   
            //Autogrid
            blocksProduced += (BlocksPerSecond * gameTime.ElapsedGameTime.Milliseconds / 1000);
            if (row != null)
            {
                if (blocksProduced >= 10)
                {
                    cash += (ulong)((blocksProduced / 10) * ((1 + blockTier) * 5));
                    blocksProduced %= 10;
                }

                while (blocksProduced >= 1)
                {
                    if (PlaceBlockInRow(row))
                        blocksProduced--;
                    else
                    {
                        for (int i = 0; i < row.Count; i++)
                            row[i] = new Block(false);
                        cash += (ulong)((1 + blockTier) * 5);
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 positon)
        {
            //Nothing yet but will be used eventually to draw upgrades in the upgrade section
        }

        public Produced_Blocks TransferBlocks()
        {
            Produced_Blocks producedBlocks = new Produced_Blocks();
            producedBlocks.block = new Block(true);
            producedBlocks.block.SetColor(BlockColors.FactoryColors[blockTier]);
            producedBlocks.numBlocks = (int)Math.Floor(blocksProduced);
            blocksProduced -= producedBlocks.numBlocks;
            return producedBlocks;
        }

        public void Upgrade()
        {
            addiplier += 0.1f;
            numUpgrades++;
        }

        protected bool PlaceBlockInRow(List<Block> row)
        {
            Block block = new Block(true);
            block.SetColor(BlockColors.FactoryColors[blockTier]);

            //Add a copy of the block to the conveyor belt
            Block b = new Block(true);
            b.blockColor = block.blockColor;
            ConveyorBelt.AddBlock(b);

            for (int i = 0; i < row.Count; i++)
            {
                if (!row[i].enabled)
                {
                    row[i] = block;
                    return true;
                }
            }

            return false;
        }
    }
}
