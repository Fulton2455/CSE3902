using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3902sprint0
{
    public class ChestSprite : ISprite

{
        private Rectangle sourceRectangle;
        private bool isOpening = false;
        private bool isOpen = false;
        private Rectangle destinationRectangle;
        private Texture2D texture;
        public void Initialize(Texture2D texture)
        {
            destinationRectangle = new Rectangle(
                (int)location.X,
                (int)location.Y,
                48,
                32
            );
            this.texture = texture;
        }
        private Vector2 location;
        private int currentFrame = 0;
        private double animationTimer = 0;
        private float animationSpeed = 0.15f;

        public void UpdateSprite(GameTime gameTime)
        {
            if (isOpening)
            {
                animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

                if (animationTimer >= animationSpeed)
                {
                    animationTimer -= animationSpeed;

                    currentFrame++;

                    // Stop on the last frame
                    if (currentFrame >= 8)
                    {
                        currentFrame = 7;
                        isOpening = false;
                        isOpen = true;
                    }
                }
            }

            int column = currentFrame % 4;
            int row = 4 + (currentFrame / 4);

            sourceRectangle = new Rectangle(
                column * 48,
                row * 32,
                48,
                32
            );

            destinationRectangle.X = (int)location.X;
            destinationRectangle.Y = (int)location.Y;
        }
        public void SetLocation(Vector2 location)
        {
            this.location = location;
            destinationRectangle = new Rectangle(
                (int)location.X,
                (int)location.Y,
                48,
                32
            );
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                texture,
                destinationRectangle,
                sourceRectangle,
                Color.White
            );
        }
        public void Open()
        {
            if (!isOpen && !isOpening)
            {
                isOpening = true;
            }
        }

        public Rectangle GetDestinationRectangle()
        {
            return destinationRectangle;
        }
        public Rectangle GetSourceRectangle()
        {
            return sourceRectangle;
        }
}
}
