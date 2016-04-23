// PMine
// Class for a mine projectile that explodes when an enemy walks over it, inherits from Projectile
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
    class PMine : Projectile
    {
        // Attributes
        private Rectangle explosion; // Explosion radius
        private int explosionCount; // Time the explosion has been out

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

        // Explode method
        public void Explode(Character c, List<Enemy> enemies, List<Projectile> projectiles, List<EnemyProjectile> eProjectiles)
        {
            // The explosion is just starting, damage enemies and player, destroy any projectiles or enemy projectiles
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

            // The explosion has ended, this projectile is removed from the list
            if (explosionCount == 10)
            {
                projectiles.Remove(this);
            }

            // Otherwise, this is exploding, increase the count
            else
            {
                explosionCount++;
            }
        }

        // Override the move method so the mine only moves when the screen changes its FPosX andd FPosY
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
