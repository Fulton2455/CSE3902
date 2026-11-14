
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3902sprint0
{
    internal class BombSpriteActive
    {
        private Texture2D texture;

        public BombSpriteActive(Texture2D texture)
        {
            this.texture = texture;
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            Rectangle destination = new Rectangle(
                (int)position.X+48, (int)position.Y+48, 48, 48);

            spriteBatch.Draw(texture, destination, Color.White);
        }
    }
}
