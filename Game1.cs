using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;
using System.Globalization;
using static System.Net.Mime.MediaTypeNames;

namespace _3902sprint0
{
    /// <summary>
    /// The main class for the game, responsible for managing game components, settings, 
    /// and platform-specific configurations.
    /// </summary>
    public class Game1 : Game
    {

        // Resources for drawing.
        private GraphicsDeviceManager DeviceManager3902;
        private Texture2D knightTexture;
        private SpriteBatch spriteBatch;
        private Player player;
        private Mousecontroller Mouse;
        private KeyboardController Keyboard;
        private Fireball fireball;

        private Inventory inventory;
        private itemDatabase database;
        private Chest chest;
        private Chest chest2;

        private Texture2D enemyBasicRunningTexture;

        private Texture2D enemyBasicIdleTexture;
        private List<Enemy> enemies;



        /// <summary>
        /// Indicates if the game is running on a desktop platform. it came from a previous template and i dont really have a reason to get rid of it.
        /// </summary>
        public readonly static bool IsDesktop = OperatingSystem.IsMacOS() || OperatingSystem.IsLinux() || OperatingSystem.IsWindows();

        /// <summary>
        /// Initializes a new instance of the game. Configures platform-specific settings, 
        /// initializes services like settings and leaderboard managers, and sets up the 
        /// screen manager for screen transitions.
        /// </summary>
        public Game1()
        {
            DeviceManager3902 = new GraphicsDeviceManager(this);
            DeviceManager3902.PreferredBackBufferWidth = 1820;
            DeviceManager3902.PreferredBackBufferHeight = 1280;

            Content.RootDirectory = "Content";

        }

        /// <summary>
        /// Initializes the game, including setting up localization and adding the 
        /// initial screens to the ScreenManager.
        /// </summary>
        protected override void Initialize()
        {

            
            Mouse = new Mousecontroller();
            Keyboard = new KeyboardController();

           

            IsMouseVisible = true;
            enemies = new List<Enemy>();
            base.Initialize();
            

        }

        /// <summary>
        /// Loads game content, such as textures and particle systems.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load the knight texture and initialize the player with it
            knightTexture = Content.Load<Texture2D>("Knight");
            Texture2D magicSwordTexture = Content.Load<Texture2D>("items/MagicSword");
            Texture2D fireScrollTexture = Content.Load<Texture2D>("items/FireScroll");
            Texture2D chestTexture = Content.Load<Texture2D>("Chest");
            Texture2D fireballTexture = Content.Load<Texture2D>("Fireball");
            Texture2D detonateTexture = Content.Load<Texture2D>("Explosion");
            inventory = new Inventory();
            player = new Player(new Vector2(300, 300), inventory);
            player.InitializeSprite(knightTexture);

            FireballSprite fireballSprite = new FireballSprite();
            detonateSprite detonateSprite = new detonateSprite();

            fireballSprite.Initialize(fireballTexture);
            detonateSprite.Initialize(detonateTexture);

            fireball = new Fireball(
             fireballTexture,
             fireballSprite,
             detonateSprite
            );

            database = new itemDatabase(magicSwordTexture, fireScrollTexture, fireball);
            chest = new Chest(
                new Vector2(600, 300),
                inventory,
                database
             );
            chest2 = new Chest(
               new Vector2(800, 300),
               inventory,
               database
            );
            enemyBasicIdleTexture = Content.Load<Texture2D>("enemyBasicIdle");

            enemyBasicRunningTexture = Content.Load<Texture2D>("enemyBasicRunning");

            Rectangle movementBounds =
                new Rectangle(
                    0,
                    0,
                    DeviceManager3902.PreferredBackBufferWidth,
                    DeviceManager3902.PreferredBackBufferHeight
                );
            for (int i = 0; i < 4; i++)
            {
                enemies.Add(EnemyCreator.CreateEnemy(
                    EnemyType.Basic,
                    enemyBasicRunningTexture,
                    enemyBasicIdleTexture,
                    movementBounds,
                    1,
                    1,
                    150,
                    48));
            }
            chest.InitializeSprite(chestTexture);
            chest2.InitializeSprite(chestTexture);
            base.LoadContent();
        }

        /// <summary>
        /// updates the game state, including player input and game logic. This method is called once per frame.
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Update(GameTime gameTime)
        {
            if (player.IsQuit())
                Exit();
            chest.Update(gameTime);
            chest2.Update(gameTime);
            player.Update(gameTime);
            fireball.Update(gameTime);
            foreach (Enemy enemy in enemies)
            {
                enemy.Update(gameTime);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the game scene, including the player and other game elements. This method is called once per frame.
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(player.Terrain.terrainColor);

            spriteBatch.Begin(
                 samplerState: SamplerState.PointClamp);

            player.Draw(spriteBatch);
            chest.Draw(spriteBatch);
            chest2.Draw(spriteBatch);
            fireball.Draw(spriteBatch);

            foreach (Enemy enemy in enemies)
            {
                enemy.Draw(spriteBatch);
            }
            spriteBatch.End();
            

            base.Draw(gameTime);
         }
    }
}