using System;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using _3902sprint0.Environment;
using _3902sprint0.Environment.Rooms;
using _3902sprint0.Environment.Tiles;

namespace _3902sprint0
{
    /// <summary>
    /// One run of the game: the player, rooms, enemies, items and HUD, and the rules for updating
    /// and drawing them.
    /// </summary>
    public class GameWorld
    {
        private const string StartRoomId = "d1_c6";

        private readonly GraphicsDevice graphicsDevice;
        private readonly DebugOverlay debugOverlay;
        private readonly LevelProgression levelProgression;
        private readonly Inventory inventory;
        private readonly InventoryHUD inventoryHUD;
        private readonly HealthHUD healthHUD;
        private readonly LevelHUD levelHUD;
        private readonly CurrentSwordHUD currentSwordHUD;
        private readonly Rupee rupee;
        private readonly RupeeHUD rupeeHUD;
        private readonly fireballManager fireballs;
        private readonly chestManager chests;
        private readonly EnemyManager enemyManager;
        private readonly RoomManager roomManager;
        private readonly FloorRenderer floorRenderer;
        private readonly ShutterController shutterController;
        private readonly ExplosionWallBreaker explosionWallBreaker;
        private readonly CombatResolver combatResolver;

        public Player Player { get; }

        public bool IsDeathAnimationFinished => Player.HasFinishedDying;

        /// <summary>The color to clear the screen with; it follows the current terrain.</summary>
        public Color BackgroundColor => Player.Terrain.terrainColor;

        // The chest still belongs to the first room until chests are made per-room.
        private bool IsInChestRoom => roomManager.Current.Data.Id == StartRoomId;

        public GameWorld(GraphicsDevice graphicsDevice, ContentManager content, GameAssets assets)
        {
            this.graphicsDevice = graphicsDevice;

            debugOverlay = new DebugOverlay(graphicsDevice);
            levelProgression = new LevelProgression();

            rupee = new Rupee(new Vector2(500, 300), assets.RupeeTexture);
            rupeeHUD = new RupeeHUD(assets.GameFont, assets.RupeeTexture);

            EffectMessage.Load(assets.GameFont);
            ChestMessage.Load(assets.GameFont);

            currentSwordHUD = new CurrentSwordHUD(graphicsDevice, assets.GameFont);
            levelHUD = new LevelHUD(assets.GameFont);

            inventory = new Inventory();
            inventoryHUD = new InventoryHUD(graphicsDevice);
            healthHUD = new HealthHUD(assets.HeartTexture);
            fireballs = new fireballManager(assets.FireballTexture, assets.DetonateTexture);
            var database = new itemDatabase(assets.MagicSwordTexture, assets.FireScrollTexture, fireballs,
                assets.ShieldOfInvulnerabilityTexture, assets.RobeOfInvisibilityTexture, assets.PanaceaTexture,
                assets.MagicArmorTexture, assets.DeckOfCardsTexture);

            Rectangle roomBounds = ScreenScaler.GetRoomBounds();

            // Rooms come from Content/rooms/*.txt. Step 1: build the first room.
            DungeonMap dungeon = DungeonMap.LoadFromDirectory(
                Path.Combine(AppContext.BaseDirectory, "Content", "rooms"));
            var roomTextures = new RoomTextures(
                assets.EnvironmentSpriteSheet,
                content.Load<Texture2D>("environment/doors"),
                content.Load<Texture2D>("items/smallKey"));
            roomManager = new RoomManager(dungeon, new RoomBuilder(roomTextures, roomBounds));
            roomManager.RoomCreated += OnRoomCreated;
            roomManager.RoomEntered += OnRoomEntered;
            Room startRoom = roomManager.Preload(StartRoomId);

            floorRenderer = new FloorRenderer(content.Load<Texture2D>("environment/floor_tiles"), roomBounds);

            chests = new chestManager(assets.ChestTexture, inventory, database, startRoom, roomBounds);

            Player = new Player(new Vector2(300, 300), inventory, inventoryHUD, fireballs, chests);
            Player.InitializeSprite(assets.KnightTexture);
            chests.SetPlayer(Player);
            chests.CreateChest(new Vector2(600, 300));

            enemyManager = new EnemyManager(Player,
                assets.EnemySlimeTexture,
                assets.EnemyZombieTexture,
                assets.EnemyMageTexture,
                assets.EnemyBatTexture,
                assets.EnemyToadTexture,
                assets.ShockwavePixel,
                fireballs);
            Player.SetEnemyManager(enemyManager);

            enemyManager.SetMovementBounds(roomBounds);
            chests.SetMovementBounds(roomBounds);

            fireballs.SetAcidTexture(assets.AcidTexture);

            // These three subscribe to room events themselves, so nothing needs to hold on to them.
            new RoomPopulator(roomManager, enemyManager);
            new RoomTerrainApplier(roomManager, Player);
            shutterController = new ShutterController(roomManager, HasLivingEnemies, () => Player.Bounds);
            explosionWallBreaker = new ExplosionWallBreaker(roomManager, fireballs);

            combatResolver = new CombatResolver(Player, enemyManager);

            // Step 2: place the player, hand the room to player/enemies/fireballs, spawn enemies.
            roomManager.Begin(Player, inventory);
        }

        public void Update(GameTime gameTime)
        {
            levelProgression.Update(gameTime);
            EffectMessage.Update(gameTime);
            ChestMessage.Update(gameTime);

            roomManager.CurrentRoom.Update(gameTime);
            if (IsInChestRoom)
                chests.Update(gameTime);

            Player.Update(gameTime);

            roomManager.Update();
            shutterController.Update();
            explosionWallBreaker.Update();

            rupee.Update(gameTime, Player);

            fireballs.Update(gameTime);

            enemyManager.update(gameTime, Player.GetTerrainAcceleration());

            combatResolver.Update();
        }

        /// <summary>Draws the world and its HUD into the SpriteBatch the caller has already begun.</summary>
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            floorRenderer.Draw(spriteBatch, Player.Terrain.currentState);
            roomManager.CurrentRoom.Draw(spriteBatch);
            roomManager.DrawPickups(spriteBatch);
            Player.Draw(spriteBatch);

            fireballs.Draw(spriteBatch);
            if (IsInChestRoom)
                chests.Draw(spriteBatch);
            enemyManager.draw(spriteBatch);
            inventoryHUD.Draw(spriteBatch, graphicsDevice, inventory, gameTime);
            healthHUD.Draw(spriteBatch, Player);

            levelHUD.Draw(spriteBatch, levelProgression.CurrentLevel);
            currentSwordHUD.Draw(spriteBatch, inventory);
            rupeeHUD.Draw(spriteBatch, Player.RupeeCount);

            debugOverlay.Draw(spriteBatch, Player, roomManager.CurrentRoom, enemyManager, fireballs, chests);
            EffectMessage.Draw(spriteBatch, Player.getPosition() - new Vector2(0, 90));
            ChestMessage.Draw(spriteBatch);
            rupee.Draw(spriteBatch);
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
            Player.SetRoom(instance.Room);
            fireballs.SetRoom(instance.Room);
            fireballs.clearFireballs();
        }

        private bool HasLivingEnemies()
        {
            return enemyManager.Enemies.Any(e => !e.IsDying && !e.IsDead);
        }
    }
}