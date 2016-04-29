// PStationary
// Class for the stationary (melee) projectile, inherits from Projectile
// Coders: Kiernan Brown, Nick Federico, Austin Richardson

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GroupGame
{
    class PStationary : Projectile
    {

        // Values for animation
        const int projY = 0;
        const int projHeight = 32;
        const int projWidth = 32;
        const int projX = 0;

        // Method to move the stationary projectile in front of the player
        public void Move(Character c, float ang)
        {
            MoveX = -(float)Math.Sin(ang - Math.PI / 2) * 4;
            MoveY = (float)Math.Cos(ang - Math.PI / 2) * 4;
            Position = new Rectangle(c.Position.X + (int)MoveX * 5, c.Position.Y + (int)MoveY * 5, Position.Width, Position.Height);
        }

        // Draw method
        public void Draw(SpriteBatch sprite, int f, float rAngle)
        {
            Vector2 origin = new Vector2(16, 16);

            // Draw the projectile at its position , and rotate it based on the rAngle passed in
            sprite.Draw(Image, new Rectangle(Position.X + Position.Width / 2, Position.Y + Position.Height / 2, Position.Width, Position.Height), new Rectangle(projX + f * projWidth, projY, projWidth, projHeight), Color.White, rAngle + (float)Math.PI / 2, origin, SpriteEffects.None, 0);
        }

        // Constructor
        public PStationary(int dmg, int w, int h, Character c, float ang, int cMax, bool p, Texture2D img): base(dmg, w, h, c,ang,cMax,p, img)
        {
        }
    }
}
