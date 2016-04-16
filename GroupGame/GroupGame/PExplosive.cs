using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GroupGame
{
    class PExplosive : Projectile
    {
        private Rectangle explosion;
        private int explosionCount;
        private bool collided;

        public Rectangle Explosion
        {
            get { return explosion; }
            set { explosion = value; }
        }

        public int ExplostionCount
        {
            get { return explosionCount; }
            set { explosionCount = value; }
        }

        public bool Collided
        {
            get { return collided; }
            set { collided = value; }
        }

        public override void Move()
        {
            if (explosionCount == 0 && collided == false) base.Move();
        }

        public void Explode(Character c, List<Enemy> enemies, List<Projectile> projectiles, List<EnemyProjectile> eProjectiles) 
        {
            if(explosionCount == 0)
            {
                explosion = new Rectangle(Position.X - 100, Position.Y - 100, 200, 200);
                for(int i = projectiles.Count - 1; i >= 0; i--)
                {
                    if (projectiles[i].Position.Intersects(explosion) && projectiles[i] != this)
                    {
                        projectiles.RemoveAt(i);
                    }
                }
                for (int i = eProjectiles.Count - 1; i >= 0; i--)
                {
                    if (eProjectiles[i].Position.Intersects(explosion))
                    {
                        eProjectiles.RemoveAt(i);
                    }
                }
                foreach(Enemy e in enemies)
                {
                    if (e.Position.Intersects(explosion))
                    {
                        e.Health -= Damage;
                    }
                }
                if (c.Position.Intersects(explosion) && (c.DashCount > 20 || c.DashCount == 0))
                {
                    c.Health -= Damage / 10;
                }
                explosionCount++;
            }
            if(explosionCount == 10)
            {
                projectiles.Remove(this);
            }
            else
            {
                explosionCount++;
            }
        }

        public PExplosive(int dmg, int w, int h, Character c, float ang, int cMax, Texture2D img) : base(dmg,w,h,c,ang,cMax,false,img)
        {
            MoveX = -(float)(Math.Sin(ang - Math.PI / 2) * 2.8);
            MoveY = (float)(Math.Cos(ang - Math.PI / 2) * 2.8);
        }

    }
}
