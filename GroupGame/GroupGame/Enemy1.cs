﻿// Enemy2
// Class for the first enemy, inherits from Enemy
// Coders: Kiernan Brown, Nick Federico, James Arao

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GroupGame
{
    class Enemy1 : Enemy
    {
        public override void Move(Character c, List<Enemy> enemies, List<Rectangle> boxes)
        {
            if(Alive == true)base.Move(c, enemies, boxes);
            else
            {
                Position = new Rectangle((int)FPosX, (int)FPosY, Position.Width, Position.Height);
            }
        }

        // Constructor
        public Enemy1(int posX, int posY) : base(posX, posY)
        {
            Position = new Rectangle(posX, posY, 50, 50); // Set position
            FPosX = posX;
            FPosY = posY;
            CRect = new Rectangle(posX + 15, posY + 15, 20, 20); // Set the cRect based on the position
            Health = 100; // Set health
            Speed = 1.9f; // Set speed
            EState = EnemyState.Chase; // Set EState to chase, for testing
        }
    }
}
