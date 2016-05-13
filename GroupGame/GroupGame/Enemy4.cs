// Enemy4
// 
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
    class Enemy4 : Enemy
    {
        private int chargeCount;
        private bool charging = false;
        private float chargeX;
        private float chargeY;
        private float chargeAngle;

        public bool Charging
        {
            get { return charging; }
            set { charging = value; }
        }

        public int ChargeCount
        {
            get { return chargeCount; }
            set { chargeCount = value; }
        }

        public override void Move(Character c, List<Enemy> enemies, List<Rectangle> boxes)
        {
            // We aren't charging, move normally and decrease chargeCount
            if (Alive == true)
            {
                if (chargeCount > 30 && chargeCount <= 270)
                {
                    base.Move(c, enemies, boxes);
                    chargeCount--;
                }

                // If chargeCount is between 30 andd 0, we don't move as it is getting ready to charge
                if ((chargeCount <= 30 || chargeCount > 270) && chargeCount > 0)
                {
                    Position = new Rectangle((int)FPosX, (int)FPosY, Position.Width, Position.Height);
                    chargeCount--;
                }

                if (chargeCount == 0)
                {
                    Position = new Rectangle((int)FPosX, (int)FPosY, Position.Width, Position.Height);
                    Charge(c);
                }
                // We're charging
                if (charging == true)
                {
                    bool collides = false;
                    Rectangle newPosition = new Rectangle((int)(FPosX + chargeX), (int)(FPosY + chargeY), Position.Width, Position.Height);
                    foreach (Rectangle box in boxes)
                    {
                        if (newPosition.Intersects(box))
                        {
                            collides = true;
                        }
                    }


                    if ((new Rectangle(newPosition.X + 15, newPosition.Y + 15, 36,36).Intersects(c.CRect) && (c.DashCount >= 20 || c.DashCount == 0 )) && collides == false)
                    {
                        c.Health -= 15;
                        FPosX += chargeX;
                        FPosY += chargeY;
                        Position = new Rectangle((int)FPosX, (int)FPosY, Position.Width, Position.Height);
                        CRect = new Rectangle(Position.X + 15, Position.Y + 15, 20, 20); // Set the cRect based on the position
                        collides = true;
                    }

                    if (collides == false)
                    {
                        FPosX += chargeX;
                        FPosY += chargeY;
                        Position = new Rectangle((int)FPosX, (int)FPosY, Position.Width, Position.Height);
                        CRect = new Rectangle(Position.X + 15, Position.Y + 15, 20, 20); // Set the cRect based on the position
                    }
                    else
                    {
                        /*foreach(Enemy e in enemies)
                        {
                            while(e.Position.Intersects(Position))
                            {
                                FPosX++;
                                FPosY++;
                                Position = new Rectangle((int)FPosX, (int)FPosY, Position.Width, Position.Height);
                            }
                        }*/
                        chargeCount = 300;
                        charging = false;
                    }
                }
            }
            else
            {
                Position = new Rectangle((int)FPosX, (int)FPosY, Position.Width, Position.Height);
            }
        }

        public void Charge(Character c)
        {
            // Get the angle between the player and this enemy
            int x = (c.Position.X + c.Position.Width / 2) - (Position.X + Position.Width / 2);
            int y = (c.Position.Y + c.Position.Height / 2) - (Position.Y + Position.Height / 2);
            chargeAngle = (float)Math.Atan2(x, y);

            // Set the X and Y values that we are using to charge
            chargeX = (float)Math.Sin(chargeAngle ) * 13;
            chargeY = (float)Math.Cos(chargeAngle) * 13;

            // We are charging
            charging = true;
            chargeCount = -1;
        }

        public override void Draw(SpriteBatch sprite, float rAngle, int f, Color color)
        {
            if(charging == false && chargeCount > 30)
            {
                base.Draw(sprite, rAngle, f, color);
            }
            else if(Charging == false)
            {
                base.Draw(sprite, rAngle, f, Color.OrangeRed);
            }
            else
            {
                base.Draw(sprite, -(chargeAngle - (float)Math.PI/2), f, Color.Red);
            }

        }

        // Parameterized constructor
        public Enemy4(int posX, int posY): base(posX,posY)
        {
            Position = new Rectangle(posX, posY, 66, 66);
            FPosX = posX;
            FPosY = posY;
            CRect = new Rectangle(posX + 15, posY + 15, 36, 36); // Set the cRect based on the position
            Speed = .7f;
            Health = 175;
            chargeCount = 270;
            EState = EnemyState.Chase; // Set EState to chase, for testing
        }
    }
}
