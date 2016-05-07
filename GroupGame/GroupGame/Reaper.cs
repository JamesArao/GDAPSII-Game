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
        private int prevAttack;
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

        public int PrevAttack
        {
            get { return prevAttack; }
            set { prevAttack = value; }
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
                // Create a Vector2 origin which equals the center of one frame of the player image
                Vector2 origin = new Vector2(Image.Width/2, Image.Height/2);

                // Draw the player at it's position plus half its size, and rotate it based on the rAngle passed in
                sprite.Draw(Image, new Rectangle(Position.X + Position.Width / 2, Position.Y + Position.Height / 2, Position.Width, Position.Height), null, color, rAngle - (float)Math.PI / 2, origin, SpriteEffects.None, 0);
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
                if (c.Energy < 200) c.Energy++;
            }
            phaseCount++;
            if(phaseCount == 180)
            {
                Health = maxHealth;
                CRect = new Rectangle(Position.X + 40, Position.Y + 40, CRect.Width + 35, CRect.Height + 35);
                c.Health = 100;
                c.Energy = 200;
                phase = 2;
                AttackNum = 5;
                attackCount = 600;
            }
        }

        // Constructor
        public Reaper(int posX, int posY, Texture2D hImg) : base(posX, posY)
        {
            Position = new Rectangle(posX, posY, 160, 160); // Set position
            FPosX = posX;
            FPosY = posY;
            CRect = new Rectangle(posX + 40, posY + 40, 80, 80); // Set the cRect based on the position
            Health = 9000; // Set health
            //Health = 90;
            maxHealth = Health;
            Speed = 0f; // Set speed
            EState = EnemyState.Chase; // Set EState to chase, for testing
            prevAttack = 6;
            attackNum = 6;
            attackCount = 360;
            healthImage = hImg;
            darkness = .45f;
            visible = true;
            phase = 1;
        }
    }
}
