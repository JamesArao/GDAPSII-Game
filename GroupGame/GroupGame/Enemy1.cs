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
        public override void Move(Character c)
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
                    if (Position.X < c.Position.X) newX++;
                    if (Position.X > c.Position.X) newX--;
                    if (Position.Y < c.Position.Y) newY++;
                    if (Position.Y > c.Position.Y) newY--;
                    Position = new Rectangle(newX, newY, Position.Width, Position.Height);
                    break;
            }
        }

        // Constructor
        public Enemy1(int posX, int posY):base(posX, posY)
        {
            Position = new Rectangle(posX, posY, 50, 50);
            Health = 100;
            EState = EnemyState.Chase; // Set EState to chase, for testing
        }
    }
}
