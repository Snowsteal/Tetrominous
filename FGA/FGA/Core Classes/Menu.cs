using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using FGA.Online;
using FGA.Steam;
using Steamworks;


namespace FGA.Core_Classes
{
    public class Menu
    {
        private enum MenuState { Main, Credits, Options, Single, Multi, Local, Online, Host, Join, Controls, Intro, Game_Description, OnlineHostLobby, OnlineClientLobby, Disconnect };

        /// <summary>
        /// Refers to the main Game1 class and is basically a connection that allows this class to call functions such as begin game and such.
        /// This helps by allowing us to not have to use flags and also not have Game1 constantly check on the flags, very slight perfermance thing
        /// </summary>
        Game1 overlordReference;

        //Menu variables
        #region Buttons
        Button exitGameButton;
        Button tugOfWarButton;
        Button lineEmUpButton;
        Button pickNDropButton;
        Button tetrisInvadersButton;
        Button tetrisFactoryButton;
        Button SwitcharooBuckarooButton;
        Button backButton;
        Button singleButton;
        Button multiButton;
        Button localButton;
        Button onlineButton;
        Button hostButton;
        Button joinButton;
        Button optionsButton;
        Button creditsButton;
        Button controlsButton;
        Button spookyButton;
        Button classicButton;
        Button dualHandjobsButton;
        Button button_Invite;
        List<Button> ControlsButtons = new List<Button>(0);
        MenuState menuState;

        #endregion

        int creditsTimer;
        Texture2D Opacity_effect;
        Texture2D Enter_IP_Box, WaitingConnection;
        Texture2D Numbers;
        Texture2D Tug_Desc, TF_Desc, Pick_Desc, Line_Desc, Switch_Desc, Spook_Desc;
        Rectangle Description_Position;
        Texture2D Options_Screen;
        Texture2D Credits_Screen;
        Texture2D Controls_Screen;
        Track_Bar SFX;
        Track_Bar Music;
        Track_Bar AI;
        Check_Box checkbox_Mute, checkbox_FullScreen;
        public bool Await_Key_Press;
        public string Button_Control_Squad;
        public int Index_Control_Squad, AIlevel;
        bool sameKeyAlreadyBound;
        Texture2D[] AI_Names = new Texture2D[11];
        string yolo;

        //Online Variables
        SpriteFont font;
        CSteamID lobbyId;
        string lobbyName;
        bool connectedToOtherPlayer;

        public Menu()
        {
            //Initialize Buttons
            #region buttons
            exitGameButton = new Button(Game1.GlobalContent.Load<Texture2D>(@"Buttons & Cursor/Exit_Button"), Vector2.Zero, new Vector2(300, 150) * Game1.displayRatio);
            tugOfWarButton = new Button(Game1.GlobalContent.Load<Texture2D>(@"Buttons & Cursor/Tugofwar_Button"), Vector2.Zero, new Vector2(300, 150) * Game1.displayRatio);
            lineEmUpButton = new Button(Game1.GlobalContent.Load<Texture2D>(@"Buttons & Cursor/Lineemup_Button"), Vector2.Zero, new Vector2(300, 150) * Game1.displayRatio);
            pickNDropButton = new Button(Game1.GlobalContent.Load<Texture2D>(@"Buttons & Cursor/PicknDrop_Button"), Vector2.Zero, new Vector2(300, 150) * Game1.displayRatio);
            tetrisInvadersButton = new Button(Game1.GlobalContent.Load<Texture2D>(@"Buttons & Cursor/Tug oF War Button"), Vector2.Zero, new Vector2(300, 150) * Game1.displayRatio);
            tetrisFactoryButton = new Button(Game1.GlobalContent.Load<Texture2D>(@"Buttons & Cursor/TetrisFactoryButton"), Vector2.Zero, new Vector2(300, 150) * Game1.displayRatio);
            SwitcharooBuckarooButton = new Button(Game1.GlobalContent.Load<Texture2D>(@"Buttons & Cursor/SwitcharooBuckaroo_Button"), Vector2.Zero, new Vector2(300, 150) * Game1.displayRatio);
            backButton = new Button(Game1.GlobalContent.Load<Texture2D>(@"Buttons & Cursor/Back_Button"), Vector2.Zero, new Vector2(300, 150) * Game1.displayRatio);
            singleButton = new Button(Game1.GlobalContent.Load<Texture2D>(@"Buttons & Cursor/Singleplayer_Button"), Vector2.Zero, new Vector2(300, 150) * Game1.displayRatio);
            multiButton = new Button(Game1.GlobalContent.Load<Texture2D>(@"Buttons & Cursor/MultiplayerButton"), Vector2.Zero, new Vector2(300, 150) * Game1.displayRatio);
            localButton = new Button(Game1.GlobalContent.Load<Texture2D>(@"Buttons & Cursor/Local_Button"), Vector2.Zero, new Vector2(300, 150) * Game1.displayRatio);
            onlineButton = new Button(Game1.GlobalContent.Load<Texture2D>(@"Buttons & Cursor/Online_Button"), Vector2.Zero, new Vector2(300, 150) * Game1.displayRatio);
            hostButton = new Button(Game1.GlobalContent.Load<Texture2D>(@"Buttons & Cursor/Host_Button"), Vector2.Zero, new Vector2(300, 150) * Game1.displayRatio);
            joinButton = new Button(Game1.GlobalContent.Load<Texture2D>(@"Buttons & Cursor/Join_Button"), Vector2.Zero, new Vector2(300, 150) * Game1.displayRatio);
            optionsButton = new Button(Game1.GlobalContent.Load<Texture2D>(@"Buttons & Cursor/OptionsButton"), Vector2.Zero, new Vector2(300, 150) * Game1.displayRatio);
            creditsButton = new Button(Game1.GlobalContent.Load<Texture2D>(@"Buttons & Cursor/Credits_Button"), Vector2.Zero, new Vector2(300, 150) * Game1.displayRatio);
            controlsButton = new Button(Game1.GlobalContent.Load<Texture2D>(@"Buttons & Cursor/Controls_Button"), Vector2.Zero, new Vector2(300, 150) * Game1.displayRatio);
            spookyButton = new Button(Game1.GlobalContent.Load<Texture2D>(@"Buttons & Cursor/NightmareButton"), Vector2.Zero, new Vector2(300, 150) * Game1.displayRatio);
            classicButton = new Button(Game1.GlobalContent.Load<Texture2D>(@"Buttons & Cursor/Classic_Button"), Vector2.Zero, new Vector2(300, 150) * Game1.displayRatio);
            dualHandjobsButton = new Button(Game1.GlobalContent.Load<Texture2D>(@"Buttons & Cursor/Dual Hand Jobs Button"), Vector2.Zero, new Vector2(300, 150) * Game1.displayRatio);
            button_Invite = new Button(Game1.GlobalContent.Load<Texture2D>(@"Buttons & Cursor/Invite_Button"));
            #endregion

            // Tetxures and description textures
            #region textures
            Opacity_effect = Game1.GlobalContent.Load<Texture2D>(@"Background Screens/Black_Square");
            Enter_IP_Box = Game1.GlobalContent.Load<Texture2D>(@"Buttons & Cursor/Enter_IP_Box");
            WaitingConnection = Game1.GlobalContent.Load<Texture2D>(@"Buttons & Cursor/WaitingConnection");
            Numbers = Game1.GlobalContent.Load<Texture2D>(@"Score Board Numbers/White_Numbers");
            Options_Screen = Game1.GlobalContent.Load<Texture2D>(@"Background Screens/Options_Screen");
            Credits_Screen = Game1.GlobalContent.Load<Texture2D>(@"Background Screens/Credits_Screen");
            Controls_Screen = Game1.GlobalContent.Load<Texture2D>(@"Background Screens/Controls_Screen");
            TF_Desc = Game1.GlobalContent.Load<Texture2D>(@"Background Screens/Tetris_Factory_Description");
            Tug_Desc = Game1.GlobalContent.Load<Texture2D>(@"Background Screens/Tug_of_War_Description");
            Line_Desc = Game1.GlobalContent.Load<Texture2D>(@"Background Screens/Line_Em_Up_Description");
            Pick_Desc = Game1.GlobalContent.Load<Texture2D>(@"Background Screens/Pick_n_Drop_Description");
            Switch_Desc = Game1.GlobalContent.Load<Texture2D>(@"Background Screens/Switcharoo_Buckaroo_Description");
            Spook_Desc = Game1.GlobalContent.Load<Texture2D>(@"Background Screens/SpookyDescription");
            #endregion

            SFX = new Track_Bar(100, 400);
            SFX.ballLocation = new Rectangle((int)(672 * Game1.displayRatio.X), (int)(346 * Game1.displayRatio.Y), (int)(13 * Game1.displayRatio.X), (int)(26 * Game1.displayRatio.Y));
            SFX.floatballLocation = new Vector2((672 * Game1.displayRatio.X), (346 * Game1.displayRatio.Y));
            SFX.startLocation = new Vector2(272, 346) * Game1.displayRatio;
            Music = new Track_Bar(100, 400);
            Music.ballLocation = new Rectangle((int)(672 * Game1.displayRatio.X), (int)(495 * Game1.displayRatio.Y), (int)(13 * Game1.displayRatio.X), (int)(26 * Game1.displayRatio.Y));
            Music.floatballLocation = new Vector2((672 * Game1.displayRatio.X), (495 * Game1.displayRatio.Y));
            Music.startLocation = new Vector2(272, 495) * Game1.displayRatio;
            checkbox_FullScreen = new Check_Box(new Rectangle((int)(596*Game1.displayRatio.X), (int)(627*Game1.displayRatio.Y), (int)(22*Game1.displayRatio.X), (int)(22*Game1.displayRatio.Y)));
            checkbox_Mute = new Check_Box(new Rectangle((int)(501 * Game1.displayRatio.X), (int)(568 * Game1.displayRatio.Y), (int)(22 * Game1.displayRatio.X), (int)(22 * Game1.displayRatio.Y)));
            Description_Position = new Rectangle((int)(1157 * Game1.displayRatio.X), (int)(650 * Game1.displayRatio.Y), (int)(565 * Game1.displayRatio.X), (int)(214 * Game1.displayRatio.Y));

            font = Game1.GlobalContent.Load<SpriteFont>("text");

            for (int Q = 0; Q < 13; Q++)
            {
                ControlsButtons.Add(new Button(Game1.GlobalContent.Load<Texture2D>(@"Textures/CheckBox"), Vector2.Zero, new Vector2(22,22) * Game1.displayRatio));
            }

            AI_Names[0] = Game1.GlobalContent.Load<Texture2D>(@"Textures/AI NAMES/jaden");
            AI_Names[1] = Game1.GlobalContent.Load<Texture2D>(@"Textures/AI NAMES/JoeEddy");
            AI_Names[2] = Game1.GlobalContent.Load<Texture2D>(@"Textures/AI NAMES/Enrique");
            AI_Names[3] = Game1.GlobalContent.Load<Texture2D>(@"Textures/AI NAMES/Angus");
            AI_Names[4] = Game1.GlobalContent.Load<Texture2D>(@"Textures/AI NAMES/Wallace");
            AI_Names[5] = Game1.GlobalContent.Load<Texture2D>(@"Textures/AI NAMES/Vladimir");
            AI_Names[6] = Game1.GlobalContent.Load<Texture2D>(@"Textures/AI NAMES/Darnell");
            AI_Names[7] = Game1.GlobalContent.Load<Texture2D>(@"Textures/AI NAMES/Yuan");
            AI_Names[8] = Game1.GlobalContent.Load<Texture2D>(@"Textures/AI NAMES/Roderick");
            AI_Names[9] = Game1.GlobalContent.Load<Texture2D>(@"Textures/AI NAMES/Harold");
            AI_Names[10] = Game1.GlobalContent.Load<Texture2D>(@"Textures/AI NAMES/HaroldAscended");

            AI = new Track_Bar(9, 400);
            AI.ballLocation = new Rectangle((int)(1640 * Game1.displayRatio.X), (int)(475 * Game1.displayRatio.Y), (int)(13 * Game1.displayRatio.X), (int)(26 * Game1.displayRatio.Y));
            AI.floatballLocation = new Vector2((1640 * Game1.displayRatio.X), (475 * Game1.displayRatio.Y));
            AI.startLocation = new Vector2(1240, 475) * Game1.displayRatio;
            AIlevel = 9;

            menuState = MenuState.Intro;
            DisableButtons();
            //SetMain();
        }

        public void SetOverlordReference(Game1 gameReference)
        {
            overlordReference = gameReference;
        }

        public void Update(GameTime gameTime)
        {
           /*
            *The Menu works by having a state for each of the screens that it can be (like options or singleplayer screens). When a button is pressed to change screens, the set function for that 
            *screen is called, placing the buttons and enabling the buttons that should be on screen. The menu state is also changed so that the correct page can be displayed.
            *In the update function, the buttons are checked to see if they were clicked on, and if so they respond accordingly.
            */

            switch (menuState)
            {
                case MenuState.Main: UpdateMain(gameTime);
                    break;
                case MenuState.Intro: UpdateIntro(gameTime);
                    break;
                case MenuState.Options: UpdateOptions(gameTime);
                    break;
                case MenuState.Single: UpdateSingle(gameTime);
                    break;
                case MenuState.Multi: UpdateMulti(gameTime);
                    break;
                case MenuState.Local: UpdateLocal(gameTime);
                    break;
                case MenuState.Online: UpdateOnline(gameTime);
                    break;
                case MenuState.Host: UpdateHost(gameTime);
                    break;
                case MenuState.Join: UpdateJoin(gameTime);
                    break;
                case MenuState.Credits: UpdateCredits(gameTime);
                    break;
                case MenuState.Controls: UpdateControls(gameTime);
                    break;
                case MenuState.OnlineHostLobby: UpdateOnlineHostLobby(gameTime);
                    break;
                case MenuState.OnlineClientLobby: UpdateOnlineClientLobby(gameTime);
                    break;
                case MenuState.Disconnect: UpdateDisconnect(gameTime);
                    break;
            }
        }

        #region misc
        private void UpdateIntro(GameTime gameTime)
        {
            creditsTimer += gameTime.ElapsedGameTime.Milliseconds;
            if (creditsTimer > 2500)
            {
                overlordReference.OnFinishSplashScreen();
                menuState = MenuState.Main;
                SetMain();
            }
        }

        private void UpdateCredits(GameTime gameTime)
        {
            backButton.Update(gameTime);
            if (backButton.IsClicked)
            {
                menuState = MenuState.Options;
                SetOptions();
            }
        }

        private void SetCredits()
        {
            DisableButtons();
            backButton.enabled = true;
            backButton.position = new Vector2(1290, 465) * Game1.displayRatio;
        }

        private void UpdateControls(GameTime gameTime)
        {            
            Keys[] Pressed_Keys = new Keys[1];
            Keys sameKey, tempKey;

            //variables used for switching
            string Temp = "";
            bool flag1, flag2;

            backButton.Update(gameTime);


            if (Await_Key_Press)
            {
                sameKeyAlreadyBound = false;
                flag1 = false;
                flag2 = false;
                if (Input.prevKeyboardState != Input.keyboardState)
                {
                    Pressed_Keys = Input.keyboardState.GetPressedKeys();
                    //look for comments in P2 Controls cause basically same code, kinda.
                    #region checking for duplicates and switching in P1 Controls
                    foreach (Keys key in Input.ControlsP1.Values)
                    {                      
                        if (Pressed_Keys[0] == key)
                        {
                            sameKeyAlreadyBound = true;
                            sameKey = key;
                            foreach (string Control in Input.ControlsP1.Keys)
                            {
                                if (Input.ControlsP1[Control] == sameKey)
                                {
                                    Temp = Control;
                                    flag1 = true;
                                }
                            }
                            break;
                        }
                    }
                    if (flag1)
                    {
                        if (Index_Control_Squad < 6)//find which control set to put the same key to
                        {
                            tempKey = Input.ControlsP1[Button_Control_Squad];//set it to a temp variable
                            Input.ControlsP1[Button_Control_Squad] = Input.ControlsP1[Temp];
                        }
                        else if (Index_Control_Squad < 12)
                        {
                            tempKey = Input.ControlsP2[Button_Control_Squad];//set it to a temp variable
                            Input.ControlsP2[Button_Control_Squad] = Input.ControlsP1[Temp];
                        }
                        else
                        {
                            tempKey = Input.ControlsP1["Pause"];//set it to a temp variable
                            Input.ControlsP1["Pause"] = Input.ControlsP1[Temp];//should be the same as P2
                        }
                        Input.ControlsP1[Temp] = tempKey;
                    }
                    #endregion//refer to region below for comments cause /basically/ same code
                    #region checking for duplicates and switching in P2 Controls
                    if (!flag1)//needed because if P2's controls were switched with P1, the above foreach would do it, and then this one would re-do the switch, in return doing nothing.
                    {
                        foreach (Keys key in Input.ControlsP2.Values)//look within P2 Controls KEY BINDINGS, the keys themselves
                        {
                            if (Pressed_Keys[0] == key)//if the player pressed down a key already in the controls
                            {
                                sameKeyAlreadyBound = true;
                                sameKey = key;
                                foreach (string Control in Input.ControlsP2.Keys)//now look for the CONTROL BINDINGS, the strings that correlate to the key bindings
                                {
                                    if (Input.ControlsP2[Control] == sameKey && Control != "Pause")//if the Control Binding is found to have the same key
                                    {
                                        Temp = Control;//used for switching
                                        flag2 = true;//used for switching
                                        //by now, we know theres a key already apparent in the P2 controls that is equivalent to a key that the player pressed down, sameKey
                                        //and we know WHERE that sameKey is in the dictionary, by using Temp
                                        //so we needed 2 foreach loops for the Values and Keys of the dictionary to pinpoint WHAT KEY NEEDS TO BE SWITCHED WITH THE PLAYERS INPUT
                                    }
                                }
                                break;
                            }
                        }
                        if (flag2)//if the flag was thrown up IE a key needs to be switched with the PLAYERS INPUT
                        {
                            if (Index_Control_Squad < 6)//find which control set to put the same key to by using the Index_Control_Squad (found by which button they press which correlates to a specific control)
                            {
                                // < 6 correlates to P1's Controls
                                tempKey = Input.ControlsP1[Temp];//set it to a temp variable
                                Input.ControlsP1[Button_Control_Squad] = Input.ControlsP2[Temp];
                            }
                            else if (Index_Control_Squad < 12)
                            {
                                // < 12 correlates to P2's controls
                                tempKey = Input.ControlsP2[Temp];//set it to a temp variable
                                Input.ControlsP2[Button_Control_Squad] = Input.ControlsP2[Temp];
                            }
                            else
                            {
                                // anything else, which is 12 exactly, correlates to the Pause control.
                                tempKey = Input.ControlsP1["Pause"];//set it to a temp variable
                                Input.ControlsP1["Pause"] = Input.ControlsP2[Temp];
                            }
                            Input.ControlsP2[Temp] = tempKey;

                        }
                    }
                    #endregion

                    //if the above code has not found a similar key IE the PLAYER INPUT is unique, then we can simply set the key binding to the PLAYER INPUT
                    if (Index_Control_Squad < 6 && !sameKeyAlreadyBound)
                        Input.ControlsP1[Button_Control_Squad] = Pressed_Keys[0];//gets the very first key pressed
                    else if (Index_Control_Squad < 12 && !sameKeyAlreadyBound)
                        Input.ControlsP2[Button_Control_Squad] = Pressed_Keys[0];//gets the very first key pressed
                    else if(!sameKeyAlreadyBound)
                    {
                        Input.ControlsP1["Pause"] = Pressed_Keys[0];//gets the very first key pressed
                    }


                    Await_Key_Press = false;
                    
                }
            }
            else
            {
                /*GUIDE FOR CONTROLBUTTONS
                * 0 - P1 Left
                * 1 - P1 Right
                * 2 - P1 Rotate
                * 3 - P1 Down
                * 4 - P1 Place Block
                * 5 - P1 Hold
                * 6 - P2 Left
                * 7 - P2 Right
                * 8 - P2 Rotate
                * 9 - P2 Down
                * 10 - P2 Place Block
                * 11 - P2 Hold
                * 12 - Pause
                */
                for (int Q = 0; Q < ControlsButtons.Count; Q++)
                {
                    ControlsButtons[Q].Update(gameTime);
                }
                #region Controls Buttons
                if (ControlsButtons[0].IsClicked)
                {
                    Await_Key_Press = true;
                    Button_Control_Squad = "Left";
                    Index_Control_Squad = 0;
                }
                if (ControlsButtons[1].IsClicked)
                {
                    Await_Key_Press = true;
                    Button_Control_Squad = "Right";
                    Index_Control_Squad = 1;
                }
                if (ControlsButtons[2].IsClicked)
                {
                    Await_Key_Press = true;
                    Button_Control_Squad = "Rotate";
                    Index_Control_Squad = 2;
                }
                if (ControlsButtons[3].IsClicked)
                {
                    Await_Key_Press = true;
                    Button_Control_Squad = "Down";
                    Index_Control_Squad = 3;
                }
                if (ControlsButtons[4].IsClicked)
                {
                    Await_Key_Press = true;
                    Button_Control_Squad = "Place";
                    Index_Control_Squad = 4;
                }
                if (ControlsButtons[5].IsClicked)
                {
                    Await_Key_Press = true;
                    Button_Control_Squad = "Hold";
                    Index_Control_Squad = 5;
                }
                if (ControlsButtons[6].IsClicked)
                {
                    Await_Key_Press = true;
                    Button_Control_Squad = "Left";
                    Index_Control_Squad = 6;
                }
                if (ControlsButtons[7].IsClicked)
                {
                    Await_Key_Press = true;
                    Button_Control_Squad = "Right";
                    Index_Control_Squad = 7;
                }
                if (ControlsButtons[8].IsClicked)
                {
                    Await_Key_Press = true;
                    Button_Control_Squad = "Rotate";
                    Index_Control_Squad = 8;
                }
                if (ControlsButtons[9].IsClicked)
                {
                    Await_Key_Press = true;
                    Button_Control_Squad = "Down";
                    Index_Control_Squad = 9;
                }
                if (ControlsButtons[10].IsClicked)
                {
                    Await_Key_Press = true;
                    Button_Control_Squad = "Place";
                    Index_Control_Squad = 10;
                }
                if (ControlsButtons[11].IsClicked)
                {
                    Await_Key_Press = true;
                    Button_Control_Squad = "Hold";
                    Index_Control_Squad = 11;
                }
                if (ControlsButtons[12].IsClicked)
                {
                    Await_Key_Press = true;
                    Button_Control_Squad = "Pause";
                    Index_Control_Squad = 12;
                }
                #endregion
            }


            if (backButton.IsClicked)
            {
                menuState = MenuState.Options;
                SetOptions();
                Await_Key_Press = false;
            }
        }

        private void SetControls()
        {
            DisableButtons();
            backButton.enabled = true;
            backButton.position = new Vector2(1290, 465) * Game1.displayRatio;
            /*GUIDE FOR CONTROLBUTTONS
             * 0 - P1 Left
             * 1 - P1 Right
             * 2 - P1 Rotate
             * 3 - P1 Down
             * 4 - P1 Place Block
             * 5 - P1 Hold
             * 6 - P2 Left
             * 7 - P2 Right
             * 8 - P2 Rotate
             * 9 - P2 Down
             * 10 - P2 Place Block
             * 11 - P2 Hold
             * 12 - Pause
             */

            for (int Q = 0; Q < 6; Q++)//P1 Buttons
            {
                ControlsButtons[Q].enabled = true;
                ControlsButtons[Q].position = new Vector2(370, 275 + (40 * (Q + 1))) * Game1.displayRatio;
            }
            for (int Q = 6; Q < 12; Q++)//P2 buttons
            {
                ControlsButtons[Q].enabled = true;
                ControlsButtons[Q].position = new Vector2(660, 275 + (40 * (Q - 5))) * Game1.displayRatio;
            }
            ControlsButtons[12].enabled = true;//pause button
            ControlsButtons[12].position = new Vector2(502, 600) * Game1.displayRatio;
        }

        private void UpdateOptions(GameTime gameTime)
        {
            creditsButton.Update(gameTime);
            controlsButton.Update(gameTime);
            backButton.Update(gameTime);
            Game1.SFX_Volume = SFX.Update() / 100;
            Game1.Music_Volume = Music.Update() / 100;

            checkbox_Mute.IsMouseOver();
            checkbox_Mute.CheckClicked();
            if (checkbox_Mute.justclick)
            {
                Game1.isMuted = !Game1.isMuted;
                checkbox_Mute.justclick = false;
            }
            



            checkbox_FullScreen.IsMouseOver();
            checkbox_FullScreen.CheckClicked();
            if (checkbox_FullScreen.justclick)
            {
                Game1.toggleFullScreen();
                checkbox_FullScreen.justclick = false;
            }

            if (creditsButton.IsClicked)
            {
                menuState = MenuState.Credits;
                SetCredits();
            }
            if (backButton.IsClicked)
            {
                menuState = MenuState.Main;
                SetMain();
            }
            if (controlsButton.IsClicked)
            {
                menuState = MenuState.Controls;
                SetControls();
            }
        }

        private void SetOptions()
        {
            DisableButtons();
            creditsButton.enabled = true;
            creditsButton.position = new Vector2(140, 750) * Game1.displayRatio;
            controlsButton.enabled = true;
            controlsButton.position = new Vector2(520, 750) * Game1.displayRatio;
            backButton.enabled = true;
            backButton.position = new Vector2(1290, 465) * Game1.displayRatio;
        }

        #endregion

        private void UpdateMain(GameTime gameTime)
        {
            singleButton.Update(gameTime);
            multiButton.Update(gameTime);
            optionsButton.Update(gameTime);
            exitGameButton.Update(gameTime);

            if (singleButton.IsClicked)
            {
                menuState = MenuState.Single;
                SetSingle();
            }

            
            if (multiButton.IsClicked)
            {
                menuState = MenuState.Multi;
                SetMulti();
            }

            if (optionsButton.IsClicked)
            {
                menuState = MenuState.Options;
                SetOptions();
            }

            if (exitGameButton.IsClicked)
            {
                overlordReference.OnBeginExitGame();
            }
            
        }

        private void SetMain()
        {
            DisableButtons();
            singleButton.enabled = true;
            singleButton.position = new Vector2(810, 410) * Game1.displayRatio;
            multiButton.enabled = true;
            multiButton.position = new Vector2(810, 565) * Game1.displayRatio;
            optionsButton.enabled = true;
            optionsButton.position = new Vector2(810, 720) * Game1.displayRatio;
            exitGameButton.enabled = true;
            exitGameButton.position = new Vector2(810, 875) * Game1.displayRatio;
        }

        private void UpdateSingle(GameTime gameTime)
        {
            tugOfWarButton.Update(gameTime);
            lineEmUpButton.Update(gameTime);
            pickNDropButton.Update(gameTime);
            SwitcharooBuckarooButton.Update(gameTime);
            tetrisFactoryButton.Update(gameTime);
            backButton.Update(gameTime);
            spookyButton.Update(gameTime);

            AI.UpdateMovement();
            if (AI.movingTrack)
            {
                AIlevel = AI.Update() / 10;
                if (AIlevel == 10)
                    AIlevel = 9;
            }

            yolo += Input.GetKeys();
            if (yolo == "THESpaceLEGEND" || yolo == "THESPACELEGEND")
            {
                yolo = "";
                AIlevel = 10;
            }
            if (Input.keyboardState.IsKeyDown(Keys.Back))
            {
                yolo = "";
            }

            if (tugOfWarButton.IsClicked)
            {
                overlordReference.CreateGame(Games.Tug, false);
            }

            if (lineEmUpButton.IsClicked)
            {
                overlordReference.CreateGame(Games.Line, false);
            }

            if (pickNDropButton.IsClicked)
            {
                overlordReference.CreateGame(Games.Pick, false);
            }

            if (SwitcharooBuckarooButton.IsClicked)
            {
                overlordReference.CreateGame(Games.Switch, false);
            }

            if (tetrisFactoryButton.IsClicked)
            {
                overlordReference.CreateGame(Games.Factory, false);
            }

            if (backButton.IsClicked)
            {
                menuState = MenuState.Main;
                SetMain();
            }
            if (spookyButton.IsClicked)
            {
                overlordReference.CreateGame(Games.Nightmare, false);
            }


        }

        private void SetSingle()
        {
            DisableButtons();

            lineEmUpButton.enabled = true;
            lineEmUpButton.position = new Vector2(150, 310) * Game1.displayRatio;
            pickNDropButton.enabled = true;
            pickNDropButton.position = new Vector2(150, 465) * Game1.displayRatio;
            SwitcharooBuckarooButton.enabled = true;
            SwitcharooBuckarooButton.position = new Vector2(150, 620) * Game1.displayRatio;
            tetrisFactoryButton.enabled = true;
            tetrisFactoryButton.position = new Vector2(510, 310) * Game1.displayRatio;
            tugOfWarButton.enabled = true;
            tugOfWarButton.position = new Vector2(510, 465) * Game1.displayRatio;
            spookyButton.enabled = true;
            spookyButton.position = new Vector2(510, 620) * Game1.displayRatio;
            backButton.enabled = true;
            backButton.position = new Vector2(1290, 575) * Game1.displayRatio;
        }

        private void UpdateMulti(GameTime gameTime)
        {
            localButton.Update(gameTime);
            onlineButton.Update(gameTime);
            backButton.Update(gameTime);

            if (localButton.IsClicked)
            {
                menuState = MenuState.Local;
                SetLocal();
            }

            if (onlineButton.IsClicked)
            {
                SetOnline();
            }

            if (backButton.IsClicked)
            {
                menuState = MenuState.Main;
                SetMain();
            }
        }

        private void SetMulti()
        {
            menuState = MenuState.Multi;
            DisableButtons();
            localButton.enabled = true;
            localButton.position = new Vector2(330, 385) * Game1.displayRatio;
            onlineButton.enabled = true;
            onlineButton.position = new Vector2(330, 545) * Game1.displayRatio;
            backButton.enabled = true;
            backButton.position = new Vector2(1290, 465) * Game1.displayRatio;
        }

        private void UpdateLocal(GameTime gameTime)
        {
            tugOfWarButton.Update(gameTime);
            lineEmUpButton.Update(gameTime);
            pickNDropButton.Update(gameTime);
            SwitcharooBuckarooButton.Update(gameTime);
            backButton.Update(gameTime);
            spookyButton.Update(gameTime);
            dualHandjobsButton.Update(gameTime);

            if (tugOfWarButton.IsClicked)
            {
                overlordReference.CreateGame(Games.Tug, true);
            }

            if (lineEmUpButton.IsClicked)
            {
                overlordReference.CreateGame(Games.Line, true);
            }

            if (pickNDropButton.IsClicked)
            {
                overlordReference.CreateGame(Games.Pick, true);
            }

            if (SwitcharooBuckarooButton.IsClicked)
            {
                overlordReference.CreateGame(Games.Switch, true);
            }

            if (spookyButton.IsClicked)
            {
                overlordReference.CreateGame(Games.Nightmare, true);
            }

            if (dualHandjobsButton.IsClicked)
            {
                overlordReference.CreateGame(Games.DoubleTrouble, true);
            }

            if (backButton.IsClicked)
            {
                menuState = MenuState.Multi;
                SetMulti();
            }
        }

        private void SetLocal()
        {
            DisableButtons();
            lineEmUpButton.enabled = true;
            lineEmUpButton.position = new Vector2(330, 155) * Game1.displayRatio;
            pickNDropButton.enabled = true;
            pickNDropButton.position = new Vector2(330, 310) * Game1.displayRatio;
            SwitcharooBuckarooButton.enabled = true;
            SwitcharooBuckarooButton.position = new Vector2(330, 465) * Game1.displayRatio;
            tugOfWarButton.enabled = true;
            tugOfWarButton.position = new Vector2(330, 620) * Game1.displayRatio;
            spookyButton.enabled = true;
            spookyButton.position = new Vector2(330, 775) * Game1.displayRatio;
            dualHandjobsButton.enabled = true;
            dualHandjobsButton.position = new Vector2(330, 930) * Game1.displayRatio;
            backButton.enabled = true;
            backButton.position = new Vector2(1290, 465) * Game1.displayRatio;
        }

        private void DisableButtons()
        {
            exitGameButton.enabled = false;
            tugOfWarButton.enabled = false;
            lineEmUpButton.enabled = false;
            pickNDropButton.enabled = false;
            tetrisInvadersButton.enabled = false;
            tetrisFactoryButton.enabled = false;
            SwitcharooBuckarooButton.enabled = false;
            backButton.enabled = false;
            singleButton.enabled = false;
            multiButton.enabled = false;
            localButton.enabled = false;
            onlineButton.enabled = false;
            hostButton.enabled = false;
            joinButton.enabled = false;
            optionsButton.enabled = false;
            spookyButton.enabled = false;
            dualHandjobsButton.enabled = false;
            button_Invite.enabled = false;
            for (int Q = 0; Q < ControlsButtons.Count; Q++)
                ControlsButtons[Q].enabled = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (menuState == MenuState.Intro)
            {
                float alpha = 1 - ((float)creditsTimer / 1000f);
                spriteBatch.Draw(Opacity_effect, new Rectangle(0, 0, (int)(1920 * Game1.displayRatio.X), (int)(1080 * Game1.displayRatio.Y)), new Color(255, 255, 255, alpha));
            }
            if (menuState == MenuState.Options)
            {
                spriteBatch.Draw(Options_Screen, new Rectangle((int)(100 * Game1.displayRatio.X), (int)(100 * Game1.displayRatio.Y), (int)(760 * Game1.displayRatio.X), (int)(880 * Game1.displayRatio.Y)), Color.White);
                controlsButton.Draw(spriteBatch);
                creditsButton.Draw(spriteBatch);
                SFX.Draw(spriteBatch, new Rectangle((int)(260 * Game1.displayRatio.X), (int)(351 * Game1.displayRatio.Y), (int)(438 * Game1.displayRatio.X), (int)(16 * Game1.displayRatio.Y)));
                Music.Draw(spriteBatch, new Rectangle((int)(260 * Game1.displayRatio.X), (int)(500 * Game1.displayRatio.Y), (int)(438 * Game1.displayRatio.X), (int)(16 * Game1.displayRatio.Y)));

                Game1.Drawnumbers(Numbers, SFX.percentString, new Vector2(496, 270) * Game1.displayRatio, spriteBatch);

                Game1.Drawnumbers(Numbers, Music.percentString, new Vector2(496, 420) * Game1.displayRatio, spriteBatch);

                checkbox_Mute.Draw(spriteBatch);
                checkbox_FullScreen.Draw(spriteBatch);
            }
            if (menuState == MenuState.Credits)
            {
                spriteBatch.Draw(Credits_Screen, new Rectangle((int)(100 * Game1.displayRatio.X), (int)(100 * Game1.displayRatio.Y), (int)(760 * Game1.displayRatio.X), (int)(880 * Game1.displayRatio.Y)), Color.White);
            }
            if (menuState == MenuState.Controls)
            {
                float scale, rotation, depth;
                rotation = 0f;
                depth = 0f;
                SpriteEffects effect = SpriteEffects.None;
                Vector2 origin = Vector2.Zero;
                if (Game1.displayRatio.X > Game1.displayRatio.Y)
                    scale = Game1.displayRatio.Y;
                else
                    scale = Game1.displayRatio.X;
                
                spriteBatch.Draw(Controls_Screen, new Rectangle((int)(100 * Game1.displayRatio.X), (int)(100 * Game1.displayRatio.Y), (int)(760 * Game1.displayRatio.X), (int)(880 * Game1.displayRatio.Y)), Color.White);
                spriteBatch.DrawString(font, "P1 Left", ControlsButtons[0].position - (new Vector2(110, 0) * Game1.displayRatio), Color.White, rotation, origin, scale, effect, depth);
                spriteBatch.DrawString(font, "P1 Right", ControlsButtons[1].position - (new Vector2(110, 0) * Game1.displayRatio), Color.White, rotation, origin, scale, effect, depth);
                spriteBatch.DrawString(font, "P1 Rotate", ControlsButtons[2].position - (new Vector2(110, 0) * Game1.displayRatio), Color.White, rotation, origin, scale, effect, depth);
                spriteBatch.DrawString(font, "P1 Down", ControlsButtons[3].position - (new Vector2(110, 0) * Game1.displayRatio), Color.White, rotation, origin, scale, effect, depth);
                spriteBatch.DrawString(font, "P1 Place", ControlsButtons[4].position - (new Vector2(110, 0) * Game1.displayRatio), Color.White, rotation, origin, scale, effect, depth);
                spriteBatch.DrawString(font, "P1 Hold", ControlsButtons[5].position - (new Vector2(110, 0) * Game1.displayRatio), Color.White, rotation, origin, scale, effect, depth);
                spriteBatch.DrawString(font, "P2 Left", ControlsButtons[6].position - (new Vector2(110, 0) * Game1.displayRatio), Color.White, rotation, origin, scale, effect, depth);
                spriteBatch.DrawString(font, "P2 Right", ControlsButtons[7].position - (new Vector2(110, 0) * Game1.displayRatio), Color.White, rotation, origin, scale, effect, depth);
                spriteBatch.DrawString(font, "P2 Rotate", ControlsButtons[8].position - (new Vector2(110, 0) * Game1.displayRatio), Color.White, rotation, origin, scale, effect, depth);
                spriteBatch.DrawString(font, "P2 Down", ControlsButtons[9].position - (new Vector2(110, 0) * Game1.displayRatio), Color.White, rotation, origin, scale, effect, depth);
                spriteBatch.DrawString(font, "P2 Place", ControlsButtons[10].position - (new Vector2(110, 0) * Game1.displayRatio), Color.White, rotation, origin, scale, effect, depth);
                spriteBatch.DrawString(font, "P2 Hold", ControlsButtons[11].position - (new Vector2(110, 0) * Game1.displayRatio), Color.White, rotation, origin, scale, effect, depth);
                spriteBatch.DrawString(font, "Pause", ControlsButtons[12].position - (new Vector2(90, 0) * Game1.displayRatio), Color.White, rotation, origin, scale, effect, depth);
                int Q = 0;
                foreach(Keys key in Input.ControlsP1.Values)
                {
                    if (Q < 6)
                        spriteBatch.DrawString(font, "" + key, ControlsButtons[Q].position + new Vector2(23, 0) * Game1.displayRatio, Color.White, rotation, origin, scale, effect, depth);
                    Q++;
                }
                Q = 6;
                foreach (Keys key in Input.ControlsP2.Values)
                {
                    if (Q < 12)
                        spriteBatch.DrawString(font, "" + key, ControlsButtons[Q].position + new Vector2(23, 0) * Game1.displayRatio, Color.White, rotation, origin, scale, effect, depth);
                    Q++;
                }
                spriteBatch.DrawString(font, "" + Input.ControlsP1["Pause"], ControlsButtons[12].position + new Vector2(23, 0) * Game1.displayRatio, Color.White, rotation, origin, scale, effect, depth);
            }
            if (menuState == MenuState.Single)
            {
                AI.Draw(spriteBatch, new Rectangle((int)(1228 * Game1.displayRatio.X), (int)(480 * Game1.displayRatio.Y), (int)(438 * Game1.displayRatio.X), (int)(16 * Game1.displayRatio.Y)));
                spriteBatch.Draw(AI_Names[AIlevel], new Rectangle((int)((1440 - AI_Names[AIlevel].Width / 2) * Game1.displayRatio.X),
                                (int)(400 * Game1.displayRatio.Y), (int)(AI_Names[AIlevel].Width * Game1.displayRatio.X), (int)(40 * Game1.displayRatio.Y)), Color.White);
                Game1.Drawnumbers(Numbers, "" + AIlevel, new Vector2(1430, 510), spriteBatch);
            }
            exitGameButton.Draw(spriteBatch);
            tugOfWarButton.Draw(spriteBatch);
            lineEmUpButton.Draw(spriteBatch);
            pickNDropButton.Draw(spriteBatch);
            tetrisInvadersButton.Draw(spriteBatch);
            tetrisFactoryButton.Draw(spriteBatch);
            SwitcharooBuckarooButton.Draw(spriteBatch);
            backButton.Draw(spriteBatch);
            singleButton.Draw(spriteBatch);
            multiButton.Draw(spriteBatch);
            localButton.Draw(spriteBatch);
            onlineButton.Draw(spriteBatch);
            hostButton.Draw(spriteBatch);
            joinButton.Draw(spriteBatch);
            optionsButton.Draw(spriteBatch);
            spookyButton.Draw(spriteBatch);
            dualHandjobsButton.Draw(spriteBatch);
            button_Invite.Draw(spriteBatch);
            for (int Q = 0; Q < ControlsButtons.Count; Q++)
                ControlsButtons[Q].Draw(spriteBatch);

            if (tugOfWarButton.buttonBounds.Contains(Input.mouseState.X, Input.mouseState.Y) && tugOfWarButton.enabled)
                spriteBatch.Draw(Tug_Desc, Description_Position, Color.White);
            if (tetrisFactoryButton.buttonBounds.Contains(Input.mouseState.X, Input.mouseState.Y) && tetrisFactoryButton.enabled)
                spriteBatch.Draw(TF_Desc, Description_Position, Color.White);
            if (lineEmUpButton.buttonBounds.Contains(Input.mouseState.X, Input.mouseState.Y) && lineEmUpButton.enabled)
                spriteBatch.Draw(Line_Desc, Description_Position, Color.White);
            if (SwitcharooBuckarooButton.buttonBounds.Contains(Input.mouseState.X, Input.mouseState.Y) && SwitcharooBuckarooButton.enabled)
                spriteBatch.Draw(Switch_Desc, Description_Position, Color.White);
            if (pickNDropButton.buttonBounds.Contains(Input.mouseState.X, Input.mouseState.Y) && pickNDropButton.enabled)
                spriteBatch.Draw(Pick_Desc, Description_Position, Color.White);
            if (spookyButton.buttonBounds.Contains(Input.mouseState.X, Input.mouseState.Y) && spookyButton.enabled)
                spriteBatch.Draw(Spook_Desc, Description_Position, Color.White);


            if (menuState == MenuState.Host)
            {
                spriteBatch.Draw(WaitingConnection, new Rectangle((int)(213 * Game1.displayRatio.X), (int)(473 * Game1.displayRatio.Y),
                                 (int)(534 * Game1.displayRatio.X), (int)(90 * Game1.displayRatio.Y)), Color.White);
            }

            if (menuState == MenuState.Join)
            {                

                //draw the "Enter your host's ip address" texture
                //spriteBatch.Draw(Enter_IP_Box, new Rectangle((int)(90 * Game1.displayRatio.X), (int)(473 * Game1.displayRatio.Y), 
                //                 (int)(780 * Game1.displayRatio.X), (int)(140 * Game1.displayRatio.Y)), Color.White);
                //draw the ip that the player is entering
                //Game1.Drawnumbers(Numbers, ipString, new Vector2(105, 553) * Game1.displayRatio, spriteBatch);


            }
        }

        #region Just Steam Stuff

        private void SetOnline()
        {
            DisableButtons();
            joinButton.enabled = true;
            joinButton.position = new Vector2(330, 385) * Game1.displayRatio;
            hostButton.enabled = true;
            hostButton.position = new Vector2(330, 545) * Game1.displayRatio;
            backButton.enabled = true;
            backButton.position = new Vector2(1290, 465) * Game1.displayRatio;
            menuState = MenuState.Online;
        }

        private void UpdateOnline(GameTime gameTime)
        {
            backButton.Update(gameTime);
            hostButton.Update(gameTime);
            joinButton.Update(gameTime);

            if (backButton.IsClicked)
            {
                SetMulti();
            }

            if (hostButton.IsClicked)
            {
                // Console.WriteLine("Trying to create lobby");

                joinButton.enabled = false;
                hostButton.enabled = false;

                // Create a steam lobby and set up flags for the lobby
                SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, 2);
            }
        }

        private void SetJoin()
        {
            menuState = MenuState.Join;
            DisableButtons();
            backButton.enabled = true;
            backButton.position = new Vector2(1290, 465) * Game1.displayRatio;
        }

        private void UpdateJoin(GameTime gameTime)
        {
            backButton.Update(gameTime);

            if (backButton.IsClicked)
            {
                SetOnline();
            }
        }

        private void SetHost()
        {
            DisableButtons();
            backButton.enabled = true;
            backButton.position = new Vector2(800, 650) * Game1.displayRatio;
            button_Invite.enabled = true;
            button_Invite.position = new Vector2(800, 200) * Game1.displayRatio;
            menuState = MenuState.Host;
        }

        private void UpdateHost(GameTime gameTime)
        {
            backButton.Update(gameTime);
            button_Invite.Update(gameTime);

            ReceiveMessages();

            if (SteamMatchmaking.GetNumLobbyMembers(lobbyId) >= 2 && connectedToOtherPlayer)
            {
                for (int i = 0; i < SteamMatchmaking.GetNumLobbyMembers(lobbyId); i++)
                {
                    if (SteamMatchmaking.GetLobbyMemberByIndex(lobbyId, i) != Steam_Manager.UserID)
                    {
                        Steam_Manager.ConnectedPlayer = SteamMatchmaking.GetLobbyMemberByIndex(lobbyId, i);
                    }
                }

                SetOnlineHostLobby();
            }

            if (button_Invite.IsClicked)
            {
                //SteamFriends.ActivateGameOverlay("LobbyInvite");
                SteamFriends.ActivateGameOverlayInviteDialog(lobbyId);
            }

            if (backButton.IsClicked)
            {
                SteamMatchmaking.LeaveLobby(lobbyId);
                SetOnline();
            }
        }

        private void SetOnlineHostLobby()
        {
            menuState = MenuState.OnlineHostLobby;

            DisableButtons();
            classicButton.enabled = true;
            classicButton.position = new Vector2(600, 230) * Game1.displayRatio;
            lineEmUpButton.enabled = true;
            lineEmUpButton.position = new Vector2(330, 230) * Game1.displayRatio;
            pickNDropButton.enabled = true;
            pickNDropButton.position = new Vector2(330, 385) * Game1.displayRatio;
            SwitcharooBuckarooButton.enabled = true;
            SwitcharooBuckarooButton.position = new Vector2(330, 545) * Game1.displayRatio;
            tugOfWarButton.enabled = true;
            tugOfWarButton.position = new Vector2(330, 700) * Game1.displayRatio;
            backButton.enabled = true;
            backButton.position = new Vector2(1290, 465) * Game1.displayRatio;
        }

        private void UpdateOnlineHostLobby(GameTime gameTime)
        {
            classicButton.Update(gameTime);
            tugOfWarButton.Update(gameTime);
            lineEmUpButton.Update(gameTime);
            pickNDropButton.Update(gameTime);
            SwitcharooBuckarooButton.Update(gameTime);
            backButton.Update(gameTime);

            ReceiveMessages();

            if (classicButton.IsClicked)
            {

            }

            if (tugOfWarButton.IsClicked)
            {
                MsgServerStartGame msg = new MsgServerStartGame();
                msg.SetGameMode(Games.OnlineTug);

                Steam_Manager.SendP2PMessage(Steam_Manager.ConnectedPlayer, msg, EP2PSend.k_EP2PSendReliable);

                overlordReference.CreateGame(Games.OnlineTug, true);
            }

            if (lineEmUpButton.IsClicked)
            {
                MsgServerStartGame msg = new MsgServerStartGame();
                msg.SetGameMode(Games.OnlineLine);

                Steam_Manager.SendP2PMessage(Steam_Manager.ConnectedPlayer, msg, EP2PSend.k_EP2PSendReliable);

                overlordReference.CreateGame(Games.OnlineLine, true);
            }

            if (pickNDropButton.IsClicked)
            {
                MsgServerStartGame msg = new MsgServerStartGame();
                msg.SetGameMode(Games.OnlinePick);

                Steam_Manager.SendP2PMessage(Steam_Manager.ConnectedPlayer, msg, EP2PSend.k_EP2PSendReliable);

                overlordReference.CreateGame(Games.OnlinePick, true);
            }

            if (SwitcharooBuckarooButton.IsClicked)
            {
                MsgServerStartGame msg = new MsgServerStartGame();
                msg.SetGameMode(Games.OnlineSwitch);

                Steam_Manager.SendP2PMessage(Steam_Manager.ConnectedPlayer, msg, EP2PSend.k_EP2PSendReliable);

                overlordReference.CreateGame(Games.OnlineSwitch, true);
            }

            if (backButton.IsClicked)
            {
                SetOnline();
            }
        }

        private void SetOnlineClientLobby()
        {
            menuState = MenuState.OnlineClientLobby;
            DisableButtons();
            backButton.enabled = true;
            backButton.position = new Vector2(1290, 465) * Game1.displayRatio;
        }

        private void UpdateOnlineClientLobby(GameTime gameTime)
        {
            backButton.Update(gameTime);

            ReceiveMessages();

            if (backButton.IsClicked)
            {
                SteamMatchmaking.LeaveLobby(lobbyId);
                SetJoin();
            }
        }

        private void SetDisconnect()
        {
            menuState = MenuState.Disconnect;
            DisableButtons();
            backButton.enabled = true;
        }

        private void UpdateDisconnect(GameTime gameTime)
        {
            backButton.Update(gameTime);

            if (backButton.IsClicked)
            {
                SetOnline();
            }
        }

        private void ReceiveMessages()
        {
            byte[] recvBuf;
            uint msgSize;
            CSteamID steamIDRemote;

            // Read through all packets on the steam network
            while (SteamNetworking.IsP2PPacketAvailable(out msgSize))
            // while (SteamGameServerNetworking.IsP2PPacketAvailable(out msgSize))
            {
                // Allocate room for the packet
                recvBuf = new byte[msgSize];

                // Read the packet
                if (!SteamNetworking.ReadP2PPacket(recvBuf, msgSize, out msgSize, out steamIDRemote))
                // if (!SteamGameServerNetworking.ReadP2PPacket(recvBuf, msgSize, out msgSize, out steamIDRemote))
                {
                    // Returns false if no data is available, done reading messages if somehow while statement doesn't catch it
                    break;
                }

                if (steamIDRemote != Steam_Manager.ConnectedPlayer)
                {
                    Console.WriteLine("Got message from: " + steamIDRemote + " | expected it from: " + Steam_Manager.ConnectedPlayer);
                }

                Message msg = (Message)Converter.ByteArrayToObject(recvBuf);

                connectedToOtherPlayer = true;

                switch (msg.GetMessageType())
                {
                    case EMessage.EMsg_ServerSendInfo:
                        if (menuState == MenuState.OnlineClientLobby)
                        {
                            MsgServerSendInfo castMsg = (MsgServerSendInfo)msg;

                            if ((CSteamID)castMsg.GetSteamIDServer() != lobbyId)
                            {
                                Console.WriteLine("Somehow in a different lobby than expected, bug check");
                                // throw new InvalidOperationException("Lobby was not the expected lobby.");
                            }

                            Console.WriteLine("Got server info: " + castMsg.GetServerName());
                        }
                        else
                        {
                            Console.WriteLine("Non client received server info from a host!");
                            // throw new InvalidOperationException("Non client received server info from a host.");
                        }
                        break;
                    case EMessage.EMsg_ServerFailAuthentication:
                        if (menuState == MenuState.OnlineClientLobby)
                        {

                        }
                        else
                        {
                            Console.WriteLine("Non client received server info from a host!");
                            // throw new InvalidOperationException("Non client received server info from a host.");
                        }
                        break;
                    case EMessage.EMsg_ServerPassAuthentication:
                        if (menuState == MenuState.OnlineClientLobby)
                        {

                        }
                        else
                        {
                            Console.WriteLine("Non client received server info from a host!");
                            // throw new InvalidOperationException("Non client received server info from a host.");
                        }
                        break;
                    case EMessage.EMsg_ServerStartGame:
                        if (menuState == MenuState.OnlineClientLobby)
                        {
                            Games gameMode = ((MsgServerStartGame)msg).GetGameMode();
                            overlordReference.CreateGame(gameMode, true);
                        }
                        else
                        {
                            Console.WriteLine("Non client received server info from a host!");
                            // throw new InvalidOperationException("Non client received server info from a host.");
                        }
                        break;
                    case EMessage.EMsg_ClientInitiateConnection:
                        if (menuState == MenuState.Host || menuState == MenuState.OnlineHostLobby)
                        {
                            MsgServerSendInfo retMsg = new MsgServerSendInfo();
                            retMsg.SetSteamIDServer((ulong)lobbyId);
                            retMsg.SetServerName(SteamMatchmaking.GetLobbyData(lobbyId, "Name"));
                            Steam_Manager.SendP2PMessage(steamIDRemote, retMsg, EP2PSend.k_EP2PSendReliable);
                        }
                        else
                        {
                            Console.WriteLine("Non host received server info from a client!");
                            // throw new InvalidOperationException("Non host received client info from a client.");
                        }
                        break;
                    case EMessage.EMsg_ClientBeginAuthentication:
                        if (menuState == MenuState.Host || menuState == MenuState.OnlineHostLobby)
                        {

                        }
                        else
                        {
                            Console.WriteLine("Non host received server info from a client!");
                            // throw new InvalidOperationException("Non host received client info from a client.");
                        }
                        break;
                    case EMessage.EMsg_ClientPing:
                        if (menuState == MenuState.Host || menuState == MenuState.OnlineHostLobby)
                        {

                        }
                        else
                        {
                            Console.WriteLine("Non host received server info from a client!");
                            // throw new InvalidOperationException("Non host received client info from a client.");
                        }
                        break;
                    case EMessage.EMsg_ClientLeavingServer:
                        if (menuState == MenuState.Host || menuState == MenuState.OnlineHostLobby)
                        {

                        }
                        else
                        {
                            Console.WriteLine("Non host received server info from a client!");
                            // throw new InvalidOperationException("Non host received client info from a client.");
                        }
                        break;
                    default:
                        Console.WriteLine("Got an invalid message on server. MessageType: " + msg.GetMessageType());
                        break;
                }
            }
        }

        public void JoinLobby(CSteamID lobbyID)
        {
            SteamMatchmaking.JoinLobby(lobbyID);
            this.lobbyId = lobbyID;
            SetJoin();
        }

        public void OnLobbyCreated(CSteamID lobbyID)
        {
            SetHost();
            this.lobbyId = lobbyID;
            SteamMatchmaking.SetLobbyData(lobbyId, "Name", Steam_Manager.Username);

        }

        public void OnLobbyCreationFailure()
        {
            SetMulti();
        }

        public void OnLobbyInvite(CSteamID friendID, CSteamID lobbyID)
        {
            JoinLobby(lobbyID);
        }

        public void OnLobbyEntered(CSteamID lobbyID)
        {
            Console.WriteLine("Joined a lobby: " + lobbyID + " | Previous Lobby: " + lobbyId);

            if (this.lobbyId != lobbyID)
            {
                Console.WriteLine("Have to set up lobby id");
                this.lobbyId = lobbyID;
            }

            if (menuState != MenuState.Host)
            {
                SetOnlineClientLobby();

                int num = SteamMatchmaking.GetNumLobbyMembers(lobbyId);
                for (int i = 0; i < num; i++)
                {
                    if (SteamMatchmaking.GetLobbyMemberByIndex(lobbyId, i) != Steam_Manager.UserID)
                    {
                        MsgClientInitiateConnection msg = new MsgClientInitiateConnection();
                        Steam_Manager.SendP2PMessage(SteamMatchmaking.GetLobbyMemberByIndex(lobbyId, i), msg, EP2PSend.k_EP2PSendReliable);
                        Console.WriteLine("Initiating connection to host");

                        break;
                    }
                }
            }
        }

        public void OnDisconnect()
        {
            if (menuState == MenuState.Host || menuState == MenuState.OnlineClientLobby || menuState == MenuState.OnlineHostLobby)
            {
                SetDisconnect();
            }
        }

        #endregion
    }
}
