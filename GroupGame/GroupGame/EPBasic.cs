using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace GroupGame
{
    class EPBasic : EnemyProjectile
    {
        // Constructor
        public EPBasic(int dmg, int w, int h, Enemy e, float ang, int speed, Texture2D img):base(dmg, w, h, e, ang, speed, img)
        {
        }

    }
}
