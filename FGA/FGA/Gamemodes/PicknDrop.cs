using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FGA.Shapes;
using Microsoft.Xna.Framework.Input;
using System.Net.Sockets;
using FGA.Online;


namespace FGA
{
    class PicknDrop : Level
    {
        bool isChoosingEnemyBlock;

        public List<Shape> shapeChoices = new List<Shape>(0);
        int choiceIndex;

        Texture2D selectEnemyBlockScreen;

        public PicknDrop(PlayerIndex player)
            : base(player)
        {
            choiceIndex = 1;

            selectEnemyBlockScreen = Game1.GlobalContent.Load<Texture2D>("Background Screens\\SelectEnemyBlockScreen");
        }

        public PicknDrop(NetworkStream netStream)
            : base(netStream)
        {

        }

        protected override void UpdateInGame(GameTime gameTime)
        {
            if (!isChoosingEnemyBlock)
            {
                //Update fall time
                UpdateFallTimer(gameTime);
            }

            UpdateGridPositions(gameTime);

            //DO NOT REMOVE
            //for (long i = 0; i < Math.PI * 500039587673; i++)
            //{
            //    i--;
            //}
        }

        protected override void DrawInGame(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.DrawInGame(gameTime, spriteBatch);

            if(isChoosingEnemyBlock)
            {
                if(player == PlayerIndex.One)
                {
                    shapeChoices[choiceIndex].Draw_Next_Block(gameTime, spriteBatch, nextBlockDisplayPosition + new Vector2(50, 1425) * Game1.displayRatio);
                    spriteBatch.Draw(selectEnemyBlockScreen, new Rectangle((int)(50 * Game1.displayRatio.X), (int)(100 * Game1.displayRatio.Y), 
                                                                           (int)(400 * Game1.displayRatio.X), (int)(880 * Game1.displayRatio.Y)), Color.White);
                }

                else if (player == PlayerIndex.Two)
                {
                    shapeChoices[choiceIndex].Draw_Next_Block(gameTime, spriteBatch, nextBlockDisplayPosition + new Vector2(1475, 1425) * Game1.displayRatio);
                    spriteBatch.Draw(selectEnemyBlockScreen, new Rectangle((int)(1470 * Game1.displayRatio.X), (int)(100 * Game1.displayRatio.Y),
                                                                           (int)(400 * Game1.displayRatio.X), (int)(880 * Game1.displayRatio.Y)), Color.White);
                }
            }
        }

        public void AssignEnemyNextBlock(int tier)//gives the player 3 unique blocks to choose for their enemy
        {
            //tier = 4;
            int[] cases = new int[3];//a list of ID's that correspond to shapes, cases will never have two of the same number in it
            int index = 0;//the current element that controls cases

            switch (tier)//sets unique numbers based on the number of shape choices possible, size, and the bounds of random generation (max/min values)
            {
                case 1: cases = generateUniqueNumbers(3, 0, 5);//3 shapes to choose from, 5 shape types giving a rnd.next(0,7);
                    break;
                case 2: cases = generateUniqueNumbers(3, 0, 5);//3 shapes to choose from, 5 shape types giving a rnd.next(0,5);
                    break;
                case 3: cases = generateUniqueNumbers(3, 0, 4);//3 shapes to choose from, 4 shape types giving a rnd.next(0,4);
                    break;
            } 

            for (int Q = 0; Q < 3; Q++)//creates the actual shape and adds it to the player's choices which will add them to the enemy's shape queue
            {
                if (shapeChoices.Count > 2)
                {
                    shapeChoices.RemoveAt(0);
                }
                Shape Temp = new OShape();//temporary shape

                switch(tier)//looks at the tier, and assigns a certain difficulty of shape based on the tier
                {
                    //the following functions take in cases and index. which will judge what shape gets assigned
                    //cases controls the unique shape type
                    //index controls the element within "cases" to assign a unique shape  
                    case 1: Temp = GenerateRandomTier1Shape(cases, index);//tier 1 shapes, or basic shapes
                        break;
                    case 2: Temp = GenerateRandomTier2Shape(cases, index);//tier 2 shapes, a little harder to place
                        break;
                    case 3: Temp = GenerateRandomTier3Shape(cases, index);//tier 3 shapes, pretty hard to place
                        break;
                    case 4: Temp = new ShapeShiftingShape();//the ultimate shape, changes form every time player rotates it
                        break;
                }


                shapeChoices.Add(Temp);//add the newly generated shape to the player's shape choices
                index++;//increase the index by one so next run of the for loop will assign the next unique shape type from "cases"
            }
        }
        protected int[] generateUniqueNumbers(int size, int randomMinimumValue, int randomMaximumValue)
        {
            int[] cases = new int[size];//these variables act the same as the ones in assignenemyblock
            int Num;

            cases[0] = Game1.rnd.Next(randomMinimumValue, randomMaximumValue);//we know the first element will always be unique because nothing has been assigned yet
            for (int q = 1; q < size; q++)
            {
                do
                {
                    Num = Game1.rnd.Next(randomMinimumValue, randomMaximumValue);//will keep generating a number based on the random bounds passed in

                } while (cases.Contains(Num));//will run again if num is already an assigned element within "cases"
                cases[q] = Num;//once a unique number has been found, assign it to "cases"
            }

            return cases;
        }
        protected Shape GenerateRandomTier1Shape(int[] cases, int index)
        {
            //this is an overloaded function from the level class, the only thing different about this function is that it assigns the shape based on the unique numbers generated in "cases"
            //instead of generating a shape type on a random number assigned in here
            Shape Temp = null;

            switch (cases[index])
            {
                case 0:
                    Temp = new OklahomaShape();
                    break;
                case 1:
                    Temp = new HockeyStickShape();
                    break;
                case 2:
                    Temp = new EasyDiagonalShape();
                    break;
                case 3:
                    Temp = new ReverseHockeyStickShape();
                    break;
                case 4:
                    Temp = new CornerShape();
                    break;
            }

            return Temp;
        }
        protected Shape GenerateRandomTier2Shape(int[] cases, int index)
        {
            //generates a shape based on "cases" unique, random set of values
            Shape Temp = null;

            switch (cases[index])
            {
                case 0: Temp = new LobsterClawShape();
                    break;
                case 1: Temp = new PlusShape();
                    break;
                case 2: Temp = new VShape();
                    break;
                case 3: Temp = new BucketShape();
                    break;
                case 4: Temp = new StairShape();
                    break;
            }
            
            return Temp;
        }
        protected Shape GenerateRandomTier3Shape(int [] cases, int index)
        {
            Shape Temp = null;

            //generates a shape based on "cases" unique, random set of values
            switch (cases[index])
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

            return Temp;
        }
        protected ShapeShiftingShape GenereateFormingShape()
        {
            //this shape will turn into a different shape type with every rotation, up to a max of 4 different forms.
            //the shape will loop in its 4 shape types
            ShapeShiftingShape Temp = new ShapeShiftingShape();
            //Texture2D mysteryBlockTexture = Game1.GlobalContent.Load<Texture2D>("Blocks\\BlackBlock");
            //int[] cases = new int[4];
            //int index = 0;

            //generateUniqueNumbers(4, 0, 4);//generate the 4, tier 3, shape types that will be the "forms" of this shape

            //for (int q = 0; q < 4; q++)
            //{
            //    Temp.forms.Add(GenerateRandomTier3Shape(cases, index));

            //    for (int i = 0; i < Temp.forms[q].blocks.Count; i++)
            //        Temp.forms[q].blocks[i].blockColor.texture = mysteryBlockTexture;

            //    Temp.keyToTheKingdom.Add(Game1.rnd.Next(0, Temp.forms[q].maxIndex + 1));
            //    index++;
            //}

            return Temp;
        }

        protected override int CheckCompletedRows()
        {
            int linesCompletedTogether = base.CheckCompletedRows();

            if (linesCompletedTogether > 0)
            {
                AssignEnemyNextBlock(linesCompletedTogether);
                isChoosingEnemyBlock = true;
            }

            return linesCompletedTogether;
        }

        public override void MoveLeft()
        {
            if (isChoosingEnemyBlock)
            {
                if (choiceIndex == 0)
                    choiceIndex = 2;
                else
                    choiceIndex--;
            }
            else
                base.MoveLeft();
        }

        public override void MoveRight()
        {
            if (isChoosingEnemyBlock)
            {
                if (choiceIndex == 2)
                    choiceIndex = 0;
                else
                    choiceIndex++;
            }
            else
                base.MoveRight();
        }
        public override void DropOne()
        {
            if (isChoosingEnemyBlock)
            {
                isChoosingEnemyBlock = false;
                //enemyLevel.shapeQueue.Add(shapeChoices[choiceIndex]);
                gameClassReference.OnEnemyShapeChosen(player, shapeChoices[choiceIndex]);
            }
            else
                base.DropOne();
        }
    }
}
