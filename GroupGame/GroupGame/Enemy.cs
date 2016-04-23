// Enemy
// Abstract class for enemies
// Coders: Kiernan Brown, James Arao, Nick Federico

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

enum EnemyState { Wander, Chase, Shoot };

namespace GroupGame
{
    abstract class Enemy:GameObject
    {
        // Attributes
        private int spawnCount = -1;
        private int health; // Health of the enemy
        private EnemyState eState; // State the enemy is in
        private bool alive = false; // Is the enemy alive
        private Rectangle cRect; // Rectangle for enemy collision
        private float fPosX;
        private float fPosY;
        private float speed;
        private int shotCount;

        // Values for animation
        const int enemyY = 0;
        const int enemyHeight = 32;
        const int enemyWidth = 32;
        const int enemyX = 0;

        // Health property
        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        // EState property
        public EnemyState EState
        {
            get { return eState; }
            set { eState = value; }
        }

        // Alive property
        public bool Alive
        {
            get { return alive; }
            set { alive = value; }
        }

        // CRect Property
        public Rectangle CRect
        {
            get { return cRect; }
            set { cRect = value; }
        }

        // FPosX property
        public float FPosX
        {
            get { return fPosX; }
            set { fPosX = value; }
        }

        // FPosY property
        public float FPosY
        {
            get { return fPosY; }
            set { fPosY = value; }
        }

        // Speed property
        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        // ShotCount property
        public int ShotCount
        {
            get { return shotCount; }
            set { shotCount = value; }
        }

        public int SpawnCount
        {
            get { return spawnCount; }
            set { spawnCount = value; }
        }

        // Move method
        public virtual void Move(Character c, List<Enemy> enemies, List<Rectangle> boxes)
        {
            // Switch statement based on EState
            switch (EState)
            {
                // Enemy is wandering
                case EnemyState.Wander:
                    break;

                // Enemy is chasing the player
                case EnemyState.Chase:
                    float newX = FPosX;
                    float newY = FPosY;

                    // Compare enemy position to player position, change values accordingly
                    if (Position.X < c.Position.X)
                    {
                        bool collides = false;
                        foreach (Enemy others in enemies)
                        {
                            // If the enemy intersects the position of another enemy that is alive, collides is true
                            if (new Rectangle((int)(newX + Speed), (int)newY, Position.Width, Position.Height).Intersects(others.Position) == true && others.Position != this.Position && others.Alive == true)
                            {
                                collides = true;
                            }

                        }
                        foreach (Rectangle otherR in boxes)
                        {
                            if (new Rectangle((int)(newX + Speed), (int)newY, Position.Width, Position.Height).Intersects(otherR) == true && otherR != this.Position)
                            {
                                collides = true;
                            }

                        }
                        // If the enemy is not colliding with another enemy (collides is not true), it moves
                        if (collides != true)
                        {
                            newX += Speed;
                        }
                    }
                    if (Position.X > c.Position.X)
                    {
                        bool collides = false;
                        foreach (Enemy others in enemies)
                        {
                            if (new Rectangle((int)(newX - Speed), (int)newY, Position.Width, Position.Height).Intersects(others.Position) == true && others.Position != this.Position && others.Alive == true)
                            {
                                collides = true;
                            }

                        }
                        foreach (Rectangle otherR in boxes)
                        {
                            if (new Rectangle((int)(newX - Speed), (int)newY, Position.Width, Position.Height).Intersects(otherR) == true && otherR != this.Position)
                            {
                                collides = true;
                            }

                        }
                        if (collides != true)
                        {
                            newX -= Speed;
                        }
                    }
                    if (Position.Y < c.Position.Y)
                    {
                        bool collides = false;
                        foreach (Enemy others in enemies)
                        {
                            if (new Rectangle((int)newX, (int)(newY + Speed), Position.Width, Position.Height).Intersects(others.Position) == true && others.Position != this.Position && others.Alive == true)
                            {
                                collides = true;
                            }

                        }
                        foreach (Rectangle otherR in boxes)
                        {
                            if (new Rectangle((int)newX, (int)(newY + Speed), Position.Width, Position.Height).Intersects(otherR) == true && otherR != this.Position)
                            {
                                collides = true;
                            }

                        }
                        if (collides != true)
                        {
                            newY += Speed;
                        }
                    }
                    if (Position.Y > c.Position.Y)
                    {
                        bool collides = false;
                        foreach (Enemy others in enemies)
                        {
                            if (new Rectangle((int)newX, (int)(newY - Speed), Position.Width, Position.Height).Intersects(others.Position) == true && others.Position != this.Position && others.Alive == true)
                            {
                                collides = true;
                            }

                        }
                        foreach (Rectangle otherR in boxes)
                        {
                            if (new Rectangle((int)newX, (int)(newY - Speed), Position.Width, Position.Height).Intersects(otherR) == true && otherR != this.Position)
                            {
                                collides = true;
                            }

                        }
                        if (collides != true)
                        {
                            newY -= Speed;
                        }
                    }
                    FPosX = newX;
                    FPosY = newY;
                    Position = new Rectangle((int)newX, (int)newY, Position.Width, Position.Height);
                    CRect = new Rectangle((int)newX + 15, (int)newY + 15, CRect.Width, CRect.Height);
                    break;
            }
        }

        // Draw method
        public virtual void Draw(SpriteBatch sprite, float rAngle, int f , Color color)
        {
            // Create a Vector2 origin which equals the center of one frame of the player image
            Vector2 origin = new Vector2(16, 16);

            // Draw the player at it's position plus half its size, and rotate it based on the rAngle passed in
            sprite.Draw(Image, new Rectangle(Position.X + Position.Width / 2, Position.Y + Position.Height / 2, Position.Width, Position.Height), new Rectangle(enemyX + f * enemyWidth, enemyY, enemyWidth, enemyHeight), color, rAngle - (float)Math.PI / 2, origin, SpriteEffects.None, 0);
        }

        // Parameterized constructor
        public Enemy(int posX, int posY)
        {
            Position = new Rectangle(posX, posY, Position.Width, Position.Height);
            fPosX = posX;
            FPosY = posY;
        }
    }
}
