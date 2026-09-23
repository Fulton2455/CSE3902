// Connor Fulton
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3902sprint0.Environment
{
    public abstract class Tile : ITile
    {
        protected Texture2D texture;
        protected Rectangle sourceRectangle;
        protected Rectangle destinationRectangle;
        protected int width;
        protected int height;

        public Vector2 Position { get; protected set; }
        public Texture2D Texture => texture;
        public Rectangle Bounds => destinationRectangle;
        public virtual bool IsSolid { get; protected set; }

        protected Tile(Texture2D texture, Vector2 position, Rectangle sourceRectangle, int width = 64, int height = 64)
        {
            this.texture = texture;
            this.sourceRectangle = sourceRectangle;
            this.width = width;
            this.height = height;
            Position = position;
            UpdateDestinationRectangle();
        }

        protected void UpdateDestinationRectangle()
        {
            destinationRectangle = new Rectangle(
                (int)(Position.X - width / 2),
                (int)(Position.Y - height / 2),
                width,
                height);
        }

        public virtual void Update(GameTime gameTime) { }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, Color.White);
        }
    }
}