using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;
using System.Globalization;
using static System.Net.Mime.MediaTypeNames;
using _3902sprint0.Environment;
using _3902sprint0.Environment.Tiles;
using _3902sprint0.Environment.Doors;

namespace _3902sprint0
{
    /// <summary>
    /// The main class for the game, responsible for managing game components, settings, 
    /// and platform-specific configurations.
    /// </summary>
    public class Game1 : Game
    {

        // Resources for drawing.
        private fireballManager FireballManager;
        private GraphicsDeviceManager DeviceManager3902;
        private Texture2D knightTexture;
        private SpriteBatch spriteBatch;
        private Player player;
        private Mousecontroller Mouse;
        private KeyboardController Keyboard;
        private Fireball fireball;
        private Fireball2 fireball2;

        private chestManager ChestManager;
        private Inventory inventory;
        private itemDatabase database;
        private Chest chest;
        private Chest chest2;
        private Chest chest3;

        private Texture2D enemySlimeTexture;
        private Texture2D enemyZombieTexture;
        private Texture2D enemyMageTexture;
        private Texture2D enemyBatTexture;
        private List<Enemy> enemies;

        private EnemyManager enemyManager;

        private Texture2D pixelTexture;
       
        public static Player currentPlayer
        {
            get;
            private set;
        }


        private Room room;

        private InventoryHUD inventoryHUD;
        private HealthHUD healthHUD;
        private Texture2D heartTexture;

        private SpriteFont gameFont;
        private LevelHUD levelHUD;
        private int currentLevel = 1;

        private CurrentSwordHUD currentSwordHUD;

        private float stairsCooldown = 0f;
        private const float stairsCooldownTime = 1.0f;

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
            
            // in LoadContent, anywhere after spriteBatch is created:
            pixelTexture = new Texture2D(GraphicsDevice, 1, 1);
            pixelTexture.SetData(new[] { Color.White });

            // Initialize the SoundManager to load all audio files into memory
            SoundManager.LoadContent(Content);

            // Load the knight texture and initialize the player with it
            knightTexture = Content.Load<Texture2D>("Knight");
            Texture2D magicSwordTexture = Content.Load<Texture2D>("items/MagicSword");
            Texture2D fireScrollTexture = Content.Load<Texture2D>("items/fireScroll");
            Texture2D chestTexture = Content.Load<Texture2D>("Chest");
            Texture2D fireballTexture = Content.Load<Texture2D>("Fireball");
            Texture2D detonateTexture = Content.Load<Texture2D>("Explosion");
            Texture2D shieldOfInvulnerabilityTexture = Content.Load<Texture2D>("items/shieldOfInvulnerability");
            Texture2D robeOfInvisibilityTexture = Content.Load<Texture2D>("items/robeOfInvisibility");
            Texture2D PanaceaTexture=Content.Load<Texture2D>("items/panacea");
            Texture2D magicArmor = Content.Load<Texture2D>("items/healthShield");
            Texture2D deckOfcardsTexture = Content.Load<Texture2D>("items/deckOfCards");


            gameFont = Content.Load<SpriteFont>("GameFont");

            currentSwordHUD = new CurrentSwordHUD(GraphicsDevice, gameFont);
            heartTexture = Content.Load<Texture2D>("Pixel Heart Sprite Sheet 32x32");
            levelHUD = new LevelHUD(gameFont);

            inventory = new Inventory();
            inventoryHUD = new InventoryHUD(GraphicsDevice);
            healthHUD = new HealthHUD(heartTexture);
            FireballManager = new fireballManager(fireballTexture, detonateTexture);
            database = new itemDatabase(magicSwordTexture, fireScrollTexture, FireballManager, shieldOfInvulnerabilityTexture, robeOfInvisibilityTexture, PanaceaTexture, magicArmor, deckOfcardsTexture);
            LoadRoom();

            Rectangle chestmovementBounds = GetRoomBounds();

            ChestManager = new chestManager(
            chestTexture,
            inventory,
            database,
            room,
            chestmovementBounds);
            player = new Player(new Vector2(300, 300), inventory, inventoryHUD, FireballManager, ChestManager);
            player.InitializeSprite(knightTexture);
            ChestManager.SetPlayer(player);
            currentPlayer = player;


           
            
            

            ChestManager.CreateChest(new Vector2(600, 300));
            
            enemySlimeTexture = Content.Load<Texture2D>("enemySlime");
            enemyZombieTexture = Content.Load<Texture2D>("enemyZombie");
            enemyMageTexture = Content.Load<Texture2D>("enemyMage");
            enemyBatTexture = Content.Load<Texture2D>("enemyBat");
            enemyManager = new EnemyManager(player,
                enemySlimeTexture,
                enemyZombieTexture,
                enemyMageTexture,
                enemyBatTexture,
                FireballManager);
            player.SetEnemyManager(enemyManager);

            Rectangle movementBounds = GetRoomBounds();
enemyManager.SetMovementBounds(movementBounds);
            ChestManager.SetMovementBounds(movementBounds);



            player.SetRoom(room);
            FireballManager.SetRoom(room);
            enemyManager.SetRoom(room);
            enemyManager.RespawnEnemies(
                EnemyType.Basic,
                EnemyType.Basic,
                EnemyType.Chaser,
                EnemyType.Fleeing,
                EnemyType.Erratic);
      






            base.LoadContent();
        }

private void DrawDebugBounds(SpriteBatch spriteBatch)
{
    if (!Microsoft.Xna.Framework.Input.Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F1))
        return;
    DrawRectOutline(spriteBatch, player.Bounds, Color.Red);
    foreach (var tile in room.Tiles)
        DrawRectOutline(spriteBatch, tile.Bounds, tile.IsSolid ? Color.Cyan : Color.Yellow);
}

private void DrawRectOutline(SpriteBatch spriteBatch, Rectangle rect, Color color)
{
    const int t = 2;
    spriteBatch.Draw(pixelTexture, new Rectangle(rect.Left, rect.Top, rect.Width, t), color);
    spriteBatch.Draw(pixelTexture, new Rectangle(rect.Left, rect.Bottom - t, rect.Width, t), color);
    spriteBatch.Draw(pixelTexture, new Rectangle(rect.Left, rect.Top, t, rect.Height), color);
    spriteBatch.Draw(pixelTexture, new Rectangle(rect.Right - t, rect.Top, t, rect.Height), color);
}

        /// <summary>
        /// updates the game state, including player input and game logic. This method is called once per frame.
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Update(GameTime gameTime)
        {
            if (stairsCooldown > 0)
            {
                stairsCooldown -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (player.IsQuit())
                Exit();
            room.Update(gameTime);
            ChestManager.Update(gameTime);

            player.Update(gameTime);
;
            FireballManager.Update(gameTime);

            enemyManager.update(gameTime, player.GetTerrainAcceleration());

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

            room.Draw(spriteBatch);
            player.Draw(spriteBatch);

            FireballManager.Draw(spriteBatch);
            ChestManager.Draw(spriteBatch);
            enemyManager.draw(spriteBatch);
            inventoryHUD.Draw(spriteBatch, GraphicsDevice, inventory, gameTime);
            healthHUD.Draw(spriteBatch, player);

            
            levelHUD.Draw(spriteBatch, currentLevel);
            currentSwordHUD.Draw(spriteBatch, inventory);

            DrawDebugBounds(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private Texture2D CreatePlaceholderTexture(Color color)
        {
            Texture2D texture = new Texture2D(GraphicsDevice, 1, 1);
            texture.SetData(new[] { color });
            return texture;
        }

       private Rectangle GetRoomBounds()
{
    int cols = DeviceManager3902.PreferredBackBufferWidth / 64;
    int rows = DeviceManager3902.PreferredBackBufferHeight / 64;
    return new Rectangle(0, 0, cols * 64, rows * 64);
}

private Vector2 GridPosition(int col, int row) => new Vector2(32 + col * 64, 32 + row * 64);

private Texture2D environmentSpriteSheet;

private void LoadRoom()
{
    Rectangle roomBounds = GetRoomBounds();
    int cols = roomBounds.Width / 64;
    int rows = roomBounds.Height / 64;

    room = new Room(tileSize: 64f, worldBounds: roomBounds);
    environmentSpriteSheet = Content.Load<Texture2D>("environment/environment_spritesheet");

    for (int col = 0; col < cols; col++)
    {
        room.AddTile(TileFactory.CreateTile(TileType.Wall, environmentSpriteSheet, GridPosition(col, 0)));
        room.AddTile(TileFactory.CreateTile(TileType.Wall, environmentSpriteSheet, GridPosition(col, rows - 1)));
    }
    for (int row = 0; row < rows; row++)
    {
        room.AddTile(TileFactory.CreateTile(TileType.Wall, environmentSpriteSheet, GridPosition(0, row)));
        room.AddTile(TileFactory.CreateTile(TileType.Wall, environmentSpriteSheet, GridPosition(cols - 1, row)));
    }

    room.AddTile(TileFactory.CreateTile(TileType.Statue, environmentSpriteSheet, GridPosition(7, 7)));
    room.AddTile(TileFactory.CreateTile(TileType.SquareBlock, environmentSpriteSheet, GridPosition(10, 7)));
    room.AddTile(TileFactory.CreateTile(TileType.PushableBlock, environmentSpriteSheet, GridPosition(13, 7)));
    room.AddTile(TileFactory.CreateTile(TileType.Fire, environmentSpriteSheet, GridPosition(16, 7)));
    room.AddTile(TileFactory.CreateTile(TileType.BlueGap, environmentSpriteSheet, GridPosition(19, 7)));

    StairsTile stairs = (StairsTile)TileFactory.CreateTile(TileType.Stairs, environmentSpriteSheet, GridPosition(22, 7));
    stairs.Traversed += OnStairsTraversed;
    room.AddTile(stairs);

    room.AddTile(TileFactory.CreateTile(TileType.OpenDoor, environmentSpriteSheet, GridPosition(15, 1)));

    room.AddTile(new BombedWallOpeningTile(environmentSpriteSheet, GridPosition(13, 10)));
    room.AddTile(new KeyholeLockedDoorTile(environmentSpriteSheet, GridPosition(16, 10)));
    room.AddTile(new DiamondLockedDoorTile(environmentSpriteSheet, GridPosition(19, 10)));
}

        private void OnStairsTraversed(StairsTile stairs)
        {
            if (stairsCooldown <= 0)
            {
                currentLevel++;
                stairsCooldown = stairsCooldownTime;
            }
        }
    }
}