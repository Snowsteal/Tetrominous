using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using Microsoft.Xna.Framework;
using FGA;
using FGA.Shapes;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Xna.Framework.Graphics;

namespace FGA.Online
{
    public class Converter
    {
        [Serializable]
        public struct DataLevelSimple
        {
            public List<List<DataBlock>> grid;
            public string shapeQueueData;
        }

        [Serializable]
        public struct DataStruct
        {
            public List<List<DataBlock>> grid;
            public DataShape currentFallingShape;
            public DataShape shapeQueue;
            public List<string> messages;

            public override bool Equals(object obj)
            {
                try
                {
                    if (!(obj is DataStruct))
                        return false;

                    DataStruct d = (DataStruct)obj;


                    for (int i = 0; i < grid.Count; i++)
                    {
                        for (int j = 0; j < grid[i].Count; j++)
                        {
                            if (!grid[i][j].Equals(d.grid[i][j]))
                                return false;
                        }
                    }

                    if (!currentFallingShape.Equals(d.currentFallingShape))
                        return false;

                    if (!shapeQueue.Equals(d.shapeQueue))
                        return false;

                    for (int i = 0; i < messages.Count; i++)
                    {
                        if (messages[i] != d.messages[i])
                            return false;
                    }

                    return true;
                }
                catch
                {
                    return false;
                }
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        };

        [Serializable]
        public struct DataBlock
        {
            public bool enabled;
            public int index;

            public override bool Equals(object obj)
            {
                if (obj is DataBlock)
                {
                    DataBlock d = (DataBlock)obj;
                    return (enabled == d.enabled && index == d.index);
                }
                else
                    return false;
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        };

        [Serializable]
        public struct DataShape
        {
            public DataVector gridPosition;
            public List<DataBlock> blocks;
            public List<DataVector> stateRelativePostions;

            public override bool Equals(object obj)
            {
                try
                {
                    if (!(obj is DataShape))
                        return false;

                    DataShape d = (DataShape)obj;
                    if (!gridPosition.Equals(d.gridPosition))
                        return false;

                    for (int i = 0; i < blocks.Count; i++)
                    {
                        if (!blocks[i].Equals(d.blocks[i]))
                            return false;
                    }

                    for (int i = 0; i < stateRelativePostions.Count; i++)
                    {
                        if (!stateRelativePostions[i].Equals(d.stateRelativePostions[i]))
                            return false;
                    }

                    return true;
                }
                catch
                {
                    return false;
                }
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        };

        [Serializable]
        public struct DataVector
        {
            public float X;
            public float Y;

            public override bool Equals(object obj)
            {
                if (obj is DataVector)
                {
                    DataVector d = (DataVector)obj;
                    return (X == d.X && Y == d.Y);
                }
                else
                    return false;
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }

        public static void Serialize(NetworkStream stream, object data)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, data);
        }

        public static object Deserialize(NetworkStream stream)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            return formatter.Deserialize(stream);
        }

        public static List<List<DataBlock>> ConvertToDataGrid(List<List<Block>> grid)
        {
            List<List<DataBlock>> dataGrid = new List<List<DataBlock>>();
            foreach (List<Block> lb in grid.ToList())
                dataGrid.Add(ConvertToDataBlocks(lb));

            return dataGrid;
        }

        public static DataBlock ConvertToDataBlock(Block block)
        {
            DataBlock dataBlock = new DataBlock();

            dataBlock.enabled = block.enabled;
            dataBlock.index = block.BlockIndex;

            return dataBlock;
        }

        public static List<DataBlock> ConvertToDataBlocks(List<Block> Blocks)
        {
            List<DataBlock> dataBlocks = new List<DataBlock>();
            foreach (Block b in Blocks.ToList())
            {
                dataBlocks.Add(ConvertToDataBlock(b));
            }

            return dataBlocks;
        }

        public static DataShape ConvertToDataShape(Shape shape)
        {
            DataShape dataShape = new DataShape();
            dataShape.gridPosition = ConvertToDataVector(shape.gridPosition);
            dataShape.blocks = ConvertToDataBlocks(shape.blocks);
            dataShape.stateRelativePostions = ConvertToDataVectors(shape.stateRelativePositions[shape.currentShapeIndex]);

            return dataShape;
        }

        public static DataVector ConvertToDataVector(Vector2 position)
        {
            DataVector dataPosition = new DataVector();
            dataPosition.X = position.X;
            dataPosition.Y = position.Y;

            return dataPosition;
        }

        public static List<DataVector> ConvertToDataVectors(List<Vector2> positions)
        {
            List<DataVector> dataPosition = new List<DataVector>();
            foreach (Vector2 v in positions)
            {
                dataPosition.Add(ConvertToDataVector(v));
            }

            return dataPosition;
        }

        public static List<List<Block>> ConvertToGrid(List<List<DataBlock>> dataGrid)
        {
            List<List<Block>> grid = new List<List<Block>>();
            foreach (List<DataBlock> ldb in dataGrid)
                grid.Add(ConvertToBlocks(ldb));

            return grid;
        }

        public static Block ConvertToBlock(DataBlock dataBlock)
        {
            Block block = new Block(dataBlock.enabled);
            block.SetColor(dataBlock.index);

            return block;
        }

        public static List<Block> ConvertToBlocks(List<DataBlock> dataBlocks)
        {
            List<Block> blocks = new List<Block>();
            foreach (DataBlock db in dataBlocks)
                blocks.Add(ConvertToBlock(db));

            return blocks;
        }

        public static Shape ConvertToOnlineShape(DataShape dataShape)
        {
            OnlineShape onlineShape = new OnlineShape(ConvertToVector(dataShape.gridPosition), ConvertToBlocks(dataShape.blocks), ConvertToVectors(dataShape.stateRelativePostions));
            return onlineShape;
        }

        public static Vector2 ConvertToVector(DataVector dataVector)
        {
            Vector2 position = new Vector2();
            position.X = dataVector.X;
            position.Y = dataVector.Y;

            return position;
        }

        public static List<Vector2> ConvertToVectors(List<DataVector> dataVectors)
        {
            List<Vector2> positions = new List<Vector2>();

            foreach (DataVector dp in dataVectors)
                positions.Add(ConvertToVector(dp));

            return positions;
        }

        public static string ConvertToString(List<List<Block>> grid)
        {
            string s = null;
            for (int i = 0; i < grid.Count; i++)
            {
                for (int j = 0; j < grid[i].Count; j++)
                {
                    //Format
                    //[enabled][color][texture]
                    s += "[" + grid[i][j].enabled + "]";
                    s += "[" + grid[i][j].blockColor.color + "]";
                    s += "[" + grid[i][j].blockColor.texture.Name + "]";
                    s += " ";
                }
            }
            s += '\n';
            return s;
        }

        public static string ConvertToString(Shape shape)
        {
            string s = "";
            //Format
            //[Name][Number of Blocks]
            //[Block Texture Number 1][Block Texture Number 2]...[Block Texture Number N]
            //Shape Shifting Shape only
            //[Num of Blocks in form][X1][Y1][X2][Y2]...[XN][YN] Repeat 4 times for each form

            s += String.Format("[{0}][{1}]", shape.Name, shape.blocks.Count);
            for (int i = 0; i < shape.blocks.Count; i++)
            {
                s += String.Format("[{0}]", shape.blocks[i].BlockIndex);
            }
            for (int i = 0; i <= shape.maxIndex; i++)
            {
                s += String.Format("[{0}]", shape.stateRelativePositions[i].Count);
                for(int j = 0; j < shape.stateRelativePositions[i].Count; j++)
                {
                    s += String.Format("[{0}][{1}]", shape.stateRelativePositions[i][j].X, shape.stateRelativePositions[i][j].Y);
                }
            }

            return s;
        }

        public static Shape ConvertToShape(string s)
        {
            Shape shape = null;
            string name= "";
            string[] ss = new string[0];        //array of all the data parts
            int numBlocks;                      //counting variables
            bool extraData = false;             //Grab extra data from the string for shape shifting shape

            try
            {
                //Get name
                ss = s.Split('[');
                for (int i = 0; i < ss.Length; i++)
                {
                    ss[i] = ss[i].Trim('[', ']');
                }
                
                //Set up initial shape design
                name = ss[0];
                switch (name)
                {
                    case "IShape":
                        shape = new IShape();
                        break;
                    case "JShape":
                        shape = new JShape();
                        break;
                    case "LShape":
                        shape = new LShape();
                        break;
                    case "OShape":
                        shape = new OShape();
                        break;
                    case "SShape":
                        shape = new SShape();
                        break;
                    case "TShape":
                        shape = new TShape();
                        break;
                    case "ZShape":
                        shape = new ZShape();
                        break;
                    case "Corner Shape":
                        shape = new CornerShape();
                        break;
                    case "Easy Diagonal Shape":
                        shape = new EasyDiagonalShape();
                        break;
                    case "Hockey Stick Shape":
                        shape = new HockeyStickShape();
                        break;
                    case "Oklahoma Shape":
                        shape = new OklahomaShape();
                        break;
                    case "Reverse Hockey Stick Shape":
                        shape = new ReverseHockeyStickShape();
                        break;
                    case "Bucket Shape":
                        shape = new BucketShape();
                        break;
                    case "Lobster Claw Shape":
                        shape = new LobsterClawShape();
                        break;
                    case "Plus Shape":
                        shape = new PlusShape();
                        break;
                    case "Stair Shape":
                        shape = new StairShape();
                        break;
                    case "VShape":
                        shape = new VShape();
                        break;
                    case "Hard Diagonal Shape":
                        shape = new HardDiagonalShape();
                        break;
                    case "Hook Shape":
                        shape = new HookShape();
                        break;
                    case "Kite Shape":
                        shape = new KiteShape();
                        break;
                    case "Tonfu Shape":
                        shape = new TonfuShape();
                        break;
                    case "Shape Shifting Shape":
                        shape = new ShapeShiftingShape();
                        extraData = true;
                        break;
                    default:
                        shape = new OShape();
                        break;
                }

                //Set up blocks
                numBlocks = int.Parse(ss[1]);
                for (int i = 0; i < numBlocks; i++)
                {
                    int blockIndex = int.Parse(ss[2 + i]);
                    shape.blocks[i].blockColor = BlockColors.AllBlocks[blockIndex];
                }

                //If shapeshifting shape, get state relative positions of the different forms
                if (extraData)
                {
                    shape.stateRelativePositions.Clear();
                    int index = 2 + numBlocks;
                    for (int i = 0; i < 4; i++)
                    {
                        shape.stateRelativePositions.Add(new List<Vector2>());
                        int numFormBlocks = int.Parse(ss[index]);
                        index++;
                        for (int j = 0; j < numFormBlocks; j++)
                        {
                                int x = int.Parse(ss[index]);
                                int y = int.Parse(ss[index + 1]);
                                index += 2;
                            
                                shape.stateRelativePositions[i].Add(new Vector2(x, y));
                        }
                    }
                }
            }
            catch
            {
                Console.WriteLine("String could not be converted to shape: {0}", s);
            }

            return shape;
        }

        public static byte[] ObjectToByteArray(object o)
        {
            if(o == null)
                return null;

            using(MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, o);
                return ms.ToArray();
            }
        }

        public static object ByteArrayToObject(byte[] b)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                ms.Write(b, 0, b.Length);
                ms.Seek(0, SeekOrigin.Begin);

                return (object)bf.Deserialize(ms);
            }
        }
    }
}