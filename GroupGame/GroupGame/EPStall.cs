﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GroupGame
{
    class EPStall : EnemyProjectile
    {
        // Attributes
        private int count;
        private int speed;
        private bool moving = false;

        // Count property
        public int Count
        {
            get { return count; }
        }

        // Speed property
        public int Speed
        {
            get { return speed; }
        }

        // Moving property
        public bool Moving
        {
            get { return moving; }
            set { moving = value; }
        }

        // Override move method so it only moves when count is 0
        public override void Move()
        {
            if (count == 0)
            {
                base.Move();
            }
            else
            {
                Position = new Rectangle((int)FPosX, (int)FPosY, Position.Width, Position.Height);
                count--;
            }
        }

        // Constructor
        public EPStall(int dmg, float x, float y, int w, int h, int spd, int c, Texture2D img):base(dmg,w,h,null,0f,spd, img)
        {
            Position = new Rectangle((int)x, (int)y, w, h);
            FPosX = x;
            FPosY = y;
            speed = spd;
            count = c;
        }
    }
}
