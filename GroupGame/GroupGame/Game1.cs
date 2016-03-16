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

enum GameState { Menu, HordeMode }; // GameState enum for keeping track of what state our game is in
enum AbilityState { a1, a2, a3, a4 }; // AbilityState enum for keeping track of the ability the player is using
enum HeroState { Still, Walking }; // HeroState enum for keeping track of the state of the player

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
        HeroState heroState = HeroState.Still;
        Random rgen = new Random();

        // Images
        Texture2D enemyImage;
        Texture2D playerImage;
        Texture2D playerWalking;
        Texture2D bulletImage;
        Texture2D meleeImage;
        Texture2D startButton;
        Texture2D rectangle;
        Texture2D circle;

        // Rectangles for buttons and mouse
        Rectangle rSButton;
        Rectangle mRectangle;
        
        // Characters, enemies, and projectiles
        Character c;
        List<Enemy> enemies = new List<Enemy>();
        List<Projectile> projectiles = new List<Projectile>();

        // Keyboard states
        KeyboardState kbState;
        KeyboardState previousKbState;
        MouseState mState;
        float rotationAngle;

        // Ints for round and score
        int round;
        int score;

        // Variables for animating
        int framePlayer;
        int frameProjectile;
        double timePerFrame = 100;
        int numFramesPlayer;
        int numFramesProjectile = 4;
        int framesElapsedPlayer;
        int framesElapsedProjectile;

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

        // Method for the player moving
        public void PlayerMove()
        {
            // Move the character based on user input
            if (kbState.IsKeyDown(Keys.W))
            {
                c.Position = new Rectangle(c.Position.X, c.Position.Y - c.Speed, c.Position.Width, c.Position.Height);
            }
            if (kbState.IsKeyDown(Keys.A))
            {
                c.Position = new Rectangle(c.Position.X - c.Speed, c.Position.Y, c.Position.Width, c.Position.Height);
            }
            if (kbState.IsKeyDown(Keys.S))
            {
                c.Position = new Rectangle(c.Position.X, c.Position.Y + c.Speed, c.Position.Width, c.Position.Height);
            }
            if (kbState.IsKeyDown(Keys.D))
            {
                c.Position = new Rectangle(c.Position.X + c.Speed, c.Position.Y, c.Position.Width, c.Position.Height);
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

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // Adjust window size
            /*graphics.PreferredBackBufferWidth = 900;
            graphics.PreferredBackBufferHeight = 30;
            graphics.ApplyChanges();*/
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
            c = new Character1(GraphicsDevice.Viewport.Width/2, GraphicsDevice.Viewport.Height/2);

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

            // Load images for start screen
            startButton = this.Content.Load<Texture2D>("Start");

            // Load images for the game
            enemyImage = this.Content.Load<Texture2D>("EnemyThing");
            playerImage = this.Content.Load<Texture2D>("Fire Still");
            playerWalking = this.Content.Load<Texture2D>("Fire Move");
            bulletImage = this.Content.Load<Texture2D>("Fire Bullet");
            meleeImage = this.Content.Load<Texture2D>("Melee");
            rectangle = this.Content.Load<Texture2D>("WhiteRectangle");
            circle = this.Content.Load<Texture2D>("WhiteCircle");

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

            // Switch statement based on gState
            switch (gState)
            {
                // Game is in Menu
                case GameState.Menu:

                    // Checks to see if the start button has been pressed
                    rSButton = new Rectangle((GraphicsDevice.Viewport.Width / 2) - (rSButton.Width/2), (GraphicsDevice.Viewport.Height / 2) - (rSButton.Height/2), startButton.Width/4, startButton.Height/4);
                    Rectangle mRectangle = new Rectangle(mState.Position.X, mState.Position.Y, 1, 1);
                    if (mState.LeftButton == ButtonState.Pressed && mRectangle.Intersects(rSButton))
                    {
                        gState = GameState.HordeMode;
                    }
                    break;

                // Game is in Horde Mode
                case GameState.HordeMode:

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
                    break;

                // Game is in Horde Mode
                case GameState.HordeMode:
                    c.Draw(spriteBatch,rotationAngle, framePlayer); // Draw the character


                    // Draw all alive enemies
                    foreach (Enemy e in enemies)
                    {
                        if (e.Alive == true) e.Draw(spriteBatch);
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
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
