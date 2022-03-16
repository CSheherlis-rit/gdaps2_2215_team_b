using System;
using System.Collections.Generic;
using System.Text;

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
        //NOTE THIS WILL BE REPLACED WITH A DATA DRIVEN VERSION
        //THAT TAKES TILE TYPES LIKE WALLS AND PISTIONS AND LOADS THEM
        //FROM FILE
        public Tile[,] GenerateTileMap()
        {
            for (int i = 0; i < level.GetLength(0); i++)
            {
                for (int j = 0; j < level.GetLength(1); j++)
                {
                    //if border tile, make it a wall
                    if (i == 0 || j == 0 || j == level.GetLength(1) - 1 || i == level.GetLength(0) - 1)
                    {
                        level[i, j] = new Tile(Type.Wall);
                    }
                    //if not border tile, make it empty
                    else
                    {
                        level[i, j] = new Tile(0);
                    }
                }
            }
            return level;
        }
    }
}
