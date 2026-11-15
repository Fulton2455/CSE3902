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

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            // Simple merchant appearance drawn with the shared one-pixel texture.
            spriteBatch.Draw(texture, Bounds, Color.White);
           
        }
    }
}
