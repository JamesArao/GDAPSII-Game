// Boss4
// ...Reaper
// Coders: Kiernan Brown,

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GroupGame
{
    class Reaper : Enemy
    {
        // Attributes
        private int attackNum;
        private int attackCount;
        private int maxHealth;
        private int phase;
        private int phaseCount;
        private float darkness;
        private bool visible; 
        Texture2D healthImage;

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

        public float Darkness
        {
            get { return darkness; }
            set { darkness = value; }
        }

        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        public int Phase
        {
            get { return phase; }
            set { phase = value; }
        }

        public override void Move(Character c, List<Enemy> enemies, List<Rectangle> boxes)
        {
            Position = new Rectangle((int)FPosX, (int)FPosY, Position.Width, Position.Height);
            if(phase == 1)CRect = new Rectangle(Position.X + 40, Position.Y + 40, CRect.Width, CRect.Height);
            if (phase == 2) CRect = new Rectangle(Position.X + 60, Position.Y + 60, CRect.Width, CRect.Height);
        }

        public override void Draw(SpriteBatch sprite, float rAngle, int f, Color color)
        {
            if (visible)
            {
                base.Draw(sprite, rAngle, 0, color);
                sprite.Draw(healthImage, new Rectangle(Position.X + (maxHealth / 50 - Position.Width / 4), Position.Y - 20, maxHealth / 50, 10), Color.Red);
                sprite.Draw(healthImage, new Rectangle(Position.X + (maxHealth / 50 - Position.Width / 4), Position.Y - 20, Health / 50, 10), Color.LawnGreen);
            }
        }

        public void ChangePhase(Character c)
        {
            Health += maxHealth / 180;
            if(phaseCount % 2 == 0)
            {
                Position = new Rectangle((int)FPosX, (int)FPosY, Position.Width + 1, Position.Height + 1);
                if(c.Health < 100) c.Health++;
            }
            phaseCount++;
            if(phaseCount == 180)
            {
                Health = maxHealth;
                CRect = new Rectangle(Position.X + 60, Position.Y + 60, CRect.Width + 25, CRect.Height + 25);
                c.Health = 100;
                phase = 2;
            }
        }

        // Constructor
        public Reaper(int posX, int posY, Texture2D hImg) : base(posX, posY)
        {
            Position = new Rectangle(posX, posY, 160, 160); // Set position
            FPosX = posX;
            FPosY = posY;
            CRect = new Rectangle(posX + 40, posY + 40, 80, 80); // Set the cRect based on the position
            Health = 6666; // Set health
            maxHealth = Health;
            Speed = 0f; // Set speed
            EState = EnemyState.Chase; // Set EState to chase, for testing
            attackNum = 6;
            attackCount = 480;
            healthImage = hImg;
            darkness = .45f;
            visible = true;
            phase = 1;
        }
    }
}
