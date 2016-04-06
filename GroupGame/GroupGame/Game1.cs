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

enum GameState { Menu, HordeMode, Paused, Options, CharacterSelection}; // GameState enum for keeping track of what state our game is in
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

        // Values used for the options menu
        bool closing = false;
        bool fullscreen = false;
        Rectangle rOKButton;
        Rectangle rCancelButton;

        // Method for advancing the round of our Horde Mode
        public void AdvanceRound()
        {
            // Clear Enemies list
            enemies.Clear();

            // Select a random round to use
            //int num = rgen.Next(1,round+1);
            BinaryReader reader = new BinaryReader(File.OpenRead(@"../../../Rounds/Round1.dat"));

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
                            Enemy e = new Enemy1(c.Position.X-500+x, c.Position.Y-300+y);
                            e.Image = enemyImage;
                            enemies.Add(e);
                            break;
                    }
                }
            }
            // Silently catch EndOfStreamExceptions
            catch(EndOfStreamException){}

            
            reader.Close(); // Close the file
            round++; // Increase the round number
        }

        // Method for moving the screen
        public void ScreenMove(string s)
        {
            if(s == "up")
            {
                globalY--;
                foreach (Enemy e in enemies)
                {
                    e.Position = new Rectangle(e.Position.X, e.Position.Y + c.Speed, e.Position.Width, e.Position.Height);
                }
                foreach (Projectile p in projectiles)
                {
                    p.FPosY += c.Speed;
                }
            }

            if (s == "down")
            {
                globalY++;
                foreach (Enemy e in enemies)
                {
                    e.Position = new Rectangle(e.Position.X, e.Position.Y - c.Speed, e.Position.Width, e.Position.Height);
                }
                foreach (Projectile p in projectiles)
                {
                    p.FPosY -= c.Speed;
                }
            }

            if (s == "left")
            {
                globalX--;
                foreach (Enemy e in enemies)
                {
                    e.Position = new Rectangle(e.Position.X + c.Speed, e.Position.Y, e.Position.Width, e.Position.Height);
                }
                foreach (Projectile p in projectiles)
                {
                    p.FPosX += c.Speed;
                }
            }

            if (s == "right")
            {
                globalX++;
                foreach (Enemy e in enemies)
                {
                    e.Position = new Rectangle(e.Position.X - c.Speed, e.Position.Y, e.Position.Width, e.Position.Height);
                }
                foreach (Projectile p in projectiles)
                {
                    p.FPosX -= c.Speed;
                }
            }
        }

        // Method for the player moving
        public void PlayerMove()
        {
            // Move the character based on user input
            if (kbState.IsKeyDown(Keys.W))
            {
                if ((c.Position.Y > 100 && globalY != 0) || (globalY == 0 && c.Position.Y > 0))
                        c.Position = new Rectangle(c.Position.X, c.Position.Y - c.Speed, c.Position.Width, c.Position.Height);
                else if (c.Position.Y > 0) ScreenMove("up");
            }
            if (kbState.IsKeyDown(Keys.A))
            {
                if ((c.Position.X > 100 && globalX != 0) || (globalX == 0 && c.Position.X > 0))
                    c.Position = new Rectangle(c.Position.X - c.Speed, c.Position.Y, c.Position.Width, c.Position.Height);
                else if (c.Position.X > 0) ScreenMove("left");
            }
            if (kbState.IsKeyDown(Keys.S))
            {
                if ((c.Position.Y < GraphicsDevice.Viewport.Height - (100 + c.Position.Height) && globalY != maxY) || (globalY == maxY && c.Position.Y < GraphicsDevice.Viewport.Height - c.Position.Height))
                    c.Position = new Rectangle(c.Position.X, c.Position.Y + c.Speed, c.Position.Width, c.Position.Height);
                else if (c.Position.Y < GraphicsDevice.Viewport.Height - c.Position.Height) ScreenMove("down");
            }
            if (kbState.IsKeyDown(Keys.D))
            {
                if ((c.Position.X < GraphicsDevice.Viewport.Width - (100 + c.Position.Width) && globalX != maxX) || (globalX == maxX && c.Position.X < GraphicsDevice.Viewport.Width - c.Position.Width))
                    c.Position = new Rectangle(c.Position.X + c.Speed, c.Position.Y, c.Position.Width, c.Position.Height);
                else if (c.Position.X < GraphicsDevice.Viewport.Width - c.Position.Width) ScreenMove("right");
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
                        Projectile p1 = new Projectile(5, c, rotationAngle, 15, true); // Create a new projectile that will travel in the direction of the mouse
                        p1.Image = meleeImage; // Set the image of the projectile
                        projectiles.Add(p1); // Add the projectile to the list
                        c.ShotDelay = 40; // Set the ShotDelay of the player. We can change this value depending on ability, stronger attacks have longer delays
                        break;

                    // The original shooting attack
                    case AbilityState.a2:
                        Projectile p2 = new Projectile(25, mState.X, mState.Y, c, rotationAngle, 100, false);
                        p2.Image = bulletImage;
                        projectiles.Add(p2);
                        c.ShotDelay = 20; 
                        break;

                    // Test of a piercing attack with a different size
                    case AbilityState.a3:
                        Projectile p3 = new Projectile(10, mState.X, mState.Y, 100, 100, c, rotationAngle, 120, true);
                        p3.Image = bulletImage;
                        projectiles.Add(p3);
                        c.ShotDelay = 80;
                        break;

                    // Test of a rapid fire attack
                    case AbilityState.a4:
                        Projectile p4 = new Projectile(3, mState.X, mState.Y, c, rotationAngle, 90, false);
                        p4.Image = bulletImage;
                        projectiles.Add(p4);
                        c.ShotDelay = 2;
                        break;
                }
            }
        }

        public void ResetGame()
        {
            enemies.Clear();
            projectiles.Clear();
            c.Position = new Rectangle(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2, c.Position.Width, c.Position.Height);
            maxX = 1500 - GraphicsDevice.Viewport.Width;
            if (maxX < 0) maxX = 0;
            maxY = 1000 - GraphicsDevice.Viewport.Height;
            if (maxY < 0) maxY = 0;
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


            // TODO: use this.Content to load your game content here

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

            // Load fonts
            sFont = this.Content.Load<SpriteFont>("SpriteFont1");
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
                    /*
                    // CHOOSING BETWEEN CHARACTERS
                    if (kbState.IsKeyDown(Keys.U) == true)
                    {
                        switchHero = SwitchHero.Fire;     
                    }
                    else if (kbState.IsKeyDown(Keys.I) == true)
                    {
                        switchHero = SwitchHero.Earth;
                    }
                    else if (kbState.IsKeyDown(Keys.O) == true)
                    {
                        switchHero = SwitchHero.Electric;
                    }
                    else if (kbState.IsKeyDown(Keys.P) == true)
                    {
                        switchHero = SwitchHero.Water;
                    }
                    
                    switch(switchHero)
                    {
                        case SwitchHero.Fire:
                            playerImage = player1Image;
                            playerWalking = player1Walking;
                            bulletImage = bullet1Image;
                            break;

                        case SwitchHero.Earth:
                            playerImage = player2Image;
                            playerWalking = player2Walking;
                            bulletImage = bullet2Image;
                            break;

                        case SwitchHero.Electric:
                            playerImage = player3Image;
                            playerWalking = player3Walking;
                            bulletImage = bullet3Image;
                            break;

                        case SwitchHero.Water:
                            playerImage = player4Image;
                            playerWalking = player4Walking;
                            bulletImage = bullet4Image;
                            break;
                    }
                    */
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

                    char1 = new Rectangle((GraphicsDevice.Viewport.Width / 4) - (rSButton.Width), (GraphicsDevice.Viewport.Height) - (rSButton.Height)*3, startButton.Width / 4, startButton.Height / 4);
                    char2 = new Rectangle((GraphicsDevice.Viewport.Width / 4)*2 - (rSButton.Width), (GraphicsDevice.Viewport.Height) - (rSButton.Height)*3, startButton.Width / 4, startButton.Height / 4);
                    char3 = new Rectangle((GraphicsDevice.Viewport.Width / 4)*3 - (rSButton.Width), (GraphicsDevice.Viewport.Height) - (rSButton.Height)*3, startButton.Width / 4, startButton.Height / 4);
                    char4 = new Rectangle((GraphicsDevice.Viewport.Width / 4)*4 - (rSButton.Width), (GraphicsDevice.Viewport.Height) - (rSButton.Height)*3, startButton.Width / 4, startButton.Height / 4);

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

                    // Find the angle between the player and the mouse, use this to rotate the player when drawing
                    int rotX = mState.X - (c.Position.X + c.Position.Width/2);
                    int rotY = mState.Y - (c.Position.Y + c.Position.Height/2);
                    rotationAngle = (float)Math.Atan2(rotY, rotX);

                    PlayerMove();
                    PlayerChangeAbility();
                    PlayerShoot();

                    // Removing shot delay
                    if(c.ShotDelay > 0)
                    {
                        c.ShotDelay--;
                    }

                    // Move projectiles
                    for (int i = projectiles.Count; i > 0; i--)
                    {
                        if (projectiles[i - 1].Moving == true) projectiles[i - 1].Move();
                        else projectiles[i - 1].MoveStationary(c,rotationAngle);
                        projectiles[i - 1].Count++;
                        if (projectiles[i - 1].Count == projectiles[i - 1].CountMax)
                        {
                            projectiles.RemoveAt(i - 1);
                        }
                    }


                    // Foreach loop that goes through all enemy objects in the enemies list
                    bool enemyAlive = false;
                    foreach (Enemy e in enemies)
                    {
                        // If the enemy is alive, it moves, and the enemyAlive boolean is set to true
                        if (e.Alive == true)
                        {
                            e.Move(c, enemies);
                            enemyAlive = true;
                        }

                        // For loop that goes through all projectile objects in the projectiles list
                        for(int i = projectiles.Count; i > 0; i--)
                        {
                            if (projectiles[i-1].CheckCollision(e) == true && e.Alive == true)
                            {
                                e.Health -= projectiles[i-1].Damage;
                                if (projectiles[i-1].Pierce == false)
                                {
                                    projectiles.RemoveAt(i-1);
                                }
                            }
                        }

                        foreach (Projectile p in projectiles)
                        {
                            // If the projectile is colliding with the enemy, and the enemy is alive, the enemy is damaged
                            if (p.CheckCollision(e) == true && e.Alive == true)
                            {
                                e.Health -= p.Damage;
                                if (p.Pierce == false)
                                {
                                    //projectiles.Remove(p);
                                }
                            }
                        }

                        // If the enemy's health is 0 or less, it dies
                        if(e.Health <= 0)
                        {
                            if (e is Enemy1 && e.Alive == true) score += 100;
                            e.Alive = false;
                        }
                    }

                    // If enemyAlive is false, no enemies are alive, and the round advances
                    if(enemyAlive == false)
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
                    rFButton = new Rectangle((GraphicsDevice.Viewport.Width / 2) - (rFButton.Width / 2), (GraphicsDevice.Viewport.Height / 2) - (rFButton.Height / 2), fullscreenButton.Width / 4, fullscreenButton.Height / 4);
                    
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

                    }
                    else
                    {
                        spriteBatch.Draw(fireButton, char1, Color.White);
                    }

                    if (char2.Intersects(mRectangle))
                    {
                        spriteBatch.Draw(earthButton, char2, Color.Red);
                    }
                    else
                    {
                        spriteBatch.Draw(earthButton, char2, Color.White);
                    }

                    if (char3.Intersects(mRectangle))
                    {
                        spriteBatch.Draw(waterButton, char3, Color.Red);
                    }
                    else
                    {
                        spriteBatch.Draw(waterButton, char3, Color.White);
                    }

                    if (char4.Intersects(mRectangle))
                    {
                        spriteBatch.Draw(electricButton, char4, Color.Red);
                    }
                    else
                    {
                        spriteBatch.Draw(electricButton, char4, Color.White);
                    }


                    break;

                // Game is in Horde Mode
                case GameState.HordeMode:
                    c.Draw(spriteBatch,rotationAngle, framePlayer); // Draw the character
                    spriteBatch.DrawString(sFont, "X: " + globalX, new Vector2(40, 275), Color.Black);
                    spriteBatch.DrawString(sFont, "Y: " + globalY, new Vector2(40, 300), Color.Black);

                    // Draw all alive enemies
                    foreach (Enemy e in enemies)
                    {
                        int aX = e.Position.X - c.Position.X;
                        int aY = e.Position.Y - c.Position.Y;
                        float enemyAngle = -(float)(Math.Atan2(aX, aY) + Math.PI / 2);
                        if (e.Alive == true) e.Draw(spriteBatch, enemyAngle, frameEnemy);
                    }

                    foreach (Projectile p in projectiles)
                    {
                        if (p.Moving == true)
                        {
                            p.Draw(spriteBatch, frameProjectile);
                        }
                        else p.DrawStationary(spriteBatch, frameProjectile, rotationAngle);
                    }

                    // Code for drawing interface
                    spriteBatch.DrawString(sFont, "Round " + round, new Vector2(GraphicsDevice.Viewport.Width - 100, GraphicsDevice.Viewport.Height - 40), Color.Black);
                    spriteBatch.DrawString(sFont, "Score", new Vector2(30, GraphicsDevice.Viewport.Height - 60), Color.Black);
                    spriteBatch.DrawString(sFont, "" + score, new Vector2(30, GraphicsDevice.Viewport.Height - 40), Color.Black);
                    spriteBatch.Draw(rectangle, new Rectangle(25, 20, 42, 50), Color.DodgerBlue);
                    spriteBatch.Draw(rectangle, new Rectangle(25, 40, 300, 35), Color.DodgerBlue);
                    spriteBatch.Draw(circle, new Rectangle(300, 10, 100, 100), Color.DodgerBlue);
                    spriteBatch.DrawString(sFont, "Life", new Vector2(30, 25), Color.Black);
                    spriteBatch.Draw(rectangle, new Rectangle(30, 45, 200, 20), Color.Red);
                    spriteBatch.Draw(rectangle, new Rectangle(30, 45, c.Health * 2, 20), Color.LawnGreen);

                    // Switch statement that draws the image for the ability the player is using for the interface
                    switch (aState)
                    {
                        case AbilityState.a1:
                            spriteBatch.Draw(meleeImage, new Rectangle(330, 40, 40, 40), Color.White);
                            break;
                        case AbilityState.a2:
                            spriteBatch.Draw(bulletImage, new Rectangle(330, 40, 40, 40), new Rectangle(32, 0, 32, 32), Color.White);
                            break;
                        case AbilityState.a3:
                            spriteBatch.Draw(bulletImage, new Rectangle(323, 30, 55, 55), new Rectangle(32, 0, 32, 32), Color.White);
                            break;
                        case AbilityState.a4:
                            spriteBatch.Draw(bulletImage, new Rectangle(336, 45, 30, 30), new Rectangle(32, 0, 32, 32), Color.White);
                            break;
                    }
                    break;

                case GameState.Paused:
                    c.Draw(spriteBatch, rotationAngle, framePlayer); // Draw the character
                    // Draw all alive enemies
                    foreach (Enemy e in enemies)
                    {
                        if (e.Alive == true) e.Draw(spriteBatch);
                    }

                    foreach (Projectile p in projectiles)
                    {
                        if (p.Moving == true) p.Draw(spriteBatch, frameProjectile);
                        else p.DrawStationary(spriteBatch, frameProjectile, rotationAngle);
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
