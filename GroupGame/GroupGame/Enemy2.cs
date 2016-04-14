using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GroupGame
{
    class Enemy2 : Enemy
    {

        private Rectangle range;
        private int shotCount;
        bool shooting;

        public Rectangle Range
        {
            get { return range; }
            set { range = value; }
        }

        public int ShotCount
        {
            get { return shotCount; }
            set { shotCount = value; }
        }

        public bool Shooting
        {
            get { return shooting; }
            set { shooting = value; }
        }

        public override void Move(Character c, List<Enemy> enemies, List<Rectangle> boxes)
        {
            base.Move(c, enemies, boxes);
            range = new Rectangle(Position.X - 250, Position.Y - 250, range.Width, range.Height);
        }

        public void Shoot(Character c)
        {
            if (c.Position.Intersects(range))
            {
                shotCount++;
                Speed = 0.15f;
                shooting = true;
            }
            else
            {
                shotCount = 0;
                Speed = 0.6f;
                shooting = false;
            }
        }

        // Constructor
        public Enemy2(int posX, int posY) : base(posX, posY)
        {
            Position = new Rectangle(posX, posY, 50, 50); // Set position
            FPosX = posX;
            FPosY = posY;
            CRect = new Rectangle(posX + 15, posY + 15, 20, 20); // Set the cRect based on the position
            Health = 80; // Set health
            Speed = 0.6f; // Set speed
            EState = EnemyState.Chase; // Set EState to chase, for testing
            range = new Rectangle(posX - 250, posY - 250, 500, 500);
        }
    }
}
