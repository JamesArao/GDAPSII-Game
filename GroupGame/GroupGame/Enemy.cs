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
        private int health;
        private EnemyState eState;
        private bool alive = true;

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

        // Method to move the enemy
        public abstract void Move(Character c);

        // Method to change enemy state
        //public abstract void ChangeState(EnemyState state);

        // Parameterized constructor
        public Enemy(int posX, int posY)
        {
            Position = new Rectangle(posX, posY, Position.Width, Position.Height);
        }
    }
}
