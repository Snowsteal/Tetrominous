using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FGA.Shapes;
using FGA.TetrisFactory;
using FGA.TetrisFactory.Upgrades;
using FGA.Backgrounds;

namespace FGA
{
    class Tetris_Factory : Level
    {
        public ulong cash;                                              //Number of moneys the player has
        float baseBlocksPerSecond;                                      //The blocks per second without global multipliers or addipliers
        float generatedBlocks;                                          //Number of generated blocks
        Button upgradeBlockTierButton;
        Button switchToAutomaticButton, switchToManualButton;           //Buttons to activate automatic
        bool isAutomatic;                                               //Determines wether to place the produced blocks into the playing grid or autogrid
        int numBuildingLots;                                            //numbers of slots for factories, upgrades will increase this so playing can build more, but it will decrease playing field size, max at 5, might change max
        SpriteFont spriteFont;                                          //try to get rid of this, for now it's used to draw cash, you can change it to use the numbers texture
        Viewport factoryBuildingViewport;                               //Was gonna use this to separate each of the grade sections since we would have to be scrolling through them
        Texture2D extendedGrid, industrialBar;                          //Textures for the autogrid and bar separating autogrid/playing grid
        public List<Block_Factory> factoryList = new List<Block_Factory>();    //List of current amount of factories
        List<List<Block>> autoGrid = new List<List<Block>>();           //Blocks in the grid below playing grid
        Vector2 autoGridPosition;                                       //Position of new extended grid

        List<Upgrade> allUpgrades = new List<Upgrade>();                //Master List of All Upgrades
        public List<Upgrade> storeUpgrades = new List<Upgrade>();       //List of upgrades currently available to buy
        public List<Upgrade> boughtUpgrades = new List<Upgrade>();      //List of upgrades already bought


        //alec's variables
        List<NewBlockFactory> blockFactoryUpgrades = new List<NewBlockFactory>(0);
        List<Increase_Block_Tier> blockTierUpgrades = new List<Increase_Block_Tier>(0);
        List<IncreaseBlockFactorySpeed> blockSpeedUpgrades = new List<IncreaseBlockFactorySpeed>(0);
        public bool showNextBlockFactoryUpgrade;
        Texture2D cashDisplay, costDisplay, whiteText;
        Viewport defaultViewPort, currentUpgradesViewport;
        bool mlg;

        //Speed of game won't increase
        protected override int CurrentLevel
        {
            get
            {
                return 1;
            }
        }

        //Speed of game won't increase
        protected override int TimeDelayMilliseconds
        {
            get
            {

                return mlg ? 0 : 500;//500;
            }
        }

        //this will include global multiplier and addiplier calulations 
        protected float BlocksPerSecond
        {
            get
            {
                return baseBlocksPerSecond;
            }
        }

        //Empty building lots for new factories
        protected int NumAvailableBuildingLots
        {
            get
            {
                return numBuildingLots - factoryList.Count;
            }
        }

        public Tetris_Factory()
            : base(PlayerIndex.One)
        {
            position = new Vector2(760, 948) * Game1.displayRatio;

            spriteFont = Game1.GlobalContent.Load<SpriteFont>(@"text");
            upgradeBlockTierButton = new Button(Game1.GlobalContent.Load<Texture2D>(@"Buttons & Cursor/TF_Upgrade_Button"), new Vector2(195, 477) * Game1.displayRatio, new Vector2(400, 140) * Game1.displayRatio);
            switchToAutomaticButton = new Button(Game1.GlobalContent.Load<Texture2D>(@"Buttons & Cursor/SwitchToAutomaticButton"), new Vector2(200, 200) * Game1.displayRatio, new Vector2(20, 20) * Game1.displayRatio);
            switchToManualButton = new Button(Game1.GlobalContent.Load<Texture2D>(@"Buttons & Cursor/SwitchToManualButton"), new Vector2(200, 200) * Game1.displayRatio, new Vector2(20, 20) * Game1.displayRatio);
            switchToAutomaticButton.enabled = true;
            switchToManualButton.enabled = false;
            upgradeBlockTierButton.enabled = false;
            isAutomatic = true;
            factoryBuildingViewport = new Viewport(1325, 335, 400, 440);
            extendedGrid = Game1.GlobalContent.Load<Texture2D>(@"Textures/GridExtended");
            industrialBar = Game1.GlobalContent.Load<Texture2D>(@"Textures/Industrial_Bar");
            numBuildingLots = 5;
            autoGridPosition = position + new Vector2(0, 13) * Game1.displayRatio;

            //upgrade stuff
            Texture2D buttontexture1 = Game1.GlobalContent.Load<Texture2D>(@"Buttons & Cursor/new block factory button");
            Texture2D buttontexture2 = Game1.GlobalContent.Load<Texture2D>(@"Buttons & Cursor/upgradeblocktierbutton");
            Texture2D buttontexture3 = Game1.GlobalContent.Load<Texture2D>(@"Buttons & Cursor/speedupgrade");
            showNextBlockFactoryUpgrade = false;
            for (int Q = 0; Q < 5; Q++)
            {
                blockFactoryUpgrades.Add(new NewBlockFactory(buttontexture1, "Block Factory #" + Q, 0, (ulong)(50.0 + (500.0 * Q))));
                blockFactoryUpgrades[Q].button.enabled = false;
                blockFactoryUpgrades[Q].button.position = new Vector2(84, 199 + (140 * Q)) * Game1.displayRatio;
                blockFactoryUpgrades[Q].button.size = new Vector2(400, 140) * Game1.displayRatio;
                blockTierUpgrades.Add(new Increase_Block_Tier(buttontexture2, "Tier Upgrades for block factory #" + Q, 0, (ulong)(50), Q));
                blockTierUpgrades[Q].button.enabled = false;
                blockTierUpgrades[Q].button.position = blockFactoryUpgrades[Q].button.position;
                blockTierUpgrades[Q].button.size = blockFactoryUpgrades[Q].button.size;
                blockSpeedUpgrades.Add(new IncreaseBlockFactorySpeed(buttontexture3, "Speed upgrade for block factory #" + Q, 0, (ulong)(50), Q));
                blockSpeedUpgrades[Q].button.enabled = false;
                blockSpeedUpgrades[Q].button.position = blockFactoryUpgrades[Q].button.position + new Vector2(400, 0) * Game1.displayRatio;
                blockSpeedUpgrades[Q].button.size = new Vector2(200, 140) * Game1.displayRatio;
            }
            blockFactoryUpgrades[0].button.enabled = true;

            cashDisplay = Game1.GlobalContent.Load<Texture2D>(@"Buttons & Cursor/cash_display");
            costDisplay = Game1.GlobalContent.Load<Texture2D>(@"Buttons & Cursor/cost_display");
            whiteText = Game1.GlobalContent.Load<Texture2D>(@"Score Board Numbers/White_Numbers");

            nextBlockDisplayPosition = new Vector2(685, -673) * Game1.displayRatio;
            
        }

        protected override void UpdateInGame(GameTime gameTime)
        {
            //Update Factories
            for(int i = 0; i < factoryList.Count; i++)
                if (isAutomatic)
                    factoryList[i].Update(gameTime);
                else
                    factoryList[i].Update(gameTime, autoGrid[i], ref cash);

            //Update blockspersecond incase it has changed, might want to change this to a function and only call it when something has been bought that will change it
            baseBlocksPerSecond = 0;
            for (int i = 0; i < factoryList.Count; i++)
            {
                baseBlocksPerSecond += factoryList[i].BlocksPerSecond;
            }

            for (int Q = 0; Q < blockTierUpgrades.Count; Q++)
            {

                blockTierUpgrades[Q].Update(gameTime, this);

            }
            for (int Q = 0; Q < blockFactoryUpgrades.Count; Q++)
            {
                blockFactoryUpgrades[Q].Update(gameTime, this);

            }
            for (int Q = 0; Q < blockSpeedUpgrades.Count; Q++)
            {
                blockSpeedUpgrades[Q].Update(gameTime, this);

            }
            if (showNextBlockFactoryUpgrade)
            {
                for (int Q = 0; Q < blockFactoryUpgrades.Count; Q++)
                {
                    if (!blockFactoryUpgrades[Q].button.enabled && !blockFactoryUpgrades[Q].purchased)
                    {
                        blockFactoryUpgrades[Q].button.enabled = true;
                        showNextBlockFactoryUpgrade = false;
                        break;
                    }
                }
                if (factoryList.Count == 5)
                    showNextBlockFactoryUpgrade = false;
                blockTierUpgrades[factoryList.Count - 1].button.enabled = true;
                blockSpeedUpgrades[factoryList.Count - 1].button.enabled = true;
            }


            #region manual/auto button updates
            switchToAutomaticButton.Update(gameTime);
            if (switchToAutomaticButton.IsClicked)
            {
                isAutomatic = true;
                switchToAutomaticButton.enabled = false;
                switchToManualButton.enabled = true;
            }

            switchToManualButton.Update(gameTime);
            if (switchToManualButton.IsClicked)
            {
                isAutomatic = false;
                switchToAutomaticButton.enabled = true;
                switchToManualButton.enabled = false;
            }
            #endregion

            #region automatic generate blocks
            if (isAutomatic)
            {
                List<FGA.TetrisFactory.Block_Factory.Produced_Blocks> producedBlocksList = new List<Block_Factory.Produced_Blocks>();

                //Transfer all blocks produced from factories to game
                for (int i = 0; i < factoryList.Count; i++)
                {
                    producedBlocksList.Add(factoryList[i].TransferBlocks());
                    generatedBlocks += producedBlocksList[i].numBlocks;
                }

                for (int i = 0; i < producedBlocksList.Count; i++)
                {
                    for (int j = 0; j < producedBlocksList[i].numBlocks; j++)
                    {
                        if (!PlaceBlockInGrid(producedBlocksList[i].block))
                        {
                            i = 99;
                            break;
                        }
                        generatedBlocks--;
                    }
                }

                CheckCompletedRows();

                if (generatedBlocks >= 10)
                {
                    ulong extrarows = (ulong)generatedBlocks / 10;
                    generatedBlocks %= 10;
                    cash += extrarows;
                }

                UpdateIndicator();
            }
            #endregion

            base.UpdateInGame(gameTime);
        }

        protected override void DrawInGame(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (factoryList.Count > 0)
            {
                spriteBatch.Draw(extendedGrid, position + new Vector2(-14, -853) * Game1.displayRatio, null, Color.White, 0f, Vector2.Zero, Game1.displayRatio, SpriteEffects.None, 1f);
                spriteBatch.Draw(industrialBar, position + new Vector2(-13, 16 - (20 + (factoryList.Count - 1) * 40)) * Game1.displayRatio, null, Color.White, 0f, Vector2.Zero, Game1.displayRatio, SpriteEffects.None, 1);
                switchToAutomaticButton.position = position + new Vector2(420, 18 - 40 * factoryList.Count) * Game1.displayRatio;
                switchToManualButton.position = position + new Vector2(420, 18 - 40 * factoryList.Count) * Game1.displayRatio;
                switchToAutomaticButton.Draw(spriteBatch);
                switchToManualButton.Draw(spriteBatch);
                upgradeBlockTierButton.Draw(spriteBatch);
            }


            for (int Q = 0; Q < blockFactoryUpgrades.Count; Q++)
            {
                blockFactoryUpgrades[Q].Draw(gameTime, spriteBatch);
                if (blockFactoryUpgrades[Q].button.enabled && blockFactoryUpgrades[Q].button.buttonBounds.Contains(Input.mouseState.X, Input.mouseState.Y))
                {
                    spriteBatch.Draw(costDisplay, new Rectangle((int)(1250 * Game1.displayRatio.X), (int)(300 * Game1.displayRatio.Y), (int)(500 * Game1.displayRatio.X), (int)(90 * Game1.displayRatio.Y)), Color.White);
                    string strcost = "" + blockFactoryUpgrades[Q].cost;
                    Texture2D scoreTexture = whiteText;
                    for (int q = 0; q < strcost.Length; q++)
                    {
                        int index = (int)Char.GetNumericValue(strcost[q]);

                        spriteBatch.Draw(scoreTexture, new Rectangle((int)((1709 - ((strcost.Length - q) * 22)) * Game1.displayRatio.X), (int)(326 * Game1.displayRatio.Y),
                                            (int)(20 * Game1.displayRatio.X), (int)(40 * Game1.displayRatio.Y)), new Rectangle((int)(20 * index), 0, 20, 40), Color.White);
                    }
                }
            }

            for (int Q = 0; Q < blockTierUpgrades.Count && Q < factoryList.Count; Q++)
            {
                blockTierUpgrades[Q].Draw(gameTime, spriteBatch);
                if (blockTierUpgrades[Q].button.enabled)
                {
                    spriteBatch.Draw(BlockColors.FactoryColors[factoryList[Q].blockTier + 1].texture,new Rectangle((int)(blockTierUpgrades[Q].button.position.X + 50 * Game1.displayRatio.X),
                        (int)(blockTierUpgrades[Q].button.position.Y + 55 * Game1.displayRatio.Y), (int)(40 * Game1.displayRatio.X), (int)(40 * Game1.displayRatio.Y)), Color.White);

                    if (blockTierUpgrades[Q].button.buttonBounds.Contains(Input.mouseState.X, Input.mouseState.Y))
                    {
                        spriteBatch.Draw(costDisplay, new Rectangle((int)(1250 * Game1.displayRatio.X), (int)(300 * Game1.displayRatio.Y), (int)(500 * Game1.displayRatio.X), (int)(90 * Game1.displayRatio.Y)), Color.White);
                        string strcost = "" + blockTierUpgrades[Q].cost;
                        Texture2D scoreTexture = whiteText;
                        for (int q = 0; q < strcost.Length; q++)
                        {
                            int index = (int)Char.GetNumericValue(strcost[q]);

                            spriteBatch.Draw(scoreTexture, new Rectangle((int)((1709 - ((strcost.Length - q) * 22)) * Game1.displayRatio.X), (int)(326 * Game1.displayRatio.Y),
                                             (int)(20 * Game1.displayRatio.X), (int)(40 * Game1.displayRatio.Y)), new Rectangle((int)(20 * index), 0, 20, 40), Color.White);
                        }
                    }
                }
            }
            for (int Q = 0; Q < blockSpeedUpgrades.Count; Q++)
            {
                blockSpeedUpgrades[Q].Draw(gameTime, spriteBatch);
                if (blockSpeedUpgrades[Q].button.enabled && blockSpeedUpgrades[Q].button.buttonBounds.Contains(Input.mouseState.X, Input.mouseState.Y))
                {
                    spriteBatch.Draw(costDisplay, new Rectangle((int)(1250 * Game1.displayRatio.X), (int)(300 * Game1.displayRatio.Y), (int)(500 * Game1.displayRatio.X), (int)(90 * Game1.displayRatio.Y)), Color.White);
                    string strcost = "" + blockSpeedUpgrades[Q].cost;
                    Texture2D scoreTexture = whiteText;
                    for (int q = 0; q < strcost.Length; q++)
                    {
                        int index = (int)Char.GetNumericValue(strcost[q]);

                        spriteBatch.Draw(scoreTexture, new Rectangle((int)((1709 - ((strcost.Length - q) * 22)) * Game1.displayRatio.X), (int)(326 * Game1.displayRatio.Y),
                                            (int)(20 * Game1.displayRatio.X), (int)(40 * Game1.displayRatio.Y)), new Rectangle((int)(20 * index), 0, 20, 40), Color.White);
                    }
                }
            }

            for (int i = 0; i < autoGrid.Count; i++)
            {
                for (int j = 0; j < autoGrid[i].Count; j++)
                {
                    if (autoGrid[i][j].enabled)
                    {
                        autoGrid[i][j].position = new Vector2(autoGridPosition.X + (j * 40 * Game1.displayRatio.X), autoGridPosition.Y - (i * 40 * Game1.displayRatio.Y));
                        autoGrid[i][j].Draw(gameTime, spriteBatch, autoGrid[i][j].position);
                    }
                }
            }

            base.DrawInGame(gameTime, spriteBatch);
        }

        //No need to draw score but rather draw game statistics
        protected override void DrawScore(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //spriteBatch.DrawString(spriteFont, "Cash: $" + cash, new Vector2(900, 500) * Game1.displayRatio, Color.White);
            spriteBatch.Draw(cashDisplay, new Rectangle((int)(1250 * Game1.displayRatio.X), (int)(200 * Game1.displayRatio.Y), (int)(500 * Game1.displayRatio.X), (int)(90 * Game1.displayRatio.Y)), Color.White);
            string strcash = "" + cash;
            Texture2D scoreTexture = whiteText;
            for (int q = 0; q < strcash.Length; q++)
            {
                int index = (int)Char.GetNumericValue(strcash[q]);
                
                spriteBatch.Draw(scoreTexture, new Rectangle((int)((1709 - ((strcash.Length - q) * 22)) * Game1.displayRatio.X), (int)(226 * Game1.displayRatio.Y),
                                    (int)(20 * Game1.displayRatio.X), (int)(40 * Game1.displayRatio.Y)), new Rectangle((int)(20 * index), 0, 20, 40), Color.White);
            }

        }

        protected override int CheckCompletedRows()
        {
            int linesCompletedTogether = 0;
            bool isCompleted;

            for (int i = factoryList.Count; i < grid.Count; i++)
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

            if (linesCompletedTogether != 0)
            {
                UpdateIndicator();
            }

            //Calculate cash per completed line and multipliers
            for (int q = 0; q <= linesCompletedTogether; q++)
            {
                cash += (ulong)(q * 5);
            }
            

            return linesCompletedTogether;
        }

        protected bool PlaceBlockInGrid(Block block)
        {
            for(int i = 0; i < grid[factoryList.Count].Count; i++)
            {
                if (!grid[factoryList.Count][i].enabled)
                {
                    grid[factoryList.Count][i] = block;

                    //Add a copy of the block to the conveyor belt
                    Block b = new Block(true);
                    b.blockColor = block.blockColor;
                    ConveyorBelt.AddBlock(b);
                    return true;
                }
            }

            return false;
        }

        public void AddEmptyLayer()
        {
            grid.RemoveRange(grid.Count - 1, 1);

            //Reverse order of all elements to make adding new row easier
            grid.Reverse(0, grid.Count);

            grid.Add(new List<Block>());
            for (int j = 0; j < grid[0].Count; j++)
            {
                int index1 = grid.Count - 1;
                grid[index1].Add(new Block(true));
                grid[index1][grid[index1].Count - 1].blockColor.texture = Game1.GlobalContent.Load<Texture2D>(@"Blocks/InvisibleBlock");
            }


            //Unreverse everything
            grid.Reverse(0, grid.Count);

            //Make sure the falling shape isnt in the unused layer now
            float minY = currentFallingShape.stateRelativePositions[currentFallingShape.currentShapeIndex].Min(Vector2 => Vector2.Y);
            if (currentFallingShape.CheckLoss(grid))
            {
                int yTransformation = 1;
                //keep trying to move the falling shape up until there's nothing on it anymore
                while (!currentFallingShape.Transform(new Vector2(0, yTransformation), grid))
                {
                    yTransformation++;
                    //if the current falling shape moves above the grid, the player loses
                    if (currentFallingShape.gridPosition.Y + yTransformation > grid.Count)
                        Lose();
                }
            }

            UpdateIndicator();
        }

        public void AddAutoGridRow()
        {
           autoGrid.Add(new List<Block>());
            for (int j = 0; j < 10; j++)
                autoGrid[autoGrid.Count - 1].Add(new Block(false));
        }

        protected override void DrawNextBlock(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //Do nothing, no need to draw the next block
        }
    }
}
