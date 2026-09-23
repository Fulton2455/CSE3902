//Connor Fulton
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3902sprint0.Environment
{
   public interface ITile
    {
        Texture2D Texture { get; }
        Vector2 Position { get; }
        Rectangle Bounds { get; }
        bool IsSolid { get; }

        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
    }
}