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
        private int damage;
        private float fPosX;
        private float fPosY;
        private float moveX;
        private float moveY;
        private float angle;
        private int count;
        private int countMax;
        private bool pierce;
        private bool moving = true;

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

        // CheckCollision method
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
            Position = new Rectangle(c.Position.X + (int)moveX*10, c.Position.Y + (int)moveY * 10,Position.Width,Position.Height);
        }

        // Draw method for moving projectile
        public void Draw(SpriteBatch sprite, int f)
        {
            // Create a Vector2 origin which equals the center of the player
            Vector2 origin = new Vector2(16, 16);

            // Draw the projectile at it's position plus half its size, and rotate it based on the rAngle passed in
            sprite.Draw(Image, new Rectangle(Position.X + Position.Width/2, Position.Y + Position.Height/2, Position.Width, Position.Height), new Rectangle(projX + f * projWidth, projY, projWidth, projHeight), Color.White, angle - (float)Math.PI / 2, origin, SpriteEffects.None, 0);
        }

        // Draw method for stationary projectile
        public void DrawStationary(SpriteBatch sprite, int f, float rAngle)
        {
            // Create a Vector2 origin which equals the center of the player
            Vector2 origin = new Vector2(16, 16);

            // Draw the player at it's position plus the origin, and rotate it based on the rAngle passed in
            sprite.Draw(Image, new Rectangle(Position.X + (int)origin.X / 4, Position.Y + (int)origin.Y / 4, Position.Width, Position.Height), new Rectangle(projX + f * projWidth, projY, projWidth, projHeight), Color.White, rAngle - (float)Math.PI / 2, origin, SpriteEffects.None, 0);
        }

        // Constructor for moving projectile with default size
        public Projectile(int dmg, int x, int y, Character c, float ang, int cMax, bool p)
        {
            damage = dmg;
            angle = ang;
            moveX = -(float)Math.Sin(ang - Math.PI / 2) * 4;
            moveY = (float)Math.Cos(ang - Math.PI / 2) * 4;
            Position = new Rectangle(c.Position.X, c.Position.Y, 40, 40);
            fPosX = c.Position.X;
            fPosY = c.Position.Y;
            countMax = cMax;
            pierce = p;
        }

        // Constructor for moving projectile with different size
        public Projectile(int dmg, int x, int y, int w, int h, Character c, float ang, int cMax, bool p)
        {
            damage = dmg;
            angle = ang;
            moveX = -(float)Math.Sin(ang - Math.PI / 2) * 4;
            moveY = (float)Math.Cos(ang - Math.PI / 2) * 4;
            Position = new Rectangle(c.Position.X, c.Position.Y, w, h);
            fPosX = c.Position.X;
            fPosY = c.Position.Y;
            countMax = cMax;
            pierce = p;
        }

        // Constructor for stationary projetile with default size
        public Projectile(int dmg, Character c, float ang, int cMax, bool p)
        {
            damage = dmg;
            angle = ang;
            moveX = -(float)Math.Sin(ang - Math.PI / 2) * 4;
            moveY = (float)Math.Cos(ang - Math.PI / 2) * 4;
            Position = new Rectangle(c.Position.X, c.Position.Y, 80, 50);
            countMax = cMax;
            pierce = p;
            moving = false;
        }
    }
}