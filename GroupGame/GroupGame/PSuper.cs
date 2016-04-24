// PSuper
// Class for the super projectile shot by the super move, inherits from Projectile
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
    class PSuper : Projectile
    {
        // Constructor
        public PSuper(int dmg, int w, int h, Character c, float ang, int cMax, bool p, Texture2D img) : base(dmg, w, h, c, ang, cMax, p, img)
        {
            // moveX and moveY values are set by taking the sin or cosine of the angle and multiplying it by five
            MoveX = -(float)Math.Sin(ang - Math.PI / 2) * 5;
            MoveY = (float)Math.Cos(ang - Math.PI / 2) * 5;
        }
    }
}
