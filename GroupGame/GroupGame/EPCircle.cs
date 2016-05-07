using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GroupGame
{
    class EPCircle : EnemyProjectile
    {
        private int shootingCount;
        private int counter;
        private int shotsFired;
        private float speed;
        private bool moving;

        public int ShootingCount
        {
            get { return shootingCount; }
            set { shootingCount = value; }
        }

        public override void Move()
        {
            if(!moving) Position = new Rectangle((int)FPosX, (int)FPosY, Position.Width, Position.Height);
        }

        public void Move(Character c)
        {
            if(moving)
            {
                int angX = (c.Position.X + c.Position.Width / 2) - (Position.X + Position.Width / 2);
                int angY = (c.Position.Y + c.Position.Height / 2) - (Position.Y + Position.Height / 2);
                float angle = (float)Math.Atan2(angY, angX);
                FPosX += -(float)Math.Sin(angle - Math.PI / 2) * speed;
                FPosY += (float)Math.Cos(angle - Math.PI / 2) * speed;
                Position = new Rectangle((int)FPosX, (int)FPosY, Position.Width, Position.Height);
            }
        }

        public override void Draw(SpriteBatch sprite)
        {
            sprite.Draw(Image, Position, Color.White);
        }

        public void Shoot(List<EnemyProjectile> ePList, Texture2D img)
        {
            if (counter == shootingCount)
            {
                for(int i = 0; i < 8; i++)
                {
                    float moveX = -(float)Math.Sin((float)(2 * Math.PI / 8) * i - Math.PI / 2) * 400;
                    float moveY = (float)Math.Cos((float)(2 * Math.PI / 8) * i - Math.PI / 2) * 400;
                    if(!moving) ePList.Add(new EPBasic(Damage, 32, 32, Position.X + Position.Width / 4, Position.Y + Position.Height / 4, ((float)(2 * Math.PI / 8) * i) + (float)(shotsFired / (Math.PI / 2)), speed, img));
                    if(moving) ePList.Add(new EPBasic(Damage, 32, 32, Position.X + Position.Width / 4, Position.Y + Position.Height / 4, ((float)(2 * Math.PI / 8) * i) + (float)(shotsFired / (Math.PI / 2)), speed * 4, img));
                }
                shotsFired++;
                counter = 0;
            }
            else
            {
                counter++;
            }
        }

        // Constructor
        public EPCircle(int dmg, int w, int h, Enemy e, float ang, float spd, int count, Texture2D img, bool m):base(dmg, w, h, e, ang, spd, img)
        {
            shootingCount = count;
            speed = spd;
            moving = m;
        }

        public EPCircle(int dmg, int w, int h, int x, int y, float ang, float spd, int count, Texture2D img, bool m):base(dmg, w, h, null, ang, spd, img)
        {
            Position = new Rectangle((int)x, (int)y, w, h);
            FPosX = x;
            FPosY = y;
            shootingCount = count;
            speed = spd;
            moving = m;
        }
    }
}
