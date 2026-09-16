using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3902sprint0
{
    public class detonateSprite : ISprite

{
        private Rectangle sourceRectangle;
        private bool Detonating = false;
        private Rectangle destinationRectangle;
        private Texture2D textureDet;
        public void Initialize(Texture2D texture)
        {
            destinationRectangle = new Rectangle(
                (int)location.X,
                (int)location.Y,
                128,
                128
            );
            this.textureDet = texture;
        }
        private Vector2 location;
        private int currentFrame = 0;
        private double animationTimer = 0;
        private float animationSpeed = 0.15f;

        public void UpdateSprite(GameTime gameTime)
        {
            if (Detonating)
            {
                animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

                if (animationTimer >= animationSpeed)
                {
                    animationTimer -= animationSpeed;

                    currentFrame++;

                    if (currentFrame >= 8)
                    {
                        currentFrame = 7;
                        Detonating = false;
                    }
                }
            }

            int column = currentFrame % 4;
            int row = 4 + (currentFrame / 4);

            sourceRectangle = new Rectangle(
                column * 32,
                row * 32,
                32,
                32
            );

            destinationRectangle.X = (int)location.X;
            destinationRectangle.Y = (int)location.Y;
        }
        
        public void SetLocation(Vector2 location)
        {
            this.location = location;
            destinationRectangle = new Rectangle(
                (int)location.X-64,
                (int)location.Y-64,
                128,
                128
            );
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (Detonating)
            {
                spriteBatch.Draw(
                    textureDet,
                    destinationRectangle,
                    sourceRectangle,
                    Color.White
                );
            }
        }
        public void activate()
            {
                Detonating = true;
                currentFrame = 0;
                animationTimer = 0;
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
