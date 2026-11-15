using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3902sprint0
{
    public class BombHUD
    {
        private SpriteFont font;
        private Texture2D texture;
        private int count;

        public BombHUD(SpriteFont font, Texture2D texture, int initialCount)
        {
            this.font = font;
            this.texture = texture;
            this.count = initialCount;
        }

        public void UpdateCount(int newCount)
        {
            count = newCount;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle destination = new Rectangle(127, 95, 32, 32);
            spriteBatch.Draw(texture, destination, Color.White);

            spriteBatch.DrawString(
                font,
                "x " + count,
                new Vector2(167, 95),
                Color.White);
        }
    }
}