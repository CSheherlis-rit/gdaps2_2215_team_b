using System;
using System.Collections.Generic;
using System.Text;

namespace Puck_Duck
{
    class Tile
    {
        //type is what kind of tile it is
        // 0 = empty
        // 1 = wall
        // 2 = piston
        // 3 = goal
        private int type;

        private const int width = 32;
        private const int height = 32;

        public int Type
        {
            get { return type; }
        }

        public Tile(int type)
        {
            this.type = type;
        }
    }
}
