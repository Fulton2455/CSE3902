using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Globalization;
using static System.Net.Mime.MediaTypeNames;
using _3902sprint0.Environment;
using _3902sprint0.Environment.Tiles;

namespace _3902sprint0
{
    /// <summary>
    /// The main class for the game.
    /// </summary>
    public class Game1 : Game
    {
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

            Rectangle roomBounds = RoomLoader.GetRoomBounds(ScreenScaler.VirtualWidth, ScreenScaler.VirtualHeight);
            room = RoomLoader.LoadRoom(roomBounds, assets.EnvironmentSpriteSheet);
            foreach (var tile in room.Tiles)
            {
                if (tile is StairsTile stairs)
                    stairs.Traversed += levelProgression.OnStairsTraversed;
            }
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

            player.SetRoom(room);
            FireballManager.SetRoom(room);
            FireballManager.SetAcidTexture(assets.AcidTexture);
            enemyManager.SetRoom(room);
            enemyManager.RespawnEnemies(
                EnemyType.Basic,
                EnemyType.Basic,
                EnemyType.Chaser,
                EnemyType.Fleeing,
                EnemyType.Erratic);

            combatResolver = new CombatResolver(player, enemyManager);

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            levelProgression.Update(gameTime);
            EffectMessage.Update(gameTime);
            ChestMessage.Update(gameTime);

            if (player.IsQuit())
                Exit();
            room.Update(gameTime);
            ChestManager.Update(gameTime);

            player.Update(gameTime);

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
            room.Draw(spriteBatch);
            player.Draw(spriteBatch);
            room.Draw(spriteBatch);
            player.Draw(spriteBatch);

            FireballManager.Draw(spriteBatch);
            ChestManager.Draw(spriteBatch);
            enemyManager.draw(spriteBatch);
            inventoryHUD.Draw(spriteBatch, GraphicsDevice, inventory, gameTime);
            healthHUD.Draw(spriteBatch, player);

            levelHUD.Draw(spriteBatch, levelProgression.CurrentLevel);
            currentSwordHUD.Draw(spriteBatch, inventory);
            rupeeHUD.Draw(spriteBatch, player.RupeeCount);

            debugOverlay.Draw(spriteBatch, player, room, enemyManager, FireballManager, ChestManager);
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