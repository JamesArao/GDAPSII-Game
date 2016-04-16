// Enemy3
// Class for the third enemy, inherits from Enemy
// Coders: Kiernan Brown

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GroupGame
{
    class Enemy3 : Enemy
    {
        // Constructor
        public Enemy3(int posX, int posY) : base(posX, posY)
        {
            Position = new Rectangle(posX, posY, 50, 50); // Set position
            FPosX = posX;
            FPosY = posY;
            CRect = new Rectangle(posX + 15, posY + 15, 20, 20); // Set the cRect based on the position
            Health = 120; // Set health
            Speed = 0.5f; // Set speed
            EState = EnemyState.Chase; // Set EState to chase, for testing
        }
    }
}
