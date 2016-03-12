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

        // Draw method, with the player rotating based on where the mouse is
        public void Draw(SpriteBatch sprite, float rAngle, int f)
        {
            // Create a Vector2 origin which equals the center of the player
            Vector2 origin = new Vector2(16, 16);

            // Draw the player at it's position plus the origin, and rotate it based on the rAngle passed in
            sprite.Draw(Image, new Rectangle(Position.X + (int)origin.X / 4, Position.Y + (int)origin.Y / 4, Position.Width, Position.Height), new Rectangle(heroX + f * heroWidth, heroY, heroWidth, heroHeight), Color.White, rAngle - (float)Math.PI / 2, origin, SpriteEffects.None, 0);
        }

        // Constructor
        public Character(int x, int y)
        {
            Position = new Rectangle(x, y, Position.Width, Position.Height);
        }
    }
}
