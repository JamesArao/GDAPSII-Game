// Game1
// Runs the game
// Coders: Kiernan Brown, James Arao, Nick Federico, Austin Richardson

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

enum GameState { Menu, HordeMode, Paused, Options, CharacterSelection, GameOver, Leaderboard, Instructions, Instructions2, Instructions3, Instructions4 }; // GameState enum for keeping track of what state our game is in
enum AbilityState { a1, a2, a3, a4, a5, a6 }; // AbilityState enum for keeping track of the ability the player is using
enum HeroState { Still, Walking }; // HeroState enum for keeping track of the state of the player
enum SwitchHero { Fire, Earth, Water, Electric}; // switch heroes

namespace GroupGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont sFont;
        SpriteFont rFont;
        GameState gState;
        AbilityState aState;
        SwitchHero switchHero = SwitchHero.Fire;
        HeroState heroState = HeroState.Still;
        Random rgen = new Random();
        Boolean Bcont;

        // charcter and projectile textures
        Texture2D enemyImage;
        Texture2D playerImage;
        Texture2D playerWalking;
        Texture2D bulletImage;
        Texture2D player1Image;
        Texture2D player1Walking;
        Texture2D bullet1Image;
        Texture2D player2Image;
        Texture2D player2Walking;
        Texture2D bullet2Image;
        Texture2D player3Image;
        Texture2D player3Walking;
        Texture2D bullet3Image;
        Texture2D player4Image;
        Texture2D player4Walking;
        Texture2D bullet4Image;
        Texture2D meleeImage;
        Texture2D meleeStillImage;
        Texture2D boss;
        Texture2D reaper;

        // menu textures
        Texture2D startButton;
        Texture2D optionsButton;
        Texture2D fullscreenButton;
        Texture2D okButton;
        Texture2D cancelButton;
        Texture2D whiteBox;
        Texture2D paused;
        Texture2D menu;
        Texture2D nextB;
        Texture2D exitB;
        Texture2D hudrectangle;
        Texture2D hudcircle;
        Texture2D continueButton;
        Texture2D rectangle;
        Texture2D circle;
        Texture2D fireButton;
        Texture2D earthButton;
        Texture2D electricButton;
        Texture2D waterButton;
        Texture2D background;
        Texture2D dot;
        Texture2D basicImage;
        Texture2D stallImage;
        Texture2D accelerateImage;
        Texture2D wobbleImage;
        Texture2D eMarker;
        Texture2D rMarker;
        Texture2D superCharge;
        Texture2D leaderboardButton;
        Texture2D instructionsButton;
        Texture2D instructionsScreen;
        Texture2D instructionsScreen2;
        Texture2D instructionsScreen3;
        Texture2D instructionsScreen4;
        Texture2D title;
        Texture2D titleBackground;
        Texture2D leaderboardBackground;

        // ability textures
        Texture2D mine;
        Texture2D grenade;
        Texture2D explosion;
        
        // in-game banner textures
        Texture2D roundMarker;
        Texture2D powerupMarker;

        // obstacle textures
        Texture2D garbage;
        Texture2D car;
        Texture2D boxes;

        // power up textures
        Texture2D medkit;
        Texture2D energy;
        Texture2D points;

        // Rectangles for buttons, mouse, and HUD
        Rectangle rSButton;
        Rectangle rOButton;
        Rectangle rPOButton;
        Rectangle mRectangle;
        Rectangle rMButton;
        Rectangle rFButton;
        Rectangle rLButton;
        Rectangle rIButton;
        Rectangle rNextButton;
        Rectangle rEButton;
        Rectangle contButton;
        Rectangle char1;
        Rectangle char2;
        Rectangle char3;
        Rectangle char4;
        Rectangle CenterCircle;
        Rectangle InnerCircle;
        Rectangle LifeBarBox;
        Rectangle LifeBar;
        Rectangle LifeWordBox;
        Rectangle SpecialMoveBox;
        Rectangle SpecialMoveCircle;
        
        // Characters, enemies, and projectiles
        Character c;
        List<Enemy> enemies = new List<Enemy>();
        List<Enemy> enemiesSpawn = new List<Enemy>();
        List<Projectile> projectiles = new List<Projectile>();
        List<EnemyProjectile> eProjectiles = new List<EnemyProjectile>();
        List<Powerup> powerups = new List<Powerup>();
        List<Vector2> ePositions = new List<Vector2>();
        List<string> eTypes = new List<string>();

        // Keyboard states
        KeyboardState kbState;
        KeyboardState previousKbState;
        MouseState mState;
        MouseState prevMState;
        float rotationAngle;

        // Ints for round and score
        int round;
        int score;
        int enemyPause;
        int roundChange;
        string roundStr;
        string roundStrPartial;
        int powerupTimer;
        string powerUpPickup;
        string deathStr;
        string powerUpPartial;
        string powerUpPartial2;
        int pickupTimer;
        int skulls;
        bool spawned;

        int maxOnScreen = -1;
        bool reaperRound = false;

        // Variables for animating
        int framePlayer;
        int frameEnemy;
        int frameProjectile;
        double timePerFrame = 100;
        int numFramesPlayer;
        int numFramesEnemy = 8;
        int numFramesProjectile = 4;
        int framesElapsedPlayer;
        int framesElapsedEnemy;
        int framesElapsedProjectile;

        // Values used for screen movement 

        int globalX;
        int globalY;
        int maxX;
        int maxY;
        Point backgroundPoint;

        // Values used for the options menu
        bool closing = false;
        bool fullscreen = false;
        Rectangle rOKButton;
        Rectangle rCancelButton;

        // Randomly generated objects list
        List<Rectangle> objects;

        // Leaderboard attributes
        LeaderboardHandler lHandler;
        List<int> leaderboardScores = new List<int>();
        List<int> leaderboardRounds = new List<int>();
        List<string> leaderboardNames = new List<string>();
        List<string> leaderboardCharacters = new List<string>();
        bool enteringName = false;
        SpriteFont lFont;

        // Attributes for sound
        SoundEffect fireSound;
        SoundEffect waterSound;
        SoundEffect earthSound;
        SoundEffect electricSound;
        SoundEffect shootSound;
        SoundEffect enemyShootSound;
        SoundEffect enemyDieSound;
        SoundEffect explosionSound;

        // Method for advancing the round of our Horde Mode
        public void AdvanceRound()
        {
            roundChange = 180;
            roundStr = "Round " + (round + 1);
            roundStrPartial = "";
            powerups.Clear();

            bool bossRound = false;
            reaperRound = false;
            spawned = false;

            enemies.Clear(); // Clear Enemies list
            enemiesSpawn.Clear(); // Clear the EnemiesSpawn list
            projectiles.Clear(); // Clear the projectiles list
            eProjectiles.Clear(); // Clear the enemy projectiles list

            // Select a random round to use
            //int num = rgen.Next(1,round+1);
            BinaryReader reader;

            Random rng = new Random();
            //if (round < 14) reader = new BinaryReader(File.OpenRead(@"../../../Rounds/Round" + (round + 1) + ".dat"));
            if ((round + 1) % 10 == 0 && round != 0)
            {
                reader = new BinaryReader(File.OpenRead(@"../../../Rounds/Round" + 10 + ".dat"));
            }
            else if (round < 5)
            {
                if(round < 3)
                {
                    reader = new BinaryReader(File.OpenRead(@"../../../Rounds/RoundE" + (rng.Next(12) + 1) + ".dat"));
                }
                else if(round == 4)
                {
                    reader = new BinaryReader(File.OpenRead(@"../../../Rounds/RoundM" + (rng.Next(14) + 1) + ".dat"));
                }
                else
                {
                    reader = new BinaryReader(File.OpenRead(@"../../../Rounds/RoundE" + (rng.Next(12) + 1) + ".dat"));
                }

            }
            else if (round >= 5 && round < 11)
            {
                if(round % 2 == 1)
                {
                    reader = new BinaryReader(File.OpenRead(@"../../../Rounds/RoundE" + (rng.Next(12) + 1)  + ".dat"));
                }
                else
                {
                    reader = new BinaryReader(File.OpenRead(@"../../../Rounds/RoundM" + (rng.Next(14) + 1) + ".dat"));
                }
                
            }
            else if (round > 11 && round < 15)
            {
                if (round % 2 == 1)
                {
                    reader = new BinaryReader(File.OpenRead(@"../../../Rounds/RoundM" + (rng.Next(14) + 1) + ".dat"));
                }
                else
                {
                    reader = new BinaryReader(File.OpenRead(@"../../../Rounds/RoundH" + (rng.Next(11) + 1) + ".dat"));
                }

            }
            else if (round >= 15 && round < 20)
            {
                if (round % 2 == 1)
                {
                    reader = new BinaryReader(File.OpenRead(@"../../../Rounds/RoundH" + (rng.Next(11) + 1) + ".dat"));
                }
                else
                {
                    reader = new BinaryReader(File.OpenRead(@"../../../Rounds/RoundM" + (rng.Next(14) + 1) + ".dat"));
                }

            }
            else if (round > 20 && round%10 != 0)
            {
                reader = new BinaryReader(File.OpenRead(@"../../../Rounds/RoundH" + (rng.Next(11) + 1) + ".dat"));
            }
            else reader = new BinaryReader(File.OpenRead(@"../../../Rounds/RoundE10.dat"));
            //reader = new BinaryReader(File.OpenRead(@"../../../Rounds/Round9.dat"));

            // Try block
            try
            {
                    maxOnScreen = reader.ReadInt32();
                    // While loop that will run until the end of the file
                    while (reader.PeekChar() != -1)
                    {
                        // Read in information from the round file
                        string type = reader.ReadString();
                        int x = reader.ReadInt32();
                        int y = reader.ReadInt32();
                        if (type == "B") bossRound = true;
                        Vector2 ePos = new Vector2(x, y);

                        ePositions.Add(ePos);
                        eTypes.Add(type);
                    }
                    reader.Close();
                }
                // Catch Exceptions
                catch (Exception ex) { }
                reader.Close();
                objects.Clear();

            if (bossRound == false)
            {
                Random rgenItem = new Random();

                for (int i = 0; i < 10; i++)
                {
                    int x = rgen.Next((background.Width - 160) + 80) + backgroundPoint.X;
                    int y = rgen.Next((background.Height - 160) + 80) + backgroundPoint.Y;

                    Rectangle randObj = new Rectangle(0, 0, 0, 0);

                    int item = rgenItem.Next(0, 3);

                    if (item == 0)
                    {
                        randObj = new Rectangle(x, y, 75, 75);
                    }
                    else if (item == 1)
                    {
                        randObj = new Rectangle(x, y, 50, 75);
                    }
                    else if(item == 2)
                    {
                        randObj = new Rectangle(x, y, 200, 100);
                    }

                    bool collision = false;
                    if (maxOnScreen != -1)
                    {
                        foreach (Enemy e in enemiesSpawn)
                        {
                            if (randObj.Intersects(new Rectangle(e.Position.X - 20, e.Position.Y - 20, e.Position.Width + 40, e.Position.Height + 40)) == true)
                            {
                                collision = true;
                            }
                        }
                    }
                    else
                    {
                        foreach (Enemy e in enemies)
                        {
                            if (randObj.Intersects(new Rectangle(e.Position.X - 20, e.Position.Y - 20, e.Position.Width + 40, e.Position.Height + 40)) == true)
                            {
                                collision = true;
                            }
                        }
                    }

                    if (collision == true || randObj.Intersects(c.Position) == true)
                    {
                        i--;
                    }
                    else
                    {
                        bool boxCollision = false;
                        foreach (Rectangle box in objects)
                        {
                            if (randObj.Intersects(box))
                            {
                                boxCollision = true;
                            }
                        }
                        if (boxCollision == false)
                        {
                            objects.Add(randObj);
                        }
                        else
                        {
                            i--;
                        }
                    }
                }
            }

            // Center the player for the boss round
            else
            {
                c.Position = new Rectangle(GraphicsDevice.Viewport.Width / 2 - c.Position.Width / 2, GraphicsDevice.Viewport.Height / 2 - c.Position.Height / 2, c.Position.Width, c.Position.Height);
                backgroundPoint = new Point(0 - maxX / 2, 0 - maxY / 2);
                globalX = maxX / 2;
                globalY = maxY / 2;
            }

            reader.Close(); // Close the file
            round++; // Increase the round number
        }

        // Method for moving the screen
        public void ScreenMove(string s, int amount)
        {
            if (s == "up")
            {
                globalY -= amount;
                foreach (Enemy e in enemies)
                {
                    e.FPosY += amount;
                }
                foreach (Projectile p in projectiles)
                {
                    p.FPosY += amount;
                }
                foreach (EnemyProjectile eP in eProjectiles)
                {
                    eP.FPosY += amount;
                }
                for (int i = 0; i < objects.Count; i++)
                {
                    objects[i] = new Rectangle(objects[i].X, objects[i].Y + amount, objects[i].Width, objects[i].Height);
                }
                foreach (Powerup pUp in powerups)
                {
                    pUp.Position = new Rectangle(pUp.Position.X, pUp.Position.Y + amount, pUp.Position.Width, pUp.Position.Height);
                }
                backgroundPoint = new Point(backgroundPoint.X, backgroundPoint.Y + amount);
            }

            if (s == "down")
            {
                globalY += amount;
                foreach (Enemy e in enemies)
                {
                    e.FPosY -= amount;
                }
                foreach (Projectile p in projectiles)
                {
                    p.FPosY -= amount;
                }
                foreach (EnemyProjectile eP in eProjectiles)
                {
                    eP.FPosY -= amount;
                }
                for (int i = 0; i < objects.Count; i++)
                {
                    objects[i] = new Rectangle(objects[i].X, objects[i].Y - amount, objects[i].Width, objects[i].Height);
                }
                foreach (Powerup pUp in powerups)
                {
                    pUp.Position = new Rectangle(pUp.Position.X, pUp.Position.Y - amount, pUp.Position.Width, pUp.Position.Height);
                }
                backgroundPoint = new Point(backgroundPoint.X, backgroundPoint.Y - amount);
            }

            if (s == "left")
            {
                globalX -= amount;
                foreach (Enemy e in enemies)
                {
                    e.FPosX += amount;
                }
                foreach (Projectile p in projectiles)
                {
                    p.FPosX += amount;
                }
                foreach (EnemyProjectile eP in eProjectiles)
                {
                    eP.FPosX += amount;
                }
                for (int i = 0; i < objects.Count; i++)
                {
                    objects[i] = new Rectangle(objects[i].X + amount, objects[i].Y, objects[i].Width, objects[i].Height);
                }
                foreach (Powerup pUp in powerups)
                {
                    pUp.Position = new Rectangle(pUp.Position.X + amount, pUp.Position.Y, pUp.Position.Width, pUp.Position.Height);
                }
                backgroundPoint = new Point(backgroundPoint.X + amount, backgroundPoint.Y);
            }

            if (s == "right")
            {
                globalX += amount;
                foreach (Enemy e in enemies)
                {
                    e.FPosX -= amount;
                }
                foreach (Projectile p in projectiles)
                {
                    p.FPosX -= amount;
                }
                foreach (EnemyProjectile eP in eProjectiles)
                {
                    eP.FPosX -= amount;
                }
                for (int i = 0; i < objects.Count; i++)
                {
                    objects[i] = new Rectangle(objects[i].X - amount, objects[i].Y, objects[i].Width, objects[i].Height);
                }
                foreach (Powerup pUp in powerups)
                {
                    pUp.Position = new Rectangle(pUp.Position.X - amount, pUp.Position.Y, pUp.Position.Width, pUp.Position.Height);
                }
                backgroundPoint = new Point(backgroundPoint.X - amount, backgroundPoint.Y);
            }
            if (globalX > maxX) globalX = maxX;
            if (globalX < 0) globalX = 0;
            if (globalY > maxY) globalY = maxY;
            if (globalY < 0) globalY = 0;
        }

        // Method for the player moving
        public void PlayerMove()
        {
            bool collides = false;
            // Move the character based on user input
            if (kbState.IsKeyDown(Keys.W))
            {
                foreach (Rectangle r in objects)
                {
                    if (new Rectangle(c.Position.X, c.Position.Y - c.Speed, c.Position.Width, c.Position.Height).Intersects(r) == true)
                    {
                        collides = true;
                    }
                }
                if ((c.Position.Y > 100 && globalY >= 0) || (globalY <= 0 && c.Position.Y > 0))
                {
                    if (collides == false)
                    {
                        c.Position = new Rectangle(c.Position.X, c.Position.Y - c.Speed, c.Position.Width, c.Position.Height);
                        c.CRect = new Rectangle(c.Position.X + 15, c.Position.Y + 15, c.CRect.Width, c.CRect.Height);
                    }
                }
                else if (c.Position.Y > 0 && collides == false) ScreenMove("up", c.Speed);
            }
            if (kbState.IsKeyDown(Keys.A))
            {
                foreach (Rectangle r in objects)
                {
                    if (new Rectangle(c.Position.X - c.Speed, c.Position.Y, c.Position.Width, c.Position.Height).Intersects(r) == true)
                    {
                        collides = true;
                    }
                }
                if ((c.Position.X > 100 && globalX >= 0) || (globalX <= 0 && c.Position.X > 0))
                {
                    if (collides == false)
                    {
                        c.Position = new Rectangle(c.Position.X - c.Speed, c.Position.Y, c.Position.Width, c.Position.Height);
                        c.CRect = new Rectangle(c.Position.X + 15, c.Position.Y + 15, c.CRect.Width, c.CRect.Height);
                    }
                }
                else if (c.Position.X > 0 && collides == false) ScreenMove("left", c.Speed);
            }
            if (kbState.IsKeyDown(Keys.S))
            {
                foreach (Rectangle r in objects)
                {
                    if (new Rectangle(c.Position.X, c.Position.Y + c.Speed, c.Position.Width, c.Position.Height).Intersects(r) == true)
                    {
                        collides = true;
                    }
                }
                if ((c.Position.Y < GraphicsDevice.Viewport.Height - (100 + c.Position.Height) && globalY <= maxY) || (globalY == maxY && c.Position.Y < GraphicsDevice.Viewport.Height - c.Position.Height))
                {
                    if (collides == false)
                    {
                        c.Position = new Rectangle(c.Position.X, c.Position.Y + c.Speed, c.Position.Width, c.Position.Height);
                        c.CRect = new Rectangle(c.Position.X + 15, c.Position.Y + 15, c.CRect.Width, c.CRect.Height);
                    }
                }
                else if (c.Position.Y < GraphicsDevice.Viewport.Height - c.Position.Height && collides == false) ScreenMove("down", c.Speed);
            }
            if (kbState.IsKeyDown(Keys.D))
            {
                foreach (Rectangle r in objects)
                {
                    if (new Rectangle(c.Position.X + c.Speed, c.Position.Y, c.Position.Width, c.Position.Height).Intersects(r) == true)
                    {
                        collides = true;
                    }
                }
                if ((c.Position.X < GraphicsDevice.Viewport.Width - (100 + c.Position.Width) && globalX <= maxX) || (globalX == maxX && c.Position.X < GraphicsDevice.Viewport.Width - c.Position.Width))
                {
                    if (collides == false)
                    {
                        c.Position = new Rectangle(c.Position.X + c.Speed, c.Position.Y, c.Position.Width, c.Position.Height);
                        c.CRect = new Rectangle(c.Position.X + 15, c.Position.Y + 15, c.CRect.Width, c.CRect.Height);
                    }
                }
                else if (c.Position.X < GraphicsDevice.Viewport.Width - c.Position.Width && collides == false) ScreenMove("right", c.Speed);
            }
        }

        // Method for changing the ability the player is using
        public void PlayerChangeAbility()
        {
            // Switch based on aState
            switch (aState)
            {
                // If E is pressed, increase the ability by one. If Q is pressed, decrease the ability by one
                case AbilityState.a1:
                    if (kbState.IsKeyDown(Keys.E) && previousKbState.IsKeyUp(Keys.E)) aState = AbilityState.a2;
                    if (kbState.IsKeyDown(Keys.Q) && previousKbState.IsKeyUp(Keys.Q)) aState = AbilityState.a5;
                    break;

                case AbilityState.a2:
                    if (kbState.IsKeyDown(Keys.E) && previousKbState.IsKeyUp(Keys.E)) aState = AbilityState.a3;
                    if (kbState.IsKeyDown(Keys.Q) && previousKbState.IsKeyUp(Keys.Q)) aState = AbilityState.a1;
                    break;

                case AbilityState.a3:
                    if (kbState.IsKeyDown(Keys.E) && previousKbState.IsKeyUp(Keys.E)) aState = AbilityState.a4;
                    if (kbState.IsKeyDown(Keys.Q) && previousKbState.IsKeyUp(Keys.Q)) aState = AbilityState.a2;
                    break;

                case AbilityState.a4:
                    if (kbState.IsKeyDown(Keys.E) && previousKbState.IsKeyUp(Keys.E)) aState = AbilityState.a5;
                    if (kbState.IsKeyDown(Keys.Q) && previousKbState.IsKeyUp(Keys.Q)) aState = AbilityState.a3;
                    break;

                case AbilityState.a5:
                    if (kbState.IsKeyDown(Keys.E) && previousKbState.IsKeyUp(Keys.E)) aState = AbilityState.a1;
                    if (kbState.IsKeyDown(Keys.Q) && previousKbState.IsKeyUp(Keys.Q)) aState = AbilityState.a4;
                    break;
            }
        }

        // Method for the player shooting
        public void PlayerShoot()
        {
            // If player is clicking and they have no ShotDelay they will fire a projectile
            if (mState.LeftButton == ButtonState.Pressed && c.ShotDelay == 0)
            {
                switch (aState)
                {
                    // Test of a melee attack
                    case AbilityState.a1:
                        projectiles.Add(new PStationary(12, 60, 60, c, rotationAngle, 15, true, meleeImage)); // Create a new projectile that will travel in the direction of the mouse
                        c.ShotDelay = 40; // Set the ShotDelay of the player. We can change this value depending on ability, stronger attacks have longer delays
                        break;

                    // The original shooting attack
                    case AbilityState.a2:
                        if (c.Energy >= 2)
                        {
                            projectiles.Add(new PBasic(25, 45, 45, c, rotationAngle, 100, false, bulletImage));
                            c.Energy -= 2;
                            c.ShotDelay = 20;
                            //shootSound.Play();
                        }
                        break;

                    // Test of a rapid fire attack
                    case AbilityState.a3:
                        if (c.Energy >= 1)
                        {
                            projectiles.Add(new PBasic(15, 28, 28, c, rotationAngle, 90, false, bulletImage));
                            c.Energy -= 1;
                            c.ShotDelay = 3;
                           //shootSound.Play();
                        }
                        break;

                    case AbilityState.a4:
                        if (c.Energy >= 10)
                        {
                            projectiles.Add(new PExplosive(150, 30, 30, c, rotationAngle, 90, grenade));
                            c.Energy -= 10;
                            c.ShotDelay = 120;
                            //shootSound.Play();
                        }
                        break;
                    case AbilityState.a5:
                        if (c.Energy >= 5)
                        {
                            projectiles.Add(new PMine(120, 30, 30, c, 0, 0, false, mine));
                            c.Energy -= 5;
                            c.ShotDelay = 100;
                            //shootSound.Play();
                        }
                        break;
                }
            }
        }

        // Method for using the SuperMove
        public void SuperMove()
        {
            if (c.SuperCount < 60)
            {
                c.SuperCount++;
            }
            if (c.SuperCount == 60 && mState.LeftButton == ButtonState.Pressed)
            {
                projectiles.Add(new PSuper(30, 300, 300, c, rotationAngle, 2000, true, bulletImage));
                c.ShotDelay = 60;
                c.Super = 0;
                c.SuperCount = 0;
                c.FiringSuper = false;
            }
        }

        // Method for the player dashing
        public void PlayerDash()
        {
            if (mState.RightButton == ButtonState.Pressed && c.Dashing == 0)
            {
                if (c.Energy >= 10)
                {
                    if (kbState.IsKeyDown(Keys.W) && kbState.IsKeyDown(Keys.D))
                    {
                        c.Energy -= 10;
                        c.Dashing = 2;
                    }
                    else if (kbState.IsKeyDown(Keys.W) && kbState.IsKeyDown(Keys.A))
                    {
                        c.Energy -= 10;
                        c.Dashing = 8;
                    }
                    else if (kbState.IsKeyDown(Keys.S) && kbState.IsKeyDown(Keys.D))
                    {
                        c.Energy -= 10;
                        c.Dashing = 4;
                    }
                    else if (kbState.IsKeyDown(Keys.S) && kbState.IsKeyDown(Keys.A))
                    {
                        c.Energy -= 10;
                        c.Dashing = 6;
                    }
                    else if (kbState.IsKeyDown(Keys.W))
                    {
                        c.Energy -= 10;
                        c.Dashing = 1;
                    }
                    else if (kbState.IsKeyDown(Keys.D))
                    {
                        c.Energy -= 10;
                        c.Dashing = 3;
                    }
                    else if (kbState.IsKeyDown(Keys.S))
                    {
                        c.Energy -= 10;
                        c.Dashing = 5;
                    }
                    else if (kbState.IsKeyDown(Keys.A))
                    {
                        c.Energy -= 10;
                        c.Dashing = 7;
                    }
                }
            }
        }

        // Method for reseting the game
        public void ResetGame()
        {
            
            enemies.Clear();
            enemiesSpawn.Clear();
            projectiles.Clear();
            c.Position = new Rectangle(GraphicsDevice.Viewport.Width / 2 - c.Position.Width / 2, GraphicsDevice.Viewport.Height / 2 - c.Position.Height / 2, c.Position.Width, c.Position.Height);
            maxX = background.Width - GraphicsDevice.Viewport.Width;
            if (maxX < 0) maxX = 0;
            maxY = background.Height - GraphicsDevice.Viewport.Height;
            if (maxY < 0) maxY = 0;
            backgroundPoint = new Point(0 - maxX/2, 0 - maxY/2);
            globalX = maxX/2;
            globalY = maxY/2;
            c.Health = 100;
            c.Energy = 200;
            c.Super = 0;
            c.SuperCount = 0;
            round = 0;
            score = 0;
            aState = AbilityState.a1;
            powerupTimer = 900;
            skulls = 0;
            AdvanceRound();
            Bcont = true;
        }

        public void ScoreEntry()
        {

            Keys[] pressedKeys = kbState.GetPressedKeys();

        }

        // Override the game exiting to create a config file upon exiting that will save user settings
        protected override void OnExiting(object sender, EventArgs args)
        {
            StreamWriter configWriter = new StreamWriter(File.OpenWrite(@"../../../Config.txt"));
            configWriter.WriteLine("fullscreen = " + fullscreen);
            configWriter.Close();

            base.OnExiting(sender, args);
        }

        // Game Constructor
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // Adjust window size
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 740;
            graphics.ApplyChanges();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            // Create a character1 in the center of the screen
            c = new Character1(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);

            // Read the config file
            try
            {
                StreamReader sr = new StreamReader(File.OpenRead(@"../../../Config.txt"));
                string line;
                while((line = sr.ReadLine()) != null)
                {
                    if (line == "fullscreen = False")
                        fullscreen = false;

                    if (line == "fullscreen = True")
                        fullscreen = true;
                }
                sr.Close();
            }
            catch(Exception ex)
            {
                Console.Write(ex);
            }

            // Read the leaderboard file
            try
            {
                BinaryReader leaderboardReader = new BinaryReader(File.OpenRead(@"../../../Leaderboard.dat"));
                while (leaderboardReader.PeekChar() != -1)
                {
                    leaderboardNames.Add(leaderboardReader.ReadString());
                    leaderboardCharacters.Add(leaderboardReader.ReadString());
                    leaderboardRounds.Add(leaderboardReader.ReadInt32());
                    leaderboardScores.Add(leaderboardReader.ReadInt32());
                }
                leaderboardReader.Close();
            }
            catch (Exception ex)
            {
                // If there is no leaderboard file, create a blank one
                if (ex is FileNotFoundException)
                {
                    BinaryWriter leaderboardWriter = new BinaryWriter(File.OpenWrite(@"../../../Leaderboard.dat"));
                    for (int i = 0; i < 5; i++)
                    {
                        leaderboardWriter.Write("");
                        leaderboardWriter.Write("");
                        leaderboardWriter.Write(0);
                        leaderboardWriter.Write(0);
                        leaderboardNames.Add("");
                        leaderboardCharacters.Add("");
                        leaderboardRounds.Add(0);
                        leaderboardScores.Add(0);
                    }
                    leaderboardWriter.Close();
                }
            }

            graphics.IsFullScreen = fullscreen;
            gState = GameState.Menu;

            objects = new List<Rectangle>();

            // random generator
            Random rgen = new Random();

            // creates 5 combinations of x and y coordinates and adds that box to the list
            for (int i = 0; i < 10; i++)
            {
                int x = rgen.Next(GraphicsDevice.Viewport.Width - 160) + 80;
                int y = rgen.Next(GraphicsDevice.Viewport.Height - 160) + 80;

                Rectangle randObj = new Rectangle(x, y, 75, 75);
                objects.Add(randObj);
            }

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load images for start and options screens
            startButton = this.Content.Load<Texture2D>("Start");
            optionsButton = this.Content.Load<Texture2D>("Options");
            fullscreenButton = this.Content.Load<Texture2D>("Fullscreen");
            okButton = this.Content.Load<Texture2D>("Ok");
            cancelButton = this.Content.Load<Texture2D>("Cancel");
            leaderboardButton = Content.Load<Texture2D>("Leaderboard");
            instructionsButton = Content.Load<Texture2D>("Instructions");
            instructionsScreen = Content.Load<Texture2D>("InstructionsScreen");
            instructionsScreen2 = Content.Load<Texture2D>("InstructionsScreen2");
            instructionsScreen3 = Content.Load<Texture2D>("InstructionsScreen3");
            instructionsScreen4 = Content.Load<Texture2D>("InstructionsScreen4");
            title = this.Content.Load<Texture2D>("Title Font");
            titleBackground = this.Content.Load<Texture2D>("Title background");

            // Load images for the game
            enemyImage = this.Content.Load<Texture2D>("Enemy");
            player1Image = this.Content.Load<Texture2D>("Fire Still");
            player1Walking = this.Content.Load<Texture2D>("Fire Move");
            bullet1Image = this.Content.Load<Texture2D>("Fire Bullet");
            player2Image = this.Content.Load<Texture2D>("Earth Still");
            player2Walking = this.Content.Load<Texture2D>("Earth Move");
            bullet2Image = this.Content.Load<Texture2D>("Earth Bullet");
            player3Image = this.Content.Load<Texture2D>("Electric Still");
            player3Walking = this.Content.Load<Texture2D>("Electric Move");
            bullet3Image = this.Content.Load<Texture2D>("Electric Bullet");
            player4Image = this.Content.Load<Texture2D>("Water Still");
            player4Walking = this.Content.Load<Texture2D>("Water Move");
            bullet4Image = this.Content.Load<Texture2D>("Water Bullet");
            meleeImage = this.Content.Load<Texture2D>("Melee");
            whiteBox = this.Content.Load<Texture2D>("whiteSquare");
            paused = this.Content.Load<Texture2D>("Pause");
            menu = this.Content.Load<Texture2D>("menuButton");
            nextB = this.Content.Load<Texture2D>("nextButton");
            exitB = this.Content.Load<Texture2D>("exitButton");
            hudrectangle = this.Content.Load<Texture2D>("WhiteRectangle");
            hudcircle = this.Content.Load<Texture2D>("WhiteCircle");
            continueButton = this.Content.Load<Texture2D>("continueButton");
            rectangle = this.Content.Load<Texture2D>("WhiteRectangle");
            circle = this.Content.Load<Texture2D>("WhiteCircle");
            fireButton = this.Content.Load<Texture2D>("fireButton");
            earthButton = this.Content.Load<Texture2D>("earthButton");
            electricButton = this.Content.Load<Texture2D>("lightningButton");
            waterButton = this.Content.Load<Texture2D>("waterButton");
            dot = this.Content.Load<Texture2D>("Dot");
            basicImage = Content.Load<Texture2D>("BasicProjectile");
            stallImage = Content.Load<Texture2D>("StallProjectile");
            accelerateImage = Content.Load<Texture2D>("AccelerateProjectile");
            wobbleImage = Content.Load<Texture2D>("WobbleProjectile");
            eMarker = Content.Load<Texture2D>("EnemyMarker");
            rMarker = Content.Load<Texture2D>("ReaperMarker");
            superCharge = Content.Load<Texture2D>("EnemyMarker");
            mine = Content.Load<Texture2D>("mine");
            grenade = Content.Load<Texture2D>("grenade");
            explosion = Content.Load<Texture2D>("Explosion");
            boss = Content.Load<Texture2D>("boss");
            meleeStillImage = Content.Load<Texture2D>("meleeImage");
            roundMarker = Content.Load<Texture2D>("RoundMarker");
            powerupMarker = Content.Load<Texture2D>("PowerupMarker");
            reaper = Content.Load<Texture2D>("Reaper");

            // Load fonts
            sFont = Content.Load<SpriteFont>("SpriteFont1");
            lFont = Content.Load<SpriteFont>("8bitoperator32");
            rFont = Content.Load<SpriteFont>("RoundChangeFont");

            // loads background
            background = this.Content.Load<Texture2D>("background");
            leaderboardBackground = this.Content.Load<Texture2D>("LeaderboardBackground");

            // loads items
            boxes = this.Content.Load<Texture2D>("boxes");
            garbage = this.Content.Load<Texture2D>("garbage");
            car = this.Content.Load<Texture2D>("car");

            // loads powerups
            medkit = Content.Load<Texture2D>("medkit");
            points = Content.Load<Texture2D>("points");
            energy = Content.Load<Texture2D>("energy");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            previousKbState = kbState; // Set previous keyboard state
            kbState = Keyboard.GetState(); // Get the keyboard state
            prevMState = mState; // Set previous mouse state
            mState = Mouse.GetState(); // Get the mouse state

            // switch for states
            switch (heroState)
            {
                case HeroState.Still:
                    if (kbState.IsKeyDown(Keys.W) || kbState.IsKeyDown(Keys.A) || kbState.IsKeyDown(Keys.S) || kbState.IsKeyDown(Keys.D))
                        heroState = HeroState.Walking;
                    break;

                case HeroState.Walking:
                    if (kbState.IsKeyUp(Keys.W) && kbState.IsKeyUp(Keys.A) && kbState.IsKeyUp(Keys.S) && kbState.IsKeyUp(Keys.D))
                        heroState = HeroState.Still;
                    break;
            }

            // sets number of frames and what sprite is being used
            if (heroState == HeroState.Walking)
            {
                c.Image = playerWalking;
                numFramesPlayer = 8;
            }
            else if (heroState == HeroState.Still)
            {
                c.Image = playerImage;
                numFramesPlayer = 3;
            }

            // which frame to draw
            framesElapsedPlayer = (int)(gameTime.TotalGameTime.TotalMilliseconds / timePerFrame);
            framePlayer = framesElapsedPlayer % numFramesPlayer;

            framesElapsedProjectile = (int)(gameTime.TotalGameTime.TotalMilliseconds / timePerFrame);
            frameProjectile = framesElapsedProjectile % numFramesProjectile;

            framesElapsedEnemy = (int)(gameTime.TotalGameTime.TotalMilliseconds / timePerFrame);
            frameEnemy = framesElapsedEnemy % numFramesEnemy;

            // Switch statement based on gState
            switch (gState)
            {
                // Game is in Menu
                case GameState.Menu:

                    // makes the continue button not visible
                    Bcont = false;

                    // Checks to see if the start button has been pressed

                    rSButton = new Rectangle((GraphicsDevice.Viewport.Width / 2) - (rSButton.Width / 2), (GraphicsDevice.Viewport.Height / 2) + (int)(rSButton.Height * 1.5), startButton.Width / 4, startButton.Height / 4);
                    rOButton = new Rectangle((GraphicsDevice.Viewport.Width / 5) - (rOButton.Width / 2), (GraphicsDevice.Viewport.Height / 2) + (int)(rOButton.Height * 3), startButton.Width / 4, startButton.Height / 4);
                    rLButton = new Rectangle((GraphicsDevice.Viewport.Width / 5) * 2 - (rLButton.Width / 2), (GraphicsDevice.Viewport.Height / 2) + (int)(rLButton.Height * 3), startButton.Width / 4, startButton.Height / 4);
                    rIButton = new Rectangle((GraphicsDevice.Viewport.Width / 5) * 3 - (rIButton.Width / 2), (GraphicsDevice.Viewport.Height / 2) + (int)(rIButton.Height * 3), startButton.Width / 4, startButton.Height / 4);
                    rEButton = new Rectangle((GraphicsDevice.Viewport.Width / 5) * 4 - (rEButton.Width / 2), (GraphicsDevice.Viewport.Height / 2) + (int)(rEButton.Height * 3), startButton.Width / 4, startButton.Height / 4);

                    Rectangle mRectangle = new Rectangle(mState.Position.X, mState.Position.Y, 1, 1);
                    if (mState.LeftButton == ButtonState.Pressed && prevMState.LeftButton != ButtonState.Pressed && mRectangle.Intersects(rSButton))
                    {
                        ResetGame();
                        gState = GameState.CharacterSelection;
                    }
                    if (mState.LeftButton == ButtonState.Pressed && prevMState.LeftButton != ButtonState.Pressed && mRectangle.Intersects(rOButton))
                    {
                        gState = GameState.Options;
                    }
                    if (mState.LeftButton == ButtonState.Pressed && prevMState.LeftButton != ButtonState.Pressed && mRectangle.Intersects(rLButton))
                    {
                        gState = GameState.Leaderboard;
                    }
                    if (mState.LeftButton == ButtonState.Pressed && prevMState.LeftButton != ButtonState.Pressed && mRectangle.Intersects(rIButton))
                    {
                        gState = GameState.Instructions;
                    }
                    if (mState.LeftButton == ButtonState.Pressed && prevMState.LeftButton != ButtonState.Pressed && mRectangle.Intersects(rEButton))
                    {
                        Exit();
                    }
                    break;

                // Game is in Character selection mode
                case GameState.CharacterSelection:

                    // Checks to see which of the character buttons had been pressed
                    char1 = new Rectangle((GraphicsDevice.Viewport.Width / 5) - (rSButton.Width/2), (GraphicsDevice.Viewport.Height) - (rSButton.Height) * 3, startButton.Width / 4, startButton.Height / 4);
                    char2 = new Rectangle((GraphicsDevice.Viewport.Width / 5)*2 - (rSButton.Width/2), (GraphicsDevice.Viewport.Height) - (rSButton.Height) * 3, startButton.Width / 4, startButton.Height / 4);
                    char3 = new Rectangle((GraphicsDevice.Viewport.Width / 5)*3 - (rSButton.Width/2), (GraphicsDevice.Viewport.Height) - (rSButton.Height) * 3, startButton.Width / 4, startButton.Height / 4);
                    char4 = new Rectangle((GraphicsDevice.Viewport.Width / 5)*4 - (rSButton.Width/2), (GraphicsDevice.Viewport.Height) - (rSButton.Height) * 3, startButton.Width / 4, startButton.Height / 4);

                    mRectangle = new Rectangle(mState.Position.X, mState.Position.Y, 1, 1);

                    if (mState.LeftButton == ButtonState.Pressed && prevMState.LeftButton != ButtonState.Pressed && mRectangle.Intersects(char1))
                    {
                        switchHero = SwitchHero.Fire;
                        playerImage = player1Image;
                        playerWalking = player1Walking;
                        bulletImage = bullet1Image;
                        shootSound = fireSound;
                        ResetGame();
                        gState = GameState.HordeMode;
                    }

                    if (mState.LeftButton == ButtonState.Pressed && prevMState.LeftButton != ButtonState.Pressed && mRectangle.Intersects(char2))
                    {
                        switchHero = SwitchHero.Earth;
                        playerImage = player2Image;
                        playerWalking = player2Walking;
                        bulletImage = bullet2Image;
                        shootSound = earthSound;
                        ResetGame();
                        gState = GameState.HordeMode;
                    }

                    if (mState.LeftButton == ButtonState.Pressed && prevMState.LeftButton != ButtonState.Pressed && mRectangle.Intersects(char3))
                    {
                        switchHero = SwitchHero.Water;
                        playerImage = player4Image;
                        playerWalking = player4Walking;
                        bulletImage = bullet4Image;
                        shootSound = waterSound;
                        ResetGame();
                        gState = GameState.HordeMode;
                    }

                    if (mState.LeftButton == ButtonState.Pressed && prevMState.LeftButton != ButtonState.Pressed && mRectangle.Intersects(char4))
                    {
                        switchHero = SwitchHero.Electric;
                        playerImage = player3Image;
                        playerWalking = player3Walking;
                        bulletImage = bullet3Image;
                        shootSound = electricSound;
                        ResetGame();
                        gState = GameState.HordeMode;
                    }

                    heroState = HeroState.Walking;

                    if (heroState == HeroState.Walking)
                    {
                        c.Image = playerWalking;
                        numFramesPlayer = 8;
                    }
                    else if (heroState == HeroState.Still)
                    {
                        c.Image = playerImage;
                        numFramesPlayer = 3;
                    }

                    break;

                // Game is in Horde Mode
                case GameState.HordeMode:

                    // checks to see if the player paused the game
                    kbState = Keyboard.GetState();

                    if (kbState.IsKeyDown(Keys.P) && previousKbState.IsKeyUp(Keys.P))
                    {
                        gState = GameState.Paused;
                        previousKbState = kbState;
                    }

                    // checks to see if the boxes collide with the charcater or enemies
                    foreach (Rectangle o in objects)
                    {
                        if (o.Intersects(c.Position))
                        {
                            c.Position = new Rectangle(c.Position.X, c.Position.Y, c.Position.Width, c.Position.Height);
                        }
                    }

                    foreach (Rectangle o in objects)
                    {
                        foreach (Enemy e in enemiesSpawn)
                        {
                            if (o.Intersects(e.Position))
                            {
                                e.Position = new Rectangle(e.Position.X, e.Position.Y, e.Position.Width, e.Position.Height);
                            }
                        }
                    }

                    // Find the angle between the player and the mouse, use this to rotate the player when drawing
                    int rotX = mState.X - (c.Position.X + c.Position.Width / 2);
                    int rotY = mState.Y - (c.Position.Y + c.Position.Height / 2);
                    rotationAngle = (float)Math.Atan2(rotY, rotX);

                    if(c.EnergyCounter == 20)
                    {
                        c.EnergyCounter = 0;
                        if(reaperRound == false) c.Energy++;
                        if (reaperRound == true) c.Energy += 3;
                        if (c.Energy > 200) c.Energy = 200;
                    }
                    else
                    {
                        c.EnergyCounter++;
                    }

                    PlayerDash();
                    if (c.Dashing == 0)
                    {
                        PlayerMove();
                        PlayerChangeAbility();
                        if (c.FiringSuper == false) PlayerShoot();
                        if (c.FiringSuper == true)
                        {
                            SuperMove();
                        }
                        if (c.Super == 100 && kbState.IsKeyDown(Keys.Space) && c.FiringSuper == false)
                        {
                            c.FiringSuper = true;
                            SuperMove();
                        }
                        /*if (c.Super == 100 && kbState.IsKeyDown(Keys.Space) && c.FiringSuper == true)
                        {
                            c.FiringSuper = false;
                            SuperMove();
                        }*/
                    }
                    if (c.DashCount == 30)
                    {
                        c.Dashing = 0;
                        c.DashCount = 0;
                    }
                    else
                    {
                        bool collides1 = false;
                        bool collides2 = false;
                        switch (c.Dashing)
                        {
                            case 1:
                                c.DashCount++;
                                foreach (Rectangle r in objects)
                                {
                                    if (new Rectangle(c.Position.X, (int)(c.Position.Y - c.Speed * 2.5), c.Position.Width, c.Position.Height).Intersects(r)) collides1 = true;
                                }
                                if ((c.Position.Y > 100 && globalY >= 0) || (globalY <= 0 && c.Position.Y > 0))
                                {
                                    if (collides1 == false) c.Position = new Rectangle(c.Position.X, (int)(c.Position.Y - c.Speed * 2.5), c.Position.Width, c.Position.Height);
                                }

                                else if (c.Position.Y > 0 && collides1 == false) ScreenMove("up", (int)(c.Speed * 2.5));
                                break;
                            case 2:
                                c.DashCount++;
                                foreach (Rectangle r in objects)
                                {
                                    if (new Rectangle((int)(c.Position.X + c.Speed * 1.77), c.Position.Y, c.Position.Width, c.Position.Height).Intersects(r)) collides1 = true;
                                    if (new Rectangle(c.Position.X, (int)(c.Position.Y - c.Speed * 1.77), c.Position.Width, c.Position.Height).Intersects(r)) collides2 = true;
                                }
                                if ((c.Position.X < GraphicsDevice.Viewport.Width - (100 + c.Position.Width) && globalX <= maxX) || (globalX == maxX && c.Position.X < GraphicsDevice.Viewport.Width - c.Position.Width))
                                {
                                    if (collides1 == false) c.Position = new Rectangle((int)(c.Position.X + c.Speed * 1.77), c.Position.Y, c.Position.Width, c.Position.Height);
                                }
                                else if (c.Position.X < GraphicsDevice.Viewport.Width - c.Position.Width && collides1 == false) ScreenMove("right", (int)(c.Speed * 1.77));
                                if ((c.Position.Y > 100 && globalY >= 0) || (globalY <= 0 && c.Position.Y > 0))
                                {
                                    if (collides2 == false) c.Position = new Rectangle(c.Position.X, (int)(c.Position.Y - c.Speed * 1.77), c.Position.Width, c.Position.Height);
                                }
                                else if (c.Position.Y > 0 && collides2 == false) ScreenMove("up", (int)(c.Speed * 1.77));
                                break;
                            case 3:
                                c.DashCount++;
                                foreach (Rectangle r in objects)
                                {
                                    if (new Rectangle((int)(c.Position.X + c.Speed * 2.5), c.Position.Y, c.Position.Width, c.Position.Height).Intersects(r)) collides1 = true;
                                }
                                if ((c.Position.X < GraphicsDevice.Viewport.Width - (100 + c.Position.Width) && globalX <= maxX) || (globalX == maxX && c.Position.X < GraphicsDevice.Viewport.Width - c.Position.Width))
                                {
                                    if (collides1 == false) c.Position = new Rectangle((int)(c.Position.X + c.Speed * 2.5), c.Position.Y, c.Position.Width, c.Position.Height);
                                }

                                else if (c.Position.X < GraphicsDevice.Viewport.Width - c.Position.Width && collides1 == false) ScreenMove("right", (int)(c.Speed * 2.5));
                                break;
                            case 4:
                                c.DashCount++;
                                foreach (Rectangle r in objects)
                                {
                                    if (new Rectangle((int)(c.Position.X + c.Speed * 1.77), c.Position.Y, c.Position.Width, c.Position.Height).Intersects(r)) collides1 = true;
                                    if (new Rectangle(c.Position.X, (int)(c.Position.Y + c.Speed * 1.77), c.Position.Width, c.Position.Height).Intersects(r)) collides2 = true;
                                }
                                if ((c.Position.X < GraphicsDevice.Viewport.Width - (100 + c.Position.Width) && globalX <= maxX) || (globalX == maxX && c.Position.X < GraphicsDevice.Viewport.Width - c.Position.Width))
                                {
                                    if (collides1 == false) c.Position = new Rectangle((int)(c.Position.X + c.Speed * 1.77), c.Position.Y, c.Position.Width, c.Position.Height);
                                }
                                else if (c.Position.X < GraphicsDevice.Viewport.Width - c.Position.Width && collides1 == false) ScreenMove("right", (int)(c.Speed * 1.77));
                                if ((c.Position.Y < GraphicsDevice.Viewport.Height - (100 + c.Position.Height) && globalY <= maxY) || (globalY == maxY && c.Position.Y < GraphicsDevice.Viewport.Height - c.Position.Height))
                                {
                                    if (collides2 == false) c.Position = new Rectangle(c.Position.X, (int)(c.Position.Y + c.Speed * 1.77), c.Position.Width, c.Position.Height);
                                }
                                else if (c.Position.Y < GraphicsDevice.Viewport.Height - c.Position.Height && collides2 == false) ScreenMove("down", (int)(c.Speed * 1.77));
                                break;
                            case 5:
                                c.DashCount++;
                                foreach (Rectangle r in objects)
                                {
                                    if (new Rectangle(c.Position.X, (int)(c.Position.Y + c.Speed * 2.5), c.Position.Width, c.Position.Height).Intersects(r)) collides1 = true;
                                }
                                if ((c.Position.Y < GraphicsDevice.Viewport.Height - (100 + c.Position.Height) && globalY <= maxY) || (globalY == maxY && c.Position.Y < GraphicsDevice.Viewport.Height - c.Position.Height))
                                {
                                    if (collides1 == false) c.Position = new Rectangle(c.Position.X, (int)(c.Position.Y + c.Speed * 2.5), c.Position.Width, c.Position.Height);
                                }
                                else if (c.Position.Y < GraphicsDevice.Viewport.Height - c.Position.Height && collides1 == false) ScreenMove("down", (int)(c.Speed * 2.5));
                                break;
                            case 6:
                                c.DashCount++;
                                foreach (Rectangle r in objects)
                                {
                                    if (new Rectangle((int)(c.Position.X - c.Speed * 1.77), c.Position.Y, c.Position.Width, c.Position.Height).Intersects(r)) collides1 = true;
                                    if (new Rectangle(c.Position.X, (int)(c.Position.Y + c.Speed * 1.77), c.Position.Width, c.Position.Height).Intersects(r)) collides2 = true;
                                }
                                if ((c.Position.X > 100 && globalX >= 0) || (globalX <= 0 && c.Position.X > 0))
                                {
                                    if (collides1 == false) c.Position = new Rectangle((int)(c.Position.X - c.Speed * 1.77), c.Position.Y, c.Position.Width, c.Position.Height);
                                }
                                else if (c.Position.X > 0 && collides1 == false) ScreenMove("left", (int)(c.Speed * 1.77));
                                if ((c.Position.Y < GraphicsDevice.Viewport.Height - (100 + c.Position.Height) && globalY <= maxY) || (globalY == maxY && c.Position.Y < GraphicsDevice.Viewport.Height - c.Position.Height))
                                {
                                    if (collides2 == false) c.Position = new Rectangle(c.Position.X, (int)(c.Position.Y + c.Speed * 1.77), c.Position.Width, c.Position.Height);
                                }
                                else if (c.Position.Y < GraphicsDevice.Viewport.Height - c.Position.Height && collides2 == false) ScreenMove("down", (int)(c.Speed * 1.77));
                                break;
                            case 7:
                                c.DashCount++;
                                foreach (Rectangle r in objects)
                                {
                                    if (new Rectangle((int)(c.Position.X - c.Speed * 2.5), c.Position.Y, c.Position.Width, c.Position.Height).Intersects(r)) collides1 = true;
                                }
                                if ((c.Position.X > 100 && globalX >= 0) || (globalX <= 0 && c.Position.X > 0))
                                {
                                    if (collides1 == false) c.Position = new Rectangle((int)(c.Position.X - c.Speed * 2.5), c.Position.Y, c.Position.Width, c.Position.Height);
                                }
                                else if (c.Position.X > 0 && collides1 == false) ScreenMove("left", (int)(c.Speed * 2.5));
                                break;
                            case 8:
                                c.DashCount++;
                                foreach (Rectangle r in objects)
                                {
                                    if (new Rectangle((int)(c.Position.X - c.Speed * 1.77), c.Position.Y, c.Position.Width, c.Position.Height).Intersects(r)) collides1 = true;
                                    if (new Rectangle(c.Position.X, (int)(c.Position.Y - c.Speed * 1.77), c.Position.Width, c.Position.Height).Intersects(r)) collides2 = true;
                                }
                                if ((c.Position.X > 100 && globalX >= 0) || (globalX <= 0 && c.Position.X > 0))
                                {
                                    if (collides1 == false) c.Position = new Rectangle((int)(c.Position.X - c.Speed * 1.77), c.Position.Y, c.Position.Width, c.Position.Height);
                                }
                                else if (c.Position.X > 0 && collides1 == false) ScreenMove("left", (int)(c.Speed * 1.77));
                                if ((c.Position.Y > 100 && globalY >= 0) || (globalY <= 0 && c.Position.Y > 0))
                                {
                                    if (collides2 == false) c.Position = new Rectangle(c.Position.X, (int)(c.Position.Y - c.Speed * 1.77), c.Position.Width, c.Position.Height);
                                }
                                else if (c.Position.Y > 0 && collides2 == false) ScreenMove("up", (int)(c.Speed * 1.77));
                                break;
                        }
                    }
                    c.CRect = new Rectangle(c.Position.X + 15, c.Position.Y + 15, c.Position.Width - 30, c.Position.Height - 30);

                    // Removing shot delay
                    if (c.ShotDelay > 0)
                    {
                        c.ShotDelay--;
                    }

                    // Move powerups
                    foreach (Powerup pUp in powerups)
                    {
                        pUp.Move();
                        if(pUp.Pickup(c))
                        {
                            powerUpPickup = pUp.Type;
                            powerUpPartial = "";
                            powerUpPartial2 = "";
                            if (pUp.Type == "Death")
                            {
                                enemyPause = 300;
                                pickupTimer = 300;
                                skulls++;
                            }
                            else pickupTimer = 210;
                            if(pUp.Type == "Max Health")
                            {
                                c.Health = 100;
                            }
                            if (pUp.Type == "Max Energy")
                            {
                                c.Energy = 200;
                            }
                            if (pUp.Type == "Bonus Points")
                            {
                                score += 1000;
                            }

                        }
                    }

                    if (pickupTimer > 0) pickupTimer--;

                    if(pickupTimer == 60 && powerUpPickup == "Death")
                    {
                        if (skulls != 3)
                        {
                            AdvanceRound();
                        }
                        else
                        {
                            enemies.Clear();
                            ePositions.Clear();
                            eTypes.Clear();
                            Enemy r = new Reaper((GraphicsDevice.Viewport.Width + maxX - globalX) / 2 - 120, (GraphicsDevice.Viewport.Height + maxY - globalY) / 2 - 120, whiteBox);
                            r.Image = reaper;
                            r.SpawnCount = 0;
                            enemies.Add(r);
                            objects.Clear();
                            skulls = 0;
                        }
                    }

                    // Spawn a powerup or decrease the time until a powerup spawns
                    if (powerupTimer == 0 && powerups.Count == 0 && reaperRound == false)
                    {
                        int pNum = rgen.Next(10) + 1;
                        string pType = "";
                        Texture2D pImg = null;
                        if(pNum == 1 || pNum == 2)
                        {
                            pType = "Max Health";
                            pImg = medkit;
                        }
                        if (pNum == 3 || pNum == 4)
                        {
                            pType = "Max Energy";
                            pImg = energy;
                        }
                        if (pNum >= 5 || pNum == 6)
                        {
                            pType = "Bonus Points";
                            pImg = points;
                        }
                        if (pNum >= 7 && pNum <= 10)
                        {
                            pType = "Death";
                            pImg = rMarker;
                        }
                        Powerup pUp = new Powerup(pType, pImg);
                        bool colliding = true;
                        while(colliding)
                        {
                            int pX = rgen.Next(0 - globalX, GraphicsDevice.Viewport.Width + maxX - globalX - 140) + 70;
                            int pY = rgen.Next(0 - globalY, GraphicsDevice.Viewport.Height + maxY - globalY - 140) + 70;
                            Rectangle pRect = new Rectangle(pX, pY, 70, 70);
                            colliding = false;
                            foreach(Rectangle obj in objects)
                            {
                                if (pRect.Intersects(obj)) colliding = true;
                            }
                            pUp.Position = pRect;
                        }
                        powerups.Add(pUp);
                        powerupTimer = 900;
                    }
                    else if (powerups.Count == 0 && powerupTimer > 0)
                    {
                        powerupTimer--;
                    }

                    try
                    {
                        // Move projectiles
                        for (int i = projectiles.Count - 1; i >= 0; i--)
                        {
                            int removing = -1;
                            if (projectiles[i] is PBasic || projectiles[i] is PExplosive || projectiles[i] is PMine || projectiles[i] is PSuper) projectiles[i].Move();
                            if (projectiles[i] is PStationary)
                            {
                                PStationary ps = (PStationary)(projectiles[i]);
                                ps.Move(c, rotationAngle);
                            }
                            projectiles[i].Count++;
                            if (projectiles[i].Count == projectiles[i].CountMax && (projectiles[i] is PExplosive) == false && (projectiles[i] is PMine) == false && (projectiles[i] is PSuper) == false)
                            {
                                removing = i;
                            }
                            if (projectiles[i].Count >= projectiles[i].CountMax && projectiles[i] is PExplosive)
                            {
                                PExplosive ex = (PExplosive)projectiles[i];
                                ex.Explode(c, enemies, projectiles, eProjectiles);
                                //explosionSound.Play();
                            }
                            if (projectiles[i] is PMine)
                            {
                                PMine mine = (PMine)projectiles[i];
                                if (mine.ExplosionCount != 0) mine.Explode(c, enemies, projectiles, eProjectiles);
                                //explosionSound.Play();
                            }
                            if (projectiles.Count != 0)
                            {
                                foreach (Rectangle r in objects)
                                {
                                    if (projectiles[i].Position.Intersects(r))
                                    {
                                        if (projectiles[i] is PExplosive)
                                        {
                                            PExplosive ex = (PExplosive)projectiles[i];
                                            ex.Collided = true;
                                            break;
                                        }
                                        else if (projectiles[i] is PSuper == false)
                                        {
                                            removing = i;
                                            break;
                                        }
                                    }
                                }
                            }

                            if (removing != -1)
                            {
                                projectiles.RemoveAt(removing);
                            }
                        }
                    }
                    catch (ArgumentOutOfRangeException)
                    { }

                    // Move Enemy projectiles and do damage
                    for (int i = eProjectiles.Count - 1; i >= 0; i--)
                    {
                        int removing = -1;
                        if (eProjectiles[i] is EPStall == false)
                        {
                            eProjectiles[i].Move();
                        }
                        if(eProjectiles[i] is EPCircle)
                        {
                            EPCircle ep = (EPCircle)eProjectiles[i];
                            ep.Move(c);
                        }
                        if (eProjectiles[i] is EPStall)
                        {
                            EPStall ep = (EPStall)eProjectiles[i];
                            if (ep.Count == 0 && ep.Moving == false)
                            {
                                int angleX = (c.Position.X + c.Position.Width / 2) - (ep.Position.X + ep.Position.Width / 2);
                                int angleY = (c.Position.Y + c.Position.Height / 2) - (ep.Position.Y + ep.Position.Height / 2);
                                float angle = (float)Math.Atan2(angleY, angleX);
                                ep.MoveX = -(float)Math.Sin(angle - Math.PI / 2) * ep.Speed;
                                ep.MoveY = (float)Math.Cos(angle - Math.PI / 2) * ep.Speed;
                                ep.Moving = true;
                            }
                            ep.Move();
                        }

                        if (eProjectiles[i].Position.Intersects(c.CRect) && (c.DashCount > 20 || c.DashCount == 0) && eProjectiles[i] is EPRectangle == false)
                        {
                            c.Health -= eProjectiles[i].Damage;
                            removing = i;
                        }

                        foreach (Rectangle r in objects)
                        {
                            if (eProjectiles[i].Position.Intersects(r))
                            {
                                removing = i;
                                break;
                            }
                        }

                        if (removing != -1)
                        {
                            eProjectiles.RemoveAt(removing);
                        }

                    }

                    bool enemyAlive = false;
                    // Foreach loop that goes through all enemy objects in the enemies list
                    if (roundChange == 0 && spawned == false)
                    {
                        if (maxOnScreen == -1) maxOnScreen = eTypes.Count;
                        //if (enemies.Count != 0)
                        //{
                                for (int i = maxOnScreen - 1; i >= 0; i--)
                                {
                                    int x = (int)ePositions[i].X;
                                    int y = (int)ePositions[i].Y;
                                    Enemy enemyspawn = null;
                                    switch (eTypes[i])
                                    {
                                        case "1":
                                            Enemy e1 = new Enemy1(c.Position.X - 500 + x, c.Position.Y - 300 + y);
                                            e1.Image = enemyImage;
                                            e1.SpawnCount = 0;
                                            enemyspawn = e1;
                                            break;

                                        case "2":
                                            Enemy e2 = new Enemy2(c.Position.X - 500 + x, c.Position.Y - 300 + y);
                                            e2.Image = enemyImage;
                                            e2.SpawnCount = 0;
                                            enemyspawn = e2;
                                            break;

                                        case "3":
                                            Enemy e3 = new Enemy3(c.Position.X - 500 + x, c.Position.Y - 300 + y);
                                            e3.Image = enemyImage;
                                            e3.SpawnCount = 0;
                                            enemyspawn = e3;
                                            break;
                                        case "4":
                                            Enemy e4 = new Enemy4(c.Position.X - 500 + x, c.Position.Y - 300 + y);
                                            e4.Image = enemyImage;
                                            e4.SpawnCount = 0;
                                            enemyspawn = e4;
                                            break;
                                        case "B":
                                            Enemy b = new Boss(c.Position.X - 500 + x, c.Position.Y - 300 + y, whiteBox);
                                            b.Image = boss;
                                            b.SpawnCount = 0;
                                            enemyspawn = b;
                                            break;
                                    }

                                    foreach (Rectangle r in objects)
                                    {
                                        while (enemyspawn.Position.Intersects(r))
                                        {
                                            x = rgen.Next(GraphicsDevice.Viewport.Width);
                                            y = rgen.Next(GraphicsDevice.Viewport.Height);
                                            enemyspawn.FPosX = x;
                                            enemyspawn.FPosY = y;
                                            enemyspawn.Position = new Rectangle((int)enemyspawn.FPosX, (int)enemyspawn.FPosY, enemyspawn.Position.Width, enemyspawn.Position.Height);
                                        }
                                    }

                                    enemies.Add(enemyspawn);
                                    eTypes.RemoveAt(i);
                                    ePositions.RemoveAt(i);
                                }
                                spawned = true;
                            //}
                    }
                    if (roundChange > 0 || enemyPause > 0)
                    {
                        enemyAlive = true;
                        if (roundChange > 0) roundChange--;
                        if (enemyPause > 0) enemyPause--;
                    }
                    else
                    {
                        for(int j = enemies.Count - 1; j >= 0; j--)
                        {
                            Enemy e = enemies[j];
                            // Move all enemies
                            e.Move(c, enemies, objects);

                            if (e is Enemy4)
                            {
                                Enemy4 e4 = (Enemy4)e;
                                if (e4.Charging == true && (e4.Position.X + globalX >= maxX + GraphicsDevice.Viewport.Width - e4.Position.Width || e4.Position.X + globalX <= 0 || e4.Position.Y + globalY >= maxY + GraphicsDevice.Viewport.Height - e4.Position.Height || e4.Position.Y + globalY <= 0))
                                {
                                    e4.Charging = false;
                                    e4.ChargeCount = 300;
                                }
                            }

                            // Reaper spawn...
                            if (e is Reaper && e.SpawnCount != -1 && e.SpawnCount <= 300)
                            {
                                // Done spawning
                                if (e.SpawnCount == 300)
                                {
                                    e.SpawnCount = -1;
                                    e.Alive = true;
                                    reaperRound = true;
                                    c.Energy = 200;
                                }
                                // Otherwise increase its spawn count
                                else
                                {
                                    if(e.SpawnCount % 2 == 0)
                                    {
                                        c.Health++;
                                        c.Energy++;
                                        if (c.Health > 100) c.Health = 100;
                                        if (c.Energy > 200) c.Energy = 200;
                                    }
                                    e.SpawnCount++;
                                    enemyAlive = true;
                                }
                            }

                            // Enemy is being spawned
                            else if (e.SpawnCount != -1 && e.SpawnCount <= 90)
                            {
                                // Enemy is done being spawned, it is now alive
                                if (e.SpawnCount == 90)
                                {
                                    e.SpawnCount = -1;
                                    e.Alive = true;
                                }
                                // Otherwise increase its spawn count
                                else
                                {
                                    e.SpawnCount++;
                                    enemyAlive = true;
                                }
                            }

                            // If the enemy is alive, it can attack, and the enemyAlive boolean is set to true
                            if (e.Alive == true)
                            {
                                enemyAlive = true;

                                // Enemy1 and Boss attack melee attack
                                if (e is Enemy1 || e is Enemy4 || e is Boss)
                                {
                                    if (e.ShotCount < 0)
                                    {
                                        e.ShotCount++;
                                    }

                                    else if (e.Position.Intersects(c.CRect))
                                    {

                                        if (e.ShotCount == 30 && e is Reaper == false)
                                        {
                                            c.Health -= 5;
                                            e.ShotCount = -60;
                                        }
                                        if (e.ShotCount == 30 && e is Reaper == true)
                                        {
                                            c.Health -= 16;
                                            e.ShotCount = -60;
                                        }
                                        e.ShotCount++;
                                    }
                                    else e.ShotCount = 0;
                                }

                                // Enemy2 attack, sniper shot at the player
                                if (e is Enemy2)
                                {
                                    Enemy2 e2 = (Enemy2)e;
                                    e2.Shoot(c);
                                    if (e2.ShotCount == 150)
                                    {
                                        int shotX = (c.Position.X + c.Position.Width / 2) - (e.Position.X + e.Position.Width / 2);
                                        int shotY = (c.Position.Y + c.Position.Height / 2) - (e.Position.Y + e.Position.Height / 2);
                                        float shotAngle = (float)Math.Atan2(shotY, shotX);
                                        eProjectiles.Add(new EPBasic(10, 30, 30, e, shotAngle, 17, basicImage));
                                        e2.ShotCount = 0;
                                    }
                                }

                                // Enemy3 attack, create five projectiles that fire at the player
                                if (e is Enemy3)
                                {
                                    Enemy3 e3 = (Enemy3)e;
                                    e3.ShotCount++;
                                    if (e3.ShotCount == 180)
                                    {
                                        int shotX = (c.Position.X + c.Position.Width / 2) - (e.Position.X + e.Position.Width / 2);
                                        int shotY = (c.Position.Y + c.Position.Height / 2) - (e.Position.Y + e.Position.Height / 2);
                                        float shotAngle = (float)Math.Atan2(shotY, shotX);
                                        eProjectiles.Add(new EPBasic(5, 35, 35, e, shotAngle, 6, basicImage));
                                        eProjectiles.Add(new EPBasic(5, 35, 35, e, shotAngle + .2f, 6, basicImage));
                                        eProjectiles.Add(new EPBasic(5, 35, 35, e, shotAngle + .4f, 6, basicImage));
                                        eProjectiles.Add(new EPBasic(5, 35, 35, e, shotAngle - .2f, 6, basicImage));
                                        eProjectiles.Add(new EPBasic(5, 35, 35, e, shotAngle - .4f, 6, basicImage));
                                        e3.ShotCount = 0;
                                    }
                                }

                                // Boss attack
                                if (e is Boss)
                                {
                                    Boss b = (Boss)e;
                                    switch (b.AttackNum)
                                    {
                                        // Attack 1, spell out DIE in stall projectiles
                                        case 1:
                                            try
                                            {
                                                // Read in the projectile pattern from the binary file and create the projectiles
                                                string attackType = "";
                                                int attackX = 0;
                                                int attackY = 0;
                                                BinaryReader attackReader = new BinaryReader(File.OpenRead(@"../../../Projectile Patterns/DIE.dat"));
                                                for (int i = 100; i >= b.AttackCount; i--)
                                                {
                                                    if (i % 2 == 0)
                                                    {
                                                        attackType = attackReader.ReadString();
                                                        attackX = attackReader.ReadInt32();
                                                        attackY = attackReader.ReadInt32();
                                                    }
                                                }
                                                eProjectiles.Add(new EPStall(5, b.Position.X - 500 + attackX, b.Position.Y - 300 + attackY, 40, 40, 10, 90, stallImage));
                                                attackReader.Close();
                                            }
                                            // Silently catch EndOfStreamExceptions
                                            catch (EndOfStreamException) { }
                                            b.AttackCount--;
                                            if (b.AttackCount == 0)
                                            {
                                                b.AttackNum = 5;
                                                b.AttackCount = 360;
                                            }
                                            break;

                                        // Attack 2, walk towards the player while spawning projectiles, slower speed smaller spread
                                        case 2:
                                            if (b.AttackCount % 15 == 0)
                                            {
                                                int shotX = (c.Position.X + c.Position.Width / 2) - (b.Position.X + b.Position.Width / 2);
                                                int shotY = (c.Position.Y + c.Position.Height / 2) - (b.Position.Y + b.Position.Height / 2);
                                                float shotAngle = (float)Math.Atan2(shotY, shotX);
                                                float modifier = (float)(rgen.Next(9) - 4) / 10;
                                                eProjectiles.Add(new EPBasic(5, 40, 40, b, shotAngle + modifier, 7, basicImage));
                                            }
                                            b.AttackCount--;
                                            if (b.AttackCount == 0)
                                            {
                                                b.AttackNum = 5;
                                                b.AttackCount = 240;
                                            }
                                            break;

                                        // Attack 3, walk towards the player while spawning projectiles, faster speed larger spread
                                        case 3:
                                            if (b.AttackCount % 10 == 0)
                                            {
                                                int shotX = (c.Position.X + c.Position.Width / 2) - (b.Position.X + b.Position.Width / 2);
                                                int shotY = (c.Position.Y + c.Position.Height / 2) - (b.Position.Y + b.Position.Height / 2);
                                                float shotAngle = (float)Math.Atan2(shotY, shotX);
                                                float modifier = (float)(rgen.Next(13) - 6) / 10;
                                                eProjectiles.Add(new EPBasic(5, 40, 40, b, shotAngle + modifier, 8, basicImage));
                                            }
                                            b.AttackCount--;
                                            if (b.AttackCount == 0)
                                            {
                                                b.AttackNum = 5;
                                                b.AttackCount = 240;
                                            }
                                            break;

                                        // Attack 4, create the four circles of wobble projectiles
                                        case 4:
                                            if (b.AttackCount % 45 == 0)
                                            {
                                                for (int i = 1; i <= 28; i++)
                                                {
                                                    eProjectiles.Add(new EPWobble(5, 35, 35, b, ((float)(2 * Math.PI) / 28) * i, 6, wobbleImage));
                                                }
                                            }

                                            b.AttackCount--;
                                            if (b.AttackCount == 0)
                                            {
                                                b.AttackNum = 5;
                                                b.AttackCount = 300;
                                            }
                                            break;

                                        // Cooldown on attacks
                                        case 5:
                                            b.Moving = true;
                                            b.AttackCount--;
                                            if (b.AttackCount == 0)
                                            {
                                                int number = 0;
                                                // Only be able to call attack 1 when the player is below the boss
                                                if (c.Position.Y > b.Position.Y) number = rgen.Next(4) + 1;
                                                else number = rgen.Next(3) + 2;
                                                switch (number)
                                                {
                                                    case 1:
                                                        b.AttackNum = 1;
                                                        b.AttackCount = 100;
                                                        b.Moving = false;
                                                        break;
                                                    case 2:
                                                        b.AttackNum = 2;
                                                        b.AttackCount = 300;
                                                        break;
                                                    case 3:
                                                        b.AttackNum = 3;
                                                        b.AttackCount = 300;
                                                        break;
                                                    case 4:
                                                        b.AttackNum = 4;
                                                        b.AttackCount = 180;
                                                        b.Moving = false;
                                                        break;
                                                }
                                            }
                                            break;
                                    }
                                }
                            }

                            // Boss attack
                            if (e is Reaper && e.Alive)
                            {
                                Reaper r = (Reaper)e;
                                if (r.Phase == 1)
                                {
                                    switch (r.AttackNum)
                                    {
                                        // Attack 1, shoot acclerating projectiles from the corners
                                        case 1:
                                            if (r.AttackCount % 50 == 0 && r.AttackCount > 90)
                                            {
                                                int corner = rgen.Next(4) + 1;
                                                switch (corner)
                                                {
                                                    // Top left corner
                                                    case 1:
                                                        for (int i = 0; i < 20; i++)
                                                        {
                                                            eProjectiles.Add(new EPAccelerate(5, 36, 36, 0 - globalX, 0 - globalY, i / 12.25f, 6, 5, accelerateImage));
                                                        }
                                                        break;

                                                    // Top right corner
                                                    case 2:
                                                        for (int i = 0; i < 20; i++)
                                                        {
                                                            eProjectiles.Add(new EPAccelerate(5, 36, 36, GraphicsDevice.Viewport.Width + maxX - globalX - 36, 0 - globalY, (i / 12.25f) + (float)Math.PI / 2, 6, 5, accelerateImage));
                                                        }
                                                        break;

                                                    // Bottom right corner
                                                    case 3:
                                                        for (int i = 0; i < 20; i++)
                                                        {
                                                            eProjectiles.Add(new EPAccelerate(5, 36, 36, GraphicsDevice.Viewport.Width + maxX - globalX - 36, GraphicsDevice.Viewport.Height + maxY - globalY - 36, (i / 12.25f) + (float)Math.PI, 6, 5, accelerateImage));
                                                        }
                                                        break;

                                                    // Bottom left corner
                                                    case 4:
                                                        for (int i = 0; i < 20; i++)
                                                        {
                                                            eProjectiles.Add(new EPAccelerate(5, 36, 36, 0 - globalX, GraphicsDevice.Viewport.Height + maxY - globalY - 36, (i / 12.25f) - (float)Math.PI / 2, 6, 5, accelerateImage));
                                                        }
                                                        break;
                                                }
                                            }
                                            r.AttackCount--;
                                            if (r.AttackCount <= 90)
                                            {
                                                r.Visible = true;
                                                r.Darkness -= .005f;
                                            }
                                            if (r.AttackCount == 0)
                                            {
                                                r.PrevAttack = r.AttackNum;
                                                r.AttackNum = 6;
                                                r.AttackCount = 400;
                                            }
                                            break;

                                        // Attack 2, random stall projectiles
                                        case 2:
                                            if (r.AttackCount % 8 == 0 && r.AttackCount > 90)
                                            {
                                                int shotX = 0;
                                                int shotY = 0;
                                                do
                                                {
                                                    shotX = rgen.Next(0 - globalX, GraphicsDevice.Viewport.Width + maxX - globalX - 40);
                                                    shotY = rgen.Next(0 - globalY, GraphicsDevice.Viewport.Height + maxY - globalY - 40);
                                                } while (new Rectangle(shotX, shotY, 40, 40).Intersects(new Rectangle(c.Position.X - 200, c.Position.Y - 200, 400, 400)));

                                                eProjectiles.Add(new EPStall(4, shotX, shotY, 40, 40, 5.55f, 60, stallImage));
                                            }
                                            r.AttackCount--;
                                            if (r.AttackCount <= 90)
                                            {
                                                r.Visible = true;
                                                r.Darkness -= .005f;
                                            }
                                            if (r.AttackCount == 0)
                                            {
                                                r.PrevAttack = r.AttackNum;
                                                r.AttackNum = 6;
                                                r.AttackCount = 400;
                                            }
                                            break;

                                        // Attack 3, circles close in on player
                                        case 3:
                                            if (r.AttackCount % 80 == 0 && r.AttackCount > 90)
                                            {

                                                for (int i = 0; i < 28; i++)
                                                {
                                                    float moveX = -(float)Math.Sin((float)(2 * Math.PI / 28) * i - Math.PI / 2) * 400;
                                                    float moveY = (float)Math.Cos((float)(2 * Math.PI / 28) * i - Math.PI / 2) * 400;
                                                    eProjectiles.Add(new EPBasic(4, 40, 40, c.Position.X + (int)moveX, c.Position.Y + (int)moveY, ((float)(2 * Math.PI / 28) * i) + (float)Math.PI, 4.75f, basicImage));
                                                }
                                            }
                                            r.AttackCount--;
                                            if (r.AttackCount <= 90)
                                            {
                                                r.Visible = true;
                                                r.Darkness -= .005f;
                                            }
                                            if (r.AttackCount == 0)
                                            {
                                                r.PrevAttack = r.AttackNum;
                                                r.AttackNum = 6;
                                                r.AttackCount = 400;
                                            }
                                            break;

                                        //Attack 4, three circle projectiles that spawn others
                                        case 4:
                                            if (r.AttackCount == 600)
                                            {
                                                eProjectiles.Add(new EPCircle(4, 80, 80, (GraphicsDevice.Viewport.Width + maxX - globalX) / 2 - 80, 0 - globalY, 0, 3.2f, 15, basicImage, false));
                                                eProjectiles.Add(new EPCircle(4, 80, 80, 0 - globalX, GraphicsDevice.Viewport.Height + maxY - globalY - 80, 0, 3.2f, 15, basicImage, false));
                                                eProjectiles.Add(new EPCircle(4, 80, 80, GraphicsDevice.Viewport.Width + maxX - globalX - 80, GraphicsDevice.Viewport.Height + maxY - globalY - 80, 0, 3.2f, 15, basicImage, false));

                                            }
                                            for (int i = 0; i < eProjectiles.Count; i++)
                                            {
                                                if (eProjectiles[i] is EPCircle && r.AttackCount > 90 && r.AttackCount <= 570)
                                                {
                                                    EPCircle circ = (EPCircle)eProjectiles[i];
                                                    circ.Shoot(eProjectiles, basicImage);
                                                }
                                            }
                                            r.AttackCount--;
                                            if (r.AttackCount <= 90)
                                            {
                                                r.Visible = true;
                                                r.Darkness -= .005f;
                                            }
                                            if (r.AttackCount == 0)
                                            {
                                                for (int i = eProjectiles.Count - 1; i >= 0; i--)
                                                {
                                                    if (eProjectiles[i] is EPCircle)
                                                    {
                                                        eProjectiles.RemoveAt(i);
                                                    }
                                                }
                                                r.PrevAttack = r.AttackNum;
                                                r.AttackNum = 6;
                                                r.AttackCount = 460;
                                            }
                                            break;

                                        // Attack 5, circles move towards player
                                        case 5:
                                            if(r.AttackCount % 35 == 0 && r.AttackCount > 90)
                                            {
                                                int x = 400;
                                                int y = 400;
                                                do
                                                {
                                                    x = rgen.Next(0 - globalX, GraphicsDevice.Viewport.Width + maxX - globalX - 200) + 100;
                                                    y = rgen.Next(0 - globalY, GraphicsDevice.Viewport.Height + maxY - globalY - 200) + 100;
                                                } while (new Rectangle(x - 400, y - 400, 800, 800).Intersects(c.Position));
                                                int angX = (c.Position.X + c.Position.Width / 2) - (x + 100);
                                                int angY = (c.Position.Y + c.Position.Height / 2) - (y + 100);
                                                float angle = (float)Math.Atan2(angY, angX);
                                                for (int i = 0; i < 16; i++)
                                                {
                                                    float moveX = -(float)Math.Sin((float)(2 * Math.PI / 16) * i - Math.PI / 2) * 68;
                                                    float moveY = (float)Math.Cos((float)(2 * Math.PI / 16) * i - Math.PI / 2) * 68;
                                                    eProjectiles.Add(new EPBasic(4, 40, 40, (int)moveX + x, (int)moveY + y, angle, 12.5f, basicImage));
                                                }
                                            }
                                            r.AttackCount--;
                                            if (r.AttackCount <= 90)
                                            {
                                                r.Visible = true;
                                                r.Darkness -= .005f;
                                            }
                                            if (r.AttackCount == 0)
                                            {
                                                r.PrevAttack = r.AttackNum;
                                                r.AttackNum = 6;
                                                r.AttackCount = 400;
                                            }
                                            break;

                                        // Cooldown on attacks
                                        case 6:
                                            r.AttackCount--;
                                            if (r.AttackCount <= 90)
                                            {
                                                r.Darkness += .005f;
                                            }
                                            if (r.AttackCount == 0)
                                            {
                                                int pos = rgen.Next(5) + 1;
                                                switch (pos)
                                                {
                                                    // Center
                                                    case 1:
                                                        r.Position = new Rectangle((GraphicsDevice.Viewport.Width + maxX - globalX) / 2 - r.Position.Width, (GraphicsDevice.Viewport.Height + maxY - globalY) / 2 - r.Position.Height, r.Position.Width, r.Position.Height);
                                                        break;

                                                    // Upper Left
                                                    case 2:
                                                        r.Position = new Rectangle((GraphicsDevice.Viewport.Width + maxX - globalX) / 2 - r.Position.Width - 200, (GraphicsDevice.Viewport.Height + maxY - globalY) / 2 - r.Position.Height - 200, r.Position.Width, r.Position.Height);
                                                        break;

                                                    // Upper Right
                                                    case 3:
                                                        r.Position = new Rectangle((GraphicsDevice.Viewport.Width + maxX - globalX) / 2 - r.Position.Width + 200, (GraphicsDevice.Viewport.Height + maxY - globalY) / 2 - r.Position.Height - 200, r.Position.Width, r.Position.Height);
                                                        break;

                                                    // Lower Right
                                                    case 4:
                                                        r.Position = new Rectangle((GraphicsDevice.Viewport.Width + maxX - globalX) / 2 - r.Position.Width + 200, (GraphicsDevice.Viewport.Height + maxY - globalY) / 2 - r.Position.Height + 200, r.Position.Width, r.Position.Height);
                                                        break;

                                                    // Lower Left
                                                    case 5:
                                                        r.Position = new Rectangle((GraphicsDevice.Viewport.Width + maxX - globalX) / 2 - r.Position.Width - 200, (GraphicsDevice.Viewport.Height + maxY - globalY) / 2 - r.Position.Height + 200, r.Position.Width, r.Position.Height);
                                                        break;
                                                }
                                                r.FPosX = r.Position.X;
                                                r.FPosY = r.Position.Y;
                                                r.Visible = false;
                                                int number = 0;
                                                do
                                                {
                                                    number = rgen.Next(5) + 1;
                                                } while (number == r.PrevAttack);
                                                switch (number)
                                                {
                                                    case 1:
                                                        r.AttackNum = 1;
                                                        r.AttackCount = 600;
                                                        break;
                                                    case 2:
                                                        r.AttackNum = 2;
                                                        r.AttackCount = 600;
                                                        break;
                                                    case 3:
                                                        r.AttackNum = 3;
                                                        r.AttackCount = 600;
                                                        break;
                                                    case 4:
                                                        r.AttackNum = 4;
                                                        r.AttackCount = 600;
                                                        break;
                                                    case 5: r.AttackNum = 5;
                                                        r.AttackCount = 600;
                                                        break;
                                                }
                                            }
                                            break;
                                    }
                                }

                                if (r.Phase == 2)
                                {
                                    if(r.AttackNum != 6 && r.Health < 4500)
                                    {
                                        if ((r.AttackCount + 120) % 180 == 0 && r.AttackCount > 180)
                                        {
                                            int x = rgen.Next(0 - globalX, GraphicsDevice.Viewport.Width + maxX - globalX - 360) + 180;
                                            eProjectiles.Add(new EPRectangle(5, 360, GraphicsDevice.Viewport.Height + maxY, x, 0 - globalY, 0, 0, 180, whiteBox));
                                        }
                                        for (int i = eProjectiles.Count - 1; i >= 0; i--)
                                        {
                                            if (eProjectiles[i] is EPRectangle)
                                            {
                                                EPRectangle rect = (EPRectangle)eProjectiles[i];
                                                rect.Shoot(eProjectiles, basicImage);
                                            }
                                        }
                                    }

                                    switch (r.AttackNum)
                                    {
                                        // Attack 1, shoot acclerating projectiles from the corners and spawn random
                                        case 1:
                                            if (r.AttackCount % 75 == 0 && r.AttackCount > 90)
                                            {
                                                int corner = rgen.Next(4) + 1;
                                                switch (corner)
                                                {
                                                    // Top left corner
                                                    case 1:
                                                        for (int i = 0; i < 20; i++)
                                                        {
                                                            eProjectiles.Add(new EPAccelerate(5, 36, 36, 0 - globalX, 0 - globalY, i / 12.25f, 6, 5, accelerateImage));
                                                        }
                                                        break;

                                                    // Top right corner
                                                    case 2:
                                                        for (int i = 0; i < 20; i++)
                                                        {
                                                            eProjectiles.Add(new EPAccelerate(5, 36, 36, GraphicsDevice.Viewport.Width + maxX - globalX - 36, 0 - globalY, (i / 12.25f) + (float)Math.PI / 2, 6, 5, accelerateImage));
                                                        }
                                                        break;

                                                    // Bottom right corner
                                                    case 3:
                                                        for (int i = 0; i < 20; i++)
                                                        {
                                                            eProjectiles.Add(new EPAccelerate(5, 36, 36, GraphicsDevice.Viewport.Width + maxX - globalX - 36, GraphicsDevice.Viewport.Height + maxY - globalY - 36, (i / 12.25f) + (float)Math.PI, 6, 5, accelerateImage));
                                                        }
                                                        break;

                                                    // Bottom left corner
                                                    case 4:
                                                        for (int i = 0; i < 20; i++)
                                                        {
                                                            eProjectiles.Add(new EPAccelerate(5, 36, 36, 0 - globalX, GraphicsDevice.Viewport.Height + maxY - globalY - 36, (i / 12.25f) - (float)Math.PI / 2, 6, 5, accelerateImage));
                                                        }
                                                        break;
                                                }
                                            }

                                            if (r.AttackCount % 13 == 0 && r.AttackCount > 90)
                                            {
                                                int shotX = 0;
                                                int shotY = 0;
                                                do
                                                {
                                                    shotX = rgen.Next(0 - globalX, GraphicsDevice.Viewport.Width + maxX - globalX - 40);
                                                    shotY = rgen.Next(0 - globalY, GraphicsDevice.Viewport.Height + maxY - globalY - 40);
                                                } while (new Rectangle(shotX, shotY, 40, 40).Intersects(new Rectangle(c.Position.X - 100, c.Position.Y - 100, 200, 200)));

                                                eProjectiles.Add(new EPStall(5, shotX, shotY, 40, 40, 3.7f, 60, stallImage));
                                            }

                                            r.AttackCount--;
                                            if (r.AttackCount <= 90)
                                            {
                                                r.Visible = true;
                                                r.Darkness -= .005f;
                                            }
                                            if (r.AttackCount == 0)
                                            {
                                                r.PrevAttack = r.AttackNum;
                                                r.AttackNum = 6;
                                                r.AttackCount = 360;
                                            }
                                            break;

                                        // Attack 2, wobble circle from center
                                        case 2:
                                            if(r.AttackCount == 600)
                                            {
                                                eProjectiles.Add(new EPCircle(5, 80, 80, c.Position.X, c.Position.Y + 450, 0, 0.95f, 24, basicImage, true));
                                                eProjectiles.Add(new EPCircle(5, 80, 80, c.Position.X, c.Position.Y - 450, 0, 0.95f, 24, basicImage, true));
                                            }
                                            
                                            if (r.AttackCount % 140 == 0 && r.AttackCount > 90 && new Rectangle((GraphicsDevice.Viewport.Width + maxX - globalX) / 2 - 200, (GraphicsDevice.Viewport.Height + maxY - globalY) / 2 - 200, 400, 400).Intersects(c.Position) == false)
                                            {
                                                for (int i = 1; i <= 22; i++)
                                                {
                                                    eProjectiles.Add(new EPWobble(5, 40, 40, (GraphicsDevice.Viewport.Width + maxX - globalX) / 2 - 20, (GraphicsDevice.Viewport.Height + maxY - globalY) / 2 - 20, ((float)(2 * Math.PI) / 22) * i, 10f, wobbleImage));
                                                }
                                            }
                                            for (int i = 0; i < eProjectiles.Count; i++)
                                            {
                                                if (eProjectiles[i] is EPCircle && r.AttackCount > 90 && r.AttackCount <= 600)
                                                {
                                                    EPCircle circ = (EPCircle)eProjectiles[i];
                                                    circ.Shoot(eProjectiles, basicImage);
                                                }
                                            }

                                            r.AttackCount--;
                                            if (r.AttackCount <= 90)
                                            {
                                                r.Visible = true;
                                                r.Darkness -= .005f;
                                            }
                                            if (r.AttackCount == 0)
                                            {
                                                for (int i = eProjectiles.Count - 1; i >= 0; i--)
                                                {
                                                    if (eProjectiles[i] is EPCircle)
                                                    {
                                                        eProjectiles.RemoveAt(i);
                                                    }
                                                }
                                                r.PrevAttack = r.AttackNum;
                                                r.AttackNum = 6;
                                                r.AttackCount = 360;
                                            }
                                            break;

                                        // Attack 3, circles close in on player
                                        case 3:
                                            if (r.AttackCount % 120 == 0 && r.AttackCount > 90)
                                            {

                                                for (int i = 0; i < 24; i++)
                                                {
                                                    float moveX = -(float)Math.Sin((float)(2 * Math.PI / 24) * i - Math.PI / 2) * 400;
                                                    float moveY = (float)Math.Cos((float)(2 * Math.PI / 24) * i - Math.PI / 2) * 400;
                                                    eProjectiles.Add(new EPBasic(5, 40, 40, c.Position.X + (int)moveX, c.Position.Y + (int)moveY, ((float)(2 * Math.PI / 24) * i) + (float)Math.PI, 4.4f, basicImage));
                                                }
                                            }

                                            if (r.AttackCount % 50 == 0 && r.AttackCount > 90)
                                            {
                                                int x = 400;
                                                int y = 400;
                                                do
                                                {
                                                    x = rgen.Next(0 - globalX, GraphicsDevice.Viewport.Width + maxX - globalX - 200) + 100;
                                                    y = rgen.Next(0 - globalY, GraphicsDevice.Viewport.Height + maxY - globalY - 200) + 100;
                                                } while (new Rectangle(x - 400, y - 400, 800, 800).Intersects(c.Position));
                                                int angX = (c.Position.X + c.Position.Width / 2) - (x + 100);
                                                int angY = (c.Position.Y + c.Position.Height / 2) - (y + 100);
                                                float angle = (float)Math.Atan2(angY, angX);
                                                for (int i = 0; i < 14; i++)
                                                {
                                                    float moveX = -(float)Math.Sin((float)(2 * Math.PI / 14) * i - Math.PI / 2) * 65;
                                                    float moveY = (float)Math.Cos((float)(2 * Math.PI / 14) * i - Math.PI / 2) * 65;
                                                    eProjectiles.Add(new EPBasic(5, 40, 40, (int)moveX + x, (int)moveY + y, angle, 4.05f, basicImage));
                                                }
                                            }
                                            r.AttackCount--;
                                            if (r.AttackCount <= 90)
                                            {
                                                r.Visible = true;
                                                r.Darkness -= .005f;
                                            }
                                            if (r.AttackCount == 0)
                                            {
                                                r.PrevAttack = r.AttackNum;
                                                r.AttackNum = 6;
                                                r.AttackCount = 360;
                                            }
                                            break;

                                        //Attack 4, three circle projectiles that spawn others
                                        case 4:
                                            if (r.AttackCount == 600)
                                            {
                                                eProjectiles.Add(new EPCircle(5, 80, 80, (GraphicsDevice.Viewport.Width + maxX - globalX) / 2 - 80, 0 - globalY, 0, 3.2f, 15, basicImage, false));
                                                eProjectiles.Add(new EPCircle(5, 80, 80, 0 - globalX, GraphicsDevice.Viewport.Height + maxY - globalY - 80, 0, 3.2f, 15, basicImage, false));
                                                eProjectiles.Add(new EPCircle(5, 80, 80, GraphicsDevice.Viewport.Width + maxX - globalX - 80, GraphicsDevice.Viewport.Height + maxY - globalY - 80, 0, 3.2f, 15, basicImage, false));
                                            }
                                            for (int i = 0; i < eProjectiles.Count; i++)
                                            {
                                                if (eProjectiles[i] is EPCircle && r.AttackCount > 90 && r.AttackCount <= 570)
                                                {
                                                    EPCircle circ = (EPCircle)eProjectiles[i];
                                                    circ.Shoot(eProjectiles, basicImage);
                                                }
                                            }
                                            r.AttackCount--;
                                            if (r.AttackCount <= 90)
                                            {
                                                r.Visible = true;
                                                r.Darkness -= .005f;
                                            }
                                            if (r.AttackCount == 0)
                                            {
                                                for (int i = eProjectiles.Count - 1; i >= 0; i--)
                                                {
                                                    if (eProjectiles[i] is EPCircle)
                                                    {
                                                        eProjectiles.RemoveAt(i);
                                                    }
                                                }
                                                r.PrevAttack = r.AttackNum;
                                                r.AttackNum = 6;
                                                r.AttackCount = 360;
                                            }
                                            break;

                                        // Attack 5, circles move towards player
                                        case 5:
                                            // Try block
                                            if(r.AttackCount == 600)
                                            {
                                                r.Visible = false;
                                                r.AttackCount = 780;
                                                r.Darkness = .45f;

                                                BinaryReader reader = new BinaryReader(File.OpenRead(@"../../../Rounds/Reaper.dat"));
                                                try
                                                {
                                                    maxOnScreen = reader.ReadInt32();
                                                    // While loop that will run until the end of the file
                                                    while (reader.PeekChar() != -1)
                                                    {
                                                        // Read in information from the round file
                                                        string type = reader.ReadString();
                                                        int x = reader.ReadInt32();
                                                        int y = reader.ReadInt32();

                                                        // Switch statement based on the type string, creates an enemy based on the type and adds it to the enemies list
                                                        switch (type)
                                                        {
                                                            case "1":
                                                                Enemy e1 = new Enemy1(c.Position.X - 500 + x, c.Position.Y - 300 + y);
                                                                e1.Image = enemyImage;
                                                                e1.SpawnCount = 0;
                                                                enemies.Add(e1);
                                                                break;

                                                            case "2":
                                                                Enemy e2 = new Enemy2(c.Position.X - 500 + x, c.Position.Y - 300 + y);
                                                                e2.Image = enemyImage;
                                                                e2.SpawnCount = 0;
                                                                enemies.Add(e2);
                                                                break;

                                                            case "3":
                                                                Enemy e3 = new Enemy3(c.Position.X - 500 + x, c.Position.Y - 300 + y);
                                                                e3.Image = enemyImage;
                                                                e3.SpawnCount = 0;
                                                                enemies.Add(e3);
                                                                break;
                                                            case "4":
                                                                Enemy e4 = new Enemy4(c.Position.X - 500 + x, c.Position.Y - 300 + y);
                                                                e4.Image = enemyImage;
                                                                e4.SpawnCount = 0;
                                                                enemies.Add(e4);
                                                                break;
                                                        }
                                                    }
                                                }
                                                // Catch Exceptions
                                                catch (Exception ex) { }
                                                reader.Close();
                                            }

                                            bool otherAlive = false;

                                            foreach (Enemy en in enemies)
                                            {
                                                if(en is Reaper == false)
                                                {
                                                    if(en.SpawnCount == -1)
                                                    {
                                                        if(en.Alive == true)
                                                        {
                                                            otherAlive = true;
                                                        }
                                                    }
                                                    if(en.SpawnCount != -1)
                                                    {
                                                        otherAlive = true;
                                                    }
                                                }
                                            }

                                            if(otherAlive == false)
                                            {
                                                for(int i = eProjectiles.Count - 1; i >= 0; i--)
                                                {
                                                    if(eProjectiles[i] is EPRectangle)
                                                    {
                                                        eProjectiles.RemoveAt(i);
                                                    }
                                                }
                                                r.Visible = true;
                                                r.PrevAttack = r.AttackNum;
                                                r.AttackNum = 6;
                                                r.AttackCount = 400;
                                            }
                                            else
                                            {
                                                r.AttackCount++;
                                            }
                                            break;

                                        // Cooldown on attacks
                                        case 6:
                                            r.AttackCount--;
                                            if (r.AttackCount <= 90)
                                            {
                                                r.Darkness += .005f;
                                            }
                                            if (r.AttackCount == 0)
                                            {
                                                int pos = rgen.Next(5) + 1;
                                                switch (pos)
                                                {
                                                    // Center
                                                    case 1:
                                                        r.Position = new Rectangle((GraphicsDevice.Viewport.Width + maxX - globalX) / 2 - r.Position.Width, (GraphicsDevice.Viewport.Height + maxY - globalY) / 2 - r.Position.Height, r.Position.Width, r.Position.Height);
                                                        break;

                                                    // Upper Left
                                                    case 2:
                                                        r.Position = new Rectangle((GraphicsDevice.Viewport.Width + maxX - globalX) / 2 - r.Position.Width - 200, (GraphicsDevice.Viewport.Height + maxY - globalY) / 2 - r.Position.Height - 200, r.Position.Width, r.Position.Height);
                                                        break;

                                                    // Upper Right
                                                    case 3:
                                                        r.Position = new Rectangle((GraphicsDevice.Viewport.Width + maxX - globalX) / 2 - r.Position.Width + 200, (GraphicsDevice.Viewport.Height + maxY - globalY) / 2 - r.Position.Height - 200, r.Position.Width, r.Position.Height);
                                                        break;

                                                    // Lower Right
                                                    case 4:
                                                        r.Position = new Rectangle((GraphicsDevice.Viewport.Width + maxX - globalX) / 2 - r.Position.Width + 200, (GraphicsDevice.Viewport.Height + maxY - globalY) / 2 - r.Position.Height + 200, r.Position.Width, r.Position.Height);
                                                        break;

                                                    // Lower Left
                                                    case 5:
                                                        r.Position = new Rectangle((GraphicsDevice.Viewport.Width + maxX - globalX) / 2 - r.Position.Width - 200, (GraphicsDevice.Viewport.Height + maxY - globalY) / 2 - r.Position.Height + 200, r.Position.Width, r.Position.Height);
                                                        break;
                                                }
                                                r.FPosX = r.Position.X;
                                                r.FPosY = r.Position.Y;
                                                r.Visible = false;
                                                int number = 0;
                                                do
                                                {
                                                    number = rgen.Next(5) + 1;
                                                } while (number == r.PrevAttack);
                                                switch (number)
                                                {
                                                    case 1:
                                                        r.AttackNum = 1;
                                                        r.AttackCount = 600;
                                                        break;
                                                    case 2:
                                                        r.AttackNum = 2;
                                                        r.AttackCount = 600;
                                                        break;
                                                    case 3:
                                                        r.AttackNum = 3;
                                                        r.AttackCount = 600;
                                                        break;
                                                    case 4:
                                                        r.AttackNum = 4;
                                                        r.AttackCount = 600;
                                                        break;
                                                    case 5:
                                                        r.AttackNum = 5;
                                                        r.AttackCount = 600;
                                                        break;
                                                }
                                            }
                                            break;
                                    }
                                }
                            }

                            // Change Reaper phase
                            if (e is Reaper)
                            {
                                Reaper r = (Reaper)e;
                                if (r.Phase == 0)
                                {
                                    r.ChangePhase(c);
                                }
                            }

                            // For loop that goes through all projectile objects in the projectiles list
                            for (int i = projectiles.Count - 1; i >= 0; i--)
                            {
                                if (projectiles[i].CheckCollision(e) == true && e.Alive == true)
                                {
                                    if (e is Reaper == false)
                                    {
                                        if (projectiles[i] is PExplosive)
                                        {
                                            PExplosive ex = (PExplosive)projectiles[i];
                                            ex.Collided = true;
                                        }
                                        else if (projectiles[i] is PMine)
                                        {
                                            PMine mine = (PMine)projectiles[i];
                                            if (mine.ExplosionCount == 0) mine.Explode(c, enemies, projectiles, eProjectiles);
                                            break;
                                        }
                                        else
                                        {
                                            e.Health -= projectiles[i].Damage;
                                            if (projectiles[i].Pierce == false)
                                            {
                                                projectiles.RemoveAt(i);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Reaper r = (Reaper)e;
                                        if (r.Phase == 1 || (r.Phase == 2 && r.AttackNum != 5))
                                        {
                                            if (projectiles[i] is PExplosive)
                                            {
                                                PExplosive ex = (PExplosive)projectiles[i];
                                                ex.Collided = true;
                                            }
                                            else if (projectiles[i] is PMine)
                                            {
                                                PMine mine = (PMine)projectiles[i];
                                                if (mine.ExplosionCount == 0) mine.Explode(c, enemies, projectiles, eProjectiles);
                                                break;
                                            }
                                            else if (projectiles[i] is PSuper && r.Visible)
                                            {
                                                e.Health -= (int)(projectiles[i].Damage / 2.5);
                                            }
                                            else if (projectiles[i] is PSuper && !r.Visible)
                                            {
                                                e.Health -= (int)(projectiles[i].Damage / 4);
                                            }
                                            else
                                            {
                                                if (r.Visible) e.Health -= projectiles[i].Damage;
                                                if (!r.Visible) e.Health -= (int)(projectiles[i].Damage / 2.2);
                                                if (projectiles[i].Pierce == false)
                                                {
                                                    projectiles.RemoveAt(i);
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            // If the enemy's health is 0 or less, it dies
                            if (e.Health <= 0)
                            {
                                bool superKilled = false;
                                foreach(Projectile p in projectiles)
                                {
                                    if(p.Position.Intersects(e.CRect) && p is PSuper)
                                    {
                                        superKilled = true;
                                    }
                                }
                                if (e is Enemy1 && e.Alive == true)
                                {
                                    score += 100;
                                    if(!superKilled) c.Super += 10;
                                    if (c.Super > 100) c.Super = 100;
                                    //enemyDieSound.Play();
                                    e.Alive = false;
                                }
                                if (e is Enemy2 && e.Alive == true)
                                {
                                    score += 150;
                                    if (!superKilled) c.Super += 10;
                                    if (c.Super > 100) c.Super = 100;
                                    Enemy2 e2 = (Enemy2)e;
                                    e2.Shooting = false;
                                    //enemyDieSound.Play();
                                    e.Alive = false;
                                }
                                if (e is Enemy3 && e.Alive == true)
                                {
                                    score += 200;
                                    if (!superKilled) c.Super += 10;
                                    if (c.Super > 100) c.Super = 100;
                                    //enemyDieSound.Play();
                                    e.Alive = false;
                                }
                                if (e is Enemy4 && e.Alive == true)
                                {
                                    score += 200;
                                    if (!superKilled) c.Super += 10;
                                    if (c.Super > 100) c.Super = 100;
                                    //enemyDieSound.Play();
                                    e.Alive = false;
                                }
                                if (e is Boss && e.Alive == true)
                                {
                                    score += 1000;
                                    if (!superKilled) c.Super += 10;
                                    if (c.Super > 100) c.Super = 100;
                                    c.Health += 10;
                                    if (c.Health > 100) c.Health = 100;
                                    //enemyDieSound.Play();
                                    e.Alive = false;
                                }
                                if (e is Reaper && e.Alive == true)
                                {
                                    Reaper r = (Reaper)e;
                                    if (r.Phase == 1 && r.AttackNum == 6)
                                    {
                                        r.Phase = 0;
                                    }
                                    if (r.Phase == 2)
                                    {
                                        score += 6666;
                                        c.Health = 100;
                                        r.Alive = false;
                                    }
                                }
                            }
                        }

                        // An enemy has been killed and there are more enemies to spawn, add another enemy to the list and start spawning it
                        if(maxOnScreen != -1 && eTypes.Count != 0)
                        {
                            for (int i = 0; i < enemies.Count; i++)
                            {
                                if (enemies[i].Alive == false && enemies[i].SpawnCount == -1)
                                {
                                    if (eTypes.Count != 0)
                                    {
                                        enemies.Remove(enemies[i]);
                                        Enemy enemyAdding = null;
                                        int x = (int)ePositions[0].X;
                                        int y = (int)ePositions[0].Y;
                                        switch (eTypes[0])
                                        {
                                            case "1":
                                                enemyAdding = new Enemy1(c.Position.X - 500 + x, c.Position.Y - 300 + y);
                                                enemyAdding.Image = enemyImage;
                                                break;

                                            case "2":
                                                enemyAdding = new Enemy2(c.Position.X - 500 + x, c.Position.Y - 300 + y);
                                                enemyAdding.Image = enemyImage;
                                                break;

                                            case "3":
                                                enemyAdding = new Enemy3(c.Position.X - 500 + x, c.Position.Y - 300 + y);
                                                enemyAdding.Image = enemyImage;
                                                break;
                                            case "4":
                                                enemyAdding = new Enemy4(c.Position.X - 500 + x, c.Position.Y - 300 + y);
                                                enemyAdding.Image = enemyImage;
                                                break;
                                            case "B":
                                                enemyAdding = new Boss(c.Position.X - 500 + x, c.Position.Y - 300 + y, whiteBox);
                                                enemyAdding.Image = boss;
                                                break;
                                        }
                                        foreach (Enemy buddies in enemies)
                                        {
                                            foreach (Rectangle boxes in objects)
                                            {
                                                if (enemyAdding.Position.Intersects(buddies.Position) || enemyAdding.Position.Intersects(boxes))
                                                {
                                                    int newx = rgen.Next(GraphicsDevice.Viewport.Width - 160) + 80;
                                                    int newy = rgen.Next(GraphicsDevice.Viewport.Height - 160) + 80;
                                                    enemyAdding.FPosX = newx;
                                                    enemyAdding.FPosY = newy;
                                                    enemyAdding.Position = new Rectangle((int)enemyAdding.FPosX, (int)enemyAdding.FPosY, enemyAdding.Position.Width, enemyAdding.Position.Height);
                                                }
                                            }
                                        }
                                        enemyAdding.SpawnCount = 0;
                                        enemies.Add(enemyAdding);
                                        eTypes.RemoveAt(0);
                                        ePositions.RemoveAt(0);
                                    }
                                }
                            }
                        }
                        
                        /*if (maxOnScreen != -1 && enemiesSpawn.Count != 0)
                        {
                            for (int i = 0; i < enemies.Count; i++)
                            {
                                if (enemies[i].Alive == false && enemies[i].SpawnCount == -1)
                                {
                                    if (enemiesSpawn.Count != 0)
                                    {
                                        enemies.Remove(enemies[i]);
                                        Enemy enemyAdding = enemiesSpawn[0];
                                        foreach (Enemy buddies in enemies)
                                        {
                                            foreach (Rectangle boxes in objects)
                                            {
                                                if (enemyAdding.Position.Intersects(buddies.Position) || enemyAdding.Position.Intersects(boxes))
                                                {
                                                    int x = rgen.Next(GraphicsDevice.Viewport.Width - 160) + 80;
                                                    int y = rgen.Next(GraphicsDevice.Viewport.Height - 160) + 80;

                                                    Rectangle randPos = new Rectangle(x, y, enemyAdding.Position.Width, enemyAdding.Position.Height);
                                                }
                                            }
                                        }
                                        enemyAdding.SpawnCount = 0;
                                        enemies.Add(enemyAdding);
                                        enemiesSpawn.RemoveAt(0);
                                    }
                                }
                            }
                        }*/
                    }

                    // If Player is dead, the game is over
                    if (c.Health <= 0)
                    {
                        // Check scores on the leaderboard
                        foreach (int s in leaderboardScores)
                        {
                            if (score > s)
                            {
                                lHandler = new LeaderboardHandler();
                                enteringName = true;
                            }
                        }
                        gState = GameState.GameOver;
                    }

                    // If enemyAlive is false, no enemies are alive, and the .a advances
                    if (enemyAlive == false)
                    {
                        AdvanceRound();
                    }

                    break;

                case GameState.Paused:
                    kbState = Keyboard.GetState();
                    if (kbState.IsKeyDown(Keys.P) && previousKbState.IsKeyUp(Keys.P))
                    {
                        gState = GameState.HordeMode;
                        previousKbState = kbState;
                    }
                    //
                    rMButton = new Rectangle((GraphicsDevice.Viewport.Width / 2) - (rMButton.Width / 2), (GraphicsDevice.Viewport.Height / 2) + (rMButton.Height / 2), menu.Width / 4, menu.Height / 4);
                    mRectangle = new Rectangle(mState.Position.X, mState.Position.Y, 1, 1);
                    if (mState.LeftButton == ButtonState.Pressed && mRectangle.Intersects(rMButton))
                    {
                        gState = GameState.Menu;
                    }

                    rPOButton = new Rectangle((GraphicsDevice.Viewport.Width / 2) - (rMButton.Width / 2), (GraphicsDevice.Viewport.Height / 2) + (rMButton.Height/2) + rMButton.Height * 2, (menu.Width/4), (menu.Height/4));
                    if (mState.LeftButton == ButtonState.Pressed && prevMState.LeftButton != ButtonState.Pressed && mRectangle.Intersects(rPOButton))
                    {
                        gState = GameState.Options;
                        prevMState = mState;
                    }


                    break;

                // Game is in Menu
                case GameState.Options:

                    // Mouse rectangle
                    mRectangle = new Rectangle(mState.Position.X, mState.Position.Y, 1, 1);

                    // Menu and options menu setting buttons
                    rMButton = new Rectangle((GraphicsDevice.Viewport.Width / 2) - (rMButton.Width / 2), (GraphicsDevice.Viewport.Height / 2) + rMButton.Height, menu.Width / 4, menu.Height / 4);
                    rFButton = new Rectangle((GraphicsDevice.Viewport.Width / 2) - (rFButton.Width / 2), (GraphicsDevice.Viewport.Height / 2) - (rFButton.Height / 2), menu.Width / 4, menu.Height / 4);
                    
                    // Confirmation dialog buttons
                    rOKButton = new Rectangle((GraphicsDevice.Viewport.Width / 2) - ((int)(1.35 * rOKButton.Width)), (GraphicsDevice.Viewport.Height / 2) - (rOKButton.Height / 2), okButton.Width / 4, okButton.Height / 4);
                    rCancelButton = new Rectangle((GraphicsDevice.Viewport.Width / 2) + (cancelButton.Width / 10), (GraphicsDevice.Viewport.Height / 2) - (rCancelButton.Height / 2), cancelButton.Width / 4, cancelButton.Height / 4);

                    // If the game is about to close, give a confirmation dialog
                    if (closing == true)
                    {
                        if (mState.LeftButton == ButtonState.Pressed && prevMState.LeftButton != ButtonState.Pressed && mRectangle.Intersects(rOKButton))
                        {
                            fullscreen = true;
                            Exit();
                        }
                        if (mState.LeftButton == ButtonState.Pressed && prevMState.LeftButton != ButtonState.Pressed && mRectangle.Intersects(rCancelButton))
                        {
                            closing = false;
                        }
                    }

                    // Otherwise have the normal options menu
                    else
                    {
                        // If the menu button is pressed, go back to the menu
                        if(mState.LeftButton == ButtonState.Pressed && prevMState.LeftButton != ButtonState.Pressed && mRectangle.Intersects(rMButton))
                        {
                            gState = GameState.Menu;
                        }

                        // If the fullscreen button is pressed
                        if (mState.LeftButton == ButtonState.Pressed && prevMState.LeftButton != ButtonState.Pressed && mRectangle.Intersects(rFButton))
                        {
                            // Game is not fullscreen
                            if (fullscreen == false)
                            {
                                // Set closing to true to create the dialog popup
                                closing = true;
                            }

                            // Game is fullscreen
                            if (fullscreen == true)
                            {
                                // Go back to windowed mode
                                fullscreen = false;
                                graphics.IsFullScreen = false;
                                graphics.ApplyChanges();
                            }
                        }

                        if (Bcont == true)
                        {
                            contButton = new Rectangle((GraphicsDevice.Viewport.Width / 2) - (contButton.Width / 2), (GraphicsDevice.Viewport.Height / 2) + (5 * (rMButton.Height / 2)), menu.Width / 4, menu.Height / 4);
                            mRectangle = new Rectangle(mState.Position.X, mState.Position.Y, 1, 1);

                            if (mState.LeftButton == ButtonState.Pressed && prevMState.LeftButton != ButtonState.Pressed && mRectangle.Intersects(contButton))
                            {
                                gState = GameState.HordeMode;
                                prevMState = mState;
                            }
                        }
                    }
                    break;

                case GameState.GameOver:
                    if(enteringName == true)
                    {
                        lHandler.Update();

                        // Mouse rectangle
                        mRectangle = new Rectangle(mState.Position.X, mState.Position.Y, 1, 1);

                        // Checks to see if the start button has been pressed
                        rOKButton = new Rectangle((GraphicsDevice.Viewport.Width / 2) - rOKButton.Width / 2, (GraphicsDevice.Viewport.Height / 2) + (rOKButton.Height / 2), okButton.Width / 4, okButton.Height / 4);

                        // Ok button is pressed or name entering is done, change leaderboard and stop name entry
                        if ((mState.LeftButton == ButtonState.Pressed && prevMState.LeftButton != ButtonState.Pressed && mRectangle.Intersects(rOKButton)) || lHandler.Done == true)
                        {
                            // Go through the leaderboard, if the player's score is greater than the current score, insert it there
                            for (int i = 0; i < 5; i++)
                            {
                                if (score > leaderboardScores[i])
                                {
                                    leaderboardScores.Insert(i, score);
                                    leaderboardRounds.Insert(i, round);
                                    leaderboardNames.Insert(i, lHandler.Name);
                                    switch(switchHero)
                                    {
                                        case SwitchHero.Fire: leaderboardCharacters.Insert(i, "Fire");
                                            break;

                                        case SwitchHero.Earth: leaderboardCharacters.Insert(i, "Earth");
                                            break;

                                        case SwitchHero.Water: leaderboardCharacters.Insert(i, "Water");
                                            break;

                                        case SwitchHero.Electric: leaderboardCharacters.Insert(i, "Electric");
                                            break;
                                    }
                                    break;
                                }
                            }
                            // Remove the 6th score on the list, as we only save the top 5
                            leaderboardScores.RemoveAt(5);
                            leaderboardRounds.RemoveAt(5);
                            leaderboardNames.RemoveAt(5);
                            leaderboardCharacters.RemoveAt(5);

                            // Change the leaderboard file
                            try
                            {
                                BinaryWriter leaderboardWriter = new BinaryWriter(File.OpenWrite(@"../../../Leaderboard.dat"));
                                for (int i = 0; i < 5; i++)
                                {
                                    leaderboardWriter.Write(leaderboardNames[i]);
                                    leaderboardWriter.Write(leaderboardCharacters[i]);
                                    leaderboardWriter.Write(leaderboardRounds[i]);
                                    leaderboardWriter.Write(leaderboardScores[i]);
                                }
                                leaderboardWriter.Close();
                            }
                            catch (Exception ex)
                            {
                                Console.Write(ex);
                            }

                            enteringName = false;
                        }
                    }
                    else
                    {
                        // Mouse rectangle
                        mRectangle = new Rectangle(mState.Position.X, mState.Position.Y, 1, 1);

                        // Checks to see if the start button has been pressed
                        rSButton = new Rectangle((GraphicsDevice.Viewport.Width / 2) - (rSButton.Width / 2), (GraphicsDevice.Viewport.Height / 2), startButton.Width / 4, startButton.Height / 4);
                        rMButton = new Rectangle((GraphicsDevice.Viewport.Width / 2) - (rMButton.Width / 2), (GraphicsDevice.Viewport.Height / 2) + (2 * rMButton.Height), startButton.Width / 4, startButton.Height / 4);
                        if (mState.LeftButton == ButtonState.Pressed && prevMState.LeftButton != ButtonState.Pressed && mRectangle.Intersects(rSButton))
                        {
                            ResetGame();
                            gState = GameState.HordeMode;
                        }
                        if (mState.LeftButton == ButtonState.Pressed && prevMState.LeftButton != ButtonState.Pressed && mRectangle.Intersects(rMButton))
                        {
                            gState = GameState.Menu;
                        }
                    }
                    break;

                case GameState.Leaderboard:
                    // Mouse rectangle
                    mRectangle = new Rectangle(mState.Position.X, mState.Position.Y, 1, 1);

                    rMButton = new Rectangle((GraphicsDevice.Viewport.Width / 2) - (rMButton.Width / 2), (GraphicsDevice.Viewport.Height) - (rMButton.Height + 50), startButton.Width / 4, startButton.Height / 4);

                    // If the menu button is pressed, go back to the menu
                    if (mState.LeftButton == ButtonState.Pressed && prevMState.LeftButton != ButtonState.Pressed && mRectangle.Intersects(rMButton))
                    {
                        gState = GameState.Menu;
                    }
                    break;

                case GameState.Instructions:
                    // Mouse rectangle
                    mRectangle = new Rectangle(mState.Position.X, mState.Position.Y, 1, 1);

                    rMButton = new Rectangle((GraphicsDevice.Viewport.Width / 4) - (rMButton.Width / 2), (GraphicsDevice.Viewport.Height) - (rMButton.Height + 50), startButton.Width / 4, startButton.Height / 4);
                    rNextButton = new Rectangle((GraphicsDevice.Viewport.Width / 4)*3 - (rMButton.Width / 2), (GraphicsDevice.Viewport.Height) - (rMButton.Height + 50), startButton.Width / 4, startButton.Height / 4);

                    // If the menu button is pressed, go back to the menu
                    if (mState.LeftButton == ButtonState.Pressed && prevMState.LeftButton != ButtonState.Pressed && mRectangle.Intersects(rMButton))
                    {
                        gState = GameState.Menu;
                    }

                    if (mState.LeftButton == ButtonState.Pressed && prevMState.LeftButton != ButtonState.Pressed && mRectangle.Intersects(rNextButton))
                    {
                        gState = GameState.Instructions2;
                    }
                    break;


                case GameState.Instructions2:
                    // Mouse rectangle
                    mRectangle = new Rectangle(mState.Position.X, mState.Position.Y, 1, 1);

                    rMButton = new Rectangle((GraphicsDevice.Viewport.Width / 4) - (rMButton.Width / 2), (GraphicsDevice.Viewport.Height) - (rMButton.Height + 50), startButton.Width / 4, startButton.Height / 4);
                    rNextButton = new Rectangle((GraphicsDevice.Viewport.Width / 4) * 3 - (rMButton.Width / 2), (GraphicsDevice.Viewport.Height) - (rMButton.Height + 50), startButton.Width / 4, startButton.Height / 4);

                    // If the menu button is pressed, go back to the menu
                    if (mState.LeftButton == ButtonState.Pressed && prevMState.LeftButton != ButtonState.Pressed && mRectangle.Intersects(rMButton))
                    {
                        gState = GameState.Menu;
                    }

                    if (mState.LeftButton == ButtonState.Pressed && prevMState.LeftButton != ButtonState.Pressed && mRectangle.Intersects(rNextButton))
                    {
                        gState = GameState.Instructions3;
                    }
                    break;

                case GameState.Instructions3:
                    // Mouse rectangle
                    mRectangle = new Rectangle(mState.Position.X, mState.Position.Y, 1, 1);

                    rMButton = new Rectangle((GraphicsDevice.Viewport.Width / 4) - (rMButton.Width / 2), (GraphicsDevice.Viewport.Height) - (rMButton.Height + 50), startButton.Width / 4, startButton.Height / 4);
                    rNextButton = new Rectangle((GraphicsDevice.Viewport.Width / 4) * 3 - (rMButton.Width / 2), (GraphicsDevice.Viewport.Height) - (rMButton.Height + 50), startButton.Width / 4, startButton.Height / 4);

                    // If the menu button is pressed, go back to the menu
                    if (mState.LeftButton == ButtonState.Pressed && prevMState.LeftButton != ButtonState.Pressed && mRectangle.Intersects(rMButton))
                    {
                        gState = GameState.Menu;
                    }

                    if (mState.LeftButton == ButtonState.Pressed && prevMState.LeftButton != ButtonState.Pressed && mRectangle.Intersects(rNextButton))
                    {
                        gState = GameState.Instructions4;
                    }
                    break;

                case GameState.Instructions4:
                    // Mouse rectangle
                    mRectangle = new Rectangle(mState.Position.X, mState.Position.Y, 1, 1);

                    rMButton = new Rectangle((GraphicsDevice.Viewport.Width / 4) - (rMButton.Width / 2), (GraphicsDevice.Viewport.Height) - (rMButton.Height + 50), startButton.Width / 4, startButton.Height / 4);

                    // If the menu button is pressed, go back to the menu
                    if (mState.LeftButton == ButtonState.Pressed && prevMState.LeftButton != ButtonState.Pressed && mRectangle.Intersects(rMButton))
                    {
                        gState = GameState.Menu;
                    }
                    break;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Cyan);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            // Switch statement based on gState
            switch(gState)
            {
                // Game is in Menu
                case GameState.Menu:
                    spriteBatch.Draw(titleBackground, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                    spriteBatch.Draw(title, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                    Rectangle mRectangle = new Rectangle(mState.Position.X, mState.Position.Y, 1, 1);
                    if (rSButton.Intersects(mRectangle))
                    {
                        spriteBatch.Draw(startButton , rSButton, Color.Red);
                    }
                    else
                    {
                        spriteBatch.Draw(startButton, rSButton, Color.White);
                    }

                    if (rOButton.Intersects(mRectangle))
                    {
                        spriteBatch.Draw(optionsButton, rOButton, Color.Red);
                    }
                    else
                    {
                        spriteBatch.Draw(optionsButton, rOButton, Color.White);
                    }

                    if (rLButton.Intersects(mRectangle))
                    {
                        spriteBatch.Draw(leaderboardButton, rLButton, Color.Red);
                    }
                    else
                    {
                        spriteBatch.Draw(leaderboardButton, rLButton, Color.White);
                    }

                    if (rIButton.Intersects(mRectangle))
                    {
                        spriteBatch.Draw(instructionsButton, rIButton, Color.Red);
                    }
                    else
                    {
                        spriteBatch.Draw(instructionsButton, rIButton, Color.White);
                    }

                    if (rEButton.Intersects(mRectangle))
                    {
                        spriteBatch.Draw(exitB, rEButton, Color.Red);
                    }
                    else
                    {
                        spriteBatch.Draw(exitB, rEButton, Color.White);
                    }
                    break;

                // Game is in character selection screen
                case GameState.CharacterSelection:
                    spriteBatch.Draw(titleBackground, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                    mRectangle = new Rectangle(mState.Position.X, mState.Position.Y, 1, 1);
                    if (char1.Intersects(mRectangle))
                    {
                        spriteBatch.Draw(fireButton, char1, Color.Red);

                        switchHero = SwitchHero.Fire;
                        playerImage = player1Image;
                        playerWalking = player1Walking;
                        bulletImage = bullet1Image;
                        c.Image = playerWalking;
                        numFramesPlayer = 8;
                        c.Position = new Rectangle((char1.X + char1.Width / 2) - c.Position.Width / 2, (char1.Y - char1.Height) - c.Position.Height, c.Position.Width, c.Position.Height);
                        c.Draw(spriteBatch, (float)Math.PI/2, framePlayer, 3);

                    }
                    else
                    {
                        spriteBatch.Draw(fireButton, char1, Color.White);
                    }

                    if (char2.Intersects(mRectangle))
                    {
                        spriteBatch.Draw(earthButton, char2, Color.Red);

                        switchHero = SwitchHero.Earth;
                        playerImage = player2Image;
                        playerWalking = player2Walking;
                        bulletImage = bullet2Image;
                        c.Image = playerWalking;
                        numFramesPlayer = 8;
                        c.Position = new Rectangle((char2.X + char2.Width / 2) - c.Position.Width / 2, (char2.Y - char2.Height) - c.Position.Height, c.Position.Width, c.Position.Height);
                        c.Draw(spriteBatch, (float)Math.PI / 2, framePlayer, 3);
                    }
                    else
                    {
                        spriteBatch.Draw(earthButton, char2, Color.White);
                    }

                    if (char3.Intersects(mRectangle))
                    {
                        spriteBatch.Draw(waterButton, char3, Color.Red);

                        switchHero = SwitchHero.Water;
                        playerImage = player4Image;
                        playerWalking = player4Walking;
                        bulletImage = bullet4Image;
                        c.Image = playerWalking;
                        numFramesPlayer = 8;
                        c.Position = new Rectangle((char3.X + char3.Width / 2) - c.Position.Width / 2, (char3.Y - char3.Height) - c.Position.Height, c.Position.Width, c.Position.Height);
                        c.Draw(spriteBatch, (float)Math.PI / 2, framePlayer, 3);
                    }
                    else
                    {
                        spriteBatch.Draw(waterButton, char3, Color.White);
                    }

                    if (char4.Intersects(mRectangle))
                    {
                        spriteBatch.Draw(electricButton, char4, Color.Red);

                        switchHero = SwitchHero.Electric;
                        playerImage = player3Image;
                        playerWalking = player3Walking;
                        bulletImage = bullet3Image;
                        c.Image = playerWalking;
                        numFramesPlayer = 8;
                        c.Position = new Rectangle((char4.X + char4.Width / 2) - c.Position.Width / 2, (char4.Y - char1.Height) - c.Position.Height, c.Position.Width, c.Position.Height);
                        c.Draw(spriteBatch, (float)Math.PI / 2, framePlayer, 3);
                    }
                    else
                    {
                        spriteBatch.Draw(electricButton, char4, Color.White);
                    }
                    break;

                // Game is in Horde Mode
                case GameState.HordeMode:
                    GraphicsDevice.Clear(Color.Black);
                    spriteBatch.Draw(background, new Vector2(backgroundPoint.X, backgroundPoint.Y), Color.White);

                    // Draw the boxes
                    foreach (Rectangle o in objects)
                    {
                        if(o.Width == 50 && o.Height == 75)
                        {
                            spriteBatch.Draw(garbage, o, Color.White);
                        }
                        else if(o.Width == 75 && o.Height == 75)
                        {
                            spriteBatch.Draw(boxes, o, Color.White);
                        }
                        else if (o.Width == 200 && o.Height == 100)
                        {
                            spriteBatch.Draw(car, o, Color.White);
                        }

                    }

                    if (reaperRound == false)
                    {
                        foreach (EnemyProjectile eP in eProjectiles)
                        {
                            eP.Draw(spriteBatch);
                        }
                    }

                    if (c.SuperCount > 0 && c.SuperCount < 60)
                    {
                        Vector2 origin = new Vector2(superCharge.Width / 2, superCharge.Height / 2);
                        spriteBatch.Draw(superCharge, new Vector2(c.Position.X + c.Position.Width / 2, c.Position.Y + c.Position.Height / 2), null, Color.White, 0, origin, (float)24 - ((float)c.SuperCount / 2.5f), SpriteEffects.None, 0);
                    }

                    foreach(Powerup pUp in powerups)
                    {
                        if(pUp.Active) pUp.Draw(spriteBatch);
                    }

                    if(reaperRound == false)
                    {
                        if (c.DashCount == 0 && c.FiringSuper == true && c.SuperCount == 60) c.Draw(spriteBatch, rotationAngle, framePlayer, Color.Purple); // Draw the character
                        if (c.DashCount == 0 && c.SuperCount != 60) c.Draw(spriteBatch, rotationAngle, framePlayer, Color.White); // Draw the character
                        if (c.DashCount < 20 && c.DashCount > 0) c.Draw(spriteBatch, rotationAngle, framePlayer, Color.CadetBlue); // Draw the character
                        if (c.DashCount >= 20) c.Draw(spriteBatch, rotationAngle, framePlayer, Color.IndianRed); // Draw the character
                    }

                    // Draw lines for the Enemy2s that are shooting
                    foreach (Enemy e in enemies)
                    {
                        if (e is Enemy2)
                        {
                            Enemy2 e2 = (Enemy2)e;
                            if (e2.Shooting == true)
                            {
                                int distance = (int)Math.Sqrt(Math.Pow((c.Position.X - e2.Position.X), 2) + Math.Pow((c.Position.Y - e2.Position.Y), 2));
                                Rectangle lineRect = new Rectangle(e2.Position.X + e2.Position.Width / 2, e2.Position.Y + e2.Position.Height / 2, distance, 2);
                                int lineX = (c.Position.X + c.Position.Width / 2) - (e2.Position.X + e2.Position.Width / 2);
                                int lineY = (c.Position.Y + c.Position.Height / 2) - (e2.Position.Y + e2.Position.Height / 2);
                                float lineAngle = (float)Math.Atan2(lineY, lineX);
                                if (lineRect.Width < 0)
                                {
                                    lineRect.Width = -lineRect.Width;
                                }
                                Color color = Color.Red;
                                if (e2.ShotCount < 120 || (e2.ShotCount > 120 && e2.ShotCount % 4 == 0 && color == Color.WhiteSmoke))
                                {
                                    color = Color.Red;
                                }
                                else if (color == Color.Red && e2.ShotCount % 4 == 0)
                                {
                                    color = Color.Yellow;
                                }
                                else if (color == Color.Yellow && e2.ShotCount % 4 == 0)
                                {
                                    color = Color.WhiteSmoke;
                                }
                                spriteBatch.Draw(dot, lineRect, null, color, lineAngle, Vector2.Zero, SpriteEffects.None, 0);
                            }
                        }
                    }

                    // Draw all alive enemies
                    foreach (Enemy e in enemies)
                    {
                        int aX = e.Position.X - c.Position.X;
                        int aY = e.Position.Y - c.Position.Y;
                        float enemyAngle = -(float)(Math.Atan2(aX, aY) + Math.PI / 2);
                        if (e.Alive == true)
                        {
                            if (e is Enemy1) e.Draw(spriteBatch, enemyAngle, frameEnemy, Color.White);
                            if (e is Enemy2) e.Draw(spriteBatch, enemyAngle, frameEnemy, Color.Blue);
                            if (e is Enemy3) e.Draw(spriteBatch, enemyAngle, frameEnemy, Color.Orange);
                            if (e is Enemy4) e.Draw(spriteBatch, enemyAngle, frameEnemy, Color.White);
                            if (e is Boss) e.Draw(spriteBatch, enemyAngle, frameEnemy, Color.White);
                        }
                        else if (e.SpawnCount != -1 && (e is Reaper != true))
                        {
                            Vector2 origin = new Vector2(eMarker.Width / 2, eMarker.Height / 2);
                            spriteBatch.Draw(eMarker, new Vector2(e.Position.X, e.Position.Y), null, Color.White, 0, origin, (float)e.SpawnCount / 30, SpriteEffects.None, 0);
                        }
                        else if (e.SpawnCount != -1 && e is Reaper)
                        {
                            Vector2 origin = new Vector2(rMarker.Width / 2, rMarker.Height / 2);
                            spriteBatch.Draw(rMarker, new Vector2(e.Position.X + e.Position.Width/2, e.Position.Y + e.Position.Height/2), null, Color.White, 0, origin, (float)e.SpawnCount / 16, SpriteEffects.None, 0);
                            spriteBatch.Draw(whiteBox, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.Black * ((float)e.SpawnCount / 666));
                        }
                    }

                    if (reaperRound == false)
                    {
                        foreach (Projectile p in projectiles)
                        {
                            if (p is PBasic || p is PSuper)
                            {
                                p.Draw(spriteBatch, frameProjectile);
                            }
                            if (p is PStationary)
                            {
                                PStationary ps = (PStationary)p;
                                ps.Draw(spriteBatch, frameProjectile, rotationAngle);
                            }
                            if (p is PExplosive)
                            {
                                PExplosive ex = (PExplosive)p;
                                if (ex.ExplosionCount == 0) ex.Draw(spriteBatch);
                                else spriteBatch.Draw(explosion, ex.Explosion, Color.White);
                            }
                            if (p is PMine)
                            {
                                PMine mine = (PMine)p;
                                if (mine.ExplosionCount == 0) mine.Draw(spriteBatch);
                                else spriteBatch.Draw(explosion, mine.Explosion, Color.White);
                            }
                        }
                    }
                    if (reaperRound == true)
                    {
                        foreach(Enemy e in enemies)
                        {
                            if(e is Reaper)
                            {
                                Reaper r = (Reaper)e;
                                spriteBatch.Draw(whiteBox, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.Black * r.Darkness);
                                spriteBatch.Draw(whiteBox, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.Black * (r.Darkness - .3f));
                            }
                        }
                        foreach (Enemy e in enemies)
                        {
                            int aX = e.Position.X - c.Position.X;
                            int aY = e.Position.Y - c.Position.Y;
                            float enemyAngle = -(float)(Math.Atan2(aX, aY) + Math.PI / 2);
                            if (e.Alive == true)
                            {
                                if (e is Reaper) e.Draw(spriteBatch, enemyAngle, 0, Color.White);
                            }
                        }
                        foreach (Projectile p in projectiles)
                        {
                            if (p is PBasic || p is PSuper)
                            {
                                p.Draw(spriteBatch, frameProjectile);
                            }
                            if (p is PStationary)
                            {
                                PStationary ps = (PStationary)p;
                                ps.Draw(spriteBatch, frameProjectile, rotationAngle);
                            }
                            if (p is PExplosive)
                            {
                                PExplosive ex = (PExplosive)p;
                                if (ex.ExplosionCount == 0) ex.Draw(spriteBatch);
                                else spriteBatch.Draw(explosion, ex.Explosion, Color.White);
                            }
                            if (p is PMine)
                            {
                                PMine mine = (PMine)p;
                                if (mine.ExplosionCount == 0) mine.Draw(spriteBatch);
                                else spriteBatch.Draw(explosion, mine.Explosion, Color.White);
                            }
                        }
                        if (c.DashCount == 0 && c.FiringSuper == true && c.SuperCount == 60) c.Draw(spriteBatch, rotationAngle, framePlayer, Color.Purple); // Draw the character
                        if (c.DashCount == 0 && c.SuperCount != 60) c.Draw(spriteBatch, rotationAngle, framePlayer, Color.White); // Draw the character
                        if (c.DashCount < 20 && c.DashCount > 0) c.Draw(spriteBatch, rotationAngle, framePlayer, Color.CadetBlue); // Draw the character
                        if (c.DashCount >= 20) c.Draw(spriteBatch, rotationAngle, framePlayer, Color.IndianRed); // Draw the character
                        foreach (EnemyProjectile eP in eProjectiles)
                        {
                            eP.Draw(spriteBatch);
                        }
                    }

                    if(roundChange >= 0)
                    {
                        if (roundChange > 120)
                        {
                            spriteBatch.Draw(roundMarker, new Vector2(GraphicsDevice.Viewport.Width/2, GraphicsDevice.Viewport.Height / 2 - 100), null, Color.White, 0, new Vector2(roundMarker.Width/2, roundMarker.Height/2), (180-roundChange)/60f, SpriteEffects.None, 0);
                        }
                        if(roundChange <= 120 && roundChange > 60)
                        {

                            spriteBatch.Draw(roundMarker, new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2 - 100), null, Color.White, 0, new Vector2(roundMarker.Width / 2, roundMarker.Height / 2), 1, SpriteEffects.None, 0);
                            if(roundChange % 6 == 0 && roundStrPartial != roundStr)
                            {
                                roundStrPartial += roundStr[roundStrPartial.Length];
                            }
                            spriteBatch.DrawString(rFont, roundStrPartial, new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2 - 100), Color.White, 0, new Vector2(rFont.MeasureString(roundStrPartial).X / 2, rFont.MeasureString(roundStrPartial).Y / 2), 1, SpriteEffects.None, 0);
                        }
                        if(roundChange <= 60)
                        {
                            float drawX = GraphicsDevice.Viewport.Width / 2 + (GraphicsDevice.Viewport.Width - 100 - GraphicsDevice.Viewport.Width / 2) * (60 - roundChange) / 60;
                            float drawY = (GraphicsDevice.Viewport.Height / 2 - 100) + ((GraphicsDevice.Viewport.Height - 40) - (GraphicsDevice.Viewport.Height / 2 - 100)) * (60 - roundChange) / 60;
                            float drawStrY = (GraphicsDevice.Viewport.Height / 2 - 100) + ((GraphicsDevice.Viewport.Height - 40) - (GraphicsDevice.Viewport.Height / 2 - 100)) * (60 - roundChange) / 60;
                            spriteBatch.Draw(roundMarker, new Vector2(drawX, drawY), null, Color.White, 0, new Vector2(roundMarker.Width / 2, roundMarker.Height / 2), 1 - (60-roundChange)/80f, SpriteEffects.None, 0);
                            spriteBatch.DrawString(rFont, roundStrPartial, new Vector2(drawX, drawStrY), Color.White, 0, new Vector2(rFont.MeasureString(roundStrPartial).X / 2, rFont.MeasureString(roundStrPartial).Y / 2), 1 - (60 - roundChange) / 80f, SpriteEffects.None, 0);
                        }
                    }

                    if(pickupTimer != 0)
                    {
                        if(powerUpPickup == "Death")
                        {
                            if (pickupTimer > 60)
                            {
                                float dark = (300 - pickupTimer) / 60f;
                                if (dark > 1) dark = 1;
                                spriteBatch.Draw(whiteBox, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.Black * dark);
                            }
                            if (pickupTimer <= 60)
                            {
                                float dark = (pickupTimer) / 60f;
                                spriteBatch.Draw(whiteBox, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.Black * dark);
                            }
                            if (skulls == 1 && pickupTimer > 60)
                            {
                                deathStr = "The souls of those you killed claw at your back";
                                if(pickupTimer % 4 == 0 && powerUpPartial.Length < 23)
                                {
                                    powerUpPartial += deathStr[powerUpPartial.Length];
                                }
                                else if(pickupTimer % 4 == 0 && powerUpPartial2.Length < deathStr.Length - powerUpPartial.Length)
                                {
                                    powerUpPartial2 += deathStr[powerUpPartial2.Length + powerUpPartial.Length];
                                }
                                if (fullscreen)
                                {
                                    spriteBatch.DrawString(rFont, powerUpPartial, new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2 - 200), Color.White, 0, new Vector2(rFont.MeasureString(powerUpPartial).X / 2, rFont.MeasureString(powerUpPartial).Y / 2), .95f, SpriteEffects.None, 0);
                                    spriteBatch.DrawString(rFont, powerUpPartial2, new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2 - 70), Color.White, 0, new Vector2(rFont.MeasureString(powerUpPartial2).X / 2, rFont.MeasureString(powerUpPartial).Y / 2), .95f, SpriteEffects.None, 0);
                                }
                                else
                                {
                                    spriteBatch.DrawString(rFont, powerUpPartial, new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2 - 200), Color.White, 0, new Vector2(rFont.MeasureString(powerUpPartial).X / 2, rFont.MeasureString(powerUpPartial).Y / 2), .7f, SpriteEffects.None, 0);
                                    spriteBatch.DrawString(rFont, powerUpPartial2, new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2 - 70), Color.White, 0, new Vector2(rFont.MeasureString(powerUpPartial2).X / 2, rFont.MeasureString(powerUpPartial).Y / 2), .7f, SpriteEffects.None, 0);
                                }
                            }

                            if(skulls == 2 && pickupTimer > 60)
                            {
                                deathStr = "You hear horrible screams coming closer";
                                if (pickupTimer % 4 == 0 && powerUpPartial.Length < 18)
                                {
                                    powerUpPartial += deathStr[powerUpPartial.Length];
                                }
                                else if (pickupTimer % 4 == 0 && powerUpPartial2.Length < deathStr.Length - powerUpPartial.Length)
                                {
                                    powerUpPartial2 += deathStr[powerUpPartial2.Length + powerUpPartial.Length];
                                }
                                if (fullscreen)
                                {
                                    spriteBatch.DrawString(rFont, powerUpPartial, new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2 - 200), Color.White, 0, new Vector2(rFont.MeasureString(powerUpPartial).X / 2, rFont.MeasureString(powerUpPartial).Y / 2), 1f, SpriteEffects.None, 0);
                                    spriteBatch.DrawString(rFont, powerUpPartial2, new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2 - 70), Color.White, 0, new Vector2(rFont.MeasureString(powerUpPartial2).X / 2, rFont.MeasureString(powerUpPartial).Y / 2), 1f, SpriteEffects.None, 0);
                                }
                                else
                                {
                                    spriteBatch.DrawString(rFont, powerUpPartial, new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2 - 200), Color.White, 0, new Vector2(rFont.MeasureString(powerUpPartial).X / 2, rFont.MeasureString(powerUpPartial).Y / 2), .7f, SpriteEffects.None, 0);
                                    spriteBatch.DrawString(rFont, powerUpPartial2, new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2 - 70), Color.White, 0, new Vector2(rFont.MeasureString(powerUpPartial2).X / 2, rFont.MeasureString(powerUpPartial).Y / 2), .7f, SpriteEffects.None, 0);
                                }
                            }

                            if(skulls == 3 && pickupTimer > 60)
                            {
                                deathStr = "He's here...";
                                if (pickupTimer % 13 == 0 && powerUpPartial != deathStr)
                                {
                                    powerUpPartial += deathStr[powerUpPartial.Length];
                                }
                                spriteBatch.DrawString(rFont, powerUpPartial, new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2 - 200), Color.White, 0, new Vector2(rFont.MeasureString(powerUpPartial).X / 2, rFont.MeasureString(powerUpPartial).Y / 2), 1f, SpriteEffects.None, 0);
                            }
                        }
                        else
                        {
                            if(pickupTimer > 150)
                            {
                                spriteBatch.Draw(powerupMarker, new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2 - 200), null, Color.White, 0, new Vector2(powerupMarker.Width / 2, powerupMarker.Height / 2), (210 - pickupTimer) / 60f, SpriteEffects.None, 0);
                            }
                            if(pickupTimer <= 150 && pickupTimer > 60)
                            {
                                spriteBatch.Draw(powerupMarker, new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2 - 200), null, Color.White, 0, new Vector2(powerupMarker.Width / 2, powerupMarker.Height / 2), 1, SpriteEffects.None, 0);
                                if (pickupTimer % 4 == 0 && powerUpPartial != powerUpPickup)
                                {
                                    powerUpPartial += powerUpPickup[powerUpPartial.Length];
                                }
                                spriteBatch.DrawString(rFont, powerUpPartial, new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2 - 200), Color.White, 0, new Vector2(rFont.MeasureString(powerUpPartial).X / 2, rFont.MeasureString(powerUpPartial).Y / 2), .85f, SpriteEffects.None, 0);
                            }
                            if(pickupTimer <= 60 && pickupTimer > 40)
                            {
                                spriteBatch.Draw(powerupMarker, new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2 - 200), null, Color.White, 0, new Vector2(powerupMarker.Width / 2, powerupMarker.Height / 2), 1, SpriteEffects.None, 0);
                                spriteBatch.DrawString(rFont, powerUpPickup, new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2 - 200), Color.White, 0, new Vector2(rFont.MeasureString(powerUpPartial).X / 2, rFont.MeasureString(powerUpPartial).Y / 2), .85f, SpriteEffects.None, 0);
                            }
                            if(pickupTimer <= 40)
                            {
                                spriteBatch.Draw(powerupMarker, new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2 - 200), null, Color.White, 0, new Vector2(powerupMarker.Width / 2, powerupMarker.Height / 2), pickupTimer/40f, SpriteEffects.None, 0);
                                float strScale = pickupTimer / 40f - .15f;
                                if(strScale > 0) spriteBatch.DrawString(rFont, powerUpPickup, new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2 - 200), Color.White, 0, new Vector2(rFont.MeasureString(powerUpPartial).X / 2, rFont.MeasureString(powerUpPartial).Y / 2), strScale, SpriteEffects.None, 0);
                            }
                        }
                    }

                    // Code for drawing interface (differs between fullscreen and windowed mode)

                    switch (fullscreen)
                    {
                        case true:
                            //spriteBatch.DrawString(sFont, "Round " + round, new Vector2(GraphicsDevice.Viewport.Width - 100, GraphicsDevice.Viewport.Height - 40), Color.Black);
                            spriteBatch.Draw(roundMarker, new Rectangle(10, GraphicsDevice.Viewport.Height - 60, 120, 60), Color.White);
                            spriteBatch.DrawString(lFont, "Score", new Vector2(30, GraphicsDevice.Viewport.Height - 60), Color.White, 0, new Vector2(0, 0), .75f, SpriteEffects.None, 0);
                            spriteBatch.DrawString(lFont, "" + score, new Vector2(30, GraphicsDevice.Viewport.Height - 35), Color.White, 0, new Vector2(0, 0), .75f, SpriteEffects.None, 0);
                            spriteBatch.Draw(hudrectangle, new Rectangle(GraphicsDevice.Viewport.Width * 10 / 33, GraphicsDevice.Viewport.Height / 35, 42, 50), Color.DodgerBlue); //rectangle around "life"
                            spriteBatch.Draw(hudrectangle, new Rectangle(GraphicsDevice.Viewport.Width * 10 / 33, GraphicsDevice.Viewport.Height / 18, 300, 35), Color.DodgerBlue); //rectangle around life bar
                            spriteBatch.Draw(hudrectangle, new Rectangle(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 18, 300, 35), Color.DodgerBlue); //rectangle around special bar
                            spriteBatch.Draw(hudcircle, new Rectangle(GraphicsDevice.Viewport.Width * 100 / 153, GraphicsDevice.Viewport.Height / 45, 100, 100), Color.DodgerBlue); //circle around special attack
                            spriteBatch.Draw(hudcircle, new Rectangle(GraphicsDevice.Viewport.Width * 5 / 11, GraphicsDevice.Viewport.Height / 45, 100, 100), Color.DodgerBlue); //circle around current weapon
                            spriteBatch.Draw(hudcircle, new Rectangle(GraphicsDevice.Viewport.Width * 100 / 217, GraphicsDevice.Viewport.Height / 30, 80, 80), Color.DarkBlue); //circle around current weapon (inner)
                            spriteBatch.DrawString(sFont, "Life", new Vector2(GraphicsDevice.Viewport.Width * 25 / 82, GraphicsDevice.Viewport.Height / 30), Color.Black); //"life"
                            int specialint = (int)gameTime.TotalGameTime.TotalMilliseconds;
                            if (c.Super < 100)
                            {
                                spriteBatch.DrawString(sFont, "SUPER", new Vector2(GraphicsDevice.Viewport.Width * 250 / 377, GraphicsDevice.Viewport.Height / 17), Color.Gray); //"special"
                            }
                            if (c.Super == 100)
                            {
                                if (specialint % 1000 < 500)
                                {
                                    spriteBatch.DrawString(sFont, "SUPER", new Vector2(GraphicsDevice.Viewport.Width * 250 / 377, GraphicsDevice.Viewport.Height / 17), Color.Red); //"special" flashing
                                }
                                else
                                {
                                    spriteBatch.DrawString(sFont, "SUPER", new Vector2(GraphicsDevice.Viewport.Width * 250 / 377, GraphicsDevice.Viewport.Height / 17), Color.Cyan); //"special" flashing
                                }
                            }
                            spriteBatch.Draw(hudrectangle, new Rectangle(GraphicsDevice.Viewport.Width * 4 / 13, GraphicsDevice.Viewport.Height / 16, 260, 20), Color.Red); //red life bar
                            spriteBatch.Draw(hudrectangle, new Rectangle(GraphicsDevice.Viewport.Width * 25 / 49, GraphicsDevice.Viewport.Height / 16, 260, 20), Color.Red); //red special bar
                            spriteBatch.Draw(hudrectangle, new Rectangle(GraphicsDevice.Viewport.Width * 4 / 13, GraphicsDevice.Viewport.Height / 16, c.Health * 13 / 5, 20), Color.LawnGreen); //green life bar
                            spriteBatch.Draw(hudrectangle, new Rectangle(GraphicsDevice.Viewport.Width * 25 / 49, GraphicsDevice.Viewport.Height / 16, c.Super * 13 / 5, 20), Color.Aqua); //purple special bar
                            spriteBatch.Draw(hudrectangle, new Rectangle(GraphicsDevice.Viewport.Width * 1 / 8, GraphicsDevice.Viewport.Height * 200 / 211, 1370, 35), Color.DodgerBlue); //rectangle around energy bar
                            spriteBatch.Draw(hudrectangle, new Rectangle(GraphicsDevice.Viewport.Width * 1 / 7, GraphicsDevice.Viewport.Height * 1000 / 1047, 1300, 20), Color.Red); //red energy bar
                            spriteBatch.Draw(hudrectangle, new Rectangle(GraphicsDevice.Viewport.Width * 1 / 7, GraphicsDevice.Viewport.Height * 1000 / 1047, c.Energy * 13 / 2, 20), Color.Gold); //energy bar
                            break;
                        case false:
                            //spriteBatch.DrawString(sFont, "Round " + round, new Vector2(GraphicsDevice.Viewport.Width - 100, GraphicsDevice.Viewport.Height - 40), Color.Black);
                            spriteBatch.Draw(roundMarker, new Rectangle(10, GraphicsDevice.Viewport.Height - 60, 120, 60), Color.White);
                            spriteBatch.DrawString(lFont, "Score", new Vector2(30, GraphicsDevice.Viewport.Height - 60), Color.White, 0, new Vector2(0, 0), .75f, SpriteEffects.None, 0);
                            spriteBatch.DrawString(lFont, "" + score, new Vector2(30, GraphicsDevice.Viewport.Height - 35), Color.White, 0, new Vector2(0, 0), .75f, SpriteEffects.None, 0);
                            spriteBatch.Draw(hudrectangle, new Rectangle(GraphicsDevice.Viewport.Width * 50 / 203, GraphicsDevice.Viewport.Height / 35, 42, 50), Color.DodgerBlue); //rectangle around "life"
                            spriteBatch.Draw(hudrectangle, new Rectangle(GraphicsDevice.Viewport.Width * 50 / 203, GraphicsDevice.Viewport.Height / 18, 300, 35), Color.DodgerBlue); //rectangle around life bar
                            spriteBatch.Draw(hudrectangle, new Rectangle(GraphicsDevice.Viewport.Width * 10 / 19, GraphicsDevice.Viewport.Height / 18, 300, 35), Color.DodgerBlue); //rectangle around special bar
                            spriteBatch.Draw(hudcircle, new Rectangle(GraphicsDevice.Viewport.Width * 25 / 33, GraphicsDevice.Viewport.Height / 75, 100, 100), Color.DodgerBlue); //circle around special attack
                            spriteBatch.Draw(hudcircle, new Rectangle(GraphicsDevice.Viewport.Width * 10 / 21, GraphicsDevice.Viewport.Height / 75, 100, 100), Color.DodgerBlue); //circle around current weapon
                            spriteBatch.Draw(hudcircle, new Rectangle(GraphicsDevice.Viewport.Width * 125 / 258, GraphicsDevice.Viewport.Height / 35, 80, 80), Color.DarkBlue); //circle around current weapon (inner)
                            spriteBatch.DrawString(sFont, "Life", new Vector2(GraphicsDevice.Viewport.Width / 4, GraphicsDevice.Viewport.Height / 30), Color.Black); //"life"
                            specialint = (int)gameTime.TotalGameTime.TotalMilliseconds;
                            if (c.Super < 100)
                            {
                                spriteBatch.DrawString(sFont, "SUPER", new Vector2(GraphicsDevice.Viewport.Width * 250 / 323, GraphicsDevice.Viewport.Height / 16), Color.Gray); //"special"
                            }
                            if (c.Super == 100)
                            {
                                if (specialint % 1000 < 500)
                                {
                                    spriteBatch.DrawString(sFont, "SUPER", new Vector2(GraphicsDevice.Viewport.Width * 250 / 323, GraphicsDevice.Viewport.Height / 16), Color.Red); //"special" flashing
                                }
                                else
                                {
                                    spriteBatch.DrawString(sFont, "SUPER", new Vector2(GraphicsDevice.Viewport.Width * 250 / 323, GraphicsDevice.Viewport.Height / 16), Color.Cyan); //"special" flashing
                                }
                            }
                            spriteBatch.Draw(hudrectangle, new Rectangle(GraphicsDevice.Viewport.Width / 4, GraphicsDevice.Viewport.Height / 15, 260, 20), Color.Red); //red life bar
                            spriteBatch.Draw(hudrectangle, new Rectangle(GraphicsDevice.Viewport.Width * 25 / 44, GraphicsDevice.Viewport.Height / 15, 230, 20), Color.Red); //red special bar
                            spriteBatch.Draw(hudrectangle, new Rectangle(GraphicsDevice.Viewport.Width / 4, GraphicsDevice.Viewport.Height / 15, c.Health * 13 / 5, 20), Color.LawnGreen); //green life bar
                            spriteBatch.Draw(hudrectangle, new Rectangle(GraphicsDevice.Viewport.Width * 50 / 89, GraphicsDevice.Viewport.Height / 15, c.Super * 12 / 5, 20), Color.Aqua); //purple special bar
                            spriteBatch.Draw(hudrectangle, new Rectangle(GraphicsDevice.Viewport.Width * 1 / 8, GraphicsDevice.Viewport.Height * 200 / 211, 915, 35), Color.DodgerBlue); //rectangle around energy bar
                            spriteBatch.Draw(hudrectangle, new Rectangle(GraphicsDevice.Viewport.Width * 1 / 7, GraphicsDevice.Viewport.Height * 1000 / 1047, 860, 20), Color.Red); //red energy bar
                            spriteBatch.Draw(hudrectangle, new Rectangle(GraphicsDevice.Viewport.Width * 1 / 7, GraphicsDevice.Viewport.Height * 1000 / 1047, c.Energy * 43 / 10, 20), Color.Gold); //sugar-free energy bar
                            break;
                    }

                    //spriteBatch.DrawString(lFont, "Energy: " + c.Energy, new Vector2(100,0),Color.Black);

                    // Switch statement that draws the image for the ability the player is using for the interface
                    switch (fullscreen)
                    {
                        case true:
                            switch (aState)
                            {
                                case AbilityState.a1:
                                    spriteBatch.Draw(meleeStillImage, new Rectangle(GraphicsDevice.Viewport.Width * 25 / 53, GraphicsDevice.Viewport.Height / 18, 40, 40), Color.White);
                                    spriteBatch.Draw(mine, new Rectangle(GraphicsDevice.Viewport.Width * 25 / 57, GraphicsDevice.Viewport.Height / 55, 30, 30), Color.White); //prev wpn
                                    spriteBatch.Draw(bulletImage, new Rectangle(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 100, 45, 45), new Rectangle(32, 0, 32, 32), Color.White); //next wpn
                                    break;
                                /*case AbilityState.a2:
                                    spriteBatch.Draw(bulletImage, new Rectangle(GraphicsDevice.Viewport.Width * 25 / 53, GraphicsDevice.Viewport.Height / 20, 40, 40), new Rectangle(32, 0, 32, 32), Color.White);
                                    spriteBatch.Draw(meleeStillImage, new Rectangle(GraphicsDevice.Viewport.Width * 25 / 57, GraphicsDevice.Viewport.Height / 55, 30, 30), Color.White);
                                    spriteBatch.Draw(bulletImage, new Rectangle(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 100, 45, 45), new Rectangle(32, 0, 32, 32), Color.White);
                                    break;*/
                                case AbilityState.a2:
                                    spriteBatch.Draw(bulletImage, new Rectangle(GraphicsDevice.Viewport.Width * 50 / 107, GraphicsDevice.Viewport.Height / 22, 55, 55), new Rectangle(32, 0, 32, 32), Color.White);
                                    spriteBatch.Draw(meleeStillImage, new Rectangle(GraphicsDevice.Viewport.Width * 25 / 57, GraphicsDevice.Viewport.Height / 55, 30, 30), Color.White);
                                    spriteBatch.Draw(bulletImage, new Rectangle(GraphicsDevice.Viewport.Width * 100 / 199, GraphicsDevice.Viewport.Height / 55, 20, 20), new Rectangle(32, 0, 32, 32), Color.White);
                                    break;
                                case AbilityState.a3:
                                    spriteBatch.Draw(bulletImage, new Rectangle(GraphicsDevice.Viewport.Width * 100 / 211, GraphicsDevice.Viewport.Height / 18, 30, 30), new Rectangle(32, 0, 32, 32), Color.White);
                                    spriteBatch.Draw(bulletImage, new Rectangle(GraphicsDevice.Viewport.Width * 10 / 23, GraphicsDevice.Viewport.Height / 55, 45, 45), new Rectangle(32, 0, 32, 32), Color.White);
                                    spriteBatch.Draw(grenade, new Rectangle(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 55, 30, 30), Color.White);
                                    break;
                                case AbilityState.a4:
                                    spriteBatch.Draw(grenade, new Rectangle(GraphicsDevice.Viewport.Width * 100 / 211, GraphicsDevice.Viewport.Height / 20, 30, 30), Color.White);
                                    spriteBatch.Draw(bulletImage, new Rectangle(GraphicsDevice.Viewport.Width * 10 / 22, GraphicsDevice.Viewport.Height / 54, 20, 20), new Rectangle(32, 0, 32, 32), Color.White);
                                    spriteBatch.Draw(mine, new Rectangle(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 55, 30, 30), Color.White);
                                    break;
                                case AbilityState.a5:
                                    spriteBatch.Draw(mine, new Rectangle(GraphicsDevice.Viewport.Width * 100 / 211, GraphicsDevice.Viewport.Height / 18, 30, 30), Color.White);
                                    spriteBatch.Draw(grenade, new Rectangle(GraphicsDevice.Viewport.Width * 25 / 57, GraphicsDevice.Viewport.Height / 65, 30, 30), Color.White);
                                    spriteBatch.Draw(meleeStillImage, new Rectangle(GraphicsDevice.Viewport.Width * 50 / 99, GraphicsDevice.Viewport.Height / 55, 30, 30), Color.White);
                                    break;
                            }
                            break;
                        case false:
                            switch (aState)
                            {
                                case AbilityState.a1:
                                    spriteBatch.Draw(meleeStillImage, new Rectangle(640, 45, 40, 40), Color.White);
                                    spriteBatch.Draw(mine, new Rectangle(590, 10, 20, 20), Color.White); //prev wpn
                                    spriteBatch.Draw(bulletImage, new Rectangle(700, 0, 30, 30), new Rectangle(32, 0, 32, 32), Color.White); //next wpn
                                    break;
                                /*case AbilityState.a2:
                                    spriteBatch.Draw(bulletImage, new Rectangle(640, 40, 40, 40), new Rectangle(32, 0, 32, 32), Color.White);
                                    spriteBatch.Draw(meleeStillImage, new Rectangle(580, 5, 30, 30), Color.White);
                                    spriteBatch.Draw(bulletImage, new Rectangle(694, -10, 45, 45), new Rectangle(32, 0, 32, 32), Color.White);
                                    break;*/
                                case AbilityState.a2:
                                    spriteBatch.Draw(bulletImage, new Rectangle(633, 30, 55, 55), new Rectangle(32, 0, 32, 32), Color.White);
                                    spriteBatch.Draw(meleeStillImage, new Rectangle(580, 5, 30, 30), Color.White);
                                    spriteBatch.Draw(bulletImage, new Rectangle(700, 10, 20, 20), new Rectangle(32, 0, 32, 32), Color.White);
                                    break;
                                case AbilityState.a3:
                                    spriteBatch.Draw(bulletImage, new Rectangle(646, 45, 30, 30), new Rectangle(32, 0, 32, 32), Color.White);
                                    spriteBatch.Draw(bulletImage, new Rectangle(581, -10, 45, 45), new Rectangle(32, 0, 32, 32), Color.White);
                                    spriteBatch.Draw(grenade, new Rectangle(710, 5, 30, 30), Color.White);
                                    break;
                                case AbilityState.a4:
                                    spriteBatch.Draw(grenade, new Rectangle(646, 45, 30, 30), Color.White);
                                    spriteBatch.Draw(bulletImage, new Rectangle(581, 10, 20, 20), new Rectangle(32, 0, 32, 32), Color.White);
                                    spriteBatch.Draw(mine, new Rectangle(710, 5, 30, 30), Color.White);
                                    break;
                                case AbilityState.a5:
                                    spriteBatch.Draw(mine, new Rectangle(646, 45, 30, 30), Color.White);
                                    spriteBatch.Draw(grenade, new Rectangle(581, -10, 45, 45), Color.White);
                                    spriteBatch.Draw(meleeStillImage, new Rectangle(710, 5, 30, 30), Color.White);
                                    break;
                            }
                            break;
                    }
                    break;

                case GameState.Paused:
                    // Draw background
                    spriteBatch.Draw(background, new Vector2(backgroundPoint.X, backgroundPoint.Y), Color.White);

                    // Draw boxes
                    foreach (Rectangle o in objects)
                    {
                        if (o.Width == 50 && o.Height == 75)
                        {
                            spriteBatch.Draw(garbage, o, Color.White);
                        }
                        else if (o.Width == 75 && o.Height == 75)
                        {
                            spriteBatch.Draw(boxes, o, Color.White);
                        }
                        else if (o.Width == 200 && o.Height == 100)
                        {
                            spriteBatch.Draw(car, o, Color.White);
                        }

                    }

                    // Draw projectiles
                    foreach (Projectile p in projectiles)
                    {
                        if (p is PBasic)
                        {
                            spriteBatch.Draw(p.Image, new Rectangle(p.Position.X + p.Position.Width / 2, p.Position.Y + p.Position.Height / 2, p.Position.Width, p.Position.Height), new Rectangle(0, 0, 32, 32), Color.White, p.Angle - (float)Math.PI / 2, new Vector2(16, 16), SpriteEffects.None, 0);
                        }
                        if (p is PStationary)
                        {
                            PStationary ps = (PStationary)p;
                            ps.Draw(spriteBatch, frameProjectile, rotationAngle);
                        }
                        if (p is PExplosive)
                        {
                            PExplosive ex = (PExplosive)p;
                            if (ex.ExplosionCount == 0) ex.Draw(spriteBatch);
                            else spriteBatch.Draw(meleeImage, ex.Explosion, Color.White);
                        }
                        if (p is PMine)
                        {
                            PMine mine = (PMine)p;
                            if (mine.ExplosionCount == 0) mine.Draw(spriteBatch);
                            else spriteBatch.Draw(meleeImage, mine.Explosion, Color.White);
                        }
                    }

                    // Draw the player
                    spriteBatch.Draw(playerImage, new Rectangle(c.Position.X + c.Position.Width / 2, c.Position.Y + c.Position.Height / 2, c.Position.Width, c.Position.Height), new Rectangle(0, 0, 32, 32), Color.White, rotationAngle - (float)Math.PI / 2, new Vector2(16, 16), SpriteEffects.None, 0);

                    // Draw all alive enemies
                    foreach (Enemy e in enemies)
                    {

                        if (e.Alive == true)
                        {
                            // Draw the enemy at it's position plus half its size, and rotate it based on enemyAngle
                            int aX = e.Position.X - c.Position.X;
                            int aY = e.Position.Y - c.Position.Y;
                            float enemyAngle = -(float)(Math.Atan2(aX, aY) + Math.PI / 2);
                            spriteBatch.Draw(e.Image, new Rectangle(e.Position.X + e.Position.Width / 2, e.Position.Y + e.Position.Height / 2, e.Position.Width, e.Position.Height), new Rectangle(0, 0, 32, 32), Color.White, enemyAngle - (float)Math.PI / 2, new Vector2(16, 16), SpriteEffects.None, 0);
                        }
                        else if (e.SpawnCount != -1)
                        {
                            Vector2 origin = new Vector2(eMarker.Width / 2, eMarker.Height / 2);
                            spriteBatch.Draw(eMarker, new Vector2(e.Position.X, e.Position.Y), null, Color.White, 0, origin, (float)e.SpawnCount / 30, SpriteEffects.None, 0);
                        }

                    }

                    // Draw pause
                    spriteBatch.Draw(whiteBox, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.Gray * .5f);
                    spriteBatch.Draw(paused, new Rectangle((GraphicsDevice.Viewport.Width / 2) - (paused.Width / 8), (GraphicsDevice.Viewport.Height / 2) - (paused.Height / 2), paused.Width / 4, paused.Height / 4), Color.White);
                    
                    // Draw menu
                    mRectangle = new Rectangle(mState.Position.X, mState.Position.Y, 1, 1);
                    if (rMButton.Intersects(mRectangle))
                    {
                        spriteBatch.Draw(menu, rMButton, Color.Red);
                    }
                    else
                    {
                        spriteBatch.Draw(menu, rMButton, Color.White);
                    }
                    // Draw options
                    if (rPOButton.Intersects(mRectangle))
                    {
                        spriteBatch.Draw(optionsButton, rPOButton, Color.Red);
                    }
                    else
                    {
                        spriteBatch.Draw(optionsButton, rPOButton, Color.White);
                    }
                    break;

                // Game is in Options
                case GameState.Options:
                    spriteBatch.Draw(titleBackground, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);

                    // Mouse rectangle
                    mRectangle = new Rectangle(mState.Position.X, mState.Position.Y, 1, 1);

                    // If the game is not in the process of closing
                    if (closing == false)
                    {
                        // Draw buttons and change button color based on mouse over or settings currently selected
                        if (fullscreen == false)
                        {
                            spriteBatch.Draw(fullscreenButton, rFButton, Color.White);
                        }
                        else
                        {
                            spriteBatch.Draw(fullscreenButton, rFButton, Color.Red);
                        }

                        if (rMButton.Intersects(mRectangle))
                        {
                            spriteBatch.Draw(menu, rMButton, Color.Red);
                        }
                        else
                        {
                            spriteBatch.Draw(menu, rMButton, Color.White);
                        }
                        if(Bcont == true)
                        {
                            if (contButton.Intersects(mRectangle))
                            {
                                spriteBatch.Draw(continueButton, contButton, Color.Red);
                            }
                            else
                            {
                                spriteBatch.Draw(continueButton, contButton, Color.White);
                            }
                        }
                    }

                    // Otherwise, the game is about to close, ask user for confirmation
                    else
                    {
                        spriteBatch.DrawString(sFont, "The game must be closed to apply these changes", new Vector2(GraphicsDevice.Viewport.Width/2 - 200, 50), Color.Black);

                        // Draw buttons and change colors on mouse over
                        if (rOKButton.Intersects(mRectangle))
                        {
                            spriteBatch.Draw(okButton, rOKButton, Color.Red);
                        }
                        else
                        {
                            spriteBatch.Draw(okButton, rOKButton, Color.White);
                        }
                        if (rCancelButton.Intersects(mRectangle))
                        {
                            spriteBatch.Draw(cancelButton, rCancelButton, Color.Red);
                        }
                        else
                        {
                            spriteBatch.Draw(cancelButton, rCancelButton, Color.White);
                        }
                    }
                    break;

                case GameState.GameOver:
                    spriteBatch.Draw(titleBackground, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);

                    mRectangle = new Rectangle(mState.Position.X, mState.Position.Y, 1, 1);
                    if(enteringName == true)
                    {
                        spriteBatch.DrawString(lFont, "NEW HIGHSCORE!", new Vector2(GraphicsDevice.Viewport.Width / 2 - 125, 200), Color.Black);
                        spriteBatch.DrawString(lFont, "Enter Your Name: " + lHandler.Name, new Vector2(GraphicsDevice.Viewport.Width/2 - 225, 250), Color.Black);
                        // Draw buttons and change colors on mouse over
                        if (rOKButton.Intersects(mRectangle))
                        {
                            spriteBatch.Draw(okButton, rOKButton, Color.Red);
                        }
                        else
                        {
                            spriteBatch.Draw(okButton, rOKButton, Color.White);
                        }
                    }
                    else
                    {
                        spriteBatch.DrawString(lFont, "GAME OVER", new Vector2(GraphicsDevice.Viewport.Width / 2 - 50, 50), Color.Black);
                        spriteBatch.DrawString(lFont, "Round: " + round, new Vector2(GraphicsDevice.Viewport.Width / 2 - 40, 100), Color.Black);
                        spriteBatch.DrawString(lFont, "Score: " + score, new Vector2(GraphicsDevice.Viewport.Width / 2 - 40, 130), Color.Black);

                        spriteBatch.DrawString(lFont, "Current Highest Score: ", new Vector2(GraphicsDevice.Viewport.Width / 2 - 500, 200), Color.Black);
                        spriteBatch.DrawString(lFont, leaderboardNames[0] +  " -   Round " + leaderboardRounds[0] + "   Score " + leaderboardScores[0], new Vector2(GraphicsDevice.Viewport.Width / 2 - 50, 200), Color.Black);
                        switch(leaderboardCharacters[0])
                        {
                            case "Fire": spriteBatch.Draw(player1Image, new Rectangle(GraphicsDevice.Viewport.Width / 2 - 100, 207, 32, 32), new Rectangle(0, 0, 32, 32), Color.White);
                                break;
                            case "Earth":
                                spriteBatch.Draw(player2Image, new Rectangle(GraphicsDevice.Viewport.Width / 2 - 100, 207, 32, 32), new Rectangle(0, 0, 32, 32), Color.White);
                                break;
                            case "Water":
                                spriteBatch.Draw(player4Image, new Rectangle(GraphicsDevice.Viewport.Width / 2 - 100, 207, 32, 32), new Rectangle(0, 0, 32, 32), Color.White);
                                break;
                            case "Electric":
                                spriteBatch.Draw(player3Image, new Rectangle(GraphicsDevice.Viewport.Width / 2 - 100, 207, 32, 32), new Rectangle(0, 0, 32, 32), Color.White);
                                break;
                        }

                        if (rSButton.Intersects(mRectangle))
                        {
                            spriteBatch.Draw(startButton, rSButton, Color.Red);
                        }
                        else
                        {
                            spriteBatch.Draw(startButton, rSButton, Color.White);
                        }

                        if (rMButton.Intersects(mRectangle))
                        {
                            spriteBatch.Draw(menu, rMButton, Color.Red);
                        }
                        else
                        {
                            spriteBatch.Draw(menu, rMButton, Color.White);
                        }
                    }
                    break;

                case GameState.Leaderboard:
                    spriteBatch.Draw(titleBackground, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);

                    if (fullscreen == true)
                    {
                        spriteBatch.Draw(leaderboardBackground, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(leaderboardBackground, new Rectangle(-320, 0, GraphicsDevice.Viewport.Width + 625, GraphicsDevice.Viewport.Height + 300), Color.White);
                    }

                    mRectangle = new Rectangle(mState.Position.X, mState.Position.Y, 1, 1);
                    spriteBatch.DrawString(lFont, "LEADERBOARD", new Vector2(GraphicsDevice.Viewport.Width / 2 - 100, 30), Color.White);

                    spriteBatch.DrawString(lFont, "Name", new Vector2(GraphicsDevice.Viewport.Width / 2 - 300, 100), Color.White);
                    spriteBatch.DrawString(lFont, "Round", new Vector2(GraphicsDevice.Viewport.Width / 2, 100), Color.White);
                    spriteBatch.DrawString(lFont, "Score", new Vector2(GraphicsDevice.Viewport.Width / 2 + 200, 100), Color.White);

                    for(int i = 1; i <= 5; i++)
                    {
                        switch (leaderboardCharacters[i - 1])
                        {
                            case "Fire":
                                spriteBatch.Draw(player1Image, new Rectangle((GraphicsDevice.Viewport.Width / 2 - 475), 100 + (75 * i), 50, 50), new Rectangle(0, 0, 32, 32), Color.White);
                                break;
                            case "Earth":
                                spriteBatch.Draw(player2Image, new Rectangle((GraphicsDevice.Viewport.Width / 2 - 475), 100 + (75 * i), 50, 50), new Rectangle(0, 0, 32, 32), Color.White);
                                break;
                            case "Water":
                                spriteBatch.Draw(player4Image, new Rectangle((GraphicsDevice.Viewport.Width / 2 - 475), 100 + (75 * i), 50, 50), new Rectangle(0, 0, 32, 32), Color.White);
                                break;
                            case "Electric":
                                spriteBatch.Draw(player3Image, new Rectangle((GraphicsDevice.Viewport.Width / 2 - 475), 100 + (75 * i), 50, 50), new Rectangle(0, 0, 32, 32), Color.White);
                                break;
                            default: break;
                        }
                        spriteBatch.DrawString(lFont, i + ".", new Vector2((GraphicsDevice.Viewport.Width / 2 - 400), 100 + (75 * i)), Color.White);
                        spriteBatch.DrawString(lFont, leaderboardNames[i - 1], new Vector2((GraphicsDevice.Viewport.Width / 2 - 350), 100 + (75 * i)), Color.White);
                        spriteBatch.DrawString(lFont, leaderboardRounds[i - 1].ToString(), new Vector2((GraphicsDevice.Viewport.Width / 2 + 25), 100 + (75 * i)), Color.White);
                        spriteBatch.DrawString(lFont, leaderboardScores[i - 1].ToString(), new Vector2((GraphicsDevice.Viewport.Width / 2 + 200), 100 + (75 * i)), Color.White);
                    }

                    if (rMButton.Intersects(mRectangle))
                    {
                        spriteBatch.Draw(menu, rMButton, Color.Red);
                    }
                    else
                    {
                        spriteBatch.Draw(menu, rMButton, Color.White);
                    }
                    break;

                case GameState.Instructions:
                    spriteBatch.Draw(titleBackground, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                    mRectangle = new Rectangle(mState.Position.X, mState.Position.Y, 1, 1);
                    spriteBatch.Draw(instructionsScreen, new Vector2(GraphicsDevice.Viewport.Width / 2 - instructionsScreen.Width / 2, 20), Color.White);
                    if (rMButton.Intersects(mRectangle))
                    {
                        spriteBatch.Draw(menu, rMButton, Color.Red);
                    }
                    else
                    {
                        spriteBatch.Draw(menu, rMButton, Color.White);
                    }

                    if (rNextButton.Intersects(mRectangle))
                    {
                        spriteBatch.Draw(nextB, rNextButton, Color.Red);
                    }
                    else
                    {
                        spriteBatch.Draw(nextB, rNextButton, Color.White);
                    }
                    break;

                case GameState.Instructions2:
                    spriteBatch.Draw(titleBackground, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                    mRectangle = new Rectangle(mState.Position.X, mState.Position.Y, 1, 1);
                    spriteBatch.Draw(instructionsScreen2, new Vector2(GraphicsDevice.Viewport.Width / 2 - instructionsScreen2.Width / 2, 20), Color.White);
                    if (rMButton.Intersects(mRectangle))
                    {
                        spriteBatch.Draw(menu, rMButton, Color.Red);
                    }
                    else
                    {
                        spriteBatch.Draw(menu, rMButton, Color.White);
                    }

                    if (rNextButton.Intersects(mRectangle))
                    {
                        spriteBatch.Draw(nextB, rNextButton, Color.Red);
                    }
                    else
                    {
                        spriteBatch.Draw(nextB, rNextButton, Color.White);
                    }
                    break;

                case GameState.Instructions3:
                    spriteBatch.Draw(titleBackground, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                    mRectangle = new Rectangle(mState.Position.X, mState.Position.Y, 1, 1);
                    spriteBatch.Draw(instructionsScreen3, new Vector2(GraphicsDevice.Viewport.Width / 2 - instructionsScreen3.Width / 2, 20), Color.White);
                    if (rMButton.Intersects(mRectangle))
                    {
                        spriteBatch.Draw(menu, rMButton, Color.Red);
                    }
                    else
                    {
                        spriteBatch.Draw(menu, rMButton, Color.White);
                    }

                    if (rNextButton.Intersects(mRectangle))
                    {
                        spriteBatch.Draw(nextB, rNextButton, Color.Red);
                    }
                    else
                    {
                        spriteBatch.Draw(nextB, rNextButton, Color.White);
                    }
                    break;

                case GameState.Instructions4:
                    spriteBatch.Draw(titleBackground, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                    mRectangle = new Rectangle(mState.Position.X, mState.Position.Y, 1, 1);
                    spriteBatch.Draw(instructionsScreen4, new Vector2(GraphicsDevice.Viewport.Width / 2 - instructionsScreen4.Width / 2, 20), Color.White);
                    if (rMButton.Intersects(mRectangle))
                    {
                        spriteBatch.Draw(menu, rMButton, Color.Red);
                    }
                    else
                    {
                        spriteBatch.Draw(menu, rMButton, Color.White);
                    }
                    break;
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}