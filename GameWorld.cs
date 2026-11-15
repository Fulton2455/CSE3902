
using System;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using _3902sprint0.Environment;
using _3902sprint0.Environment.Rooms;
using _3902sprint0.Environment.Tiles;
using _3902sprint0.NPCs;
using _3902sprint0.NeutralProjectiles;

namespace _3902sprint0
{
    /// <summary>
    /// One run of the game: the player, rooms, enemies, items and HUD,
    /// and the rules for updating and drawing them.
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
        private readonly NPCmanager npcManager;
        private readonly NeutralProjectileManager neutralProjectiles;

        private readonly BombManager bombManager;
        private readonly BombHUD bombHUD;

        private readonly CompassPointer compassPointer;
        private readonly Compass compass;
        private readonly CompassHUD compassHUD;
        private RoomSide? compassDir;

        public Player Player { get; }

        public bool IsDeathAnimationFinished => Player.HasFinishedDying;

        public Color BackgroundColor => Player.Terrain.terrainColor;

        private bool IsInChestRoom =>
            roomManager.Current.Data.Id == StartRoomId;

        public GameWorld(
            GraphicsDevice graphicsDevice,
            ContentManager content,
            GameAssets assets)
        {
            this.graphicsDevice = graphicsDevice;

            debugOverlay = new DebugOverlay(graphicsDevice);
            levelProgression = new LevelProgression();

            rupee = new Rupee(new Vector2(500, 300), assets.RupeeTexture);
            rupeeHUD = new RupeeHUD(assets.GameFont, assets.RupeeTexture);

            // Compass
            compass = new Compass(assets.CompassTexture);
            compassHUD = new CompassHUD(assets.GameFont, assets.CompassTexture);

            EffectMessage.Load(assets.GameFont);
            ChestMessage.Load(assets.GameFont);

            currentSwordHUD = new CurrentSwordHUD(graphicsDevice, assets.GameFont);
            levelHUD = new LevelHUD(assets.GameFont);

            // Inventory and item systems
            inventory = new Inventory();
            inventoryHUD = new InventoryHUD(graphicsDevice);
            healthHUD = new HealthHUD(assets.HeartTexture);

            fireballs = new fireballManager(
                assets.FireballTexture,
                assets.DetonateTexture);

            var database = new itemDatabase(
                assets.MagicSwordTexture,
                assets.FireScrollTexture,
                fireballs,
                assets.ShieldOfInvulnerabilityTexture,
                assets.RobeOfInvisibilityTexture,
                assets.PanaceaTexture,
                assets.MagicArmorTexture,
                assets.DeckOfCardsTexture);

            Rectangle roomBounds = ScreenScaler.GetRoomBounds();

            // Dungeon and rooms
            DungeonMap dungeon = DungeonMap.LoadFromDirectory(
                Path.Combine(AppContext.BaseDirectory, "Content", "rooms"));

            compassPointer = new CompassPointer(dungeon, StartRoomId);
            compassDir = compassPointer.DirectionFrom(StartRoomId);

            var roomTextures = new RoomTextures(
                assets.EnvironmentSpriteSheet,
                content.Load<Texture2D>("environment/doors"),
                content.Load<Texture2D>("items/smallKey"));

            roomManager = new RoomManager(
                dungeon,
                new RoomBuilder(roomTextures, roomBounds));

            roomManager.RoomCreated += OnRoomCreated;
            roomManager.RoomEntered += OnRoomEntered;

            Room startRoom = roomManager.Preload(StartRoomId);

            floorRenderer = new FloorRenderer(
                content.Load<Texture2D>("environment/floor_tiles"),
                roomBounds);

            // Chests
            chests = new chestManager(
                assets.ChestTexture,
                inventory,
                database,
                startRoom,
                roomBounds);

            // Player
            Player = new Player(
                new Vector2(300, 300),
                inventory,
                inventoryHUD,
                fireballs,
                chests);

            Player.InitializeSprite(assets.KnightTexture);

            chests.SetPlayer(Player);
            chests.CreateChest(new Vector2(600, 300));

            // Bombs
            bombHUD = new BombHUD(
                assets.GameFont,
                assets.BombTexture,
                Player.BombCount);

            bombManager = new BombManager(
                assets.BombTexture,
                assets.DetonateTexture,
                inventory,
                database,
                startRoom,
                roomBounds);

            bombManager.SetPlayer(Player);
            Player.SetBombManager(bombManager);

            // Enemies
            enemyManager = new EnemyManager(
                Player,
                assets.EnemySlimeTexture,
                assets.EnemyZombieTexture,
                assets.EnemyMageTexture,
                assets.EnemyBatTexture,
                assets.EnemyToadTexture,
                assets.ShockwavePixel,
                fireballs);

            Player.SetEnemyManager(enemyManager);
            enemyManager.SetBombManager(bombManager);

            enemyManager.SetMovementBounds(roomBounds);
            chests.SetMovementBounds(roomBounds);

            fireballs.SetAcidTexture(assets.AcidTexture);

            // Room behavior
            new RoomPopulator(roomManager, enemyManager);
            new RoomTerrainApplier(roomManager, Player);

            shutterController = new ShutterController(
                roomManager,
                HasLivingEnemies,
                () => Player.Bounds);

            explosionWallBreaker = new ExplosionWallBreaker(
                roomManager,
                bombManager);

            combatResolver = new CombatResolver(Player, enemyManager);

            npcManager = new NPCmanager(Player, inventory, roomManager, assets.WhitePixel, assets.GameFont,
                assets.PanaceaTexture, assets.FireScrollTexture, fireballs);
            neutralProjectiles = new NeutralProjectileManager(Player, roomManager, assets.FireballTexture);

            // Step 2: place the player, hand the room to player/enemies/fireballs, spawn enemies.
            roomManager.Begin(Player, inventory);
        }

        public void Update(GameTime gameTime)
        {
            levelProgression.Update(gameTime);

            EffectMessage.Update(gameTime);
            ChestMessage.Update(gameTime);

            roomManager.CurrentRoom.Update(gameTime);

            // Chest behavior
            chests.Update(gameTime);

            Player.Update(gameTime);

            roomManager.Update();
            npcManager.Update(gameTime);
            neutralProjectiles.Update(gameTime);
            shutterController.Update();

            // Bomb behavior
            bombManager.Update(gameTime);
            explosionWallBreaker.Update();
            bombHUD.UpdateCount(Player.BombCount);

            rupee.Update(gameTime, Player);

            // Compass only updates in the starting chest room.
            if (IsInChestRoom && Player.HasCompass)
                compass.Update(Player);

            fireballs.Update(gameTime);

            enemyManager.update(
                gameTime,
                Player.GetTerrainAcceleration());

            combatResolver.Update();
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            floorRenderer.Draw(
                spriteBatch,
                Player.Terrain.currentState);

            roomManager.CurrentRoom.Draw(spriteBatch);
            roomManager.DrawPickups(spriteBatch);

            Player.Draw(spriteBatch);

            npcManager.Draw(spriteBatch);
            neutralProjectiles.Draw(spriteBatch);
            fireballs.Draw(spriteBatch);
            chests.Draw(spriteBatch);
            enemyManager.draw(spriteBatch);
            bombManager.Draw(spriteBatch);

            // HUD
            inventoryHUD.Draw(
                spriteBatch,
                graphicsDevice,
                inventory,
                gameTime);

            healthHUD.Draw(spriteBatch, Player);
            bombHUD.Draw(spriteBatch);

            levelHUD.Draw(
                spriteBatch,
                levelProgression.CurrentLevel);

            currentSwordHUD.Draw(spriteBatch, inventory);

            rupeeHUD.Draw(
                spriteBatch,
                Player.RupeeCount);

            if (Player.HasCompass)
                compassHUD.Draw(spriteBatch, compassDir);

            debugOverlay.Draw(
                spriteBatch,
                Player,
                roomManager.CurrentRoom,
                enemyManager,
                fireballs,
                chests);

            EffectMessage.Draw(
                spriteBatch,
                Player.getPosition() - new Vector2(0, 90));

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

            // Update chest room
            chests.SetRoom(
                instance.Room,
                instance.Data.Id);

            // Update compass direction
            if (Player.HasCompass)
            {
                compassDir = compassPointer.DirectionFrom(
                    instance.Data.Id);
            }

            // Update fireballs and clear old room objects
            fireballs.SetRoom(instance.Room);

            inventory.ResetItems();
            fireballs.clearFireballs();
            bombManager.Clear();
        }

        private bool HasLivingEnemies()
        {
            return enemyManager.Enemies.Any(
                e => !e.IsDying && !e.IsDead);
        }
    }
}
