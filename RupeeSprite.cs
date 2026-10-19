
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3902sprint0
{
    internal class RupeeSprite
    {
        private Texture2D texture;
        private int frame = 0;
        private float timer = 0f;

        private const int FrameSize = 16;
        private const float FrameTime = 0.1f;

        public RupeeSprite(Texture2D texture)
        {
            this.texture = texture;
        }

        public void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timer >= FrameTime)
            {
                timer = 0f;
                frame = (frame + 1) % (texture.Width / FrameSize);
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            Rectangle source = new Rectangle(
                frame * FrameSize, 0, FrameSize, FrameSize);

            Rectangle destination = new Rectangle(
                (int)position.X, (int)position.Y, 48, 48);

            spriteBatch.Draw(texture, destination, source, Color.White);
        }
    }
}
