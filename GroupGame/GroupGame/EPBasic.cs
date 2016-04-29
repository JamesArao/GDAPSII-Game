// EPBasic
// Class for the basic enemy projectile, inherits from EnemyProjectile
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
    class EPBasic : EnemyProjectile
    {
        // Constructor
        public EPBasic(int dmg, int w, int h, Enemy e, float ang, float speed, Texture2D img):base(dmg, w, h, e, ang, speed, img)
        {
        }

        public EPBasic(int dmg, int w, int h, int x, int y, float ang, float speed, Texture2D img):base(dmg, w, h, null, ang, speed, img)
        {
            Position = new Rectangle((int)x, (int)y, w, h);
            FPosX = x;
            FPosY = y;
        }
    }
}
