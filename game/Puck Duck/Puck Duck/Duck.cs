using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        //start with the duck not moving
        private Direction movement;

        private int speed = 3;
        private Texture2D duck;
        private Rectangle position;
        private bool spawned = false;

        //constructor
        ///set size of duck rectangle??
        ///set speed to width of tiles
        public Duck(Texture2D duck, Rectangle position, Direction movement)
        {
            this.position = position;
            this.movement = movement;
            this.duck = duck;
        }

        //properties
        public Direction Movement
        {
            get { return movement; }
            set { movement = value; }
        }

        public Rectangle Position
        {
            get { return position; }
            set { position = value; }
        }

        //methods

        /// <summary>
        /// Spawns duck at starting postition
        /// </summary>
        /// <param name="startPos"></param>
        public void Spawn(Rectangle startPos)
        {
            while (!spawned)
            {
                position = startPos;
                spawned = true;
            }
        }

        /// <summary>
        /// Draws the duck
        /// </summary>
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(duck, position, Color.White);
        }

        /// <summary>
        /// Move puck in the opposite direction
        /// </summary>
        public Direction Bounce()
        {
            switch (movement)
            {
                case Direction.Up:
                    return Direction.Down;

                case Direction.Down:
                    return Direction.Up;

                case Direction.Right:
                    return Direction.Left;

                case Direction.Left:
                    return Direction.Right;

                default:
                    return Direction.Stop;
            }
        }

        /// <summary>
        /// Check for tile collisions by looping through the tile map.
        /// If one is detected, return true
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        public Direction CheckCollision(TileMap map)
        {
            //run through all the tilemap tiles
            for (int i = 0; i < map.Level.GetLength(0); i++)
            {
                for (int j = 0; j < map.Level.GetLength(1); j++)
                {
                    //check for an intersection with a tile
                    if (position.Intersects(map.Level[j, i].Position))
                    {

                        //bounce the puck if it hits a wall or a closed piston
                        if (map.Level[j, i].Type == Type.Wall ||
                            map.Level[j, i].Type == Type.UpPiston ||
                            map.Level[j, i].Type == Type.DownPiston ||
                            map.Level[j, i].Type == Type.LeftPiston ||
                            map.Level[j, i].Type == Type.RightPiston)
                        {
                            return Bounce();
                        }

                        //stops the puck in the goal
                        else if (map.Level[j, i].Type == Type.Goal)
                        {
                            return Direction.Stop;
                        }
                    }
                }
            }
            //if no tile collisions are detected, return
            return Movement;
        }

        /// <summary>
        /// return the direction of a interacted piston head
        /// </summary>
        /// <param name="pistonHeads"></param>
        /// <returns></returns>
        public Direction PistonPush(List<Tile> pistonHeads)
        {
            //temp variables for later comparison
            Type headType;

            foreach(Tile head in pistonHeads)
            {
                if (position.Intersects(head.Position))
                {
                    //variable for switch statement
                    headType = head.Type;

                    //set the direction to the direction of the piston head
                    switch (headType)
                    {
                        case Type.UpPiston:
                            position.X = head.Position.X;
                            return Direction.Up;

                        case Type.DownPiston:
                            position.X = head.Position.X;
                            return Direction.Down;

                        case Type.LeftPiston:
                            position.Y = head.Position.Y;
                            return Direction.Left;

                        case Type.RightPiston:
                            position.Y = head.Position.Y;
                            return Direction.Right;
                    }
                }
            }

            return Movement;
        }

        /// <summary>
        /// updates the position of the duck
        /// by incrementing it every second if no collisions are detected
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="map"></param>
        public void Update(GameTime gameTime, TileMap map, List<Tile> pistonHeads)
        {
            switch (movement)
            {
                case Direction.Up:
                    position.Y = position.Y - speed;
                    Movement = CheckCollision(map);
                    if (pistonHeads != null)
                    {
                        Movement = PistonPush(pistonHeads);
                    }
                    break;

                case Direction.Down:
                    position.Y = position.Y + speed;
                    Movement = CheckCollision(map);
                    if (pistonHeads != null)
                    {
                        Movement = PistonPush(pistonHeads);
                    }
                    break;

                case Direction.Right:
                    position.X = position.X + speed;
                    Movement = CheckCollision(map);
                    if (pistonHeads != null)
                    {
                        Movement = PistonPush(pistonHeads);
                    }
                    break;

                case Direction.Left:
                    position.X = position.X - speed;
                    Movement = CheckCollision(map);
                    if (pistonHeads != null)
                    {
                        Movement = PistonPush(pistonHeads);
                    }
                    break;

                case Direction.Stop:
                    break;
            }
        }
    }
}
