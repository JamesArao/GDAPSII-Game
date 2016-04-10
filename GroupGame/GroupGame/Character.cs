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
        private int speed; // Speed the character moves at
        private int shotDelay; // Int that acts as a way of delaying player shooting, decreases by one every frame
        private int health; // Health of the player
        private Rectangle cRect; // Rectangle for character collision
        private int dashing;
        private int dashCount;

        // Values for animation
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

        // CRect property
        public Rectangle CRect
        {
            get { return cRect; }
            set { cRect = value; }
        }

        // Dashing property
        public int Dashing
        {
            get { return dashing; }
            set { dashing = value; }
        }

        // DashCount property
        public int DashCount
        {
            get { return dashCount; }
            set { dashCount = value; }
        }

        // Draw method, with the player rotating based on where the mouse is
        public void Draw(SpriteBatch sprite, float rAngle, int f, Color color)
        {
            // Create a Vector2 origin which equals the center of one frame of the player image
            Vector2 origin = new Vector2(16, 16);

            // Draw the player at it's position plus half its size, and rotate it based on the rAngle passed in
            sprite.Draw(Image, new Rectangle(Position.X + Position.Width / 2, Position.Y + Position.Height / 2, Position.Width, Position.Height), new Rectangle(heroX + f * heroWidth, heroY, heroWidth, heroHeight), color, rAngle - (float)Math.PI / 2, origin, SpriteEffects.None, 0);
        }

        // Draw method, with the player rotating based on where the mouse is, with scaling
        public void Draw(SpriteBatch sprite, float rAngle, int f, float scale)
        {
            // Create a Vector2 origin which equals the center of one frame of the player image
            Vector2 origin = new Vector2(16, 16);

            // Draw the player at it's position plus half its size, and rotate it based on the rAngle passed in
            sprite.Draw(Image, new Vector2(Position.X + Position.Width / 2, Position.Y + Position.Height / 2), new Rectangle(heroX + f * heroWidth, heroY, heroWidth, heroHeight), Color.White, rAngle - (float)Math.PI / 2, origin, scale, SpriteEffects.None, 0);
            //sprite.Draw(Image, new Rectangle(Position.X + Position.Width/2, Position.Y + Position.Height/2, Position.Width, Position.Height), new Rectangle(heroX + f * heroWidth, heroY, heroWidth, heroHeight), Color.White, rAngle - (float)Math.PI / 2, origin, scale, SpriteEffects.None, 0);
        }

        // Constructor
        public Character(int x, int y)
        {
            Position = new Rectangle(x, y, Position.Width, Position.Height);
            cRect = new Rectangle(Position.X + 10, Position.Y + 10, Position.Width - 20, Position.Height - 20);
            Health = 100;
        }
    }
}
