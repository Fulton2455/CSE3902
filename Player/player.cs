using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using _3902sprint0.Environment;

namespace _3902sprint0
{
    public class Player : IDamageable
    {
        public const int Width = 128;
        public const int Height = 128;

        public const int MaxMana = 100;

        public int RupeeCount { get; set; } = 0;

        public const float ManaRegenerationRate = 4f;

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
        public int Health { get; internal set; } = 6;

        internal KeyboardController Keyboard;
        internal Inventory inventory;
        internal Mousecontroller Mouse;
        internal Room currentRoom;
        public Vector2 AimDirection;
        internal bool invulnerable = false;
        internal double invulnerableTimer = 0;

        internal bool invisible = false;
        internal double invisibleTimer = 0;

        public bool IsInvisivble => invisible;

        internal InventoryHUD inventoryHUD;

        internal fireballManager fireballManager;
        internal PlayerSprite PlayerSprite1;

        private const int InsetLeft = 32;
        private const int InsetRight = 32;
        private const int InsetTop = 16;
        private const int InsetBottom = 0;
        internal chestManager chestManager;
        private readonly PlayerMovement movement;
        private readonly PlayerCombat combat;
        private readonly PlayerItems items;
        private readonly SwitchTerrain terrainHandler;

        public Rectangle Bounds => GetBoundsAt(Location);

        public bool IsDead => Health <= 0;
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

            Terrain = new terrain();
            Terrain.currentState = terrain.terrainState.Stone;

        }
        public void SetEnemyManager(EnemyManager enemyManager)
        {
            this.enemyManager = enemyManager;
            Terrain.SetEnemyManager(enemyManager);
        }

        internal Rectangle GetBoundsAt(Vector2 location)
        {
            return new Rectangle(
                (int)location.X + InsetLeft,
                (int)location.Y + InsetTop,
                Width - InsetLeft - InsetRight,
                Height - InsetTop - InsetBottom);
        }
        public void SetRoom(Room room)
        {
            currentRoom = room;
        }
        public void heal(int amount)
        { combat.Heal(amount); }
        public void ActivateShield()
        { combat.ActivateShield(); }
        public void RemoveShield()
        { combat.RemoveShield(); }
        public void TakeDamage(int amount)
        { combat.TakeDamage(amount); }
        public void InitializeSprite(Texture2D texture)
        {
            PlayerSprite1.SetLocation(Location);
            PlayerSprite1.Initialize(texture);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            PlayerSprite1.Draw(spriteBatch);
        }
        public void Update(GameTime gameTime)
        {
            foreach (Iitem item in inventory.items)
            {
                item.Update(gameTime);
            }

            if (invulnerable)
            {
                invulnerableTimer -= gameTime.ElapsedGameTime.TotalSeconds;

                if (invulnerableTimer <= 0)
                {
                    invulnerable = false;
                }
            }
            UpdateInvisibility(gameTime);
            fireballHitCheck();
            Keyboard.update();
            Vector2 movementDirection = Keyboard.direction;
            Mouse.update(Location);
            AimDirection = Mouse.AimDirection;
            if (!PlayerSprite1.IsAnimation(PlayerSprite.AnimationState.Death))
                PlayerSprite1.SetDirection(AimDirection);
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
                Velocity = Vector2.Zero;
                TakeDamage(12);
                PlayerSprite1.SetAnimation(PlayerSprite.AnimationState.Death);
                PlayerSprite1.UpdateSprite(gameTime);

                return;
            }
            if (PlayerSprite1.IsAnimation(PlayerSprite.AnimationState.Attack))
            {
                Move(gameTime, movementDirection);
                Location.X = MathHelper.Clamp(Location.X, 0, 1820 - 128);
                Location.Y = MathHelper.Clamp(Location.Y, 0, 1280 - 128);
                PlayerSprite1.SetLocation(Location);

                PlayerSprite1.UpdateSprite(gameTime);
                if (PlayerSprite1.IsAnimationFinished())
                {
                    if (movementDirection != Vector2.Zero)
                    {
                        PlayerSprite1.SetAnimation(PlayerSprite.AnimationState.Walk);
                    }
                    else
                    {
                        PlayerSprite1.SetAnimation(PlayerSprite.AnimationState.Idle);
                    }
                }
                return;
            }
            if (Mouse.IsLeftClickPressed())
            {
                Attack();
                PlayerSprite1.UpdateSprite(gameTime);
                return;
            }
            Move(gameTime, movementDirection);
            if (!PlayerSprite1.IsAnimation(PlayerSprite.AnimationState.Death))
            {
                Location.X = MathHelper.Clamp(Location.X, 0, 1820 - 128);
                Location.Y = MathHelper.Clamp(Location.Y, 0, 1280 - 128);

                newTerrain(gameTime);
                PlayerSprite1.SetLocation(Location);
                if (movementDirection != Vector2.Zero)
                {
                    PlayerSprite1.SetAnimation(PlayerSprite.AnimationState.Walk);
                }
                else
                {
                    PlayerSprite1.SetAnimation(PlayerSprite.AnimationState.Idle);
                }
                PlayerSprite1.UpdateSprite(gameTime);
            }
        }
        internal void RegenerateMana(GameTime gameTime)
        { items.RegenerateMana(gameTime); }
        internal void fireballHitCheck()
        { combat.FireballHitCheck(); }
        public float GetTerrainAcceleration()
        { return movement.GetTerrainAcceleration(); }

        public Vector2 getPosition()
        {
            Vector2 position = new Vector2(Location.X + Width / 2, Location.Y + Height / 2);
            return position;
        }

        public void becomeInvulnerable()
        { combat.BecomeInvulnerable(); }
        public void becomeInvisible()
        { combat.BecomeInvisible(); }
        internal void UpdateInvisibility(GameTime gameTime)
        { combat.UpdateInvisibility(gameTime); }
        internal void Move(GameTime gameTime, Vector2 movementDirection)
        { movement.Move(gameTime, movementDirection); }
        internal void MoveAxis(Vector2 delta)
        { movement.MoveAxis(delta); }
        public bool IsQuit()
        {
            return Keyboard.isQuit();
        }
        internal void Attack()
        { combat.Attack(); }
        internal void useItem(GameTime gameTime)
        { items.UseItem(gameTime); }
        internal void Die()
        { combat.Die(); }
        internal void Revive()
        { combat.Revive(); }
        public void ForceChestItem()
        { items.ForceChestItem(); }
        internal void newTerrain(GameTime gameTime)
        { terrainHandler.UpdateTerrain(gameTime); }

        public Rectangle SwordBounds
        {
            get
            {
                Rectangle bounds = Bounds;
                int swordRange = 70;
                int swordWidth = 80;

                if (Math.Abs(AimDirection.X) > Math.Abs(AimDirection.Y))
                {
                    if (AimDirection.X > 0)
                        return new Rectangle(bounds.Right, bounds.Center.Y - swordWidth / 2, swordRange, swordWidth);
                    else
                        return new Rectangle(bounds.Left - swordRange, bounds.Center.Y - swordWidth / 2, swordRange, swordWidth);
                }
                else
                {
                    if (AimDirection.Y > 0)
                        return new Rectangle(bounds.Center.X - swordWidth / 2, bounds.Bottom, swordWidth, swordRange);
                    else
                        return new Rectangle(bounds.Center.X - swordWidth / 2, bounds.Top - swordRange, swordWidth, swordRange);
                }
            }
        }

        public bool IsAttacking => PlayerSprite1.IsAnimation(PlayerSprite.AnimationState.Attack);

    }
}