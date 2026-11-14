
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3902sprint0
{
    internal class BombSprite
    {
        private Texture2D texture;

        public BombSprite(Texture2D texture)
        {
            this.texture = texture;
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            Rectangle destination = new Rectangle(
                (int)position.X, (int)position.Y, 48, 48);

            spriteBatch.Draw(texture, destination, Color.White);
        }
    }
}
