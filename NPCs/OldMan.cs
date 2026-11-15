using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3902sprint0.NPCs
{
    /// <summary>A stationary shopkeeper who sells inventory items for the player's rupees.</summary>
    public sealed class OldMan
    {
        public Vector2 Position { get; }
        public Rectangle Bounds => new Rectangle((int)Position.X - 20, (int)Position.Y - 30, 40, 60);

        public OldMan(Vector2 position)
        {
            Position = position;
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D pixel)
        {
            // Simple merchant appearance drawn with the shared one-pixel texture.
            spriteBatch.Draw(pixel, new Rectangle(Bounds.X + 4, Bounds.Y + 25, 32, 35), new Color(75, 105, 145));
            spriteBatch.Draw(pixel, new Rectangle(Bounds.X + 8, Bounds.Y + 2, 24, 27), new Color(225, 190, 145));
            spriteBatch.Draw(pixel, new Rectangle(Bounds.X + 5, Bounds.Y, 30, 12), new Color(115, 85, 55));
            spriteBatch.Draw(pixel, new Rectangle(Bounds.X + 10, Bounds.Y + 18, 20, 15), new Color(235, 230, 215));
            spriteBatch.Draw(pixel, new Rectangle(Bounds.X + 13, Bounds.Y + 12, 3, 3), Color.Black);
            spriteBatch.Draw(pixel, new Rectangle(Bounds.X + 24, Bounds.Y + 12, 3, 3), Color.Black);
        }
    }
}
