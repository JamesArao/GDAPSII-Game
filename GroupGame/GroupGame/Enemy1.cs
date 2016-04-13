// Enemy1
// Class for the first enemy, inherits from Enemy
// Coders: Kiernan Brown, James Arao

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GroupGame
{
    class Enemy1 : Enemy
    {

        // Override the Move method
        public override void Move(Character c, List<Enemy> enemies, List<Rectangle> objects)
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
                        foreach (Rectangle otherR in objects)
                        {
                            if (new Rectangle((int)(newX + Speed), (int)newY, 50, 50).Intersects(otherR) == true && otherR != this.Position)
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
                        foreach (Rectangle otherR in objects)
                        {
                            if (new Rectangle((int)(newX - Speed), (int)newY, 50, 50).Intersects(otherR) == true && otherR != this.Position)
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
                        foreach (Rectangle otherR in objects)
                        {
                            if (new Rectangle((int)newX, (int)(newY + Speed), 50, 50).Intersects(otherR) == true && otherR != this.Position)
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
                        foreach (Rectangle otherR in objects)
                        {
                            if (new Rectangle((int)newX, (int)(newY - Speed), 50, 50).Intersects(otherR) == true && otherR != this.Position)
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

        /*
        public Vector2 Rotate(Vector2 source, Vector2 target, float rad)
        {
            Vector2 around = source - target;
            float newX = (float)(around.X * Math.Cos(rad) - around.Y * Math.Sin(rad));
            float newY = (float)(around.X * Math.Sin(rad) - around.Y * Math.Cos(rad));
            return target + new Vector2(newX, newY);
        }*/

        // Constructor
        public Enemy1(int posX, int posY) : base(posX, posY)
        {
            Position = new Rectangle(posX, posY, 50, 50); // Set position
            FPosX = posX;
            FPosY = posY;
            CRect = new Rectangle(posX + 15, posY + 15, 20, 20); // Set the cRect based on the position
            Health = 100; // Set health
            Speed = 1.2f; // Set speed
            EState = EnemyState.Chase; // Set EState to chase, for testing
        }
    }
}
