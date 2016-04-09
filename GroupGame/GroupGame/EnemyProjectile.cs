// EnemyProjectile
// Class for projectiles the enemies shoot
// Coders: Kiernan Brown

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroupGame
{
    class EnemyProjectile : GameObject
    {
        private int damage; // Damage the projectile does
        private float fPosX; // Float value of the x component of the projectile's position
        private float fPosY; // Float value of the y component of the projectile's position
        private float moveX; // Float value the projectile will be moving in the x direction
        private float moveY; // Float value the projectile will be moving in the y direction
        private float angle; // Angle of the projectile
        private int count; // Count of how long the projectile has been out
        private int countMax; // Maxmimum count the projectile can be out

        // Values for animation
        const int eProjY = 0;
        const int eProjHeight = 32;
        const int eProjWidth = 32;
        const int eProjX = 0;

        // Damage Property
        public int Damage
        {
            get { return damage; }
            set { damage = value; }
        }

        // MovementCount Property
        public int Count
        {
            get { return count; }
            set { count = value; }
        }

        // CountMax Property
        public int CountMax
        {
            get { return countMax; }
            set { countMax = value; }
        }

        // FPosX property
        public float FPosX
        {
            get { return fPosX; }
            set { fPosX = value; }
        }

        // FPosY property
        public float FPosY
        {
            get { return fPosY; }
            set { fPosY = value; }
        }

        // CheckCollision method
        // Returns true if the projectile's position intersects the enemy's cRect
        public bool CheckCollision(Character c)
        {
            if (Position.Intersects(c.CRect)) return true;
            else return false;
        }

        // Move method
        public void Move()
        {
            fPosX += moveX;
            fPosY += moveY;
            Position = new Rectangle((int)fPosX, (int)fPosY, Position.Width, Position.Height);
        }

        // Method to move the stationary projectile in front of the player
        public void MoveStationary(Character c, float ang)
        {
            moveX = -(float)Math.Sin(ang - Math.PI / 2) * 4;
            moveY = (float)Math.Cos(ang - Math.PI / 2) * 4;
            Position = new Rectangle(c.Position.X + (int)moveX * 10, c.Position.Y + (int)moveY * 10, Position.Width, Position.Height);
        }

        // Draw method
        public void Draw(SpriteBatch sprite, int f)
        {
            // Create a Vector2 origin which equals the center of the projectile
            Vector2 origin = new Vector2(16, 16);

            // Draw the projectile at its position plus half its size, and rotate it based on the rAngle passed in
            //sprite.Draw(Image, new Rectangle(Position.X + Position.Width / 2, Position.Y + Position.Height / 2, Position.Width, Position.Height), new Rectangle(eProjX + f * eProjWidth, eProjY, eProjWidth, eProjHeight), Color.White, angle - (float)Math.PI / 2, origin, SpriteEffects.None, 0);
            sprite.Draw(Image, new Rectangle(Position.X + Position.Width / 2, Position.Y + Position.Height / 2, Position.Width, Position.Height), null, Color.White, angle - (float)Math.PI / 2, origin, SpriteEffects.None, 0);
        }

        // Constructor for moving projectile with different size
        public EnemyProjectile(int dmg, int w, int h, Enemy e, float ang, int speed)
        {
            // Values for damage, angle, countMax, and pierce
            damage = dmg;
            angle = ang;
            //countMax = cMax;

            // Position and movement
            Position = new Rectangle(e.Position.X, e.Position.Y, w, h);
            fPosX = e.Position.X + ((e.Position.Width - Position.Width) / 2);
            fPosY = e.Position.Y + ((e.Position.Height - Position.Height) / 2);

            // moveX and moveY values are set by taking the sin or cosine of the angle and multiplying it by speed
            moveX = -(float)Math.Sin(ang - Math.PI / 2) * speed;
            moveY = (float)Math.Cos(ang - Math.PI / 2) * speed;
        }

    }
}
