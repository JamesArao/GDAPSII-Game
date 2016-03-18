// Enemy
// Abstract class for enemies
// Coders: Kiernan Brown, James Arao

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
        private int health; // Health of the enemy
        private EnemyState eState; // State the enemy is in
        private bool alive = true; // Is the enemy alive
        private Rectangle cRect; // Rectangle for enemy collision

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

        // Method to move the enemy
        public abstract void Move(Character c, List<Enemy> enemies);

        // Parameterized constructor
        public Enemy(int posX, int posY)
        {
            Position = new Rectangle(posX, posY, Position.Width, Position.Height);
        }
    }
}
