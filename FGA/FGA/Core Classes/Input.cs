using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace FGA
{
    class Input
    {
        public static KeyboardState keyboardState, prevKeyboardState;
        public static MouseState mouseState, prevMouseState;
        public static GamePadState gamePadStateP1, prevGamePadStateP1, gamePadStateP2, prevGamePadStateP2;
        public static Dictionary<string, Keys> ControlsP1 = new Dictionary<string, Keys>();
        public static Dictionary<string, Keys> ControlsP2 = new Dictionary<string, Keys>();

        public static bool P1Rotate
        {
            get
            {
                if (keyboardState.IsKeyDown(ControlsP1["Rotate"]) && prevKeyboardState.IsKeyUp(ControlsP1["Rotate"]))
                    return true;

                if (gamePadStateP1.IsButtonDown(Buttons.DPadUp) && prevGamePadStateP1.IsButtonUp(Buttons.DPadUp))
                    return true;

                if (gamePadStateP1.IsButtonDown(Buttons.RightShoulder) && prevGamePadStateP1.IsButtonUp(Buttons.RightShoulder))
                    return true;

                return false;
            }
        }

        public static bool P2Rotate
        {
            get
            {
                if (keyboardState.IsKeyDown(ControlsP2["Rotate"]) && prevKeyboardState.IsKeyUp(ControlsP2["Rotate"]))
                    return true;

                if (gamePadStateP2.IsButtonDown(Buttons.DPadUp) && prevGamePadStateP2.IsButtonUp(Buttons.DPadUp))
                    return true;

                if (gamePadStateP2.IsButtonDown(Buttons.RightShoulder) && prevGamePadStateP2.IsButtonUp(Buttons.RightShoulder))
                    return true;

                return false;
            }
        }

        public static bool P1Left
        {
            get
            {
                if (keyboardState.IsKeyDown(ControlsP1["Left"]) && prevKeyboardState.IsKeyUp(ControlsP1["Left"]))
                    return true;

                if (gamePadStateP1.IsButtonDown(Buttons.DPadLeft) && prevGamePadStateP1.IsButtonUp(Buttons.DPadLeft))
                    return true;

                return false;
            }
            
        }

        public static bool P2Left
        {
            get
            {
                if (keyboardState.IsKeyDown(ControlsP2["Left"]) && prevKeyboardState.IsKeyUp(ControlsP2["Left"]))
                    return true;

                if (gamePadStateP2.IsButtonDown(Buttons.DPadLeft) && prevGamePadStateP2.IsButtonUp(Buttons.DPadLeft))
                    return true;

                return false;
            }
        }

        public static bool P1Right
        {
            get
            {
                if (keyboardState.IsKeyDown(ControlsP1["Right"]) && prevKeyboardState.IsKeyUp(ControlsP1["Right"]))
                    return true;

                if (gamePadStateP1.IsButtonDown(Buttons.DPadRight) && prevGamePadStateP1.IsButtonUp(Buttons.DPadRight))
                    return true;

                return false;
            }
        }

        public static bool P2Right
        {
            get
            {
                if (keyboardState.IsKeyDown(ControlsP2["Right"]) && prevKeyboardState.IsKeyUp(ControlsP2["Right"]))
                    return true;

                if (gamePadStateP2.IsButtonDown(Buttons.DPadRight) && prevGamePadStateP2.IsButtonUp(Buttons.DPadRight))
                    return true;

                return false;
            }
        }

        public static bool P1DropAll
        {
            get
            {
                if (keyboardState.IsKeyDown(ControlsP1["Place"]) && prevKeyboardState.IsKeyUp(ControlsP1["Place"]))
                    return true;

                if (gamePadStateP1.IsButtonDown(Buttons.DPadDown) && prevGamePadStateP1.IsButtonUp(Buttons.DPadDown))
                    return true;

                return false;
            }
        }

        public static bool P2DropAll
        {
            get
            {
                if (keyboardState.IsKeyDown(ControlsP2["Place"]) && prevKeyboardState.IsKeyUp(ControlsP2["Place"]))
                    return true;

                if (gamePadStateP2.IsButtonDown(Buttons.DPadDown) && prevGamePadStateP2.IsButtonUp(Buttons.DPadDown))
                    return true;

                return false;
            }
        }

        public static bool P1Drop1
        {
            get
            {
                if (keyboardState.IsKeyDown(ControlsP1["Down"]) && prevKeyboardState.IsKeyUp(ControlsP1["Down"]))
                    return true;

                if (gamePadStateP1.IsButtonDown(Buttons.A) && prevGamePadStateP1.IsButtonUp(Buttons.A))
                    return true;

                return false;
            }
        }

        public static bool P2Drop1
        {
            get
            {
                if (keyboardState.IsKeyDown(ControlsP2["Down"]) && prevKeyboardState.IsKeyUp(ControlsP2["Down"]))
                    return true;

                if (gamePadStateP2.IsButtonDown(Buttons.A) && prevGamePadStateP2.IsButtonUp(Buttons.A))
                    return true;

                return false;
            }
        }

        public static bool P1Hold
        {
            get
            {
                if (Input.keyboardState.IsKeyDown(ControlsP1["Hold"]) && Input.prevKeyboardState.IsKeyUp(ControlsP1["Hold"]))
                {
                    return true;
                }

                return false;
            }
        }

        public static bool P2Hold
        {
            get
            {
                if (Input.keyboardState.IsKeyDown(ControlsP2["Hold"]) && Input.prevKeyboardState.IsKeyUp(ControlsP2["Hold"]))
                {
                    return true;
                }

                return false;
            }
        }

        public static bool Pause
        {
            get
            {
                if ((keyboardState.IsKeyDown(ControlsP1["Pause"]) && prevKeyboardState.IsKeyUp(ControlsP1["Pause"])) ||
                    (gamePadStateP1.IsButtonDown(Buttons.Start) && prevGamePadStateP1.IsButtonUp(Buttons.Start)) ||
                    (gamePadStateP2.IsButtonDown(Buttons.Start) && prevGamePadStateP2.IsButtonUp(Buttons.Start)))
                {
                    return true;
                }

                return false;
            }
        }

        public static bool Back
        {
            get
            {
                if ((keyboardState.IsKeyDown(Keys.Escape) && prevKeyboardState.IsKeyUp(Keys.Escape)) ||
                    (gamePadStateP1.IsButtonDown(Buttons.Back) && prevGamePadStateP1.IsButtonUp(Buttons.Back)) ||
                    (gamePadStateP2.IsButtonDown(Buttons.Back) && prevGamePadStateP2.IsButtonUp(Buttons.Back)))
                {
                    return true;
                }

                return false;
            }
        }

        public static bool Enter
        {
            get
            {
                if (keyboardState.IsKeyDown(Keys.Enter) && prevKeyboardState.IsKeyUp(Keys.Enter))
                    return true;

                return false;
            }
        }

        public static bool Backspace
        {
            get
            {
                if (keyboardState.IsKeyDown(Keys.Back) && prevKeyboardState.IsKeyUp(Keys.Back))
                    return true;

                return false;
            }
        }

        public static bool IsKeyPress(Keys key)
        {
            if (keyboardState.IsKeyDown(key) && prevKeyboardState.IsKeyUp(key))
                return true;

            return false;
        }

        public static string GetNumbers()
        {
            string pressedKeys = "";

            foreach (Keys k in keyboardState.GetPressedKeys())
            {
                if (prevKeyboardState.IsKeyUp(k))
                {
                    int charindex = (int)k;
                    if (charindex >= 48 && charindex <= 57)
                        pressedKeys += (char)charindex;
                }
            }

            return pressedKeys;
        }

        public static string GetUpperCaseLetters()
        {
            string pressedKeys = "";

            foreach (Keys k in keyboardState.GetPressedKeys())
            {
                if (prevKeyboardState.IsKeyUp(k))
                {
                    int charindex = (int)k;
                    if (charindex >= 65 && charindex <= 90)
                        pressedKeys += (char)charindex;
                }
            }

            return pressedKeys;
        }

        //Find another way to get these types of keys
        #region NotWorking
        //public static string GetLowerCaseLetters()
        //{
        //    string pressedKeys = "";

        //    foreach (Keys k in keyboardState.GetPressedKeys())
        //    {
        //        if (prevKeyboardState.IsKeyUp(k))
        //        {
        //            int charindex = (int)k;
        //            if (charindex >= 97 && charindex <= 122)
        //                pressedKeys += (char)charindex;
        //        }
        //    }

        //    return pressedKeys;
        //}

        //public static string GetLetters()
        //{
        //    return GetUpperCaseLetters() + GetLowerCaseLetters();
        //}

        //public static string GetSymbols()
        //{
        //    string pressedKeys = "";

        //    foreach (Keys k in keyboardState.GetPressedKeys())
        //    {
        //        if (prevKeyboardState.IsKeyUp(k))
        //        {
        //            int charindex = (int)k;
        //            if ((charindex >= 32 && charindex <= 47) ||
        //                (charindex >= 58 && charindex <= 64) ||
        //                (charindex >= 91 && charindex <= 96) ||
        //                (charindex >= 123 && charindex <= 126))
        //                pressedKeys += (char)charindex;
        //        }
        //    }

        //    return pressedKeys;
        //}

        //public static string GetAll()
        //{
        //    return GetNumbers() + GetLetters() + GetSymbols();
        //}
        #endregion

        public static string GetKeys()
        {
            string pressedKeys = "";

            foreach (Keys k in keyboardState.GetPressedKeys())
            {
                if (prevKeyboardState.IsKeyUp(k))
                {
                    pressedKeys += k;
                }
            }

            return pressedKeys;
        }

        public static void Update(GameTime gameTime)
        {
            prevKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();

            prevMouseState = mouseState;
            mouseState = Mouse.GetState();

            prevGamePadStateP1 = gamePadStateP1;
            prevGamePadStateP2 = gamePadStateP2;
            gamePadStateP1 = GamePad.GetState(PlayerIndex.One);
            gamePadStateP2 = GamePad.GetState(PlayerIndex.Two);
        }

        public static void SetDefaultControls()
        {
            Input.ControlsP1.Add("Left", Keys.A);
            Input.ControlsP1.Add("Right", Keys.D);
            Input.ControlsP1.Add("Rotate", Keys.W);
            Input.ControlsP1.Add("Down", Keys.Space);
            Input.ControlsP1.Add("Place", Keys.S);
            Input.ControlsP1.Add("Hold", Keys.Q);
            Input.ControlsP1.Add("Pause", Keys.Escape);

            Input.ControlsP2.Add("Left", Keys.Left);
            Input.ControlsP2.Add("Right", Keys.Right);
            Input.ControlsP2.Add("Rotate", Keys.Up);
            Input.ControlsP2.Add("Down", Keys.Enter);
            Input.ControlsP2.Add("Place", Keys.Down);
            Input.ControlsP2.Add("Hold", Keys.RightShift);
            Input.ControlsP2.Add("Pause", Keys.Escape);
        }

        /// <summary>
        /// Saves the current control configuration to a file, not yet implemented
        /// </summary>
        public static void SaveControls()
        {
            // Save current control configuration to a file
        }

        /// <summary>
        /// Loads a configuration from control save file, not yet implemented
        /// </summary>
        public static void LoadControls()
        {
            // Load configuration controls file
        }
    }
}
