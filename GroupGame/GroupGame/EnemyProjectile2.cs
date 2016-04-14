using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroupGame
{
    class EnemyProjectile2 : EnemyProjectile
    {
        private int count;
        private int speed;
        private bool moving = false;

        public int Count
        {
            get { return count; }
        }

        public int Speed
        {
            get { return speed; }
        }

        public bool Moving
        {
            get { return moving; }
            set { moving = value; }
        }

        public override void Move()
        {
            if (count == 0)
            {
                base.Move();
            }
            else count--;
        }

        public EnemyProjectile2(int dmg, int w, int h, Enemy e, float ang, int spd, int c):base(dmg,w,h,e,ang,spd)
        {
            speed = spd;
            count = c;
        }
    }
}
