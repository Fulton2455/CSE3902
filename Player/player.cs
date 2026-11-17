using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using _3902sprint0.Environment;

namespace _3902sprint0
{
    public class Player : IDamageable
    {
        public const int Width = 128;
        public const int Height = 128;
        public const int MaxMana = 100;
        public const float ManaRegenerationRate = 4f;

        private const int InsetLeft = 32;
        private const int InsetRight = 32;
        private const int InsetTop = 16;
        private const int InsetBottom = 0;
        private const int SwordRange = 70;
        private const int SwordWidth = 80;
        private const float FlickerRate = 12f;

        public int RupeeCount { get; set; } = 3;
        public int BombCount { get; set; } = 0;

        public int mana = MaxMana;
        internal float manaRegenerationTimer = 0f;
        public Vector2 Location;
        public terrain Terrain;
        internal EnemyManager enemyManager;
        public Vector2 Velocity;
        public float Speed = 200f;
        public float damage = 1f;

        public int MaxShieldHealth = 6;
        public int ShieldHealth { get; internal set; } = 0;
        public float Acceleration = 500f;
        public readonly int playerMaxHealth = 12;
        public int Health { get; internal set; }

        internal KeyboardController Keyboard;
        internal Inventory inventory;
        internal Mousecontroller Mouse;
        public bool HasCompass => inventory.haveCompass;
        public bool HasMap => inventory.haveMap;
        internal Room currentRoom;
        public Vector2 AimDirection;
        public Facing Facing { get; private set; } = Facing.Down;

        internal bool invulnerable = false;
        internal double invulnerableTimer = 0;
        internal bool invisible = false;
        internal double invisibleTimer = 0;
        public bool IsInvisivble => invisible;

        internal InventoryHUD inventoryHUD;
        internal fireballManager fireballManager;
        internal PlayerSprite PlayerSprite1;
        internal chestManager chestManager;
        private BombManager bombManager;

        private readonly PlayerMovement movement;
        private readonly PlayerCombat combat;
        private readonly PlayerItems items;
        private readonly SwitchTerrain terrainHandler;

        public Rectangle Bounds => GetBoundsAt(Location);
        public bool IsDead => Health <= 0;
        public bool HasFinishedDying =>
            PlayerSprite1.IsAnimation(PlayerSprite.AnimationState.Death) && PlayerSprite1.IsAnimationFinished();
        public bool IsAttacking => PlayerSprite1.IsAnimation(PlayerSprite.AnimationState.Attack);

        private bool IsHurt => PlayerSprite1.IsAnimation(PlayerSprite.AnimationState.Hurt);
        private bool IsCelebrating => PlayerSprite1.IsAnimation(PlayerSprite.AnimationState.Pickup);
        private bool IsBusy => IsAttacking || PlayerSprite1.IsAnimation(PlayerSprite.AnimationState.UseItem);

        public Player(Vector2 startingLocation, Inventory inventory, InventoryHUD inventoryHUD, fireballManager fireballManager, chestManager chestManager)
        {
            Location = startingLocation;
            PlayerSprite1 = new PlayerSprite();
            Keyboard = new KeyboardController();
            Mouse = new Mousecontroller();
            this.inventory = inventory;
            this.inventoryHUD = inventoryHUD;
            this.fireballManager = fireballManager;
            this.chestManager = chestManager;
            movement = new PlayerMovement(this);
            combat = new PlayerCombat(this);
            items = new PlayerItems(this);
            terrainHandler = new SwitchTerrain(this);
            Health = playerMaxHealth;

            Terrain = new terrain();
            Terrain.currentState = terrain.terrainState.Stone;
        }

        public void SetEnemyManager(EnemyManager enemyManager)
        {
            this.enemyManager = enemyManager;
            Terrain.SetEnemyManager(enemyManager);
        }

        public void SetRoom(Room room) { currentRoom = room; }
        public void SetBombManager(BombManager bombManager) { this.bombManager = bombManager; }

        public void InitializeSprite(Texture2D texture)
        {
            PlayerSprite1.SetLocation(Location);
            PlayerSprite1.Initialize(texture);
        }

       
        public void Update(GameTime gameTime)
        {
            UpdateInventoryItems(gameTime);
            UpdateInvulnerability(gameTime);
            UpdateInvisibility(gameTime);
            fireballHitCheck();

            Keyboard.update();
            Mouse.update(Location);
            AimDirection = Mouse.AimDirection;

            if (Keyboard.Bomb())
                bombManager?.SpawnActiveBomb();

            if (PlayerSprite1.IsAnimation(PlayerSprite.AnimationState.Death))
            {
                if (!Keyboard.revive())
                {
                    Velocity = Vector2.Zero;
                    PlayerSprite1.UpdateSprite(gameTime);
                    return;
                }
                Revive();
            }

            useItem(gameTime);
            if (Keyboard.forceItem())
                ForceChestItem();
            RegenerateMana(gameTime);

            if (Keyboard.die())
            {
                KillInstantly();
                PlayerSprite1.UpdateSprite(gameTime);
                return;
            }

            UpdateActions(gameTime, Keyboard.direction);
        }

      
        private void UpdateActions(GameTime gameTime, Vector2 direction)
        {
            if (IsCelebrating)
            {
                Velocity = Vector2.Zero;
            }
            else
            {
                if (!IsBusy && !IsHurt && Mouse.IsLeftClickPressed())
                {
                    Facing = FacingExtensions.FromDirection(AimDirection, Facing);
                    Attack();
                }
                else if (!IsBusy && !IsHurt)
                {
                    Facing = FacingExtensions.FromDirection(direction, Facing);
                }

                // While hurt, no steering
                Move(gameTime, IsHurt ? Vector2.Zero : direction);
                ClampToWorld();

                if (!IsAttacking)
                    newTerrain(gameTime);
            }

            PlayerSprite1.SetFacing(Facing);
            PlayerSprite1.SetLocation(Location);
            PlayerSprite1.UpdateSprite(gameTime);
            ReturnToLocomotion(direction);
        }


        private void ReturnToLocomotion(Vector2 direction)
        {
            bool inTimedPose = IsBusy || IsHurt || IsCelebrating;
            if (inTimedPose && !PlayerSprite1.IsAnimationFinished())
                return;

            PlayerSprite1.SetAnimation(direction != Vector2.Zero
                ? PlayerSprite.AnimationState.Walk
                : PlayerSprite.AnimationState.Idle);
        }

        private void ClampToWorld()
        {
            Location.X = MathHelper.Clamp(Location.X, 0, 1820 - Width);
            Location.Y = MathHelper.Clamp(Location.Y, 0, 1280 - Height);
        }

        private void UpdateInventoryItems(GameTime gameTime)
        {
            foreach (Iitem item in inventory.items)
                item.Update(gameTime);
        }

        private void UpdateInvulnerability(GameTime gameTime)
        {
            if (!invulnerable)
                return;

            invulnerableTimer -= gameTime.ElapsedGameTime.TotalSeconds;
            if (invulnerableTimer <= 0)
                invulnerable = false;
        }

        private void KillInstantly()
        {
            Velocity = Vector2.Zero;
            Health = 0;
            Die();
        }

   

        public void Draw(SpriteBatch spriteBatch)
        {
            bool flickerOff = invulnerable && ((int)(invulnerableTimer * FlickerRate) % 2 == 0);
            PlayerSprite1.Tint = flickerOff ? Color.White * 0.35f : Color.White;
            PlayerSprite1.Draw(spriteBatch);
        }


        // Call when the player collects an item worth celebrating
        public void CelebratePickup()
        {
            if (IsDead)
                return;

            Velocity = Vector2.Zero;
            PlayerSprite1.SetAnimation(PlayerSprite.AnimationState.Pickup);
        }

        internal void PlayUseItemPose()
        {
            if (IsBusy || IsHurt || IsCelebrating || IsDead)
                return;

            Facing = FacingExtensions.FromDirection(AimDirection, Facing);
            PlayerSprite1.SetAnimation(PlayerSprite.AnimationState.UseItem);
        }

        // ---------- sword ----------

        public Rectangle SwordBounds
        {
            get
            {
                Rectangle b = Bounds;
                switch (Facing)
                {
                    case Facing.Right: return new Rectangle(b.Right, b.Center.Y - SwordWidth / 2, SwordRange, SwordWidth);
                    case Facing.Left:  return new Rectangle(b.Left - SwordRange, b.Center.Y - SwordWidth / 2, SwordRange, SwordWidth);
                    case Facing.Down:  return new Rectangle(b.Center.X - SwordWidth / 2, b.Bottom, SwordWidth, SwordRange);
                    default:           return new Rectangle(b.Center.X - SwordWidth / 2, b.Top - SwordRange, SwordWidth, SwordRange);
                }
            }
        }

        // ---------- geometry ----------

        internal Rectangle GetBoundsAt(Vector2 location)
        {
            return new Rectangle(
                (int)location.X + InsetLeft,
                (int)location.Y + InsetTop,
                Width - InsetLeft - InsetRight,
                Height - InsetTop - InsetBottom);
        }

        public Vector2 getPosition()
        {
            return new Vector2(Location.X + Width / 2, Location.Y + Height / 2);
        }

        public void heal(int amount) { combat.Heal(amount); }
        public void ActivateShield() { combat.ActivateShield(); }
        public void RemoveShield() { combat.RemoveShield(); }
        public void TakeDamage(int amount) { combat.TakeDamage(amount, null); }
        public void TakeDamage(int amount, Vector2 sourcePosition) { combat.TakeDamage(amount, sourcePosition); }
        public void becomeInvulnerable() { combat.BecomeInvulnerable(); }
        public void becomeInvisible() { combat.BecomeInvisible(); }
        internal void UpdateInvisibility(GameTime gameTime) { combat.UpdateInvisibility(gameTime); }
        internal void fireballHitCheck() { combat.FireballHitCheck(); }
        internal void Attack() { combat.Attack(); }
        internal void Die() { combat.Die(); }
        internal void Revive() { combat.Revive(); }

        internal void RegenerateMana(GameTime gameTime) { items.RegenerateMana(gameTime); }
        internal void useItem(GameTime gameTime) { items.UseItem(gameTime); }
        public void ForceChestItem() { items.ForceChestItem(); }
        internal void SpawnActiveBomb() { bombManager?.SpawnActiveBomb(); }

        public float GetTerrainAcceleration() { return movement.GetTerrainAcceleration(); }
        public float getTerrainspeed() { return Speed; }
        internal void Move(GameTime gameTime, Vector2 movementDirection) { movement.Move(gameTime, movementDirection); }
        internal void MoveAxis(Vector2 delta) { movement.MoveAxis(delta); }
        internal void newTerrain(GameTime gameTime) { terrainHandler.UpdateTerrain(gameTime); }

        public bool IsQuit() { return Keyboard.isQuit(); }
    }
}