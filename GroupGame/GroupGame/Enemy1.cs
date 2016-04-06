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
    class Enemy1:Enemy
    {

        // Override the Move method
        public override void Move(Character c, List<Enemy> enemies)
        {

            // Switch statement based on EState
            switch (EState)
            {
                // Enemy is wandering
                case EnemyState.Wander:
                    break;
                
                // Enemy is chasing the player
                case EnemyState.Chase:
                    int newX = Position.X;
                    int newY = Position.Y;

                    // Compare enemy position to player position, change values accordingly
                    if (Position.X < c.Position.X)
                    {
                        bool collides = false;
                        foreach (Enemy others in enemies)
                        {
                            // If the enemy intersects the position of another enemy that is alive, collides is true
                            if (new Rectangle(newX + 1, newY, 50, 50).Intersects(others.Position) == true && others.Position != this.Position && others.Alive == true)
                            {
                                collides = true;
                            }

                        }
                        // If the enemy is not colliding with another enemy (collides is not true), it moves
                        if (collides != true)
                        {
                            newX++; ;
                        }
                    }
                    if (Position.X > c.Position.X) 
                    {
                        bool collides = false;
                        foreach (Enemy others in enemies)
                        {
                            if(new Rectangle(newX - 1, newY, 50, 50).Intersects(others.Position) == true && others.Position != this.Position && others.Alive == true)
                            {
                                collides = true;
                            }
                            
                        }
                        if(collides != true)
                        {
                            newX--;
                        }
                    }
                    if (Position.Y < c.Position.Y) 
                    {
                        bool collides = false;
                        foreach (Enemy others in enemies)
                        {
                            if (new Rectangle(newX, newY + 1, 50, 50).Intersects(others.Position) == true && others.Position != this.Position && others.Alive == true)
                            {
                                collides = true;
                            }

                        }
                        if (collides != true)
                        {
                            newY++;
                        }
                    }
                    if (Position.Y > c.Position.Y) 
                    {
                        bool collides = false;
                        foreach (Enemy others in enemies)
                        {
                            if (new Rectangle(newX, newY - 1, 50, 50).Intersects(others.Position) == true && others.Position != this.Position && others.Alive == true)
                            {
                                collides = true;
                            }

                        }
                        if (collides != true)
                        {
                            newY--;
                        }
                    }
                    Position = new Rectangle(newX, newY, Position.Width, Position.Height);
                    CRect = new Rectangle(newX + 15, newY + 15, CRect.Width, CRect.Height);
                    break;
            }
        }

        // Constructor
        public Enemy1(int posX, int posY):base(posX, posY)
        {
            Position = new Rectangle(posX, posY, 50, 50); // Set position
            CRect = new Rectangle(posX + 15, posY + 15, 20, 20); // Set the cRect based on the position
            Health = 100; // Set health
            EState = EnemyState.Chase; // Set EState to chase, for testing
        }
    }
}
