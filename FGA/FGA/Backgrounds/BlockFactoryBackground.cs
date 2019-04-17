using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using FGA.Backgrounds.Background_Objects;

namespace FGA.Backgrounds
{
    class BlockFactoryBackground : Background
    {
        public BlockFactoryBackground()
        {
            backgroundTexture = Game1.GlobalContent.Load<Texture2D>(@"Background Screens/Block Factory Screen/Tetris Factory Screen");
            //1307, 894

            //backgroundObjects.Add(new BackgroundObject_PointToPointTeleport(Game1.GlobalContent.Load<Texture2D>(@"Background Screens/Block Factory Screen/Block Tray"), new Vector2(1325, 24) * Game1.displayRatio, new Vector2(1325, 800) * Game1.displayRatio, 200f * Game1.displayRatio.Y));
            backgroundObjects.Add(new ConveyorBelt(new Vector2(1325, 24) * Game1.displayRatio, -100 * Game1.displayRatio.Y, 0));
            backgroundObjects.Add(new ConveyorBelt(new Vector2(1490, 24) * Game1.displayRatio, -100 * Game1.displayRatio.Y, 1));
            backgroundObjects.Add(new ConveyorBelt(new Vector2(1655, 24) * Game1.displayRatio, -100 * Game1.displayRatio.Y, 2));
            backgroundObjects.Add(new BackgroundObject(Game1.GlobalContent.Load<Texture2D>(@"Background Screens/Block Factory Screen/Top Bar"), Vector2.Zero));
            backgroundObjects.Add(new BackgroundObject(Game1.GlobalContent.Load<Texture2D>(@"Background Screens/Block Factory Screen/Factory Machines"), new Vector2(1307, 894) * Game1.displayRatio));
        }
    }

    class ConveyorBelt : BackgroundObject_PointToPointTeleport
    {
        //Keeps track of all the block trays
        public List<BlockTray> blockTrays = new List<BlockTray>();

        //Helps determine when a block is being placed, 3 for bufferage
        private static List<Block> Blocks = new List<Block>();

        //order of which conveyor belt it is
        //always tells which of the 3 Blocks to choose for
        public int index;

        public ConveyorBelt(Vector2 position, float speed, int index)
            : base(Game1.GlobalContent.Load<Texture2D>(@"Background Screens/Block Factory Screen/Block Tray"), position, position - new Vector2(0, 75) * Game1.displayRatio, speed)
        {
            //Initialze the buffer with three disabled blocks if it hasnt been done
            if (Blocks.Count == 0)
                for (int i = 0; i < 3; i++)
                    Blocks.Add(new Block(false));

            this.index = index;

            //creates 14 trays so that one is always hidden for adding on blocks
            for(int i = 0; i < 14; i++)
                blockTrays.Add(new BlockTray(texture));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //add the block buffer with the index of this instance to the last
            //tray in order to hide it
            if (Blocks[index].enabled)
            {
                if (blockTrays[blockTrays.Count - 1].SetBlock(Blocks[index]))
                {
                    Blocks[index].enabled = false;
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Vector2 drawPosition = position;

            for (int i = 0; i < blockTrays.Count; i++)
            {
                blockTrays[i].Draw(gameTime, spriteBatch, drawPosition);
                drawPosition.Y += (float)texture.Height * Game1.displayRatio.Y;
            }
        }

        protected override void EndReached()
        {
            base.EndReached();

            blockTrays.RemoveAt(0);
            blockTrays.Add(new BlockTray(texture));
        }

        public static bool AddBlock(Block block)
        {
            //add block to a random place
            int num = Game1.rnd.Next(0, 3);
            //Try to add the block if it'll fit
            //overwrite if the tier is higher so that only the good blocks get seen
            if (!Blocks[num].enabled || block.BlockIndex > Blocks[num].BlockIndex)
            {
                Blocks[num] = block;
            }

            return false;
        }
    }

    class BlockTray : BackgroundObject
    {
        Block block = null;

        public BlockTray(Texture2D texture)
            : base(texture, Vector2.Zero)
        {
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position)
        {
            //set up drawing
            this.position = position;
            base.Draw(gameTime, spriteBatch);

            //draw the block if it's there
            if (block != null)
                block.Draw(gameTime, spriteBatch, position + new Vector2(36, 24) * Game1.displayRatio);
        }

        public bool SetBlock(Block block)
        {
            if (this.block == null)
            {
                this.block = block;
                return true;
            }

            return false;
        }
    }
}
