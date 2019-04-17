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

namespace FGA.Core_Classes
{
    class Track_Bar
    {
        Texture2D Track_Texture;
        Texture2D Ball_Texture;
        public Vector2 startLocation;//starts on the very left of the track
        public Rectangle ballLocation;//rectangle of the balls location
        public Vector2 floatballLocation;
        public double Progress_Percent;
        float trackLength;
        public bool movingTrack;
        public string percentString;
        public float increments;

        public Track_Bar(float increments, float length)
        {
            Track_Texture = Game1.GlobalContent.Load<Texture2D>(@"Textures/Track");
            Ball_Texture = Game1.GlobalContent.Load<Texture2D>(@"Textures/Track_Bar_Ball");
            trackLength = (length * Game1.displayRatio.X);
            movingTrack = false;
            Progress_Percent = 100;
            percentString = "100";
            this.increments = increments;
        }
        public void UpdateMovement()
        {
            if (ballLocation.Contains(Input.mouseState.X, Input.mouseState.Y) && Input.mouseState.LeftButton == ButtonState.Pressed)
            {
                movingTrack = true;
            }
            else if (Input.mouseState.LeftButton == ButtonState.Released)
                movingTrack = false;
        }
        public int Update()
        {

            Progress_Percent = 0;
            if (movingTrack)
            {
                float incrementLength = trackLength / increments;
                float incrementNum = ((floatballLocation.X - startLocation.X) / incrementLength);

                //make the bounds of the track
                if (Input.mouseState.X < startLocation.X)
                {
                    ballLocation.X = (int)startLocation.X;
                    floatballLocation.X = startLocation.X;
                }
                else if (Input.mouseState.X > (startLocation.X + trackLength))
                {
                    ballLocation.X = (int)(startLocation.X + trackLength);
                    floatballLocation.X = startLocation.X + trackLength;
                }
                else if (Input.mouseState.X - floatballLocation.X >= incrementLength)
                {
                    incrementNum++;
                    ballLocation.X += (int)incrementLength;
                    floatballLocation.X += incrementLength;
                }
                else if (Input.mouseState.X - floatballLocation.X <= (-1) * (incrementLength))
                {
                    incrementNum--;
                    ballLocation.X -= (int)incrementLength;
                    floatballLocation.X -= incrementLength;
                }

                Progress_Percent = (incrementNum / increments);
                Progress_Percent *= 100;
                Progress_Percent = (int)Progress_Percent;
                percentString = "" + Progress_Percent;
            }

            return (int)Progress_Percent;
        }
        public void Draw(SpriteBatch spriteBatch, Rectangle trackLocation)
        {
            spriteBatch.Draw(Track_Texture, trackLocation, Color.White);
            spriteBatch.Draw(Ball_Texture, ballLocation, Color.White);
        }

    }
}
