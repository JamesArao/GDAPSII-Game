// Enemy3
// Class for the third enemy, inherits from Enemy
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
    class Enemy3 : Enemy
    {

        private Rectangle range;
        private int shotCount;

        public Rectangle Range
        {
            get { return range; }
            set { range = value; }
        }

        public int ShotCount
        {
            get { return shotCount; }
            set { shotCount = value; }
        }

        // Override the Move method
        public override void Move(Character c, List<Enemy> enemies, List<Rectangle> boxes)
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
                            if (new Rectangle((int)(newX + Speed), (int)newY, 50, 50).Intersects(others.Position) == true && others.Position != this.Position && others.Alive == true)
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
                            if (new Rectangle((int)(newX - Speed), (int)newY, 50, 50).Intersects(others.Position) == true && others.Position != this.Position && others.Alive == true)
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
                            if (new Rectangle((int)newX, (int)(newY + Speed), 50, 50).Intersects(others.Position) == true && others.Position != this.Position && others.Alive == true)
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
                            if (new Rectangle((int)newX, (int)(newY - Speed), 50, 50).Intersects(others.Position) == true && others.Position != this.Position && others.Alive == true)
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

        // Constructor
        public Enemy3(int posX, int posY) : base(posX, posY)
        {
            Position = new Rectangle(posX, posY, 50, 50); // Set position
            FPosX = posX;
            FPosY = posY;
            CRect = new Rectangle(posX + 15, posY + 15, 20, 20); // Set the cRect based on the position
            Health = 120; // Set health
            Speed = 0.5f; // Set speed
            EState = EnemyState.Chase; // Set EState to chase, for testing
        }
    }
}
