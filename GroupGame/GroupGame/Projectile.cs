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
        private Vector2 direction;

        // Damage Property
        public int Damage
        {
            get { return damage; }
            set { damage = value; }
        }

        // CheckCollision method
        public bool CheckCollision(GameObject obj)
        {
            if (Position.Intersects(obj.Position)) return true;
            else return false;
        }

        // Move method
        public void Move(Vector2 dir)
        {
            Position = new Rectangle(Position.X, Position.Y, Position.Height, Position.Width);
        }

        // Constructor
        public Projectile(int dmg)
        {
            damage = dmg;
        }
    }
}
