using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Puck_Duck
{
    enum TileType
    {
        Floor,
        Wall,
        Goal,
        Piston,
    }

    /// <summary>
    /// Author: Christian Sheherlis
    /// Assignment: Puck Duck Project
    /// Purpose: Differentiating between different types of tiles
    /// </summary>
    class Tile : Game
    {
        // fields
        private TileType currentTile;
        Rectangle tile;
        private int posX;
        private int posY;
        private const int Size = 50;

        // constructors
        public Tile(int posX, int posY)
        {
            this.posX = posX;
            this.posY = posY;

            Rectangle tile = new Rectangle(posX, posY, Size, Size);
        }

        // properties

        // get-set property for the enum stating the current tile type
        public TileType CurrentTile
        {
            get { return currentTile; }
            set { currentTile = value; }
        }

        // methods
        public void Draw(SpriteBatch sb)
        {
            ShapeBatch.ShapeBatch.Begin(GraphicsDevice);

            switch (currentTile)
            {
                case TileType.Floor:
                    ShapeBatch.ShapeBatch.Box(tile, Color.LightBlue);
                    break;

                case TileType.Wall:
                    ShapeBatch.ShapeBatch.Box(tile, Color.Black);
                    break;

                case TileType.Piston:
                    ShapeBatch.ShapeBatch.Box(tile, Color.Brown);
                    break;

                case TileType.Goal:
                    ShapeBatch.ShapeBatch.Box(tile, Color.Green);
                    break;
            }

            ShapeBatch.ShapeBatch.End();
        }

    }
}
