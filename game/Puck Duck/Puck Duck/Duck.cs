using Microsoft.Xna.Framework;
using System.Threading;
using System;
using System.Collections.Generic;
using System.Text;

namespace Puck_Duck
{
    /// <summary>
    /// Author: Amanda Rowe
    /// Purpose: Dictates the movement of the puck duck!
    /// </summary>

    //enum to track which direction the duck is moving in
    enum Direction
    {
        Up,
        Down,
        Right,
        Left,
        Stop
    }

    class Duck
    {
        //fields
        private Direction movement;
        private Rectangle duck;
        private TileMap map;

        
        //properties
        public Direction Movement
        {
            get { return movement; }
            set { movement = value; }
        }

        //constructor
        ///set size of duck rectangle??
        ///set speed to width of tiles
        public Duck(Rectangle rectangle, Direction movement, TileMap tileMap)
        {
            this.duck = rectangle;
            this.movement = movement;
            this.map = tileMap;
        }

        //methods

        //return the direction of a interacted piston
        /*public Direction PistonPush()
        {

        }*/

        //return movement in the opposite direction
        public void Bounce()
        {
            switch (movement)
            {
                case Direction.Up:
                    movement = Direction.Down;
                    break;

                case Direction.Down:
                    movement = Direction.Up;
                    break;

                case Direction.Right:
                    movement = Direction.Left;
                    break;

                case Direction.Left:
                    movement = Direction.Right;
                    break;

                default:
                    movement = Direction.Stop;
                    break;
            }
        }
        
        ///check for tile collisions by looping through the tile map
        ///if one is detected, return new puck direction
        public bool CheckCollision()
        {
            //run through all the tilemap tiles
            for (int i = 0; i < map.Level.GetLength(0); i++)
            {
                for (int j = 0; j < map.Level.GetLength(1); j++)
                {
                    //check for an intersection with a tile
                    if(duck.Intersects(map.Level[j, i].Position))
                    {

                        //bounce the puck if it hits a wall
                        if(map.Level[j,i].Type == Type.Wall)
                        {
                            Bounce();
                            return true;
                        }

                        //stops the puck in the goal
                        else if(map.Level[j, i].Type == Type.Goal)
                        {
                            movement = Direction.Stop;
                            return true;
                        }

                        //move in the direction of a piston-type
                        /*else if(map.Level[j, i].Type == Type.Piston)
                        {
                            //CHANGE WHEN PISTONS ARE IMPLEMENTED*******
                            //PistonPush();
                            return true;
                        }*/
                    }
                }
            }
            //if no tile collisions are detected, return false
            return false;
        }
        
        /// <summary>
        /// updates the position of the duck
        /// by incrementing it every second if no collisions are detected
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime, TileMap map)
        {
            switch (movement)
            {
                case Direction.Up:
                    if(CheckCollision() == false)
                    {
                        Thread.Sleep(1000);
                        duck.Y = duck.Y - 32;
                    }
                    break;

                case Direction.Down:
                    if (CheckCollision() == false)
                    {
                        Thread.Sleep(1000);
                        duck.Y = duck.Y + 32;
                    }
                    break;

                case Direction.Right:
                    if (CheckCollision() == false)
                    {
                        Thread.Sleep(1000);
                        duck.X = duck.X + 32;
                    }
                    break;

                case Direction.Left:
                    if (CheckCollision() == false)
                    {
                        Thread.Sleep(1000);
                        duck.X = duck.X - 32;
                    }
                    break;

                case Direction.Stop:
                    CheckCollision();
                    break;
            }
        }
    }
}
