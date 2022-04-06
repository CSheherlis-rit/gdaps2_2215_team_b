using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Puck_Duck
{
    // enumeration to say what type of tile the current one is
    enum Type
    {
        Empty,
        Wall,
        UpPiston,
        DownPiston,
        RightPiston,
        LeftPiston,
        Goal,
        Start,
        EvilStart,
        Fail
    }

    class Tile
    {
        private Type type;
        private Rectangle position;

        private const int width = 32;
        private const int height = 32;

        public Type Type
        {
            get { return type; }
        }

        public Rectangle Position
        {
            get { return position; }
            set { position = value; }
        }

        public Tile(Type type)
        {
            this.type = type;
        }
    }
}