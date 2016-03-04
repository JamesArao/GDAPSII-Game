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

        // Draw method, with the player rotating based on where the mouse is
        public void Draw(SpriteBatch sprite, float rAngle)
        {
            // Create a Vector2 origin which equals the center of the player
            Vector2 origin = new Vector2(Image.Width/2, Image.Height/2);

            // Draw the player at it's position plus the origin, and rotate it based on the rAngle passed in
            sprite.Draw(Image, new Rectangle(Position.X + (int)origin.X/4, Position.Y+(int)origin.Y/4, Position.Width, Position.Height), null, Color.White, rAngle, origin, SpriteEffects.None, 0);
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
