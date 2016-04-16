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

enum GameState { Menu, HordeMode, Paused, Options, CharacterSelection, GameOver}; // GameState enum for keeping track of what state our game is in
enum AbilityState { a1, a2, a3, a4 }; // AbilityState enum for keeping track of the ability the player is using
enum HeroState { Still, Walking }; // HeroState enum for keeping track of the state of the player
enum SwitchHero { Fire, Earth, Water, Electric}; // switch heroes

namespace GroupGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont sFont;
        GameState gState;
        AbilityState aState;
        SwitchHero switchHero = SwitchHero.Fire;
        HeroState heroState = HeroState.Still;
        Random rgen = new Random();

        // Images
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
        Texture2D startButton;
        Texture2D optionsButton;
        Texture2D fullscreenButton;
        Texture2D okButton;
        Texture2D cancelButton;
        Texture2D whiteBox;
        Texture2D paused;
        Texture2D menu;
        Texture2D rectangle;
        Texture2D circle;
        Texture2D fireButton;
        Texture2D earthButton;
        Texture2D electricButton;
        Texture2D waterButton;
        Texture2D background;
        Texture2D dot;
        Texture2D boxes;
        Texture2D basicImage;
        Texture2D stallImage;


        // Rectangles for buttons and mouse
        Rectangle rSButton;
        Rectangle rOButton;
        Rectangle mRectangle;
        Rectangle rMButton;
        Rectangle rFButton;
        Rectangle char1;
        Rectangle char2;
        Rectangle char3;
        Rectangle char4;
        
        // Characters, enemies, and projectiles
        Character c;
        List<Enemy> enemies = new List<Enemy>();
        List<Projectile> projectiles = new List<Projectile>();
        List<EnemyProjectile> eProjectiles = new List<EnemyProjectile>();

        // Keyboard states
        KeyboardState kbState;
        KeyboardState previousKbState;
        MouseState mState;
        MouseState prevMState;
        float rotationAngle;

        // Ints for round and score
        int round;
        int score;

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

        // Method for advancing the round of our Horde Mode
        public void AdvanceRound()
        {
            // Clear Enemies list
            enemies.Clear();

            // Select a random round to use
            //int num = rgen.Next(1,round+1);
            BinaryReader reader;
            if (round < 7) reader = new BinaryReader(File.OpenRead(@"../../../Rounds/Round" + (round + 1) + ".dat"));
            else reader = new BinaryReader(File.OpenRead(@"../../../Rounds/Round7.dat"));

            // Try block
            try
            {
                // While loop that will run until the end of the file
                while(true)
                {
                    // Read in information from the round file
                    string type = reader.ReadString();
                    int x = reader.ReadInt32();
                    int y = reader.ReadInt32();

                    // Switch statement based on the type string, creates an enemy based on the type and adds it to the enemies list
                    switch(type)
                    {
                        case "1":
                            Enemy e1 = new Enemy1(c.Position.X-500+x, c.Position.Y-300+y);
                            e1.Image = enemyImage;
                            enemies.Add(e1);
                            break;

                        case "2":
                            Enemy e2 = new Enemy2(c.Position.X - 500 + x, c.Position.Y - 300 + y);
                            e2.Image = enemyImage;
                            enemies.Add(e2);
                            break;

                        case "3":
                            Enemy e3 = new Enemy3(c.Position.X - 500 + x, c.Position.Y - 300 + y);
                            e3.Image = enemyImage;
                            enemies.Add(e3);
                            break;
                    }
                }
            }
            // Silently catch EndOfStreamExceptions
            catch(EndOfStreamException){}

            objects.Clear();

            for (int i = 0; i < 5; i++)
            {
                int x = rgen.Next(80, 1220);
                int y = rgen.Next(80, 1020);

                Rectangle randObj = new Rectangle(x, y, 150, 150);

                bool collision = false;
                foreach(Enemy e in enemies)
                {
                    if(randObj.Intersects(e.Position) == true)
                    {
                        collision = true;
                    }
                }
                if (collision == true || randObj.Intersects(c.Position) == true)
                {
                    i--;
                }
                else
                {
                    bool boxCollision = false;
                    foreach(Rectangle box in objects)
                    {
                        if(randObj.Intersects(box))
                        {
                            boxCollision = true;
                        }
                    }
                    if(boxCollision == false)
                    {
                        objects.Add(randObj);
                    }
                    else
                    {
                        i--;
                    }
                }
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
                    if(collides == false)
                    {
                        c.Position = new Rectangle(c.Position.X, c.Position.Y - c.Speed, c.Position.Width, c.Position.Height);
                        c.CRect = new Rectangle(c.Position.X + 10, c.Position.Y + 10, c.CRect.Width, c.CRect.Height);
                    }
                }
                else if (c.Position.Y > 0) ScreenMove("up", c.Speed);
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
                        c.CRect = new Rectangle(c.Position.X + 10, c.Position.Y + 10, c.CRect.Width, c.CRect.Height);
                    }
                }
                else if (c.Position.X > 0) ScreenMove("left", c.Speed);
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
                        c.CRect = new Rectangle(c.Position.X + 10, c.Position.Y + 10, c.CRect.Width, c.CRect.Height);
                    }
                }
                else if (c.Position.Y < GraphicsDevice.Viewport.Height - c.Position.Height) ScreenMove("down", c.Speed);
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
                        c.CRect = new Rectangle(c.Position.X + 10, c.Position.Y + 10, c.CRect.Width, c.CRect.Height);
                    }
                }
                else if (c.Position.X < GraphicsDevice.Viewport.Width - c.Position.Width) ScreenMove("right", c.Speed);
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
                    if (kbState.IsKeyDown(Keys.Q) && previousKbState.IsKeyUp(Keys.Q)) aState = AbilityState.a4;
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
                    if (kbState.IsKeyDown(Keys.E) && previousKbState.IsKeyUp(Keys.E)) aState = AbilityState.a1;
                    if (kbState.IsKeyDown(Keys.Q) && previousKbState.IsKeyUp(Keys.Q)) aState = AbilityState.a3;
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
                        projectiles.Add(new PStationary(5, 60, 60, c, rotationAngle, 15, true, meleeImage)); // Create a new projectile that will travel in the direction of the mouse
                        c.ShotDelay = 40; // Set the ShotDelay of the player. We can change this value depending on ability, stronger attacks have longer delays
                        break;

                    // The original shooting attack
                    case AbilityState.a2:
                        projectiles.Add(new PBasic(25, 40,40, c, rotationAngle, 100, false, bulletImage));
                        c.ShotDelay = 20; 
                        break;

                    // Test of a piercing attack with a different size
                    case AbilityState.a3:
                        projectiles.Add(new PBasic(10, 100, 100, c, rotationAngle, 120, true, bulletImage));
                        c.ShotDelay = 80;
                        break;

                    // Test of a rapid fire attack
                    case AbilityState.a4:
                        //projectiles.Add(new PBasic(3, 30, 30, c, rotationAngle, 90, false, bulletImage));
                        projectiles.Add(new PExplosive(150, 30, 30, c, rotationAngle, 90, dot));
                        c.ShotDelay = 120;
                        break;
                }
            }
        }

        // Method for the player dashing
        public void PlayerDash()
        {
            if (mState.RightButton == ButtonState.Pressed && c.Dashing == 0)
            {
                if (kbState.IsKeyDown(Keys.W) && kbState.IsKeyDown(Keys.D)) c.Dashing = 2;
                else if (kbState.IsKeyDown(Keys.W) && kbState.IsKeyDown(Keys.A)) c.Dashing = 8;
                else if (kbState.IsKeyDown(Keys.S) && kbState.IsKeyDown(Keys.D)) c.Dashing = 4;
                else if (kbState.IsKeyDown(Keys.S) && kbState.IsKeyDown(Keys.A)) c.Dashing = 6;
                else if (kbState.IsKeyDown(Keys.W)) c.Dashing = 1;
                else if (kbState.IsKeyDown(Keys.D)) c.Dashing = 3;
                else if (kbState.IsKeyDown(Keys.S)) c.Dashing = 5;
                else if (kbState.IsKeyDown(Keys.A)) c.Dashing = 7;
            }
        }

        // Method for reseting the game
        public void ResetGame()
        {
            enemies.Clear();
            projectiles.Clear();
            c.Position = new Rectangle(GraphicsDevice.Viewport.Width / 2 - c.Position.Width / 2, GraphicsDevice.Viewport.Height / 2 - c.Position.Height / 2, c.Position.Width, c.Position.Height);
            maxX = 1500 - GraphicsDevice.Viewport.Width;
            if (maxX < 0) maxX = 0;
            maxY = 1000 - GraphicsDevice.Viewport.Height;
            if (maxY < 0) maxY = 0;
            backgroundPoint = new Point(0 - maxX, 0 - maxY);
            globalX = maxX/2;
            globalY = maxY/2;
            c.Health = 100;
            round = 0;
            score = 0;
            aState = AbilityState.a1;
            AdvanceRound();
        }

        // Override the game exiting to create a config file upon exiting that will save user settings
        protected override void OnExiting(object sender, EventArgs args)
        {
            StreamWriter sw = new StreamWriter((File.OpenWrite(@"../../../Config.txt")));
            sw.WriteLine("fullscreen = " + fullscreen);
            sw.Close();
            base.OnExiting(sender, args);
        }

        // Game Constructor
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // Adjust window size
            graphics.PreferredBackBufferWidth = 1200;
            graphics.PreferredBackBufferHeight = 700;
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
            }

            graphics.IsFullScreen = fullscreen;
            gState = GameState.Menu;

            objects = new List<Rectangle>();

            // random generator
            Random rgen = new Random();

            // creates 5 combinations of x and y coordinates and adds that box to the list
            for (int i = 0; i < 5; i++)
            {
                int x = rgen.Next(80, 1220);
                int y = rgen.Next(80, 1020);

                Rectangle randObj = new Rectangle(x, y, 150, 150);
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
            rectangle = this.Content.Load<Texture2D>("WhiteRectangle");
            circle = this.Content.Load<Texture2D>("WhiteCircle");
            fireButton = this.Content.Load<Texture2D>("fireButton");
            earthButton = this.Content.Load<Texture2D>("earthButton");
            electricButton = this.Content.Load<Texture2D>("lightningButton");
            waterButton = this.Content.Load<Texture2D>("waterButton");
            dot = this.Content.Load<Texture2D>("Dot");
            stallImage = Content.Load<Texture2D>("StallProjectile");
            basicImage = Content.Load<Texture2D>("BasicProjectile");

            // Load fonts
            sFont = this.Content.Load<SpriteFont>("SpriteFont1");

            // loads background
            background = this.Content.Load<Texture2D>("background");

            // loads boxes
            boxes = this.Content.Load<Texture2D>("boxes");
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
                    
                    // Checks to see if the start button has been pressed
                    rSButton = new Rectangle((GraphicsDevice.Viewport.Width / 2) - (rSButton.Width/2), (GraphicsDevice.Viewport.Height / 2) - (rSButton.Height/2), startButton.Width/4, startButton.Height/4);
                    rOButton = new Rectangle((GraphicsDevice.Viewport.Width / 2) - (rOButton.Width / 2), (GraphicsDevice.Viewport.Height / 2) + (rOButton.Height), startButton.Width / 4, startButton.Height / 4);
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
                        ResetGame();
                        gState = GameState.HordeMode;
                    }

                    if (mState.LeftButton == ButtonState.Pressed && prevMState.LeftButton != ButtonState.Pressed && mRectangle.Intersects(char2))
                    {
                        switchHero = SwitchHero.Earth;
                        playerImage = player2Image;
                        playerWalking = player2Walking;
                        bulletImage = bullet2Image;
                        ResetGame();
                        gState = GameState.HordeMode;
                    }

                    if (mState.LeftButton == ButtonState.Pressed && prevMState.LeftButton != ButtonState.Pressed && mRectangle.Intersects(char3))
                    {
                        switchHero = SwitchHero.Water;
                        playerImage = player4Image;
                        playerWalking = player4Walking;
                        bulletImage = bullet4Image;
                        ResetGame();
                        gState = GameState.HordeMode;
                    }

                    if (mState.LeftButton == ButtonState.Pressed && prevMState.LeftButton != ButtonState.Pressed && mRectangle.Intersects(char4))
                    {
                        switchHero = SwitchHero.Electric;
                        playerImage = player3Image;
                        playerWalking = player3Walking;
                        bulletImage = bullet3Image;
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
                    foreach(Rectangle o in objects)
                    {
                        if(o.Intersects(c.Position))
                        {
                            c.Position = new Rectangle(c.Position.X, c.Position.Y, c.Position.Width, c.Position.Height);
                        }
                    }

                    foreach (Rectangle o in objects)
                    {
                        foreach (Enemy e in enemies)
                        {
                            if (o.Intersects(e.Position))
                            {
                                e.Position = new Rectangle(e.Position.X, e.Position.Y, e.Position.Width, e.Position.Height);
                            }
                        }
                    }

                    // Find the angle between the player and the mouse, use this to rotate the player when drawing
                    int rotX = mState.X - (c.Position.X + c.Position.Width/2);
                    int rotY = mState.Y - (c.Position.Y + c.Position.Height/2);
                    rotationAngle = (float)Math.Atan2(rotY, rotX);

                    PlayerDash();
                    if (c.Dashing == 0)
                    {
                        PlayerMove();
                        PlayerChangeAbility();
                        PlayerShoot();
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
                                foreach(Rectangle r in objects)
                                {
                                    if (new Rectangle(c.Position.X, (int)(c.Position.Y - c.Speed * 2.5), c.Position.Width, c.Position.Height).Intersects(r)) collides1 = true;
                                }
                                if ((c.Position.Y > 100 && globalY >= 0) || (globalY <= 0 && c.Position.Y > 0))
                                {
                                    if (collides1 == false) c.Position = new Rectangle(c.Position.X, (int)(c.Position.Y - c.Speed * 2.5), c.Position.Width, c.Position.Height);
                                }
                                    
                                else if (c.Position.Y > 0) ScreenMove("up", (int)(c.Speed * 2.5));
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
                                else if (c.Position.X < GraphicsDevice.Viewport.Width - c.Position.Width) ScreenMove("right", (int)(c.Speed * 1.77));
                                if ((c.Position.Y > 100 && globalY >= 0) || (globalY <= 0 && c.Position.Y > 0))
                                {
                                   if (collides2 == false) c.Position = new Rectangle(c.Position.X, (int)(c.Position.Y - c.Speed * 1.77), c.Position.Width, c.Position.Height);
                                }
                                else if (c.Position.Y > 0) ScreenMove("up", (int)(c.Speed * 1.77));
                                break;
                            case 3:
                                c.DashCount++;
                                foreach (Rectangle r in objects)
                                {
                                    if (new Rectangle((int)(c.Position.X + c.Speed * 2.5), c.Position.Y, c.Position.Width, c.Position.Height).Intersects(r)) collides1 = true;
                                }
                                if ((c.Position.X < GraphicsDevice.Viewport.Width - (100 + c.Position.Width) && globalX <= maxX) || (globalX == maxX && c.Position.X < GraphicsDevice.Viewport.Width - c.Position.Width))
                                {
                                    if(collides1 == false) c.Position = new Rectangle((int)(c.Position.X + c.Speed * 2.5), c.Position.Y, c.Position.Width, c.Position.Height);
                                }
                                    
                                else if (c.Position.X < GraphicsDevice.Viewport.Width - c.Position.Width) ScreenMove("right", (int)(c.Speed * 2.5));
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
                                    if(collides1 == false) c.Position = new Rectangle((int)(c.Position.X + c.Speed * 1.77), c.Position.Y, c.Position.Width, c.Position.Height);
                                }
                                else if (c.Position.X < GraphicsDevice.Viewport.Width - c.Position.Width) ScreenMove("right", (int)(c.Speed * 1.77));
                                if ((c.Position.Y < GraphicsDevice.Viewport.Height - (100 + c.Position.Height) && globalY <= maxY) || (globalY == maxY && c.Position.Y < GraphicsDevice.Viewport.Height - c.Position.Height))
                                {
                                    if (collides2 == false) c.Position = new Rectangle(c.Position.X, (int)(c.Position.Y + c.Speed * 1.77), c.Position.Width, c.Position.Height);
                                }
                                else if (c.Position.Y < GraphicsDevice.Viewport.Height - c.Position.Height) ScreenMove("down", (int)(c.Speed * 1.77));
                                break;
                            case 5:
                                c.DashCount++;
                                foreach (Rectangle r in objects)
                                {
                                    if (new Rectangle(c.Position.X, (int)(c.Position.Y + c.Speed * 2.5), c.Position.Width, c.Position.Height).Intersects(r)) collides1 = true;
                                }
                                if ((c.Position.Y < GraphicsDevice.Viewport.Height - (100 + c.Position.Height) && globalY <= maxY) || (globalY == maxY && c.Position.Y < GraphicsDevice.Viewport.Height - c.Position.Height))
                                {
                                    if(collides1 == false) c.Position = new Rectangle(c.Position.X, (int)(c.Position.Y + c.Speed * 2.5), c.Position.Width, c.Position.Height);
                                }
                                else if (c.Position.Y < GraphicsDevice.Viewport.Height - c.Position.Height) ScreenMove("down", (int)(c.Speed * 2.5));
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
                                else if (c.Position.X > 0) ScreenMove("left", (int)(c.Speed * 1.77));
                                if ((c.Position.Y < GraphicsDevice.Viewport.Height - (100 + c.Position.Height) && globalY <= maxY) || (globalY == maxY && c.Position.Y < GraphicsDevice.Viewport.Height - c.Position.Height))
                                {
                                    if(collides2 == false) c.Position = new Rectangle(c.Position.X, (int)(c.Position.Y + c.Speed * 1.77), c.Position.Width, c.Position.Height);
                                }
                                else if (c.Position.Y < GraphicsDevice.Viewport.Height - c.Position.Height) ScreenMove("down", (int)(c.Speed * 1.77));
                                break;
                            case 7:
                                c.DashCount++;
                                foreach (Rectangle r in objects)
                                {
                                    if (new Rectangle((int)(c.Position.X - c.Speed * 2.5), c.Position.Y, c.Position.Width, c.Position.Height).Intersects(r)) collides1 = true;
                                }
                                if ((c.Position.X > 100 && globalX >= 0) || (globalX <= 0 && c.Position.X > 0))
                                {
                                    if(collides1 == false) c.Position = new Rectangle((int)(c.Position.X - c.Speed * 2.5), c.Position.Y, c.Position.Width, c.Position.Height);
                                }
                                else if (c.Position.X > 0) ScreenMove("left", (int)(c.Speed * 2.5));
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
                                    if(collides1 == false) c.Position = new Rectangle((int)(c.Position.X - c.Speed * 1.77), c.Position.Y, c.Position.Width, c.Position.Height);
                                }
                                else if (c.Position.X > 0) ScreenMove("left", (int)(c.Speed * 1.77));
                                if ((c.Position.Y > 100 && globalY >= 0) || (globalY <= 0 && c.Position.Y > 0))
                                {
                                    if(collides2 == false) c.Position = new Rectangle(c.Position.X, (int)(c.Position.Y - c.Speed * 1.77), c.Position.Width, c.Position.Height);
                                }
                                else if (c.Position.Y > 0) ScreenMove("up", (int)(c.Speed * 1.77));
                                break;
                        }
                    }
                    c.CRect = new Rectangle(c.Position.X + 10, c.Position.Y + 10, c.Position.Width - 20, c.Position.Height - 20);

                    // Removing shot delay
                    if (c.ShotDelay > 0)
                    {
                        c.ShotDelay--;
                    }

                    // Move projectiles
                    for (int i = projectiles.Count - 1; i >= 0; i--)
                    {
                        int removing = -1;
                        if (projectiles[i] is PBasic || projectiles[i] is PExplosive) projectiles[i].Move();
                        if (projectiles[i] is PStationary)
                        {
                            PStationary ps = (PStationary)(projectiles[i]);
                            ps.Move(c, rotationAngle);
                        }
                        projectiles[i].Count++;
                        if (projectiles[i].Count == projectiles[i].CountMax && (projectiles[i] is PExplosive) == false)
                        {
                            removing = i;
                        }
                        if(projectiles[i].Count >= projectiles[i].CountMax && projectiles[i] is PExplosive)
                        {
                            PExplosive ex = (PExplosive)projectiles[i];
                            ex.Explode(c, enemies, projectiles, eProjectiles);
                        }
                        if(projectiles.Count != 0)
                        {
                            foreach (Rectangle r in objects)
                            {
                                if (projectiles[i].Position.Intersects(r))
                                {
                                    if(projectiles[i] is PExplosive)
                                    {
                                        PExplosive ex = (PExplosive)projectiles[i];
                                        ex.Collided = true;
                                        break;
                                    }
                                    else
                                    {
                                        removing = i;
                                        break;
                                    }
                                }
                            }
                        }

                        if(removing != -1)
                        {
                            projectiles.RemoveAt(removing);
                        }

                    }

                    // Move Enemy projectiles and do damage
                    for (int i = eProjectiles.Count - 1; i >= 0; i--)
                    {
                        int removing = -1;
                        if(eProjectiles[i] is EPBasic || eProjectiles[i] is EPAccelerate)
                        {
                            eProjectiles[i].Move();
                        }
                        else if(eProjectiles[i] is EPStall)
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

                        if (eProjectiles[i].Position.Intersects(c.CRect) && (c.DashCount > 20 || c.DashCount == 0))
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

                        if(removing != -1)
                        {
                            eProjectiles.RemoveAt(removing);
                        }

                    }


                    // Foreach loop that goes through all enemy objects in the enemies list
                    bool enemyAlive = false;
                    foreach (Enemy e in enemies)
                    {
                        // If the enemy is alive, it moves, and the enemyAlive boolean is set to true
                        if (e.Alive == true)
                        {
                            enemyAlive = true;
                            e.Move(c, enemies, objects);
                            if (e is Enemy2)
                            {
                                Enemy2 e2 = (Enemy2)e;
                                e2.Shoot(c);
                                if (e2.ShotCount == 150)
                                {
                                    int shotX = (c.Position.X + c.Position.Width / 2) - (e.Position.X + e.Position.Width / 2);
                                    int shotY = (c.Position.Y + c.Position.Height / 2) - (e.Position.Y + e.Position.Height / 2);
                                    float shotAngle = (float)Math.Atan2(shotY, shotX);
                                    eProjectiles.Add(new EPBasic(10, 30, 30, e, shotAngle, 15, basicImage));
                                    e2.ShotCount = 0;
                                }
                            }
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
                            if(e is Boss)
                            {
                                Boss b = (Boss)e;
                                switch(b.AttackNum)
                                {
                                    case 1:
                                        try
                                        {
                                            string attackType = "";
                                            int attackX = 0;
                                            int attackY = 0;
                                            BinaryReader attackReader = new BinaryReader(File.OpenRead(@"../../../Projectile Patterns/DIE.dat"));
                                            for (int i = 100; i > b.AttackCount; i--)
                                            {
                                                if (i % 2 == 0)
                                                {
                                                    attackType = attackReader.ReadString();
                                                    attackX = attackReader.ReadInt32();
                                                    attackY = attackReader.ReadInt32();
                                                }
                                            }

                                            switch (attackType)
                                            {
                                                case "1": break;
                                                case "2":
                                                    eProjectiles.Add(new EPStall(5, b.Position.X - 500 + attackX, b.Position.Y - 300 + attackY, 30, 30, 10, 90, stallImage));
                                                    break;
                                            }

                                            b.AttackCount--;
                                        }
                                        // Silently catch EndOfStreamExceptions
                                        catch (EndOfStreamException) { }
                                        break;
                                }
                            }
                        }
                        else if (e is Enemy2)
                        {
                            Enemy2 e2 = (Enemy2)e;
                            e2.Shooting = false;
                        }

                        // For loop that goes through all projectile objects in the projectiles list
                        for (int i = projectiles.Count - 1; i >= 0; i--)
                        {
                            if (projectiles[i].CheckCollision(e) == true && e.Alive == true)
                            {
                                if(projectiles[i] is PExplosive)
                                {
                                    PExplosive ex = (PExplosive)projectiles[i];
                                    ex.Collided = true;
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
                        }

                        // If the enemy's health is 0 or less, it dies
                        if (e.Health <= 0)
                        {
                            if (e is Enemy1 && e.Alive == true) score += 100;
                            e.Alive = false;
                        }
                    }

                    // If enemyAlive is false, no enemies are alive, and the round advances
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
                    rMButton = new Rectangle((GraphicsDevice.Viewport.Width / 2) - (rMButton.Width / 2), (GraphicsDevice.Viewport.Height / 2) + (rMButton.Height / 2), menu.Width / 4, menu.Height / 4);
                    mRectangle = new Rectangle(mState.Position.X, mState.Position.Y, 1, 1);
                    if (mState.LeftButton == ButtonState.Pressed && mRectangle.Intersects(rMButton))
                    {
                        gState = GameState.Menu;
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
                    break;

                // Game is in character selection screen
                case GameState.CharacterSelection:
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
                        c.Draw(spriteBatch, rotationAngle, framePlayer, 3);

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
                        c.Draw(spriteBatch, rotationAngle, framePlayer, 3);
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
                        c.Draw(spriteBatch, rotationAngle, framePlayer, 3);
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
                        c.Draw(spriteBatch, rotationAngle, framePlayer, 3);
                    }
                    else
                    {
                        spriteBatch.Draw(electricButton, char4, Color.White);
                    }
                    break;

                // Game is in Horde Mode
                case GameState.HordeMode:
                    spriteBatch.Draw(background, new Rectangle(backgroundPoint, new Point(1800, 1300)), Color.White);

                    foreach (Rectangle o in objects)
                    {
                        spriteBatch.Draw(boxes, o, Color.White);
                        
                    }

                    foreach (Projectile p in projectiles)
                    {
                        if (p is PBasic)
                        {
                            p.Draw(spriteBatch, frameProjectile);
                        }
                        if (p is PStationary)
                        {
                            PStationary ps = (PStationary)p;
                            ps.Draw(spriteBatch, frameProjectile, rotationAngle);
                        }
                        if(p is PExplosive)
                        {
                            PExplosive ex = (PExplosive)p;
                            if (ex.ExplostionCount == 0) ex.Draw(spriteBatch);
                            else spriteBatch.Draw(meleeImage, ex.Explosion, Color.White);
                        }
                    }

                    foreach (EnemyProjectile eP in eProjectiles)
                    {
                        eP.Draw(spriteBatch);
                        
                    }

                    if (c.DashCount == 0) c.Draw(spriteBatch, rotationAngle, framePlayer, Color.White); // Draw the character
                    else if (c.DashCount < 20) c.Draw(spriteBatch, rotationAngle, framePlayer, Color.CadetBlue); // Draw the character
                    else c.Draw(spriteBatch, rotationAngle, framePlayer, Color.IndianRed); // Draw the character

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
                                spriteBatch.Draw(dot, lineRect, null, Color.Red, lineAngle, Vector2.Zero, SpriteEffects.None, 0);
                            }
                        }
                    }

                    // Draw all alive enemies
                    foreach (Enemy e in enemies)
                    {
                        int aX = e.Position.X - c.Position.X;
                        int aY = e.Position.Y - c.Position.Y;
                        float enemyAngle = -(float)(Math.Atan2(aX, aY) + Math.PI / 2);
                        if (e.Alive == true && e is Enemy1) e.Draw(spriteBatch, enemyAngle, frameEnemy,Color.White);
                        if (e.Alive == true && e is Enemy2) e.Draw(spriteBatch, enemyAngle, frameEnemy, Color.Blue);
                        if (e.Alive == true && e is Enemy3) e.Draw(spriteBatch, enemyAngle, frameEnemy, Color.Orange);
                        if (e.Alive == true && e is Boss) e.Draw(spriteBatch, enemyAngle, frameEnemy, Color.Red);
                    }

                    // Code for drawing interface
                    spriteBatch.DrawString(sFont, "Round " + round, new Vector2(GraphicsDevice.Viewport.Width - 100, GraphicsDevice.Viewport.Height - 40), Color.Black);
                    spriteBatch.DrawString(sFont, "Score", new Vector2(30, GraphicsDevice.Viewport.Height - 60), Color.Black);
                    spriteBatch.DrawString(sFont, "" + score, new Vector2(30, GraphicsDevice.Viewport.Height - 40), Color.Black);
                    spriteBatch.Draw(rectangle, new Rectangle(295, 20, 42, 50), Color.DodgerBlue);
                    spriteBatch.Draw(rectangle, new Rectangle(295, 40, 300, 35), Color.DodgerBlue);
                    spriteBatch.Draw(rectangle, new Rectangle(640, 40, 300, 35), Color.DodgerBlue);
                    spriteBatch.Draw(circle, new Rectangle(900, 10, 100, 100), Color.DodgerBlue);
                    spriteBatch.Draw(circle, new Rectangle(570, 10, 100, 100), Color.DodgerBlue);
                    spriteBatch.Draw(circle, new Rectangle(580, 20, 80, 80), Color.DarkBlue);
                    spriteBatch.DrawString(sFont, "Life", new Vector2(300, 25), Color.Black);
                    spriteBatch.Draw(rectangle, new Rectangle(300, 45, 260, 20), Color.Red);
                    spriteBatch.Draw(rectangle, new Rectangle(300, 45, c.Health * 13/5, 20), Color.LawnGreen);


                    // Switch statement that draws the image for the ability the player is using for the interface
                    switch (aState)
                    {
                        case AbilityState.a1:
                            spriteBatch.Draw(meleeImage, new Rectangle(600, 40, 40, 40), Color.White);
                            spriteBatch.Draw(bulletImage, new Rectangle(550, 10, 20, 20), new Rectangle(32, 0, 32, 32), Color.White); //prev wpn
                            spriteBatch.Draw(bulletImage, new Rectangle(660, 0, 30, 30), new Rectangle(32, 0, 32, 32), Color.White); //next wpn
                            break;
                        case AbilityState.a2:
                            spriteBatch.Draw(bulletImage, new Rectangle(600, 40, 40, 40), new Rectangle(32, 0, 32, 32), Color.White);
                            spriteBatch.Draw(meleeImage, new Rectangle(540, 5, 30, 30), new Rectangle(32, 0, 32, 32), Color.White);
                            spriteBatch.Draw(bulletImage, new Rectangle(654, -10, 45, 45), new Rectangle(32, 0, 32, 32), Color.White);
                            break;
                        case AbilityState.a3:
                            spriteBatch.Draw(bulletImage, new Rectangle(593, 30, 55, 55), new Rectangle(32, 0, 32, 32), Color.White);
                            spriteBatch.Draw(bulletImage, new Rectangle(540, 0, 30, 30), new Rectangle(32, 0, 32, 32), Color.White);
                            spriteBatch.Draw(bulletImage, new Rectangle(660, 10, 20, 20), new Rectangle(32, 0, 32, 32), Color.White);
                            break;
                        case AbilityState.a4:
                            spriteBatch.Draw(bulletImage, new Rectangle(606, 45, 30, 30), new Rectangle(32, 0, 32, 32), Color.White);
                            spriteBatch.Draw(bulletImage, new Rectangle(541, -10, 45, 45), new Rectangle(32, 0, 32, 32), Color.White);
                            spriteBatch.Draw(meleeImage, new Rectangle(670, 5, 30, 30), new Rectangle(32, 0, 32, 32), Color.White);
                            break;
                    }
                    break;

                case GameState.Paused:
                    // Draw background
                    spriteBatch.Draw(background, new Rectangle(backgroundPoint, new Point(1800, 1300)), Color.White);

                    // Draw projectiles
                    foreach (Projectile p in projectiles)
                    {
                        if (p is PBasic)
                        {
                            p.Draw(spriteBatch, frameProjectile);
                        }
                        if (p is PStationary)
                        {
                            PStationary ps = (PStationary)p;
                            ps.Draw(spriteBatch, frameProjectile, rotationAngle);
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
                    break;

                // Game is in Options
                case GameState.Options:
                    
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
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
