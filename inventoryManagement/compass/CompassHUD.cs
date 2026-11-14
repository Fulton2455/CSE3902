using _3902sprint0.Environment.Rooms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3902sprint0
{
    internal class CompassHUD
    {
        private SpriteFont font;
        private Texture2D texture;

        public CompassHUD(SpriteFont font, Texture2D texture)
        {
            this.font = font;
            this.texture = texture;
        }

        public void Draw(SpriteBatch spriteBatch, RoomSide? direction)
        {
            spriteBatch.Draw(texture, new Rectangle(15, 256, 32, 32),
                new Rectangle(0, 0, 16, 16), Color.White);

            if (direction == null) return;

            string arrow = direction == RoomSide.North ? "^"
                         : direction == RoomSide.South ? "v"
                         : direction == RoomSide.West ? "<" : ">";

            spriteBatch.DrawString(font, arrow, new Vector2(55, 256), Color.Yellow);
        }
    }
}
