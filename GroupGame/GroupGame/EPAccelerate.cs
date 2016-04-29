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
        private float accelerate;

        // Override move method so that the MoveX and MoveY increase over time to make the projectile faster
        public override void Move()
        {
                base.Move();
                MoveX += MoveX/(150 - accelerate);
                MoveY += MoveY/(150 - accelerate);
        }

        // Constructor
        public EPAccelerate(int dmg, int w, int h, Enemy e, float ang, float speed, float accel, Texture2D img):base(dmg,w,h,e,ang,speed, img)
        {
            accelerate = accel;
        }

        // Constructor X and Y
        public EPAccelerate(int dmg, int w, int h, int x, int y, float ang, float speed, float accel, Texture2D img) : base(dmg, w, h, null, ang, speed, img)
        {
            accelerate = accel;
            Position = new Rectangle(x, y, w, h);
            FPosX = x;
            FPosY = y;
        }
    }
}
