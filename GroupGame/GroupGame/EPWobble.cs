// EPWobble
// Class for an enemy projectile that moves forward and then back, inherits from EnemyProjectile
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
    class EPWobble : EnemyProjectile
    {
        private int wobbleCount = 0;

        // Override move method so the projectile moves both forward and back
        public override void Move()
        {
            // Move forward for 20 frames
            if(wobbleCount < 20)
            {
                base.Move();
            }
            // Then move back for 15 frames
            else
            {
                FPosX -= MoveX/1.75f;
                FPosY -= MoveY/1.75f;
                Position = new Rectangle((int)FPosX, (int)FPosY, Position.Width, Position.Height);
            }
            // Reset this pattern
            if(wobbleCount == 35)
            {
                wobbleCount = 0;
            }
            wobbleCount++;
        }

        // Constructor
        public EPWobble(int dmg, int w, int h, Enemy e, float ang, int speed, Texture2D img):base(dmg,w,h,e,ang,speed, img)
        {
        }
    }
}
