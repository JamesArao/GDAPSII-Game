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

        public int ShootingCount
        {
            get { return shootingCount; }
            set { shootingCount = value; }
        }

        public override void Move()
        {
            Position = new Rectangle((int)FPosX, (int)FPosY, Position.Width, Position.Height);
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
                    ePList.Add(new EPBasic(Damage, 32, 32, Position.X + Position.Width / 4, Position.Y + Position.Height / 4, ((float)(2 * Math.PI / 8) * i) + (float)(shotsFired / (Math.PI / 2)), speed, img));
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
        public EPCircle(int dmg, int w, int h, Enemy e, float ang, float spd, int count, Texture2D img):base(dmg, w, h, e, ang, spd, img)
        {
            shootingCount = count;
            speed = spd;
        }

        public EPCircle(int dmg, int w, int h, int x, int y, float ang, float spd, int count, Texture2D img):base(dmg, w, h, null, ang, spd, img)
        {
            Position = new Rectangle((int)x, (int)y, w, h);
            FPosX = x;
            FPosY = y;
            shootingCount = count;
            speed = spd;
        }
    }
}
