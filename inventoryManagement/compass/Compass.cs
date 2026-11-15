using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3902sprint0
{
    public class Compass
    {
        public Vector2 Location;
        public bool IsCollected = false;
        private readonly Texture2D texture;

        public Rectangle Bounds => new Rectangle((int)Location.X, (int)Location.Y, 48, 48);

        public Compass(Texture2D texture)
        {
           
            this.texture = texture;
        }

        public void Update(Player player)
        {
            if (IsCollected) return;
            if (Bounds.Intersects(player.Bounds))
            {
                IsCollected = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!IsCollected)
                spriteBatch.Draw(texture, Bounds, Color.White);
        }
    }
}