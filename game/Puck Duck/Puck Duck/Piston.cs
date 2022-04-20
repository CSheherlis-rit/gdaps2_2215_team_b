using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Puck_Duck
{
    /// <summary>
    /// Author: John Derstine
    /// Purpose: Handles the extension of the pistons
    /// </summary>
    class Piston
    {
        private TileMap tileMap;
        private KeyboardState currentKbState = new KeyboardState();
        private List<Tile> pistons = new List<Tile>();

        public Piston(TileMap tileMap)
        {
            this.tileMap = tileMap;
        }

        public List<Tile> CheckInput()
        {
            //get current keyboard state
            currentKbState = Keyboard.GetState();

            if (currentKbState.IsKeyDown(Keys.Up))
            {
                return GetPistons(Type.UpPiston);
            }
            if (currentKbState.IsKeyDown(Keys.Down))
            {
                return GetPistons(Type.DownPiston);
            }
            if (currentKbState.IsKeyDown(Keys.Left))
            {
                return GetPistons(Type.LeftPiston);
            }
            if (currentKbState.IsKeyDown(Keys.Right))
            {
                return GetPistons(Type.RightPiston);
            }
            return null;
        }

        //Checks all tiles to check if they are the same type of piston as the one that needs to be
        //extended

        //returns a list of all of them
        public List<Tile> GetPistons(Type type)
        {
            pistons.Clear();

            for (int i = 0; i < tileMap.Level.GetLength(0); i++)
            {
                for (int j = 0; j < tileMap.Level.GetLength(1); j++)
                {
                    if (tileMap.Level[j,i].Type == type)
                    {
                        pistons.Add(tileMap.Level[j,i]);
                    }
                }
            }
            return pistons;
        }
    }
}
