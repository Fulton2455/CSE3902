
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3902sprint0
{
    internal class Rupee
    {
        public Vector2 Location;
        public bool IsCollected = false;

        private RupeeSprite sprite;

        public Rectangle Bounds =>
            new Rectangle((int)Location.X, (int)Location.Y, 48, 48);

        public Rupee(Vector2 location, Texture2D texture)
        {
            Location = location;
            sprite = new RupeeSprite(texture);
        }

        public void Update(GameTime gameTime, Player player)
        {
            if (IsCollected)
                return;

            sprite.Update(gameTime);

            if (Bounds.Intersects(player.Bounds))
            {
                IsCollected = true;
                player.RupeeCount++;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!IsCollected)
                sprite.Draw(spriteBatch, Location);
        }

        public void Reset()
        {
            IsCollected = false;
        }
    }
}
