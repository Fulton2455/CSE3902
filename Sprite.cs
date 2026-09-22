using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace _3902sprint0
{
	public class Sprite : ISprite
	{
        protected Texture2D texture;

        public Vector2 Position { get; protected set; }

        protected int width;

        protected int height;
        protected Rectangle destinationRectangle;

        protected Rectangle sourceRectangle;

        private Vector2 location;

        public Sprite(
            Texture2D texture,
            Vector2 position,
            int width,
            int height)
        {
            this.texture = texture;
            Position = position;

            this.width = width;
            this.height = height;
            UpdateDestinationRectangle();
        }
        public void Initialize(Texture2D texture) //just added for interface
        {
            this.texture = texture;

            sourceRectangle = new Rectangle(0, 0, 32, 32);
            UpdateDestinationRectangle();
            location = Vector2.Zero;
        }


        public virtual void UpdateSprite(GameTime gameTime)
        {
            UpdateDestinationRectangle();
        }
        public Rectangle GetSourceRectangle() //just added for interface
        {
            return sourceRectangle;
        }


        protected void UpdateDestinationRectangle()
        {
            destinationRectangle = new Rectangle(
                (int)(Position.X - width / 2),
                (int)(Position.Y - height / 2),
                width,
                height
            );


        }


        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                texture,
                destinationRectangle,
                Color.White
            );
        }
    }
}