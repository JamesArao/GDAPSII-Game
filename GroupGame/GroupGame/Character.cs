using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GroupGame
{
    abstract class Character:GameObject
    {

        private int speed;

        // Speed property
        public int Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        // Abstract Shoot method
        abstract public void Shoot();

        // Constructor
        public Character(int x, int y)
        {
            Position = new Rectangle(x, y, Position.Width, Position.Height);
        }
    }
}
