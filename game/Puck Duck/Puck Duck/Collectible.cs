using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Puck_Duck
{

    /// <summary>
    /// Author: Christian Sheherlis
    /// Purpose: Manages the collectibles in each level
    /// </summary>
    class Collectible
    {
        // fields
        private Texture2D pickup;
        private Rectangle position;
        private bool isActive;
        
        /// <summary>
        /// get-set property for the collectibles in the list being active
        /// </summary>
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        /// <summary>
        /// parameterized constructor
        /// </summary>
        /// <param name="pickup"></param>
        /// <param name="position"></param>
        public Collectible(Texture2D pickup, Rectangle position)
        {
            this.pickup = pickup;
            this.position = position;

            isActive = true;
        }

        /// <summary>
        /// checks to see if the duck is running into a collectible
        /// </summary>
        /// <param name="check"></param>
        /// <returns>
        /// bool based on if the collectible is active and if the duck is sliding into it
        /// </returns>
        public bool CheckCollision(Duck check)
        {
            if (isActive)
            {
                if (check.Position.Intersects(position))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// handles the drawing of the collectibles
        /// </summary>
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb)
        {
            if (isActive)
            {
                sb.Draw(pickup, position, Color.White);
            }
        }
    }
}
