using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GroupGame
{
    class Projectile:GameObject
    {
        private int damage;
        private int movementCount;
        private float fPosX;
        private float fPosY;
        private float moveX;
        private float moveY;
        private float angle;
        private bool pierce;

        // Damage Property
        public int Damage
        {
            get { return damage; }
            set { damage = value; }
        }

        // MovementCount Property
        public int MovementCount
        {
            get { return movementCount; }
            set { movementCount = value; }
        }

        // Pierce Property
        public bool Pierce
        {
            get { return pierce; }
            set { pierce = value; }
        }

        // CheckCollision method
        public bool CheckCollision(GameObject obj)
        {
            if (Position.Intersects(obj.Position)) return true;
            else return false;
        }

        // Move method
        public void Move()
        {
            fPosX += moveX;
            fPosY += moveY;
            Position = new Rectangle((int)fPosX, (int)fPosY, Position.Width, Position.Height);
        }

        // Override draw method
        public void Draw(SpriteBatch sprite)
        {
            // Create a Vector2 origin which equals the center of the player
            Vector2 origin = new Vector2(Image.Width / 2, Image.Height / 2);

            // Draw the player at it's position plus the origin, and rotate it based on the rAngle passed in
            sprite.Draw(Image, new Rectangle(Position.X + (int)origin.X / 4, Position.Y + (int)origin.Y / 4, Position.Width, Position.Height), null, Color.White, angle, origin, SpriteEffects.None, 0);
        }

        // Constructor
        public Projectile(int dmg, int x, int y, Character c, float ang, bool p)
        {
            damage = dmg;
            angle = ang;
            moveX = -(float)Math.Sin(ang - Math.PI / 2) * 4;
            moveY = (float)Math.Cos(ang - Math.PI / 2) * 4;
            Position = new Rectangle(c.Position.X, c.Position.Y, 40, 40);
            fPosX = c.Position.X;
            fPosY = c.Position.Y;
            pierce = p;
        }
    }
}
