using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using FGA.Shapes;
using FGA.Core_Classes;
using FGA.Online;
using FGA.TetrisAI;
using FGA.Backgrounds;
using FGA.Steam;
using FGA.GameClasses;
using Steamworks;

namespace FGA
{
    public enum Games { Classic, OnlineClassic, Tug, OnlineTug, Line, OnlineLine, Pick, OnlinePick, Switch, OnlineSwitch, Factory, Nightmare, OnlineNightmare, DoubleTrouble, OnlineDoubleTrouble };
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        // Constants
        public static AppId_t APPID = (AppId_t)509150;      // Steam Game ID

        public enum GameState
        {
            Menu,
            Game,
            Credits
        };

        public static GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GameState gameState = GameState.Menu;
        //Background
        Background currentBackground;
        Texture2D background;
        Menu menu;

        public static Random rnd = new Random();
        public static ContentManager GlobalContent;
        public static Game1 game;

        //Game Variables
        GameClass thisIsOurGame;

        //sound variables
        public static float SFX_Volume;
        public static float Music_Volume;
        public static bool isMuted;

        public static Vector2 displayRatio;
        public static GraphicsDevice GameGraphicsDevice;

        public static bool isFullscreen
        {
            get
            {
                return isFullscreen;
            }
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            game = this;

            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            //graphics.ToggleFullScreen();


            //IntPtr hWnd = this.Window.Handle;
            //var control = System.Windows.Forms.Control.FromHandle(hWnd);
            //var form = control.FindForm();
            //form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;

            //graphics.PreferredBackBufferWidth = 1200;
            //graphics.PreferredBackBufferHeight = 1000;

            displayRatio.X = (float)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 1920.0);
            displayRatio.Y = (float)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 1080.0);
            //displayRatio.X = (float)(graphics.PreferredBackBufferWidth / 1920.0);
            //displayRatio.Y = (float)(graphics.PreferredBackBufferHeight/ 1080.0);

            IsMouseVisible = true;

            Steam_Manager.SetOverlordReference(this);
        }

        protected override void Initialize()
        {
            base.Initialize();

        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            GlobalContent = Content;
            //background = Content.Load<Texture2D>(@"Background Screens/SmithDynasty");
            currentBackground = new SplashBackground();

            menu = new Menu();
            menu.SetOverlordReference(this);

            BlockColors.Initialize();

            //Initialize Game Score Textures
            Level.ScoreTextures[0] = Content.Load<Texture2D>(@"Score Board Numbers/Red_Numbers");
            Level.ScoreTextures[1] = Content.Load<Texture2D>(@"Score Board Numbers/Blue_Numbers");

            LineEmUp.colorChances[0] = .20;
            LineEmUp.colorChances[1] = .20;
            LineEmUp.colorChances[2] = .20;
            LineEmUp.colorChances[3] = .20;
            LineEmUp.colorChances[4] = .10;
            LineEmUp.colorChances[5] = .10;

            GameGraphicsDevice = GraphicsDevice;

            isMuted = false;

            //initialize the default set of controls
            Input.SetDefaultControls();
        }

        protected override void UnloadContent()
        {
            
        }

        protected override void Update(GameTime gameTime)
        {
            // Steam Update
            Steam_Manager.Update();

            currentBackground.Update(gameTime);

            //Update input
            Input.Update(gameTime);
            
            switch (gameState)
            {
                case GameState.Menu:
                    menu.Update(gameTime);

                    break;
                case GameState.Game:

                    thisIsOurGame.Update(gameTime);

                    break;
            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //Draw background
            spriteBatch.Begin();
            //spriteBatch.Draw(background, GraphicsDevice.Viewport.Bounds, Color.White);
            currentBackground.Draw(gameTime, spriteBatch);
            spriteBatch.End();

            switch (gameState)
            {
                case GameState.Menu:
                    spriteBatch.Begin();
                    menu.Draw(spriteBatch);
                    spriteBatch.End();

                    break;
                case GameState.Game:
                    spriteBatch.Begin();
                    thisIsOurGame.Draw(gameTime, spriteBatch);
                    //PlayerOne.Draw(gameTime, spriteBatch);
                    //PlayerTwo.Draw(gameTime, spriteBatch);
                    //if (levelP1 != null)
                    //    levelP1.Draw(gameTime, spriteBatch);
                    //if (levelP2 != null)
                    //    levelP2.Draw(gameTime, spriteBatch);
                    spriteBatch.End();

                    //if (levelP1 != null && !levelP1.isPaused)
                    //    levelP1.DrawExplosions();
                    //if (levelP2 != null && !levelP2.isPaused)
                    //    levelP2.DrawExplosions();


                    break;
            }
           

            base.Draw(gameTime);
        }

        public void CreateGame(Games newGameMode, bool multiplayer)
        {
            gameState = GameState.Game;

            switch (newGameMode)
            {
                case Games.Classic:
                    currentBackground = new LineEmUpBackground();

                    thisIsOurGame = new SinglePlayerGame();
                    thisIsOurGame.CreateGame(Games.Classic);

                    break;
                case Games.Tug:
                    currentBackground = new TugOfWarBackground();

                    thisIsOurGame = new LocalMultiplayerGame();
                    thisIsOurGame.CreateGame(Games.Tug);

                    break;
                case Games.Line:
                    currentBackground = new LineEmUpBackground();

                    thisIsOurGame = new LocalMultiplayerGame();
                    thisIsOurGame.CreateGame(Games.Line);

                    break;
                case Games.Pick:
                    currentBackground = new PickNDropBackground();

                    thisIsOurGame = new LocalMultiplayerGame();
                    thisIsOurGame.CreateGame(Games.Pick);

                    break;
                case Games.Switch:
                    currentBackground = new SwitcharooBuckarooBackground();

                    thisIsOurGame = new LocalMultiplayerGame();
                    thisIsOurGame.CreateGame(Games.Switch);

                    break;
                case Games.Factory:
                    currentBackground = new BlockFactoryBackground();

                    thisIsOurGame = new SinglePlayerGame();
                    thisIsOurGame.CreateGame(Games.Factory);
                    break;
                case Games.Nightmare:
                    currentBackground = new NightmareBackground();

                    thisIsOurGame = new LocalMultiplayerGame();
                    thisIsOurGame.CreateGame(Games.Nightmare);
                    
                    break;
                case Games.OnlineClassic:
                    currentBackground = new LineEmUpBackground();


                    
                    break;
                case Games.OnlineTug:
                    currentBackground = new TugOfWarBackground();

                    thisIsOurGame = new OnlineMultiplayerGame();
                    thisIsOurGame.CreateGame(Games.OnlineTug);

                    break;
                case Games.OnlineLine:
                    currentBackground = new LineEmUpBackground();

                    thisIsOurGame = new OnlineMultiplayerGame();
                    thisIsOurGame.CreateGame(Games.OnlineLine);

                    break;
                case Games.OnlinePick:
                    currentBackground = new PickNDropBackground();

                    thisIsOurGame = new OnlineMultiplayerGame();
                    thisIsOurGame.CreateGame(Games.OnlinePick);

                    break;
                case Games.OnlineSwitch:
                    currentBackground = new SwitcharooBuckarooBackground();

                    thisIsOurGame = new OnlineMultiplayerGame();
                    thisIsOurGame.CreateGame(Games.OnlineSwitch);

                    break;
                case Games.DoubleTrouble:
                    currentBackground = new DoubleTroubleBackground();

                    thisIsOurGame = new SinglePlayerGame();
                    thisIsOurGame.CreateGame(Games.DoubleTrouble);

                    break;
                default:
                    gameState = GameState.Menu;
                    break;
            }

            thisIsOurGame.SetOverlordReference(this);

        }

        public void ReturnToMenu()
        {
            gameState = GameState.Menu;
            currentBackground = new MainMenuBackground();
        }

        public static void toggleFullScreen()
        {
            graphics.ToggleFullScreen();
        }

        public static void toggleBorderless()
        {
            if (graphics.IsFullScreen)
                graphics.ToggleFullScreen();

            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;

            IntPtr hWnd = game.Window.Handle;
            var control = System.Windows.Forms.Control.FromHandle(hWnd);
            var form = control.FindForm();
            form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;

            displayRatio.X = (float)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 1920.0);
            displayRatio.Y = (float)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 1080.0);
        }

        public static void Drawnumbers(Texture2D texture, string Number, Vector2 Position, SpriteBatch spriteBatch)
        {
            int index;
            Rectangle Reference_Rectangle;
            for (int Q = 0; Q < Number.Length; Q++)
            {
                if (Number[Q] != '.')//will break if this if statement isnt here, because a period isnt a number
                {
                    index = (int)Char.GetNumericValue(Number[Q]);
                    Reference_Rectangle = new Rectangle(20 * index, 0, 20, 40);
                }
                else
                    Reference_Rectangle = new Rectangle(200, 0, 20, 40);

                spriteBatch.Draw(texture, new Rectangle((int)(Position.X + (21 * Q * Game1.displayRatio.X)), (int)(Position.Y), (int)(20 * Game1.displayRatio.X), (int)(40 * Game1.displayRatio.Y)),
                                 Reference_Rectangle, Color.White);
                
            }
        }

        public static void Drawnumbers(string Number, Vector2 Position, PlayerIndex player, SpriteBatch spriteBatch)
        {
            Drawnumbers(Level.ScoreTextures[(int)player], Number, Position, spriteBatch);
        }

        public void OnFinishSplashScreen()
        {
            currentBackground = new MainMenuBackground();
        }

        public void OnBeginExitGame()
        {
            Steam_Manager.Unload();
            this.Exit();
        }

        public GameClass getGameClass()
        {
            return thisIsOurGame;
        }

        public GameState getGameState()
        {
            return gameState;
        }

        public Menu getMenu()
        {
            return menu;
        }

        public void OnDisconnect()
        {
            if ((gameState == GameState.Game && thisIsOurGame is OnlineMultiplayerGame) || gameState == GameState.Menu)
            {
                gameState = GameState.Menu;
                menu.OnDisconnect();
            }
        }
    }
}
