// Enemy
// Abstract class for enemies
// Coders: Kiernan Brown, James Arao

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

enum EnemyState { Wander, Chase, Shoot };

namespace GroupGame
{
    abstract class Enemy:GameObject
    {
        private int health; // Health of the enemy
        private EnemyState eState; // State the enemy is in
        private bool alive = true; // Is the enemy alive
        private Rectangle cRect; // Rectangle for enemy collision

        const int enemyY = 0;
        const int enemyHeight = 32;
        const int enemyWidth = 32;
        const int enemyX = 0;

        // Health property
        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        // EState property
        public EnemyState EState
        {
            get { return eState; }
            set { eState = value; }
        }

        // Alive property
        public bool Alive
        {
            get { return alive; }
            set { alive = value; }
        }

        // CRect Property
        public Rectangle CRect
        {
            get { return cRect; }
            set { cRect = value; }
        }

        // Method to move the enemy
        public abstract void Move(Character c, List<Enemy> enemies);

        /*
        public void RotateTest(float rAngle)
        {
            Vector2 points = new Vector2(CRect.X, CRect.Y);
            points = Vector2.Transform(points, Matrix.CreateRotationZ(rAngle));
            CRect = new Rectangle(new Point((int)points.X, (int)points.Y), new Point(CRect.Width, CRect.Height));
        }*/

        public void Draw(SpriteBatch sprite, float rAngle, int f)
        {
            // Create a Vector2 origin which equals the center of one frame of the player image
            Vector2 origin = new Vector2(16, 16);

            // Draw the player at it's position plus half its size, and rotate it based on the rAngle passed in
            sprite.Draw(Image, new Rectangle(Position.X + Position.Width / 2, Position.Y + Position.Height / 2, Position.Width, Position.Height), new Rectangle(enemyX + f * enemyWidth, enemyY, enemyWidth, enemyHeight), Color.White, rAngle - (float)Math.PI / 2, origin, SpriteEffects.None, 0);
        }

        // Parameterized constructor
        public Enemy(int posX, int posY)
        {
            Position = new Rectangle(posX, posY, Position.Width, Position.Height);
        }
    }
}
