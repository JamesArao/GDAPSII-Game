// EPWobble
// Class for the basic projectile, inherits from Projectile
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
    class PBasic : Projectile
    {
        // Constructor
        public PBasic(int dmg, int w, int h, Character c, float ang, int cMax, bool p, Texture2D img) : base(dmg, w, h, c, ang, cMax, p, img)
        {
        }
    }
}
