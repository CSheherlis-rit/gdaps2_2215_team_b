using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Puck_Duck
{
    /// <summary>
    /// Author: Christian Sheherlis
    /// Purpose: Tracks what type of tile any given tile is
    /// </summary>
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
        Collectible,
        Fail
    }

    /// <summary>
    /// Author: John Derstine
    /// Purpose: Handles individual tiles
    /// </summary>
    class Tile
    {
        private Type type;
        private Rectangle position;

        private const int width = 32;
        private const int height = 32;

        //for randomized wall colors
        private int wallColor;
        private Random rng = new Random();

        public Type Type
        {
            get { return type; }
        }

        public Rectangle Position
        {
            get { return position; }
            set { position = value; }
        }

        public int WallColor
        {
            get { return wallColor; }
        }

        public Tile(Type type)
        {
            this.type = type;

            //only used for wall tile colors
            this.wallColor = rng.Next(6);
        }
    }
}