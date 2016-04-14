using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace GroupGame
{
    class EnemyProjectile1 : EnemyProjectile
    {
        public EnemyProjectile1(int dmg, int w, int h, Enemy e, float ang, int speed):base(dmg,w,h,e,ang,speed)
        {
        }

    }
}
