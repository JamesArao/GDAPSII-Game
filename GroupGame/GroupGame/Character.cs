// Character
// Abstract class for characters
// Coders: Kiernan Brown, Nick Federico

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GroupGame
{
    abstract class Character : GameObject
    {
        private int speed;
        private int shotDelay;
        private int health;

        const int heroY = 0;
        const int heroHeight = 32;
        const int heroWidth = 32;
        const int heroX = 0;

        // Speed property
        public int Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        // Speed property
        public int ShotDelay
        {
            get { return shotDelay; }
            set { shotDelay = value; }
        }

        // Health property
        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        // Draw method, with the player rotating based on where the mouse is
        public void Draw(SpriteBatch sprite, float rAngle, int f)
        {
            // Create a Vector2 origin which equals the center of one frame of the player image
            Vector2 origin = new Vector2(16, 16);

            // Draw the player at it's position plus half its size, and rotate it based on the rAngle passed in
            sprite.Draw(Image, new Rectangle(Position.X + Position.Width/2, Position.Y + Position.Height/2, Position.Width, Position.Height), new Rectangle(heroX + f * heroWidth, heroY, heroWidth, heroHeight), Color.White, rAngle - (float)Math.PI / 2, origin, SpriteEffects.None, 0);
        }

        // Constructor
        public Character(int x, int y)
        {
            Position = new Rectangle(x, y, Position.Width, Position.Height);
            Health = 100;
        }
    }
}
