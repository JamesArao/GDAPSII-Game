using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GroupGame
{
    class PMine : Projectile
    {
        private Rectangle explosion;
        private int explosionCount;

        public Rectangle Explosion
        {
            get { return explosion; }
            set { explosion = value; }
        }

        public int ExplosionCount
        {
            get { return explosionCount; }
            set { explosionCount = value; }
        }

        public void Explode(Character c, List<Enemy> enemies, List<Projectile> projectiles, List<EnemyProjectile> eProjectiles)
        {
            if (explosionCount == 0)
            {
                explosion = new Rectangle((int)FPosX - 100, (int)FPosY - 100, 200, 200);
                for (int i = projectiles.Count - 1; i >= 0; i--)
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
                foreach (Enemy e in enemies)
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
            if (explosionCount == 10)
            {
                projectiles.Remove(this);
            }
            else
            {
                explosionCount++;
            }
        }

        public override void Move()
        {
            Position = new Rectangle((int)FPosX, (int)FPosY, Position.Width, Position.Height);
        }

        // Constructor
        public PMine(int dmg, int w, int h, Character c, float ang, int cMax, bool p, Texture2D img):base(dmg,w,h,c,0,0,false,img)
        {
        }
    }
}
