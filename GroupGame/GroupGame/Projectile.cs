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
    class Projectile : GameObject
    {
        private int damage; // Damage the projectile does
        private float fPosX; // Float value of the x component of the projectile's position
        private float fPosY; // Float value of the y component of the projectile's position
        private float moveX; // Float value the projectile will be moving in the x direction
        private float moveY; // Float value the projectile will be moving in the y direction
        private float angle; // Angle of the projectile
        private int count; // Count of how long the projectile has been out
        private int countMax; // Maxmimum count the projectile can be out
        private bool pierce; // Boolean of if the projectile pierces through enemies
        private bool moving = true; // Boolean of if the projectile moves, default is true

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

        // MovementCount Property
        public int Count
        {
            get { return count; }
            set { count = value; }
        }

        // MovementCount Property
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

        // Moving Property
        public bool Moving
        {
            get { return moving; }
            set { moving = value; }
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
        public bool CheckCollision(Enemy e)
        {
            if (Position.Intersects(e.CRect)) return true;
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

        // Draw method for moving projectile
        public void Draw(SpriteBatch sprite, int f)
        {
            // Create a Vector2 origin which equals the center of the projectile
            Vector2 origin = new Vector2(16, 16);

            // Draw the projectile at its position plus half its size, and rotate it based on the rAngle passed in
            sprite.Draw(Image, new Rectangle(Position.X + Position.Width/2, Position.Y + Position.Height/2, Position.Width, Position.Height), new Rectangle(projX + f * projWidth, projY, projWidth, projHeight), Color.White, angle - (float)Math.PI / 2, origin, SpriteEffects.None, 0);
        }

        // Draw method for stationary projectile
        public void DrawStationary(SpriteBatch sprite, int f, float rAngle)
        {
            // Create a Vector2 origin which equals the center of the projectile
            Vector2 origin = new Vector2(16, 16);

            // Draw the projectile at its position , and rotate it based on the rAngle passed in
            sprite.Draw(Image, new Rectangle(Position.X + Position.Width/2, Position.Y + Position.Height/2, Position.Width, Position.Height), new Rectangle(projX + f * projWidth, projY, projWidth, projHeight), Color.White, rAngle - (float)Math.PI / 2, origin, SpriteEffects.None, 0);
        }

        // Constructor for moving projectile with default size
        public Projectile(int dmg, int x, int y, Character c, float ang, int cMax, bool p)
        {
            // Values for damage, angle, countMax, and pierce
            damage = dmg;
            angle = ang;
            countMax = cMax;
            pierce = p;

            // Position and movement
            Position = new Rectangle(c.Position.X + 5, c.Position.Y + 5, 40, 40);
            fPosX = c.Position.X + 5;
            fPosY = c.Position.Y + 5;

            // moveX and moveY values are set by taking the sin or cosine of the angle and multiplying it by four
            moveX = -(float)Math.Sin(ang - Math.PI / 2) * 4;
            moveY = (float)Math.Cos(ang - Math.PI / 2) * 4;
        }

        // Constructor for moving projectile with different size
        public Projectile(int dmg, int x, int y, int w, int h, Character c, float ang, int cMax, bool p)
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

            // moveX and moveY values are set by taking the sin or cosine of the angle and multiplying it by four
            moveX = -(float)Math.Sin(ang - Math.PI / 2) * 4;
            moveY = (float)Math.Cos(ang - Math.PI / 2) * 4;
        }

        // Constructor for stationary projetile with default size
        public Projectile(int dmg, Character c, float ang, int cMax, bool p)
        {
            // Values for damage, angle, countMax, and pierce
            damage = dmg;
            angle = ang;
            countMax = cMax;
            pierce = p;

            // Position and size are set
            Position = new Rectangle(c.Position.X, c.Position.Y, 50, 50);

            // moveX and moveY values are set by taking the sin or cosine of the angle and multiplying it by four
            // In this case, moveX and moveY are used to move the projectile in front of the character instead of across the screen
            moveX = -(float)Math.Sin(ang - Math.PI / 2) * 4;
            moveY = (float)Math.Cos(ang - Math.PI / 2) * 4;

            // moving is false because the projectile is stationary
            moving = false;
        }
    }
}