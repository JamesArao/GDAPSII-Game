using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GroupGame
{
    class Boss : Enemy
    {
        private Rectangle range;
        private int shotCount;

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

        // Constructor
        public Boss(int posX, int posY) : base(posX, posY)
        {
            Position = new Rectangle(posX, posY, 120, 120); // Set position
            FPosX = posX;
            FPosY = posY;
            CRect = new Rectangle(posX + 20, posY + 20, 80, 80); // Set the cRect based on the position
            Health = 6000; // Set health
            Speed = 0.3f; // Set speed
            EState = EnemyState.Chase; // Set EState to chase, for testing
        }
    }
}