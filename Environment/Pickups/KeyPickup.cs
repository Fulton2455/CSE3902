using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3902sprint0.Environment.Pickups
{
    // key lying on the floor. Picked up by touching it.
    public class KeyPickup
    {
        private const int DrawSize = 48;

        private readonly Texture2D texture;
        private readonly Rectangle bounds;
        private readonly LockType lockType;

        public bool Collected { get; private set; }

        public KeyPickup(Texture2D texture, Vector2 center, LockType lockType)
        {
            this.texture = texture;
            this.lockType = lockType;
            bounds = new Rectangle((int)center.X - DrawSize / 2, (int)center.Y - DrawSize / 2, DrawSize, DrawSize);
        }

        public void Update(Rectangle collectorBounds, IKeyHolder keyHolder)
        {
            if (Collected || !bounds.Intersects(collectorBounds))
                return;

            Collected = true;
            keyHolder.AddKey(lockType);
            if (lockType == LockType.Diamond)
                EffectMessage.Show("Diamond Key!", Color.Gold);
            else
                EffectMessage.Show("Key!", Color.Gold);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Collected) return;
            Color tint = lockType == LockType.Diamond ? Color.MediumPurple : Color.White;
            spriteBatch.Draw(texture, bounds, tint);
        }
    }
}
