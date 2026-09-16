

namespace _3902sprint0
{
    /// <summary>
    /// Interface for sprite objects that can be initialized, updated, and drawn.
    /// </summary>
    public interface ISprite
    {
        void Initialize(Texture2D texture);
        void UpdateSprite(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);

        // it returns the source rectangle of the sprite, which is used for rendering the correct portion of the texture.
        Rectangle GetSourceRectangle();
    }
}