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
        private int accelerate;

        // Override move method so it only moves when count is 0
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
