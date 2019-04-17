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
    class LineEmUp : Level
    {
        public static List<Texture2D> BlockTierTextures = new List<Texture2D>();

        public Vector2 textPosition;
        public List<Action> actions = new List<Action>(0);
        public int health;
        public int shield;
        public static double[] colorChances = new double[6];
     

        public LineEmUp(PlayerIndex player)
            : base(player)
        {
            currentFallingShape.loose = true;
            health = 1000;
            shield = 0;

            if (player == PlayerIndex.One)
            {
                position = new Vector2(289, 954) * Game1.displayRatio;
                nextBlockDisplayPosition = new Vector2(520, -703) * Game1.displayRatio;
                textPosition = new Vector2(830, 500) * Game1.displayRatio;
            }
            else if (player == PlayerIndex.Two)
            {
                position = new Vector2(1250, 954) * Game1.displayRatio;
                nextBlockDisplayPosition = new Vector2(-175, -703) * Game1.displayRatio;
                textPosition = new Vector2(1085, 500) * Game1.displayRatio;
            }
        }

        public LineEmUp(NetworkStream netStream)
            : base(netStream)
        {
            //enemyLevel = new LineEmUp(PlayerIndex.Two);
            //enemyLevel.scoreDisplayPosition = new Vector2(1060, 470) * Game1.displayRatio;
            //enemyLevel.nextBlockDisplayPosition = new Vector2(-175, -703) * Game1.displayRatio;
            //((LineEmUp)enemyLevel).textPosition = new Vector2(1085, 500) * Game1.displayRatio;
        }

        protected void CheckCompletion(List<Vector2> positions, int combo)
        {
            int i, j, q;
            int left, right, up, down;
            bool stop;
            bool gold = false;
            Block current = new Block(true);
            List<Action> actionQueue = new List<Action>(0);
            List<Vector2> deletePositions = new List<Vector2>(0);
            List<Vector2> newCheckPositions = new List<Vector2>(0);

            List<List<CheckGrid>> checker = new List<List<CheckGrid>>(0);

            for (i = 0; i < grid.Count; i++)
            {
                checker.Add(new List<CheckGrid>(0));
                for (j = 0; j < grid[i].Count; j++)
                {
                    checker[i].Add(new CheckGrid());
                }
            }

            for (i = 0; i < positions.Count; i++)
            {

                //if the vertical and horizontal are checked already, then it cant be, so end this current loop.
                left = 0;
                right = 0;
                up = 0;
                down = 0;

                current.blockColor.color = grid[(int)positions[i].Y][(int)positions[i].X].blockColor.color;
                gold = current.blockColor.color == Colors.Gold;

                if (current.blockColor.color != Colors.Rainbow)
                {
                    if (!checker[(int)positions[i].Y][(int)positions[i].X].checkedHorizontal)
                    {
                        stop = false;
                        q = 0;

                        do
                        {
                            q--; //Move the grid position in the direction we want
                            try //Prevent the index from going out of range
                            {
                                if ((grid[(int)positions[i].Y][(int)(positions[i].X + q)].blockColor.color == current.blockColor.color && checker[(int)positions[i].Y][(int)(positions[i].X + q)].checkedHorizontal != true) || (!gold && grid[(int)positions[i].Y][(int)(positions[i].X + q)].blockColor.color == Colors.Rainbow))
                                {
                                    //If they are the same color, then that block has been successfully checked and add it to our current count
                                    left++;
                                    if (grid[(int)positions[i].Y][(int)(positions[i].X + q)].blockColor.color != Colors.Rainbow)
                                        checker[(int)positions[i].Y][(int)(positions[i].X + q)].checkedHorizontal = true;
                                }
                                else
                                    stop = true;

                            }
                            catch
                            {
                                stop = true;
                            }


                        } while (!stop);

                        stop = false;
                        q = 0;

                        do
                        {
                            q++; //Move the grid position in the direction we want
                            try //Prevent the index from going out of range
                            {
                                if ((grid[(int)positions[i].Y][(int)(positions[i].X + q)].blockColor.color == current.blockColor.color && checker[(int)positions[i].Y][(int)(positions[i].X + q)].checkedHorizontal != true) || (!gold && grid[(int)positions[i].Y][(int)(positions[i].X + q)].blockColor.color == Colors.Rainbow))
                                {
                                    //If they are the same color, then that block has been successfully checked and add it to our current count
                                    right++;
                                    if (grid[(int)positions[i].Y][(int)(positions[i].X + q)].blockColor.color != Colors.Rainbow)
                                        checker[(int)positions[i].Y][(int)(positions[i].X + q)].checkedHorizontal = true;
                                }
                                else
                                    stop = true;

                            }
                            catch
                            {
                                stop = true;
                            }


                        } while (!stop);
                    }

                    if (!checker[(int)positions[i].Y][(int)positions[i].X].checkedVertical)
                    {
                        stop = false;
                        q = 0;

                        do
                        {
                            q++; //Move the grid position in the direction we want
                            try //Prevent the index from going out of range
                            {
                                if ((grid[(int)positions[i].Y + q][(int)(positions[i].X)].blockColor.color == current.blockColor.color && checker[(int)positions[i].Y + q][(int)(positions[i].X)].checkedVertical != true) || (!gold && grid[(int)positions[i].Y + q][(int)(positions[i].X)].blockColor.color == Colors.Rainbow))
                                {
                                    //If they are the same color, then that block has been successfully checked and add it to our current count
                                    up++;
                                    if (grid[(int)positions[i].Y + q][(int)(positions[i].X)].blockColor.color != Colors.Rainbow)
                                        checker[(int)positions[i].Y + q][(int)(positions[i].X)].checkedVertical = true;
                                }
                                else
                                    stop = true;

                            }
                            catch
                            {
                                stop = true;
                            }


                        } while (!stop);

                        stop = false;
                        q = 0;

                        do
                        {
                            q--; //Move the grid position in the direction we want
                            try //Prevent the index from going out of range
                            {
                                if ((grid[(int)positions[i].Y + q][(int)(positions[i].X)].blockColor.color == current.blockColor.color && checker[(int)positions[i].Y + q][(int)(positions[i].X)].checkedVertical != true) || (!gold && grid[(int)positions[i].Y + q][(int)(positions[i].X)].blockColor.color == Colors.Rainbow))
                                {
                                    //If they are the same color, then that block has been successfully checked and add it to our current count
                                    down++;
                                    if (grid[(int)positions[i].Y + q][(int)(positions[i].X)].blockColor.color != Colors.Rainbow)
                                        checker[(int)positions[i].Y + q][(int)(positions[i].X)].checkedVertical = true;
                                }
                                else
                                    stop = true;

                            }
                            catch
                            {
                                stop = true;
                            }


                        } while (!stop);
                    }

                    if (left + right >= 3)
                    {
                        deletePositions.Add(positions[i]);
                        //Add to the delete list and collect values
                        Action action = new Action();

                        switch (current.blockColor.color)
                        {
                            //Assign defense value based on color
                            case Colors.Blue: action.defense = (150 + ((left + right - 3) * 10)) * combo;
                                break;
                            case Colors.Red: action.defense = (25 + ((left + right - 3) * 5)) * combo;
                                break;
                            case Colors.Green: action.defense = (50 + ((left + right - 3) * 5)) * combo;
                                break;
                            case Colors.Purple: action.defense = (75 + ((left + right - 3) * 40)) * combo;
                                break;
                            case Colors.Gold: action.defense = (200 + ((left + right - 3) * 20)) * combo;
                                break;

                        }

                        //Add this action to the list of actions for this check
                        actionQueue.Add(action);

                        for (j = 1; j <= left; j++)
                        {
                            deletePositions.Add(new Vector2(positions[i].X - j, positions[i].Y));
                        }

                        for (j = 1; j <= right; j++)
                        {
                            deletePositions.Add(new Vector2(positions[i].X + j, positions[i].Y));
                        }


                    }

                    if (up + down >= 4)
                    {
                        deletePositions.Add(positions[i]);
                        //Add to the delete list and collect values
                        Action action = new Action();

                        switch (current.blockColor.color)
                        {
                            //Assign damage value based on color
                            case Colors.Blue: action.damage = (25 + ((up + down - 4) * 5)) * combo;
                                action.againstArmor = (25 + ((up + down - 4) * 5)) * combo;
                                break;
                            case Colors.Red: action.damage = (100 + ((up + down - 4) * 10)) * combo;
                                action.againstArmor = (75 + ((up + down - 4) * 5)) * combo;
                                break;
                            case Colors.Green: action.damage = (50 + ((up + down - 4) * 5)) * combo;
                                action.againstArmor = (125 + ((up + down - 4) * 15)) * combo;
                                break;
                            case Colors.Purple: action.damage = (50 + ((up + down - 4) * 40)) * combo;
                                action.againstArmor = (50 + ((up + down - 4) * 40)) * combo;
                                break;
                            case Colors.Gold: action.damage = (200 + ((up + down - 4) * 20)) * combo;
                                action.againstArmor = (200 + ((up + down - 4) * 20)) * combo;
                                break;

                        }

                        //Add this action to the list of actions for this check
                        actionQueue.Add(action);

                        for (j = 1; j <= up; j++)
                        {
                            deletePositions.Add(new Vector2(positions[i].X, positions[i].Y + j));
                        }

                        for (j = 1; j <= down; j++)
                        {
                            deletePositions.Add(new Vector2(positions[i].X, positions[i].Y - j));
                        }
                    }

                    checker[(int)positions[i].Y][(int)positions[i].X].checkedHorizontal = true;
                    checker[(int)positions[i].Y][(int)positions[i].X].checkedVertical = true;

                }
                else
                {
                    //do code here
                    Colors upColor;
                    Colors downColor;
                    Colors leftColor;
                    Colors rightColor;
                    int leftRainbows = 0;
                    int rightRainbows = 0;
                    int upRainbows = 0;
                    int downRainbows = 0;
                   // int verticalRainbows = 0;


                    //Determine the color on all sides of the rainbow block, but dont go out of bounds
                    try
                    {
                        q = 0;
                        do{

                            q++;
                            upColor = grid[(int)positions[i].Y + q][(int)positions[i].X].blockColor.color;
                            if (upColor == Colors.Rainbow)
                            {
                                upRainbows++;
                            }

                        } while (upColor == Colors.Rainbow);
                        
                    }
                    catch
                    {
                        upColor = Colors.Empty;
                    }

                    try
                    {
                        q = 0;
                        do{

                            q--;
                            downColor = grid[(int)positions[i].Y + q][(int)positions[i].X].blockColor.color;
                            if (downColor == Colors.Rainbow)
                            {
                                downRainbows++;
                            }

                        } while (downColor == Colors.Rainbow);
                    }
                    catch
                    {
                        downColor = Colors.Empty;
                    }

                    try
                    {
                        q = 0;
                        do{

                            q--;
                            leftColor = grid[(int)positions[i].Y][(int)positions[i].X + q].blockColor.color;
                            if (leftColor == Colors.Rainbow)
                            {
                                leftRainbows++;
                            }

                        } while (leftColor == Colors.Rainbow);
                    }
                    catch
                    {
                        leftColor = Colors.Empty;
                    }

                    try
                    {
                        q = 0;
                        do{

                            q++;
                            rightColor = grid[(int)positions[i].Y][(int)positions[i].X + q].blockColor.color;
                            if (rightColor == Colors.Rainbow)
                            {
                                rightRainbows++;
                            }

                        } while (rightColor == Colors.Rainbow);
                    }
                    catch
                    {
                        rightColor = Colors.Empty;
                    }



                    if(leftColor != Colors.Empty && leftColor != Colors.Gold)
                    {
                        stop = false;
                        q = -leftRainbows;
                        left = 0;

                        do
                        {
                            q--; //Move the grid position in the direction we want
                            try //Prevent the index from going out of range
                            {
                                if ((grid[(int)positions[i].Y][(int)(positions[i].X + q)].blockColor.color == leftColor && checker[(int)positions[i].Y][(int)(positions[i].X + q)].checkedHorizontal != true) || (!gold && grid[(int)positions[i].Y][(int)(positions[i].X + q)].blockColor.color == Colors.Rainbow))
                                {
                                    //If they are the same color, then that block has been successfully checked and add it to our current count
                                    left++;
                                    if (grid[(int)positions[i].Y][(int)(positions[i].X + q)].blockColor.color != Colors.Rainbow)
                                        checker[(int)positions[i].Y][(int)(positions[i].X + q)].checkedHorizontal = true;
                                }
                                else
                                    stop = true;

                            }
                            catch
                            {
                                stop = true;
                            }


                        } while (!stop);

                    }

                    if (rightColor != Colors.Empty && leftColor != Colors.Gold)
                    {
                        stop = false;
                        q = rightRainbows;
                        right = 0;
                    

                        do
                        {
                            q++; //Move the grid position in the direction we want
                            try //Prevent the index from going out of range
                            {
                                if ((grid[(int)positions[i].Y][(int)(positions[i].X + q)].blockColor.color == rightColor && checker[(int)positions[i].Y][(int)(positions[i].X + q)].checkedHorizontal != true) || (!gold && grid[(int)positions[i].Y][(int)(positions[i].X + q)].blockColor.color == Colors.Rainbow))
                                {
                                    //If they are the same color, then that block has been successfully checked and add it to our current count
                                    right++;
                                    if (grid[(int)positions[i].Y][(int)(positions[i].X + q)].blockColor.color != Colors.Rainbow)
                                        checker[(int)positions[i].Y][(int)(positions[i].X + q)].checkedHorizontal = true;
                                }
                                else
                                    stop = true;

                            }
                            catch
                            {
                                stop = true;
                            }


                        } while (!stop);
                    }

                    if (upColor != Colors.Empty && leftColor != Colors.Gold)
                    {
                        stop = false;
                        q = upRainbows;
                        up = 0;

                        do
                        {
                            q++; //Move the grid position in the direction we want
                            try //Prevent the index from going out of range
                            {
                                if ((grid[(int)positions[i].Y + q][(int)(positions[i].X)].blockColor.color == upColor && checker[(int)positions[i].Y + q][(int)(positions[i].X)].checkedVertical != true) || (!gold && grid[(int)positions[i].Y + q][(int)(positions[i].X)].blockColor.color == Colors.Rainbow))
                                {
                                    //If they are the same color, then that block has been successfully checked and add it to our current count
                                    up++;
                                    if (grid[(int)positions[i].Y + q][(int)(positions[i].X)].blockColor.color != Colors.Rainbow)
                                        checker[(int)positions[i].Y + q][(int)(positions[i].X)].checkedVertical = true;
                                }
                                else
                                    stop = true;

                            }
                            catch
                            {
                                stop = true;
                            }


                        } while (!stop);
                    }

                    if (downColor != Colors.Empty && leftColor != Colors.Gold)
                    {
                        stop = false;
                        q = -downRainbows;
                        down = 0;

                        do
                        {
                            q--; //Move the grid position in the direction we want
                            try //Prevent the index from going out of range
                            {
                                if ((grid[(int)positions[i].Y + q][(int)(positions[i].X)].blockColor.color == downColor && checker[(int)positions[i].Y + q][(int)(positions[i].X)].checkedVertical != true) || (!gold && grid[(int)positions[i].Y + q][(int)(positions[i].X)].blockColor.color == Colors.Rainbow))
                                {
                                    //If they are the same color, then that block has been successfully checked and add it to our current count
                                    down++;
                                    if (grid[(int)positions[i].Y + q][(int)(positions[i].X)].blockColor.color != Colors.Rainbow)
                                        checker[(int)positions[i].Y + q][(int)(positions[i].X)].checkedVertical = true;
                                }
                                else
                                    stop = true;

                            }
                            catch
                            {
                                stop = true;
                            }


                        } while (!stop);
                    }

                    if (leftColor == rightColor && leftColor != Colors.Empty && leftColor != Colors.Gold)
                    {
                        //If both sides of the rainbow are the same color, add them all up
                        int total = left + right + leftRainbows + rightRainbows;
                        if (total >= 3)
                        {
                            deletePositions.Add(positions[i]);
                            //Add to the delete list and collect values
                            Action action = new Action();

                            switch (leftColor)
                            {
                                //Assign defense value based on color
                                case Colors.Blue: action.defense = (150 + ((total - 3) * 10)) * combo;
                                    break;
                                case Colors.Red: action.defense = (25 + ((total - 3) * 5)) * combo;
                                    break;
                                case Colors.Green: action.defense = (50 + ((total - 3) * 5)) * combo;
                                    break;
                                case Colors.Purple: action.defense = (75 + ((total - 3) * 40)) * combo;
                                    break;
                                case Colors.Gold: action.defense = (200 + ((total - 3) * 20)) * combo;
                                    break;

                            }

                            //Add this action to the list of actions for this check
                            actionQueue.Add(action);

                            for (j = 1; j <= left + leftRainbows; j++)
                            {
                                deletePositions.Add(new Vector2(positions[i].X - j, positions[i].Y));
                            }

                            for (j = 1; j <= right + rightRainbows; j++)
                            {
                                deletePositions.Add(new Vector2(positions[i].X + j, positions[i].Y));
                            }
                        }
                    }
                    else
                    {
                        bool cleared = false;
                        //If the colors on either side are different, check them independently
                        if (leftColor != Colors.Empty && leftColor != Colors.Gold)
                        {
                            int total = rightRainbows + leftRainbows + left;
                            if (total >= 3)
                            {
                                cleared = true;
                                //Add to the delete list and collect values
                                Action action = new Action();

                                switch (leftColor)
                                {
                                    //Assign defense value based on color
                                    case Colors.Blue: action.defense = (150 + ((total - 3) * 10)) * combo;
                                        break;
                                    case Colors.Red: action.defense = (25 + ((total - 3) * 5)) * combo;
                                        break;
                                    case Colors.Green: action.defense = (50 + ((total - 3) * 5)) * combo;
                                        break;
                                    case Colors.Purple: action.defense = (75 + ((total - 3) * 40)) * combo;
                                        break;
                                    case Colors.Gold: action.defense = (200 + ((total - 3) * 20)) * combo;
                                        break;

                                }

                                //Add this action to the list of actions for this check
                                actionQueue.Add(action);

                                for (j = leftRainbows+1; j <= leftRainbows + left; j++)
                                {
                                    deletePositions.Add(new Vector2(positions[i].X - j, positions[i].Y));
                                }

                            }
                        }

                        if (rightColor != Colors.Empty && rightColor != Colors.Gold)
                        {
                            int total = rightRainbows + leftRainbows + right;
                            if (total >= 3)
                            {
                                cleared = true;
                                //Add to the delete list and collect values
                                Action action = new Action();

                                switch (rightColor)
                                {
                                    //Assign defense value based on color
                                    case Colors.Blue: action.defense = (150 + ((total - 3) * 10)) * combo;
                                        break;
                                    case Colors.Red: action.defense = (25 + ((total - 3) * 5)) * combo;
                                        break;
                                    case Colors.Green: action.defense = (50 + ((total - 3) * 5)) * combo;
                                        break;
                                    case Colors.Purple: action.defense = (75 + ((total - 3) * 40)) * combo;
                                        break;
                                    case Colors.Gold: action.defense = (200 + ((total - 3) * 20)) * combo;
                                        break;

                                }

                                //Add this action to the list of actions for this check
                                actionQueue.Add(action);

                                for (j = rightRainbows+1; j <= rightRainbows+right; j++)
                                {
                                    deletePositions.Add(new Vector2(positions[i].X + j, positions[i].Y));
                                }

                            }
                        }

                        if (cleared)
                        {
                            deletePositions.Add(positions[i]);
                            for (j = 1; j <= leftRainbows; j++)
                            {
                                deletePositions.Add(new Vector2(positions[i].X - j, positions[i].Y));
                            }

                            for (j = 1; j <= rightRainbows; j++)
                            {
                                deletePositions.Add(new Vector2(positions[i].X + j, positions[i].Y));
                            }
                        }

                    }




                    if (upColor == downColor && upColor != Colors.Empty && upColor != Colors.Gold)
                    {
                        //If both sides of the rainbow are the same color, add them all up
                        int total = up+down+upRainbows+downRainbows;
                        if (total >= 4)
                        {
                            deletePositions.Add(positions[i]);
                            //Add to the delete list and collect values
                            Action action = new Action();

                            switch (upColor)
                            {
                                //Assign damage value based on color
                                case Colors.Blue: action.damage = (25 + ((up + down - 4) * 5)) * combo;
                                    action.againstArmor = (25 + ((up + down - 4) * 5)) * combo;
                                    break;
                                case Colors.Red: action.damage = (100 + ((up + down - 4) * 10)) * combo;
                                    action.againstArmor = (75 + ((up + down - 4) * 5)) * combo;
                                    break;
                                case Colors.Green: action.damage = (50 + ((up + down - 4) * 5)) * combo;
                                    action.againstArmor = (125 + ((up + down - 4) * 15)) * combo;
                                    break;
                                case Colors.Purple: action.damage = (50 + ((up + down - 4) * 40)) * combo;
                                    action.againstArmor = (50 + ((up + down - 4) * 40)) * combo;
                                    break;
                                case Colors.Gold: action.damage = (200 + ((up + down - 4) * 20)) * combo;
                                    action.againstArmor = (200 + ((up + down - 4) * 20)) * combo;
                                    break;

                            }

                            //Add this action to the list of actions for this check
                            actionQueue.Add(action);

                            for (j = 1; j <= up + upRainbows; j++)
                            {
                                deletePositions.Add(new Vector2(positions[i].X, positions[i].Y + j));
                            }

                            for (j = 1; j <= down + downRainbows; j++)
                            {
                                deletePositions.Add(new Vector2(positions[i].X, positions[i].Y - j));
                            }
                        }
                    }
                    else
                    {
                        bool cleared = false;
                        //If the colors on either side are different, check them independently
                        if (upColor != Colors.Empty && upColor != Colors.Gold)
                        {
                            int total = downRainbows + upRainbows + up;
                            if (total >= 4)
                            {
                                cleared = true;
                                //Add to the delete list and collect values
                                Action action = new Action();

                                switch (upColor)
                                {
                                    //Assign damage value based on color
                                    case Colors.Blue: action.damage = (25 + ((up + down - 4) * 5)) * combo;
                                        action.againstArmor = (25 + ((up + down - 4) * 5)) * combo;
                                        break;
                                    case Colors.Red: action.damage = (100 + ((up + down - 4) * 10)) * combo;
                                        action.againstArmor = (75 + ((up + down - 4) * 5)) * combo;
                                        break;
                                    case Colors.Green: action.damage = (50 + ((up + down - 4) * 5)) * combo;
                                        action.againstArmor = (125 + ((up + down - 4) * 15)) * combo;
                                        break;
                                    case Colors.Purple: action.damage = (50 + ((up + down - 4) * 40)) * combo;
                                        action.againstArmor = (50 + ((up + down - 4) * 40)) * combo;
                                        break;
                                    case Colors.Gold: action.damage = (200 + ((up + down - 4) * 20)) * combo;
                                        action.againstArmor = (200 + ((up + down - 4) * 20)) * combo;
                                        break;

                                }

                                //Add this action to the list of actions for this check
                                actionQueue.Add(action);

                                for (j = upRainbows + 1; j <= upRainbows + up; j++)
                                {
                                    deletePositions.Add(new Vector2(positions[i].X, positions[i].Y + j));
                                }

                            }
                        }

                        if (downColor != Colors.Empty && downColor != Colors.Gold)
                        {
                            int total = upRainbows + downRainbows + down;
                            if (total >= 4)
                            {
                                cleared = true;
                                //Add to the delete list and collect values
                                Action action = new Action();

                                switch (downColor)
                                {
                                    //Assign damage value based on color
                                    case Colors.Blue: action.damage = (25 + ((up + down - 4) * 5)) * combo;
                                        action.againstArmor = (25 + ((up + down - 4) * 5)) * combo;
                                        break;
                                    case Colors.Red: action.damage = (100 + ((up + down - 4) * 10)) * combo;
                                        action.againstArmor = (75 + ((up + down - 4) * 5)) * combo;
                                        break;
                                    case Colors.Green: action.damage = (50 + ((up + down - 4) * 5)) * combo;
                                        action.againstArmor = (125 + ((up + down - 4) * 15)) * combo;
                                        break;
                                    case Colors.Purple: action.damage = (50 + ((up + down - 4) * 40)) * combo;
                                        action.againstArmor = (50 + ((up + down - 4) * 40)) * combo;
                                        break;
                                    case Colors.Gold: action.damage = (200 + ((up + down - 4) * 20)) * combo;
                                        action.againstArmor = (200 + ((up + down - 4) * 20)) * combo;
                                        break;

                                }

                                //Add this action to the list of actions for this check
                                actionQueue.Add(action);

                                for (j = downRainbows + 1; j <= downRainbows + down; j++)
                                {
                                    deletePositions.Add(new Vector2(positions[i].X, positions[i].Y - j));
                                }

                            }
                        }

                        if (cleared)
                        {
                            deletePositions.Add(positions[i]);
                            for (j = 1; j <= upRainbows; j++)
                            {
                                deletePositions.Add(new Vector2(positions[i].X, positions[i].Y + j));
                            }

                            for (j = 1; j <= downRainbows; j++)
                            {
                                deletePositions.Add(new Vector2(positions[i].X, positions[i].Y - j));
                            }
                        }

                    }


                }
            }
            
            //Delete blocks
            for (i = 0; i < deletePositions.Count; i++)
            {
                stop = false;
                for (j = (int)deletePositions[i].Y; j < grid.Count && !stop; j++)
                {
                    for (q = 0; q < deletePositions.Count; q++)
                    {
                        if (deletePositions[q].X == deletePositions[i].X && deletePositions[q].Y > deletePositions[i].Y)
                            deletePositions[q] = new Vector2(deletePositions[q].X, deletePositions[q].Y -1);

                    }

                    //Move down the blocks in that column
                    try
                    {
                        if (grid[j + 1][(int)deletePositions[i].X].enabled)
                        {
                            grid[j][(int)deletePositions[i].X] = grid[j + 1][(int)deletePositions[i].X];
                        }
                        else
                        {
                            stop = true;
                            grid[j][(int)deletePositions[i].X] = new Block(false);
                        }
                    }
                    catch
                    {
                        stop = true;
                        grid[j][(int)deletePositions[i].X] = new Block(false);
                    }
                }
            }

            //If multiple lines are completed at the same time, then they all get the highest stat of the bunch
            int maxDamage = 0;
            int maxAgainstArmor = 0;
            int maxDefense = 0;

            for (i = 0; i < actionQueue.Count; i++)
             {
                if (actionQueue[i].damage > maxDamage)
                    maxDamage = actionQueue[i].damage;
                else
                    actionQueue[i].damage = maxDamage;

                if (actionQueue[i].againstArmor > maxAgainstArmor)
                    maxAgainstArmor = actionQueue[i].againstArmor;
                else
                    actionQueue[i].againstArmor = maxAgainstArmor;

                if (actionQueue[i].defense > maxDefense)
                    maxDefense = actionQueue[i].defense;
                else
                    actionQueue[i].defense = maxDefense;



            }

            //Add all new actions into the player's action list
            actions.AddRange(actionQueue);

            //Holds the lowest y value for delete blocks in that column
            float?[] lowests = new float?[10] {null,null,null,null,null,null,null,null,null,null};

            //Determine the lowests
            for (i = 0; i < deletePositions.Count; i++)
            {
                if (lowests[(int)deletePositions[i].X] == null || deletePositions[i].Y < lowests[(int)deletePositions[i].X])
                    lowests[(int)deletePositions[i].X] = deletePositions[i].Y;
            }

            //Go from the lowest position up until you hit an unenabled block, then add that to the new list of positions we need to check
            int c;
            for (i = 0; i < lowests.Length; i++)
            {
                if (lowests[i] != null && grid[(int)lowests[i]][i].enabled)
                {
                    newCheckPositions.Add(new Vector2((float)i, (float)lowests[i]));

                    c = 1;

                    while (grid[(int)lowests[i]+c][i].enabled)
                    {
                        newCheckPositions.Add(new Vector2((float)i, (float)lowests[i] + c));
                        c++;
                    }

                }
            }

            if (newCheckPositions.Count > 0)
                CheckCompletion(newCheckPositions, combo + 1);
        }

        protected override Shape GenerateRandomShape()
        {
            Shape generatedShape = base.GenerateRandomShape();
            setBlockColor(generatedShape, BlockColors.LineEmUpColors, colorChances);

            return generatedShape;
        }

        protected override void AddShapeToGrid()
        {
            //Add deleting unenabled blocks underneath the new blocks
            List<Vector2> checkPositions = new List<Vector2>(0);
            Texture2D temp;
            Vector2 superTemp;
            Colors ultraTemp;
            int i, j;


            //Sort the current shape based off of relative positions so that it can match the indicator
            for (i = 0; i < currentFallingShape.blocks.Count; i++)
            {
                for (j = i + 1; j < currentFallingShape.blocks.Count; j++)
                {
                    if(currentFallingShape.stateRelativePositions[currentFallingShape.currentShapeIndex][i].Y > currentFallingShape.stateRelativePositions[currentFallingShape.currentShapeIndex][j].Y)
                    {
                        temp = currentFallingShape.blocks[i].blockColor.texture;
                        currentFallingShape.blocks[i].blockColor.texture = currentFallingShape.blocks[j].blockColor.texture;
                        currentFallingShape.blocks[j].blockColor.texture = temp;

                        superTemp = currentFallingShape.stateRelativePositions[currentFallingShape.currentShapeIndex][i];
                        currentFallingShape.stateRelativePositions[currentFallingShape.currentShapeIndex][i] = currentFallingShape.stateRelativePositions[currentFallingShape.currentShapeIndex][j];
                        currentFallingShape.stateRelativePositions[currentFallingShape.currentShapeIndex][j] = superTemp;

                        ultraTemp = currentFallingShape.blocks[i].blockColor.color;
                        currentFallingShape.blocks[i].blockColor.color = currentFallingShape.blocks[j].blockColor.color;
                        currentFallingShape.blocks[j].blockColor.color = ultraTemp;

                    }

                }

            }

            //Place the blocks where the indicator says they should go
            for(i=0; i<currentFallingShape.blocks.Count; i++)
            {
                currentFallingShape.stateRelativePositions[currentFallingShape.currentShapeIndex][i] = indicatorShape.shapes[indicatorShape.indicatorShapeIndex].blocks[i].position - currentFallingShape.gridPosition;
                checkPositions.Add(indicatorShape.shapes[indicatorShape.indicatorShapeIndex].blocks[i].position);
            }

            currentFallingShape.AddShapeToGrid(grid);

            //Check to see if any lines are made
            CheckCompletion(checkPositions, 1);

            //Pull next shape from queue and generate new shape for queue
            NextFallingShape();
        }

        protected override void UpdateInGame(GameTime gameTime)
        {
            base.UpdateInGame(gameTime);

            //If any lines have been completed, the associated actions need to be carried out
            if (actions.Count > 0)
            {
                for (int i = 0; i < actions.Count; i++)
                {
                    shield += actions[i].defense;

                    //Deal damage to the enemy
                    //Enemy will now do the damage themselves instead of calculating on this side of the field
                    gameClassReference.OnAttackEnemy(player, actions[i]);
                }

                //Get rid of the actions after finished
                while (actions.Count > 0)
                {
                    actions.RemoveAt(0);
                }
            }
        }

        public void TakeDamage(Action action)
        {
            //Damage is first done against shield. If there is left over damage, 
            //the percentage left over is then multiplied by the against health 
            //damage to get the resulting reduction of health
            if (action.againstArmor > 0)
            {
                if (shield > action.againstArmor)
                    shield -= action.againstArmor;
                else
                {
                    float percent = (float)action.againstArmor - (float)shield / (float)action.againstArmor;
                    shield = 0;
                    health -= (int)(percent * action.damage);
                }
            }
        }

        //Draw Health and Shield instead of Score
        protected override void DrawScore(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //Draw Health and Shield
            Game1.Drawnumbers(health.ToString(), textPosition, player, spriteBatch);
            Game1.Drawnumbers(shield.ToString(), textPosition + new Vector2(0, 155) * Game1.displayRatio, player, spriteBatch);

 
        }

        public override void SetIndicator()
        {
            indicatorShape = new IndicatorShape(currentFallingShape, true);
            UpdateIndicator();
        }

        protected override void UpdateIndicator()
        {
            indicatorShape.DetermineLoosePosition(currentFallingShape, grid);
        }

        private void setBlockColor(Shape shape, BlockColor[] blockColors, double[] colorChances)
        {
            if (blockColors.Length == colorChances.Length)
            {
                double chance = Game1.rnd.NextDouble();
                double chanceTotal = 0;
                int element = 0;

                for (int i = 0; i < shape.blocks.Count; i++)
                {
                    if(i == 0 || ((Game1.rnd.NextDouble() < .6) || blockColors[element].color == Colors.Rainbow))
                    for (int j = 0; j < colorChances.Length; j++)
                    {
                        chanceTotal += colorChances[j];
                        if (chance < chanceTotal)
                        {
                            element = j;
                            shape.blocks[i].SetColor(blockColors[element]);
                            break;
                        }
                    }
                }
            }
        }

        public override void Lose()
        {
            shield = 0;
            health -= 150;

            //Only clear the board if player can continue playing
            if (health > 0)
            {
                grid.Clear();
                for (int i = 0; i < 22; i++)
                {
                    GenerateNewRow();
                }
                SetIndicator();
            }
            else
            {
                base.Lose();
            }
        }
    }
}
