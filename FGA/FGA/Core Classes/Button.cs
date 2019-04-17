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

namespace FGA
{
    public class Button
    {
        protected Texture2D texture;
        public Vector2 position;
        public Vector2 size;
        //additional details if button needs to be changed
        protected float rotation;
        protected state buttonState, prevButtonState;
        public bool isPressed;

        //allows disabling the button
        public bool enabled = true;

        public enum state
        {
            idle = 0,
            selected = 1,
            clicked = 2
        };

        public Rectangle buttonBounds
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            }
        }

        //user has clicked and released the button
        public bool IsClicked
        {
            get 
            {
                if (prevButtonState == state.clicked && buttonState == state.selected && enabled)
                    return true;
                else
                    return false;
            }
        }

        //button is pressed but not released
        public bool IsPressed
        {
            get
            {
                if (!enabled)
                    return false;

                return isPressed;
                //if (buttonState == state.clicked)
                //    return true;
                //else
                //    return false;
            }
        }

        //creates a button on top right screen with images size as the size
        public Button(Texture2D texture)
            : this(texture, Vector2.Zero, new Vector2(texture.Width, texture.Height / 3), 0.0f)
        {
        }

        //creates button without resizing it
        public Button(Texture2D texture, Vector2 position)
            : this(texture, position, new Vector2(texture.Width, texture.Height / 3) * Game1.displayRatio, 0.0f)
        {
        }

        //constructor for a basic button and resizes it to the specified size
        public Button(Texture2D texture, Vector2 position, Vector2 size)
            : this(texture, position, size, 0.0f)
        {
        }

        //constructor for a button with rotational value
        public Button(Texture2D texture, Vector2 position, Vector2 size, float rotation)
        {
            this.texture = texture;
            this.position = position;
            this.size = size;
            this.rotation = rotation;
        }

        //button update for non scrolling camera
        public virtual void Update(GameTime gameTime)
        {
            Update(gameTime, Vector2.Zero);
        }

        //updates for a scrolling camera
        public void Update(GameTime gameTime, Vector2 camerPosition)
        {
            if (!enabled)
                return;

            prevButtonState = buttonState;

            if (buttonBounds.Contains(Input.mouseState.X + (int)camerPosition.X, Input.mouseState.Y + (int)camerPosition.Y))
            {
                buttonState = state.selected;

                if (Input.mouseState.LeftButton == ButtonState.Pressed)
                {
                    buttonState = state.clicked;
                    isPressed = true;
                }
            }
            else
            {
                buttonState = state.idle;
            }

            if (isPressed)
            {
                if (Input.mouseState.LeftButton == ButtonState.Released)
                    isPressed = false;
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if(enabled)
                spriteBatch.Draw(texture, buttonBounds,
                    new Rectangle(0, (int)buttonState * texture.Height / 3, texture.Width, texture.Height / 3),
                        Color.White, rotation, Vector2.Zero,
                        SpriteEffects.None, 1.0f);
        }
    }
}
