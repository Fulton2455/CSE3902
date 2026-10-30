using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;
using _3902sprint0.Environment;
using _3902sprint0.Environment.Rooms;
using _3902sprint0.Environment.Tiles;

namespace _3902sprint0
{
    /// <summary>
    /// The main class for the game.
    /// </summary>
    public class Game1 : Game
    {
        private const string StartRoomId = "d1_c6";

        private fireballManager FireballManager;
        private GraphicsDeviceManager DeviceManager3902;
        private SpriteBatch spriteBatch;
        private RenderTarget2D sceneTarget;
        private bool applyingWindowSize;
        private Player player;
        private Mousecontroller Mouse;
        private KeyboardController Keyboard;

        private chestManager ChestManager;
        private Inventory inventory;
        private itemDatabase database;

        private EnemyManager enemyManager;

        private GameAssets assets;
        private DebugOverlay debugOverlay;
        private CombatResolver combatResolver;
        private LevelProgression levelProgression;

        private RoomManager roomManager;
        private RoomPopulator roomPopulator;
        private RoomTerrainApplier roomTerrainApplier;
        private ShutterController shutterController;
        private ExplosionWallBreaker explosionWallBreaker;

        public static Player currentPlayer { get; private set; }

        private Room room;

        private InventoryHUD inventoryHUD;
        private HealthHUD healthHUD;

        private LevelHUD levelHUD;

        private CurrentSwordHUD currentSwordHUD;

        private Rupee rupee;
        private RupeeHUD rupeeHUD;

        private FloorRenderer floorRenderer;

        public readonly static bool IsDesktop = OperatingSystem.IsMacOS() || OperatingSystem.IsLinux() || OperatingSystem.IsWindows();

        public Game1()
        {
            DeviceManager3902 = new GraphicsDeviceManager(this);
            DisplayMode desktop = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
            Point startSize = ScreenScaler.FitWindowSize((int)(desktop.Width * 0.9f), (int)(desktop.Height * 0.9f));
            DeviceManager3902.PreferredBackBufferWidth = startSize.X;
            DeviceManager3902.PreferredBackBufferHeight = startSize.Y;
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += OnClientSizeChanged;
            Content.RootDirectory = "Content";
        }

        private void OnClientSizeChanged(object sender, EventArgs e)
        {
            if (applyingWindowSize)
                return;
            int width = Window.ClientBounds.Width;
            int height = Window.ClientBounds.Height;
            if (width <= 0 || height <= 0)
                return;

            applyingWindowSize = true;
            DeviceManager3902.PreferredBackBufferWidth = width;
            DeviceManager3902.PreferredBackBufferHeight = height;
            DeviceManager3902.ApplyChanges();
            ScreenScaler.Update(width, height);
            applyingWindowSize = false;
        }

        protected override void Initialize()
        {
            Mouse = new Mousecontroller();
            Keyboard = new KeyboardController();
            levelProgression = new LevelProgression();

            IsMouseVisible = true;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            sceneTarget = new RenderTarget2D(GraphicsDevice, ScreenScaler.VirtualWidth, ScreenScaler.VirtualHeight);
            ScreenScaler.Update(GraphicsDevice.PresentationParameters.BackBufferWidth,
                GraphicsDevice.PresentationParameters.BackBufferHeight);

            debugOverlay = new DebugOverlay(GraphicsDevice);

            SoundManager.LoadContent(Content);

            assets = new GameAssets();
            assets.Load(Content, GraphicsDevice);

            rupee = new Rupee(new Vector2(500, 300), assets.RupeeTexture);
            rupeeHUD = new RupeeHUD(assets.GameFont, assets.RupeeTexture);

            EffectMessage.Load(assets.GameFont);
            ChestMessage.Load(assets.GameFont);

            currentSwordHUD = new CurrentSwordHUD(GraphicsDevice, assets.GameFont);
            levelHUD = new LevelHUD(assets.GameFont);

            inventory = new Inventory();
            inventoryHUD = new InventoryHUD(GraphicsDevice);
            healthHUD = new HealthHUD(assets.HeartTexture);
            FireballManager = new fireballManager(assets.FireballTexture, assets.DetonateTexture);
            database = new itemDatabase(assets.MagicSwordTexture, assets.FireScrollTexture, FireballManager,
                assets.ShieldOfInvulnerabilityTexture, assets.RobeOfInvisibilityTexture, assets.PanaceaTexture,
                assets.MagicArmorTexture, assets.DeckOfCardsTexture);

            Rectangle roomBounds = ScreenScaler.GetRoomBounds();

            // Rooms come from Content/rooms/*.txt. Step 1: build the first room.
            DungeonMap dungeon = DungeonMap.LoadFromDirectory(
                Path.Combine(AppContext.BaseDirectory, "Content", "rooms"));
            var roomTextures = new RoomTextures(
                assets.EnvironmentSpriteSheet,
                Content.Load<Texture2D>("environment/doors"),
                Content.Load<Texture2D>("items/smallKey"));
            roomManager = new RoomManager(dungeon, new RoomBuilder(roomTextures, roomBounds));
            roomManager.RoomCreated += OnRoomCreated;
            roomManager.RoomEntered += OnRoomEntered;
            room = roomManager.Preload(StartRoomId);

            floorRenderer = new FloorRenderer(
            Content.Load<Texture2D>("environment/floor_tiles"),
            roomBounds);

            ChestManager = new chestManager(assets.ChestTexture, inventory, database, room, roomBounds);

            player = new Player(new Vector2(300, 300), inventory, inventoryHUD, FireballManager, ChestManager);
            player.InitializeSprite(assets.KnightTexture);
            ChestManager.SetPlayer(player);
            currentPlayer = player;

            ChestManager.CreateChest(new Vector2(600, 300));

            enemyManager = new EnemyManager(player,
                assets.EnemySlimeTexture,
                assets.EnemyZombieTexture,
                assets.EnemyMageTexture,
                assets.EnemyBatTexture,
                assets.EnemyToadTexture,
                assets.ShockwavePixel,
                FireballManager);
            player.SetEnemyManager(enemyManager);

            enemyManager.SetMovementBounds(roomBounds);
            ChestManager.SetMovementBounds(roomBounds);

            FireballManager.SetAcidTexture(assets.AcidTexture);

            // Per-room behaviour that reacts to RoomEntered / the current room.
            roomPopulator = new RoomPopulator(roomManager, enemyManager);
            shutterController = new ShutterController(roomManager, HasLivingEnemies, () => player.Bounds);
            explosionWallBreaker = new ExplosionWallBreaker(roomManager, FireballManager);

            combatResolver = new CombatResolver(player, enemyManager);

            roomTerrainApplier = new RoomTerrainApplier(roomManager, player);
            // Step 2: place the player, hand the room to player/enemies/fireballs, spawn enemies.
            roomManager.Begin(player, inventory);

            base.LoadContent();
        }

        private void OnRoomCreated(RoomInstance instance)
        {
            foreach (var tile in instance.Room.Tiles)
            {
                if (tile is StairsTile stairs)
                    stairs.Traversed += levelProgression.OnStairsTraversed;
            }
        }

        private void OnRoomEntered(RoomInstance instance)
        {
            room = instance.Room;
            player.SetRoom(instance.Room);
            FireballManager.SetRoom(instance.Room);
            // TODO: clear in-flight fireballs here if fireballManager has a method for it.
        }

        // TODO: confirm these member names against Enemy / EnemyManager.
        private bool HasLivingEnemies()
        {
            return enemyManager.Enemies.Any(e => !e.IsDying && !e.IsDead);
        }

        // The chest still belongs to the first room until chests are made per-room.
        private bool IsInChestRoom => roomManager.Current.Data.Id == StartRoomId;

        protected override void Update(GameTime gameTime)
        {
            levelProgression.Update(gameTime);
            EffectMessage.Update(gameTime);
            ChestMessage.Update(gameTime);

            if (player.IsQuit())
                Exit();
            roomManager.CurrentRoom.Update(gameTime);
            if (IsInChestRoom)
                ChestManager.Update(gameTime);

            player.Update(gameTime);

            roomManager.Update();
            shutterController.Update();
            explosionWallBreaker.Update();

            rupee.Update(gameTime, player);

            FireballManager.Update(gameTime);

            enemyManager.update(gameTime, player.GetTerrainAcceleration());

            combatResolver.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(sceneTarget);
            GraphicsDevice.Clear(player.Terrain.terrainColor);

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            floorRenderer.Draw(spriteBatch, player.Terrain.currentState);
            roomManager.CurrentRoom.Draw(spriteBatch);
            roomManager.DrawPickups(spriteBatch);
            player.Draw(spriteBatch);

            FireballManager.Draw(spriteBatch);
            if (IsInChestRoom)
                ChestManager.Draw(spriteBatch);
            enemyManager.draw(spriteBatch);
            inventoryHUD.Draw(spriteBatch, GraphicsDevice, inventory, gameTime);
            healthHUD.Draw(spriteBatch, player);

            levelHUD.Draw(spriteBatch, levelProgression.CurrentLevel);
            currentSwordHUD.Draw(spriteBatch, inventory);
            rupeeHUD.Draw(spriteBatch, player.RupeeCount);

            debugOverlay.Draw(spriteBatch, player, roomManager.CurrentRoom, enemyManager, FireballManager, ChestManager);
            EffectMessage.Draw(spriteBatch, player.getPosition() - new Vector2(0, 90));
            ChestMessage.Draw(spriteBatch);
            rupee.Draw(spriteBatch);
            spriteBatch.End();
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.Black);

            bool wholeNumberScale = ScreenScaler.Scale == (float)Math.Floor(ScreenScaler.Scale);
            spriteBatch.Begin(samplerState: wholeNumberScale ? SamplerState.PointClamp : SamplerState.LinearClamp);
            spriteBatch.Draw(sceneTarget, ScreenScaler.Destination, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}