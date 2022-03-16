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
                    // reading a "w" creates a wall
                    if (data[j] == "w")
                    {
                        level[j, i] = new Tile(Type.Wall);
                    }
                    // reading an "e" creates an empty tile
                    else if (data[j] == "e")
                    {
                        level[j, i] = new Tile(Type.Empty);
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
