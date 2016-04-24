// EPAccelerate
// Class for an enemy projectile that accelerates, inherits from EnemyProjectile
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
    class EPAccelerate : EnemyProjectile
    {
        // Value that the projectiles will accelerate by, the larger the value, the faster they move
        private int accelerate;

        // Override move method so that the MoveX and MoveY increase over time to make the projectile faster
        public override void Move()
        {
                base.Move();
                MoveX += MoveX/(100 - accelerate);
                MoveY += MoveY/(100 - accelerate);
        }

        // Constructor
        public EPAccelerate(int dmg, int w, int h, Enemy e, float ang, int speed, int accel, Texture2D img):base(dmg,w,h,e,ang,speed, img)
        {
            accelerate = accel;
        }
    }
}
