using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Puck_Duck
{
    class TileMap
    {
        //fields
        private Tile[,] level;

        //properties
        public Tile[,] Level
        {
            get { return level; }
        }


        //constructor
        public TileMap(int width, int height)
        {
            level = new Tile[width, height];
        }

        //methods
        //creates multi-dimensional array of tiles
        public Tile[,] GenerateTileMap()
        {
            // variable that may later be changed into a field in order to use different filenames
            string filename = "../../../test.csv";
            
            StreamReader input = null;

            input = new StreamReader(filename);

            string line = null;

            // reading the text file
            // advancing to the next row
            for (int i = 0; i < level.GetLength(0); i++)
            {
                line = input.ReadLine();
                string[] data = line.Split(',');

                // filling the row
                for (int j = 0; j < level.GetLength(1); j++)
                {
                    switch (data[j])
                    {
                        // "w" creates a wall
                        case "w":
                            level[j, i] = new Tile(Type.Wall);
                            break;

                        // "e" creates an empty tile
                        case "e":
                            level[j, i] = new Tile(Type.Empty);
                            break;

                        // "g" creates the goal
                        case "g":
                            level[j, i] = new Tile(Type.Goal);
                            break;

                        // "u" creates an upwards facing piston
                        case "u":
                            level[j, i] = new Tile(Type.UpPiston);
                            break;

                        // "d" creates a downwards facing piston
                        case "d":
                            level[j, i] = new Tile(Type.DownPiston);
                            break;

                        // "r" creates a right facing piston
                        case "r":
                            level[j, i] = new Tile(Type.RightPiston);
                            break;

                        // "l" creates a left facing piston
                        case "l":
                            level[j, i] = new Tile(Type.LeftPiston);
                            break;

                        // "s" creates the start tile
                        case "s":
                            level[j, i] = new Tile(Type.Start);
                            break;
                    }
                }
            }

            // closing the opened text file
            if (input != null)
            {
                input.Close();
            }
            return level;
        }
    }
}
