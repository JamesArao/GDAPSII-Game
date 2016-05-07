// EPRectangle
// A flashing rectangle that gets filled with projectiles
// Coders: Kiernan Brown, 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GroupGame
{
    class EPRectangle : EnemyProjectile
    {
        private int count;
        private Color color;

        public void Shoot(List<EnemyProjectile> eps, Texture2D img)
        {
            if (count > 0)
            {
                if (count % 15 == 0)
                {
                    if (color == Color.Red)
                    {
                        color = Color.Transparent;
                    }
                    else
                    {
                        color = Color.Red;
                    }
                }
            }
            if(count <= 0 && count > -30)
            {
                if(count % 2 == 0)
                {
                    for (int i = 1; i < 8; i++)
                    {
                        eps.Add(new EPBasic(Damage, 35, 35, Position.X + (Position.Width / 8) * i, Position.Y, (float)Math.PI / 2, 16, img));
                        eps.Add(new EPBasic(Damage, 35, 35, Position.X + (Position.Width / 8) * i, Position.Y + Position.Height, -(float)Math.PI / 2, 16, img));
                    }
                }
            }
            if(count == -30)
            {
                eps.Remove(this);
            }
            count--;
        }

        public override void Draw(SpriteBatch sprite)
        {
            if(count > 0) sprite.Draw(Image, Position, color);
        }

        public EPRectangle(int dmg, int w, int h, Enemy e, float ang, int speed, Texture2D img):base(dmg,w,h,e,ang,speed, img)
        {
        }

        public EPRectangle(int dmg, int w, int h, int x, int y, float ang, float speed, int c, Texture2D img):base(dmg, w, h, null, ang, speed, img)
        {
            Position = new Rectangle((int)x, (int)y, w, h);
            FPosX = x;
            FPosY = y;
            count = c;
        }
    }
}
