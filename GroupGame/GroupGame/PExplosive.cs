// PExplosive
// Class for a projectile that explodes after a time, inherits from Projectile
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
    class PExplosive : Projectile
    {
        // Attributes
        private Rectangle explosion; // Explosion radius
        private int explosionCount; // Time the explosion has been out
        private bool collided; // Has the explosive collided with an enemy or object

        // Explosion property
        public Rectangle Explosion
        {
            get { return explosion; }
            set { explosion = value; }
        }

        // ExplosionCount property
        public int ExplosionCount
        {
            get { return explosionCount; }
            set { explosionCount = value; }
        }

        // Collided property
        public bool Collided
        {
            get { return collided; }
            set { collided = value; }
        }

        // Override Move so the explosive stops after colliding
        public override void Move()
        {
            if (explosionCount == 0 && collided == false) base.Move();
            else Position = new Rectangle((int)FPosX, (int)FPosY, Position.Width, Position.Height);
        }

        // Explode method, make the projectile explode
        public void Explode(Character c, List<Enemy> enemies, List<Projectile> projectiles, List<EnemyProjectile> eProjectiles) 
        {
            // The explosion is just starting, damage enemies and player, destroy any projectiles or enemy projectiles
            if(explosionCount == 0)
            {
                explosion = new Rectangle((int)FPosX - 100, (int)FPosY - 100, 200, 200);
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

            // The explosion has ended, this projectile is removed from the list
            if(explosionCount == 10)
            {
                projectiles.Remove(this);
            }

            // Otherwise, this is exploding, increase the count
            else
            {
                explosionCount++;
            }
        }

        // Constructor
        public PExplosive(int dmg, int w, int h, Character c, float ang, int cMax, Texture2D img) : base(dmg,w,h,c,ang,cMax,false,img)
        {
            // This projectile moves slower than the basic one, MoveX and MoveY are multiplied by 2.8 instead of 4
            MoveX = -(float)(Math.Sin(ang - Math.PI / 2) * 2.8);
            MoveY = (float)(Math.Cos(ang - Math.PI / 2) * 2.8);
        }

    }
}
