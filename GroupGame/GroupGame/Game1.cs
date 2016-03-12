using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

enum GameState { Menu, HordeMode }; // GameState enum for keeping track of what state our game is in

namespace GroupGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GameState gState;
        // images
        Texture2D enemyImage;
        Texture2D playerImage;
        Texture2D bulletImage;
        Texture2D startButton;
        Rectangle rSButton;
        Rectangle mRectangle;
        Character c;
        List<Enemy> enemies = new List<Enemy>();
        List<Projectile> projectiles = new List<Projectile>();
        Random rgen = new Random();
        KeyboardState kbState;
        MouseState mState;
        float rotationAngle;
        int round;

        // Method for advancing the round of our Horde Mode
        public void AdvanceRound()
        {
            // Clear Enemies list
            enemies.Clear();

            // Select a random round to use
            //int num = rgen.Next(1,round+1);
            //BinaryReader reader = new BinaryReader(File.OpenRead(@"../../../Rounds/Round" + num + ".dat"));
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

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // Change the GameState to HordeMode for testing
            //gState = GameState.HordeMode;

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

            // Load images and set playerImage
            enemyImage = this.Content.Load<Texture2D>("EnemyThing");
            playerImage = this.Content.Load<Texture2D>("Hero");
            bulletImage = this.Content.Load<Texture2D>("Bullet");
            c.Image = playerImage;
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

            kbState = Keyboard.GetState(); // Get the keyboard state
            mState = Mouse.GetState(); // Get the mouse state

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
                    int rotX = mState.X - c.Position.X;
                    int rotY = mState.Y - c.Position.Y;
                    rotationAngle = (float)Math.Atan2(rotY, rotX);

                    // Move the character based on user input, might change this to a method elsewhere to make this cleaner looking
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

                    // Shooting
                    if(mState.LeftButton == ButtonState.Pressed && c.ShotDelay == 0)
                    {
                        Projectile p = new Projectile(25, mState.X, mState.Y, c, rotationAngle, false);
                        p.Image = bulletImage;
                        projectiles.Add(p);
                        c.ShotDelay = 20; // We can change this value depending on ability, stronger attacks have longer delays
                    }

                    // Removing shot delay
                    if(c.ShotDelay > 0)
                    {
                        c.ShotDelay--;
                    }

                    // Move projectiles
                    foreach(Projectile p in projectiles)
                    {
                        if (p.MovementCount < 150) p.Move();
                        else projectiles.Remove(p);
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

                        // Foreach loop that goes through all projectile objects in the projectiles list
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
                    c.Draw(spriteBatch,rotationAngle); // Draw the character

                    // Draw all alive enemies
                    foreach(Enemy e in enemies)
                    {
                        if(e.Alive == true) e.Draw(spriteBatch);
                    }

                    foreach(Projectile p in projectiles)
                    {
                        p.Draw(spriteBatch);
                    }

                    break;
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
