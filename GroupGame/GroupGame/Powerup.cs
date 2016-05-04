// Powerup
// Class for powerups that will apear during the game
// Coders: Kiernan Brown, 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GroupGame
{
    class Powerup : GameObject
    {
        private string type;
        private bool active;

        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        public void Move()
        {
            Position = new Rectangle(Position.X, Position.Y, Position.Width, Position.Height);
        }

        public bool Pickup(Character c)
        {
            if(Position.Intersects(c.CRect) && active == true)
            {
                active = false;
                return true;
            }
            return false;
        }

        public Powerup(string typ, Texture2D img)
        {
            type = typ;
            Image = img;
            active = true;
        }
    }
}
