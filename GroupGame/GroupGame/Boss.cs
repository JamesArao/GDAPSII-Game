// Boss
// Class for the boxx in our game
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
    class Boss : Enemy
    {
        // Attributes
        private int shotCount;
        private int attackNum;
        private int attackCount;
        private int maxHealth;
        private bool moving = true;
        Texture2D healthImage;
        Random attackGen = new Random();

        public bool Moving
        {
            get { return moving; }
            set { moving = value; }
        }

        public int AttackNum
        {
            get { return attackNum; }
            set { attackNum = value; }
        }

        public int AttackCount
        {
            get { return attackCount; }
            set { attackCount = value; }
        }

        public override void Move(Character c, List<Enemy> enemies, List<Rectangle> boxes)
        {
            if (Alive == true)
            {
                if (moving == true) base.Move(c, enemies, boxes);
                else Position = new Rectangle((int)FPosX, (int)FPosY, Position.Width, Position.Height);
            }
            else Position = new Rectangle((int)FPosX, (int)FPosY, Position.Width, Position.Height);
        }

        public override void Draw(SpriteBatch sprite, float rAngle, int f, Color color)
        {
            base.Draw(sprite, rAngle, f, color);
            sprite.Draw(healthImage, new Rectangle(Position.X, Position.Y - 20, maxHealth / 50, 10), Color.Red);
            sprite.Draw(healthImage, new Rectangle(Position.X, Position.Y - 20, Health / 50, 10), Color.LawnGreen);
        }

        // Constructor
        public Boss(int posX, int posY, Texture2D hImg) : base(posX, posY)
        {
            Position = new Rectangle(posX, posY, 120, 120); // Set position
            FPosX = posX;
            FPosY = posY;
            CRect = new Rectangle(posX + 20, posY + 20, 80, 80); // Set the cRect based on the position
            Health = 6000; // Set health
            maxHealth = Health;
            Speed = 0.5f; // Set speed
            EState = EnemyState.Chase; // Set EState to chase, for testing
            attackNum = 5;
            attackCount = 240;
            healthImage = hImg;
        }
    }
}