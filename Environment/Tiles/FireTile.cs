// Connor Fulton

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3902sprint0.Environment.Tiles
{
    
    // Walkable hazard tile. Doesn't block movement, but damages anything standing on it.
    
    public class FireTile : Tile, IHazard
    {
        public int Damage { get; }
        private double tickCooldownMs;
        private const double TickIntervalMs = 500;

        public FireTile(Texture2D texture, Vector2 position, Rectangle sourceRectangle, int damage = 1, int width = 64, int height = 64)
            : base(texture, position, sourceRectangle, width, height)
        {
            IsSolid = false;
            Damage = damage;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (tickCooldownMs > 0)
                tickCooldownMs -= gameTime.ElapsedGameTime.TotalMilliseconds;
        }

        public void ApplyEffect(IDamageable target)
        {
            if (tickCooldownMs > 0)
                return;

            target.TakeDamage(Damage);
            tickCooldownMs = TickIntervalMs;
        }
    }
}