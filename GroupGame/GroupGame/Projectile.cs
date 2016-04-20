// Projectile
// Class for projectiles the player shoots
// Coders: Kiernan Brown, Nick Federico, Austin Richardson

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GroupGame
{
    abstract class Projectile : GameObject
    {
        // Attributes
        private int damage; // Damage the projectile does
        private float fPosX; // Float value of the x component of the projectile's position
        private float fPosY; // Float value of the y component of the projectile's position
        private float moveX; // Float value the projectile will be moving in the x direction
        private float moveY; // Float value the projectile will be moving in the y direction
        private float angle; // Angle of the projectile
        private int count; // Count of how long the projectile has been out
        private int countMax; // Maxmimum count the projectile can be out
        private bool pierce; // Boolean of if the projectile pierces through enemies

        // Values for animation
        const int projY = 0;
        const int projHeight = 32;
        const int projWidth = 32;
        const int projX = 0;

        // Damage Property
        public int Damage
        {
            get { return damage; }
            set { damage = value; }
        }

        // Count Property
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

        // Pierce Property
        public bool Pierce
        {
            get { return pierce; }
            set { pierce = value; }
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

        // MoveX property
        public float MoveX
        {
            get { return moveX; }
            set { moveX = value; }
        }

        // MoveY property
        public float MoveY
        {
            get { return moveY; }
            set { moveY = value; }
        }

        // CheckCollision method
        // Returns true if the projectile's position intersects the enemy's cRect
        public bool CheckCollision(Enemy e)
        {
            if (Position.Intersects(e.CRect)) return true;
            else return false;
        }

        // Move method
        public virtual void Move()
        {
            fPosX += moveX;
            fPosY += moveY;
            Position = new Rectangle((int)fPosX, (int)fPosY, Position.Width, Position.Height);
        }

        // Draw method
        public virtual void Draw(SpriteBatch sprite, int f)
        {
            // Create a Vector2 origin which equals the center of the projectile
            Vector2 origin = new Vector2(16, 16);

            // Draw the projectile at its position plus half its size, and rotate it based on the rAngle passed in
            sprite.Draw(Image, new Rectangle(Position.X + Position.Width / 2, Position.Y + Position.Height / 2, Position.Width, Position.Height), new Rectangle(projX + f * projWidth, projY, projWidth, projHeight), Color.White, angle - (float)Math.PI / 2, origin, SpriteEffects.None, 0);
        }

        // Constructor
        public Projectile(int dmg, int w, int h, Character c, float ang, int cMax, bool p, Texture2D img)
        {
            // Values for damage, angle, countMax, and pierce
            damage = dmg;
            angle = ang;
            countMax = cMax;
            pierce = p;

            // Position and movement
            Position = new Rectangle(c.Position.X, c.Position.Y, w, h);
            fPosX = c.Position.X + ((c.Position.Width - Position.Width) / 2);
            fPosY = c.Position.Y + ((c.Position.Height - Position.Height) / 2);

            // moveX and moveY values are set by taking the sin or cosine of the angle and multiplying it by six
            moveX = -(float)Math.Sin(ang - Math.PI / 2) * 6;
            moveY = (float)Math.Cos(ang - Math.PI / 2) * 6;

            // Set image
            Image = img;
        }
    }
}