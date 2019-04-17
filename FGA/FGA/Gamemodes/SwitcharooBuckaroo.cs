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
using FGA.Shapes;
using System.Net.Sockets;
namespace FGA
{
    class SwitcharooBuckaroo : Level
    {
        public int linePenalty;
        public List<int> inputs = new List<int>();

        protected override int TimeDelayMilliseconds
        {
            get
            {
                return (850 - (CurrentLevel * 50)) - (linePenalty * 50);
            }
        }
        public SwitcharooBuckaroo(PlayerIndex player)
            : base(player)
        {
            if (player == PlayerIndex.One)
            {
                position = new Vector2(289, 954) * Game1.displayRatio;
                scoreDisplayPosition = new Vector2(705, 470) * Game1.displayRatio;
                nextBlockDisplayPosition = new Vector2(520, -703) * Game1.displayRatio;
            }
            else if (player == PlayerIndex.Two)
            {
                position = new Vector2(1250, 954) * Game1.displayRatio;
                scoreDisplayPosition = new Vector2(960, 470) * Game1.displayRatio;
                nextBlockDisplayPosition = new Vector2(-175, -703) * Game1.displayRatio;
            }

            ShuffleInput();
        }

        public SwitcharooBuckaroo(NetworkStream netStream)
            : base(netStream)
        {
            //enemyLevel.scoreDisplayPosition = new Vector2(960, 470) * Game1.displayRatio;
            //enemyLevel.nextBlockDisplayPosition = new Vector2(-175, -703) * Game1.displayRatio;

            ShuffleInput();
        }

        public override void MoveLeft()
        {
            ManageInput(inputs[0]);
        }

        public override void MoveRight()
        {
            ManageInput(inputs[1]);
        }

        public override void Rotate()
        {
            ManageInput(inputs[2]);
        }

        public override void DropOne()
        {
            ManageInput(inputs[3]);
        }

        protected void ManageInput(int index)
        {
            switch (index)
            {
                case 0: base.MoveLeft();
                    break;
                case 1: base.MoveRight();
                    break;
                case 2: base.Rotate();
                    break;
                case 3: base.DropOne();
                    break;
            }
        }

        public void ShuffleInput()
        {
            inputs.Clear();
            List<int> integers = new List<int> { 0, 1, 2, 3 };
            int num;

            for (int i = 0; i < 4; i++)
            {
                num = Game1.rnd.Next(0, integers.Count);
                inputs.Add(integers[num]);
                integers.RemoveAt(num);
            }

        }

        protected override void AddShapeToGrid()
        {
            currentFallingShape.AddShapeToGrid(grid);
            NextFallingShape();
            int num = CheckCompletedRows();
            if (num > 0)
            {
                linePenalty = Math.Max(0, linePenalty - num);

                gameClassReference.OnLinesCompleted(player, num);
            }
        }

        public void OnEnemyCompletedLines(int lines)
        {
            linePenalty += lines;
            ShuffleInput();
        }
    }
}
