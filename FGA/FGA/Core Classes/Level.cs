using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using FGA.Shapes;
using FGA.Online;
using FGA.GameClasses;

namespace FGA
{
    class Level
    {
        public enum LevelState
        {
            Start,
            InGame,
            GameOver
        };
       
        public bool returnToMenu = false;

        //Player and position
        protected PlayerIndex player;
        protected Vector2 position;

        // Reference to game class in order to allow communication
        public GameClass gameClassReference;

        //Grid of all tetris pieces
        protected List<List<Block>> grid = new List<List<Block>>();

        //Falling tetris piece and piece queue
        protected Shape currentFallingShape;
        protected IndicatorShape indicatorShape;
        public List<Shape> shapeQueue = new List<Shape>();
        protected int fallingTimer;

        //Levels
        protected int linesCompleted, startingLevel = 1;
        protected LevelState levelState = LevelState.Start;

        //score of the player
        public int score;
        public static Texture2D[] ScoreTextures = new Texture2D[2];
        //hud variables
        public Vector2 scoreDisplayPosition;
        public Vector2 nextBlockDisplayPosition;

        //Level Start variables
        int countDown, elapsedCountDownTime;
        Vector2 countDownSize;
        float countDownRotation;

        //Level GameOver variables
        protected Texture2D endMessage;
        List<FallingObject> fallingBlocks = new List<FallingObject>();
        int timeSinceLastBlock;
        int emptyRows;
        bool reverseSearch;

        protected virtual int CurrentLevel
        {
            get
            {
                if ((linesCompleted / 10) + 1 < startingLevel)
                {
                    return startingLevel;
                }
                else
                    return (linesCompleted / 10) + 1;
            }
        }

        protected virtual int TimeDelayMilliseconds
        {
            get
            {
                return Math.Max(550 - (CurrentLevel * 50), 50);
            }
        }

        public List<List<Block>> Grid
        {
            get { return grid; }
            set { grid = value; }
        }

        public Shape FallingShape
        {
            get { return currentFallingShape; }
            set { currentFallingShape = value; }
        }

        public Shape QueueZero
        {
            get { return shapeQueue[0]; }
            set { shapeQueue[0] = value; }
        }

        public Level(PlayerIndex player)
            : this(1, player)
        {

        }

        public Level(int startingLevel, PlayerIndex player)
        {
            //Initialize Grid aka the playing field
            for (int i = 0; i < 22; i++)
            {
                GenerateNewRow();
            }            

            //Set falling peice and queue peices
            currentFallingShape = GenerateRandomShape();
            for (int i = 0; i < 2; i++)
            {
                shapeQueue.Add(GenerateRandomShape());
            }

            //Setup indicators for the falling shape
            SetIndicator();

            //Set up Level
            linesCompleted = 0;
            this.startingLevel = startingLevel;
            fallingTimer = TimeDelayMilliseconds;
            this.player = player;

            if (player == PlayerIndex.One)
            {
                this.position = new Vector2(50, 940) * Game1.displayRatio;
                scoreDisplayPosition = new Vector2(665, 470) * Game1.displayRatio;
                nextBlockDisplayPosition = new Vector2(685, -673) * Game1.displayRatio;
            }
            else if (player == PlayerIndex.Two)
            {
                this.position = new Vector2(1470, 940) * Game1.displayRatio;
                scoreDisplayPosition = new Vector2(1060, 470) * Game1.displayRatio;
                nextBlockDisplayPosition = new Vector2(-325, -673) * Game1.displayRatio;
            }

            //Start game setup
            countDown = 3;
            countDownSize = new Vector2(400, 800);
            countDownRotation = 7 * MathHelper.PiOver2;

            Initialize();
        }

        public Level(NetworkStream netStream)
            : this(PlayerIndex.One)
        {
            //enemyLevel = new Level(PlayerIndex.Two);
            //enemyLevel.scoreDisplayPosition = new Vector2((1060 * Game1.displayRatio.X), (470 * Game1.displayRatio.Y));
            //enemyLevel.nextBlockDisplayPosition = new Vector2(-325, -673) * Game1.displayRatio;
        }

        protected virtual void Initialize()
        {

        }

        public virtual void Update(GameTime gameTime)
        {
            switch (levelState)
            {
                case LevelState.Start: UpdateStart(gameTime);
                    break;
                case LevelState.InGame: UpdateInGame(gameTime);
                    break;
                case LevelState.GameOver: UpdateGameOver(gameTime);
                    break;
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            switch (levelState)
            {
                case LevelState.Start:
                    DrawStart(gameTime, spriteBatch);
                    break;
                case LevelState.InGame:
                    DrawInGame(gameTime, spriteBatch);
                    break;
                case LevelState.GameOver:
                    DrawGameOver(gameTime, spriteBatch);
                    break;
            }
        }

        #region LevelState updates

        protected virtual void UpdateStart(GameTime gameTime)
        {
            //Update Countdown timers
            elapsedCountDownTime += gameTime.ElapsedGameTime.Milliseconds;
            if (elapsedCountDownTime > 1000)
            {
                //reset for next number
                countDown--;
                elapsedCountDownTime = 0;
                countDownSize = new Vector2(400, 800);
                countDownRotation = 7 * MathHelper.PiOver2;
            }
            //Begin game
            if (countDown < 0)
            {
                levelState = LevelState.InGame;
                gameClassReference.OnCountdownFinish(player);
            }

            //Update rotation and size
            if (elapsedCountDownTime >= 500)
            {
                countDownSize = new Vector2(40, 80);
                countDownRotation = 0.0f;
            }
            else
            {
                countDownSize = new Vector2(40, 80) * 10 * (500 - elapsedCountDownTime) / 500;
                countDownRotation = 7 * MathHelper.PiOver2 * (500 - elapsedCountDownTime) / 500;
            }
        }

        protected virtual void UpdateInGame(GameTime gameTime)
        {
            //Update the positions of every block
            UpdateGridPositions(gameTime);

            //Update fall time
            UpdateFallTimer(gameTime);
        }
        
        protected virtual void UpdateGameOver(GameTime gameTime)
        {
            UpdateGridPositions(gameTime);

            //UpdateExplosions(gameTime);

            timeSinceLastBlock += gameTime.ElapsedGameTime.Milliseconds;

            if (timeSinceLastBlock > 150)
            {
                timeSinceLastBlock = 0;

                Block tempBlock = FindNextBlock();
                if (tempBlock == null)
                {
                    if (fallingBlocks.Count == 0)
                        returnToMenu = true;
                }
                else
                {
                    Vector2 velocity = new Vector2();
                    velocity.X = Game1.rnd.Next(-3, 3);
                    velocity.Y = Game1.rnd.Next(-10, 0);
                    fallingBlocks.Add(new FallingObject(tempBlock.blockColor.texture, tempBlock.position, new Vector2((int)(40 * Game1.displayRatio.X), (int)(40 * Game1.displayRatio.Y)), velocity));
                }
            }

            for (int i = 0; i < fallingBlocks.Count; i++)
            {
                fallingBlocks[i].Update(gameTime);
                if (fallingBlocks[i].position.Y > Game1.GameGraphicsDevice.Viewport.Height + 500)
                {
                    fallingBlocks.RemoveAt(i);
                    i--;
                }
            }
        }
        #endregion

        #region InGame update functions

        //Move left
        public virtual void MoveLeft()
        {
            if (levelState == LevelState.InGame)
                if (currentFallingShape.Transform(new Vector2(-1, 0), grid))
                    UpdateIndicator();
        }

        //Move right
        public virtual void MoveRight()
        {
            //if (levelState == LevelState.InGame)
                if (currentFallingShape.Transform(new Vector2(1, 0), grid))
                    UpdateIndicator();
        }

        //Rotate
        public virtual void Rotate()
        {
            if (levelState == LevelState.InGame)
            {
                if (currentFallingShape.Rotate(grid))
                {

                    if (indicatorShape.indicatorShapeIndex == indicatorShape.maxIndicatorIndex)
                        indicatorShape.indicatorShapeIndex = 0;
                    else
                        indicatorShape.indicatorShapeIndex++;

                    UpdateIndicator();
                }
            }
        }
        

        // Drop the falling shape one block
        public virtual void DropOne()
        {
            if (levelState == LevelState.InGame)
            {
                //check movement was valid and don't need to AddShapeToGrid
                if (!currentFallingShape.Transform(new Vector2(0, -1), grid))
                {
                    //reset auto drop timer
                    fallingTimer = TimeDelayMilliseconds;
                }
            }
        }

        // Drop down all the way
        public virtual void DropAll()
        {
            if (levelState == LevelState.InGame)
            {
                bool success;

                //keep trying to drop down one block until it can't anymore
                do
                {
                    success = currentFallingShape.Transform(new Vector2(0, -1), grid);
                    score += 5;
                }
                while (success);

                AddShapeToGrid();
            }
        }

        protected virtual void UpdateFallTimer(GameTime gameTime)
        {
            //Update auto fall timer
            fallingTimer -= gameTime.ElapsedGameTime.Milliseconds;
            if (fallingTimer <= 0)
            {
                //drop shape down and reset autofall timer
                //check if the shape has stuck the bottom
                if (!currentFallingShape.Transform(new Vector2(0, -1), grid))
                {
                    AddShapeToGrid();
                }
                fallingTimer = TimeDelayMilliseconds;
            }
        }

        public virtual void SetIndicator()
        {
            indicatorShape = new IndicatorShape(currentFallingShape);
            UpdateIndicator();
        }

        protected virtual void UpdateIndicator()
        {
            indicatorShape.DeterminePosition(currentFallingShape, grid);
        }

        public virtual void SetIndicatorShapeIndex(int index)
        {
            indicatorShape.indicatorShapeIndex = index;
        }

        protected virtual void DrawIndicator(GameTime gameTime, SpriteBatch spriteBatch)
        {
            indicatorShape.Draw(gameTime, spriteBatch, currentFallingShape, position);
        }
        #endregion

        #region GameOver update functions

        protected virtual void ResetLevel()
        {
            for (int i = 0; i < grid.Count; i++)
                for (int j = 0; j < grid[i].Count; j++)
                    grid[i][j].enabled = false;
            score = 0;
            linesCompleted = 0;
            emptyRows = 0;
            //Reset shape queue and currnet falling shape
            NextFallingShape();
            NextFallingShape();
            levelState = LevelState.Start;
        }

        protected Block FindNextBlock()
        {
            //start from top rows to bottom rows
            for (int i = grid.Count - emptyRows - 1; i >= 0; i--)
            {
                //alternate between left and right searching
                if (reverseSearch)
                {
                    for (int j = grid[i].Count - 1; j >= 0; j--)
                    {
                        if (grid[i][j].enabled)
                        {
                            reverseSearch = !reverseSearch;
                            grid[i][j].enabled = false;
                            grid[i][j].position = new Vector2(position.X + (j * 40 * Game1.displayRatio.X), position.Y - (i * 40 * Game1.displayRatio.Y));
                            return grid[i][j];
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < grid[i].Count; j++)
                    {
                        if (grid[i][j].enabled)
                        {
                            reverseSearch = !reverseSearch;
                            grid[i][j].enabled = false;
                            grid[i][j].position = new Vector2(position.X + (j * 40 * Game1.displayRatio.X), position.Y - (i * 40 * Game1.displayRatio.Y));
                            return grid[i][j];
                        }
                    }
                }

                emptyRows++;
            }

            return null;
        }

        #endregion

        #region LevelState Draws

        protected virtual void DrawStart(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Texture2D countDownTexture = ScoreTextures[(int)player];
            Rectangle drawArea = new Rectangle((int)(position.X + 200 * Game1.displayRatio.X),
                (int)(position.Y - 500 * Game1.displayRatio.Y),
                (int)(countDownSize.X * Game1.displayRatio.X),
                (int)(countDownSize.Y * Game1.displayRatio.Y));
            Vector2 origin = new Vector2(10 * Game1.displayRatio.X,
                20 * Game1.displayRatio.Y);

            spriteBatch.Draw(countDownTexture, drawArea, new Rectangle(1 +(20 * countDown), 0, 18, 40), Color.White, countDownRotation, origin, SpriteEffects.None, 1);
        }

        protected virtual void DrawInGame(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < grid.Count; i++)
            {
                for (int j = 0; j < grid[i].Count; j++)
                {
                    if (grid[i][j].enabled)
                    {
                        grid[i][j].Draw(gameTime, spriteBatch, grid[i][j].position);
                    }
                }
            }

            currentFallingShape.DrawToGrid(gameTime, spriteBatch, position);
            DrawIndicator(gameTime, spriteBatch);

            DrawNextBlock(gameTime, spriteBatch);

            DrawScore(gameTime, spriteBatch);
        }

        protected virtual void DrawGameOver(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //Draw left over blocks
            for (int i = 0; i < grid.Count; i++)
            {
                for (int j = 0; j < grid[i].Count; j++)
                {
                    if (grid[i][j].enabled)
                    {
                        grid[i][j].Draw(gameTime, spriteBatch, grid[i][j].position);
                    }
                }
            }

            foreach (FallingObject f in fallingBlocks)
            {
                f.Draw(gameTime, spriteBatch);
            }

            DrawScore(gameTime, spriteBatch);
            spriteBatch.Draw(endMessage, new Rectangle((int)(((180 + (int)(player) * 960) * Game1.displayRatio.X)),
                                                       (int)(300 * Game1.displayRatio.Y), (int)(600 * Game1.displayRatio.X), (int)(300 * Game1.displayRatio.Y)), Color.White);

        }

        protected virtual void DrawScore(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //Drawing Score
            string scoreString = "" + score;

            //FFFFFFFFFFFFFFFFFIIIIIIIIIIIIIIIIXXXXXXXXXXXXXEEEEEEEEEEEEEEDDDDDDDDDDDDDDDDDDDDD
            Game1.Drawnumbers(scoreString, new Vector2(scoreDisplayPosition.X, scoreDisplayPosition.Y), player, spriteBatch);
        }

        protected virtual void DrawNextBlock(GameTime gameTime, SpriteBatch spriteBatch)
        {
            shapeQueue[0].Draw_Next_Block(gameTime, spriteBatch, position + nextBlockDisplayPosition);
        }

        #endregion

        protected virtual Shape GenerateRandomShape()
        {
            int Random_Color_Element;
            Random_Color_Element = Game1.rnd.Next(0, 8);
            Shape generatedShape = null;

            switch (Game1.rnd.Next(7))
            {
                case 0:
                    generatedShape = new OShape();
                    break;
                case 1:
                    generatedShape = new IShape();
                    break;
                case 2:
                    generatedShape = new SShape();
                    break;
                case 3:
                    generatedShape = new ZShape();
                    break;
                case 4:
                    generatedShape = new LShape();
                    break;
                case 5:
                    generatedShape = new JShape();
                    break;
                case 6:
                    generatedShape = new TShape();
                    break;
            }

            return generatedShape;
        }

        protected virtual void GenerateNewRow()
        {
            grid.Add(new List<Block>());
            for (int j = 0; j < 10; j++)
                grid[grid.Count - 1].Add(new Block(false));
        }

        protected virtual void AddShapeToGrid()
        {
            currentFallingShape.AddShapeToGrid(grid);
            NextFallingShape();
            CheckCompletedRows();
        }

        protected virtual void NextFallingShape()
        {
            //Pull next shape from queue and generate new shape for queue
            currentFallingShape = shapeQueue[0];

            if (currentFallingShape.CheckLoss(grid))
                Lose();

            SetIndicator();
            shapeQueue.RemoveAt(0);
            if (shapeQueue.Count < 2)
            {
                shapeQueue.Add(GenerateRandomShape());
            }

            fallingTimer = TimeDelayMilliseconds;
        }

        protected virtual int CheckCompletedRows()
        {
            int linesCompletedTogether = 0;
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
                    linesCompleted++;
                    linesCompletedTogether++;
                    grid.RemoveAt(i);
                    GenerateNewRow();
                    i--;
                }
            }

            score += (int)Math.Max(0.0, 1000 * linesCompletedTogether + (500 * (linesCompletedTogether - 1)));

            if (linesCompletedTogether > 0)
                UpdateIndicator();

            return linesCompletedTogether;
        }

        public virtual void Win()
        {
            levelState = LevelState.GameOver;
            if(player == PlayerIndex.One)
                endMessage = Game1.GlobalContent.Load<Texture2D>(@"Score Board Numbers/WinRed");
            else
                endMessage = Game1.GlobalContent.Load<Texture2D>(@"Score Board Numbers/WinBlue");
        }

        public virtual void Lose()
        {
            levelState = LevelState.GameOver;
            if (player == PlayerIndex.One)
                endMessage = Game1.GlobalContent.Load<Texture2D>(@"Score Board Numbers/LoseRed");
            else
                endMessage = Game1.GlobalContent.Load<Texture2D>(@"Score Board Numbers/LoseBlue");

            gameClassReference.OnLose(player);
        }

        public virtual void UpdateGridPositions(GameTime gameTime)
        {
            for(int i = 0; i < grid.Count; i++)
                for (int j = 0; j < grid[i].Count; j++)
                {
                    grid[i][j].UpdateGridPosition(gameTime, position, new Vector2(j, i));
                }
        }

        public virtual void FinishEverything()
        {
            returnToMenu = true;
        }
    }
}