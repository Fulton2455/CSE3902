using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3902sprint0
{
    internal class RupeeHUD
    {
        private SpriteFont font;
        private Texture2D texture;

        public RupeeHUD(SpriteFont font, Texture2D texture)
        {
            this.font = font;
            this.texture = texture;
        }

        public void Draw(SpriteBatch spriteBatch, int count)
        {
            Rectangle source = new Rectangle(0, 0, 16, 16);
            Rectangle destination = new Rectangle(15, 95, 32, 32);

            spriteBatch.Draw(texture, destination, source, Color.White);

            spriteBatch.DrawString(
                font,
                "x " + count,
                new Vector2(55, 95),
                Color.White);
        }
    }
}
