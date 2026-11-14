using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace _3902sprint0
{
    public class Bomb
    {
        public Vector2 Location;
        public bool IsCollected = false;
        public bool activated = false;

        private BombSprite sprite;
        private BombSpriteActive spriteActive;

       
        private float explosionTimer = 0.5f;

        public bool IsExplosionActive { get; private set; }


        private readonly detonateSprite explosionSprite;
        private float fuseTimer = 3f;
        public bool HasBrokenWalls { get; set; }
        public int ExplosionDamage { get; set; } = 1;

        private readonly HashSet<Enemy> explosionDamagedEnemies =
            new HashSet<Enemy>();

        private bool hasDamagedPlayer = false;

        public bool HasExplosionDamaged(Enemy enemy)
        {
            return explosionDamagedEnemies.Contains(enemy);
        }

        public void MarkExplosionDamage(Enemy enemy)
        {
            explosionDamagedEnemies.Add(enemy);
        }
        public Bomb(Vector2 location, Texture2D texture,
            Texture2D detonateTexture, bool activated)
        {
            Location = location;
            this.activated = activated;

            if (activated)
                spriteActive = new BombSpriteActive(texture);
            else
                sprite = new BombSprite(texture);

            explosionSprite = new detonateSprite();
            explosionSprite.Initialize(detonateTexture);
            explosionSprite.SetLocation(location);
        }

        public Rectangle Bounds =>
            new Rectangle((int)Location.X, (int)Location.Y, 48, 48);

        public Rectangle ExplosionBounds =>
            new Rectangle(
                Bounds.Center.X - 64,
                Bounds.Center.Y - 64,
                128,
                128);

        public Bomb(Vector2 location, Texture2D texture, bool activated)
        {
            Location = location;
            this.activated = activated;

            if (activated)
                spriteActive = new BombSpriteActive(texture);
            else
                sprite = new BombSprite(texture);
        }

        public void Update(GameTime gameTime, Player player)
        {

            if (IsCollected)
                return;

            if (activated && !IsExplosionActive)
            {
                fuseTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (fuseTimer <= 0)
                {
                    IsExplosionActive = true;
                    explosionSprite.SetLocation(Location);
                    explosionSprite.activate();
                    SoundManager.PlayExplosion();
                }
            }

            if (IsExplosionActive)
            {
                explosionSprite.UpdateSprite(gameTime);
                if (!hasDamagedPlayer && ExplosionBounds.Intersects(player.Bounds))
                {
                    player.TakeDamage(ExplosionDamage);
                    hasDamagedPlayer = true;
                }

                if (!explosionSprite.IsDetonating)
                {
                    IsExplosionActive = false;
                    IsCollected = true;
                }

                return;
            }

            if (activated)
                spriteActive.Update(gameTime);
            else
                sprite.Update(gameTime);

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsCollected)
                return;


            if (IsExplosionActive)
            {
                explosionSprite.Draw(spriteBatch);
            }
            else if (activated)
            {
                spriteActive.Draw(spriteBatch, Location);
            }
            else
            {
                sprite.Draw(spriteBatch, Location);
            }

        }
    }
}