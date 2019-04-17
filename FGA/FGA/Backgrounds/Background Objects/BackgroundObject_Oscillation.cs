using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace FGA.Backgrounds
{
    class BackgroundObject_Oscillation : BackgroundObject
    {
        Vector2 centerPosition;
        double periodX, periodY;                //period in milliseconds
        double periodOffsetX, periodOffsetY;    //offset in milliseconds
        float amplitudeX, amplitudeY;           //amplitude in pixels

        public BackgroundObject_Oscillation(Texture2D texture, Vector2 position, float rotation,
            double periodX, double periodOffsetX, float amplitudeX,
            double periodY, double periodOffsetY, float amplitudeY)
            : base(texture, position, rotation)
        {
            this.centerPosition = position;

            //X movement
            this.periodX = periodX;
            this.periodOffsetX = periodOffsetX;
            this.amplitudeX = amplitudeX;

            //Y movement
            this.periodY = periodY;
            this.periodOffsetY = periodOffsetY;
            this.amplitudeY = amplitudeY;
        }

        public BackgroundObject_Oscillation(Texture2D texture, Vector2 position,
            double periodX, double periodOffsetX, float amplitudeX,
            double periodY, double periodOffsetY, float amplitudeY)
            : this(texture, position, 0f, periodX, periodOffsetX, amplitudeX, periodY, periodOffsetY, amplitudeY)
        {
        }

        public BackgroundObject_Oscillation(Texture2D texture, Vector2 position,
            double periodX, double periodOffsetX, float amplitudeX)
            : this(texture, position, 0f, periodX, periodOffsetX, amplitudeX, 0d, 0d, 0f)
        {
        }

        public BackgroundObject_Oscillation(Texture2D texture, Vector2 position, bool distinguishingVariableThatDoesAbsolutelyNothingButPreventAmbiguityBetweenThisConstructorAndTheOneAbove,
            double periodY, double periodOffsetY, float amplitudeY)
            : this(texture, position, 0f, 0d, 0d, 0f, periodY, periodOffsetY, amplitudeY)
        {
        }

        public BackgroundObject_Oscillation(Texture2D texture, Vector2 position)
            : this(texture, position, 0f, 0d, 0d, 0f, 0d, 0d, 0f)
        {
        }

        public override void Update(GameTime gameTime)
        {
            float xDisplacement = 0f;
            float yDisplacement = 0f;
            if (periodX != 0)
                xDisplacement = amplitudeX * (float)Math.Sin(MathHelper.TwoPi * (gameTime.TotalGameTime.TotalMilliseconds + periodOffsetX) / periodX);
            if (periodY != 0)
                yDisplacement = amplitudeY * (float)Math.Sin(MathHelper.TwoPi * (gameTime.TotalGameTime.TotalMilliseconds + periodOffsetY) / periodY);

            position = centerPosition + new Vector2(xDisplacement, yDisplacement);
        }
    }
}
