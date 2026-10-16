
using System;

namespace _3902sprint0
{
    public class detonateSprite : ISprite
    {
        private Rectangle sourceRectangle;
        private bool Detonating = false;
        private Rectangle destinationRectangle;
        private Texture2D textureDet;
        private Vector2 location;
        private int currentFrame = 0;
        private double animationTimer = 0;
        private float animationSpeed = 0.15f;

        private const int FrameWidth = 32;
        private const int FrameHeight = 32;
        private const int StartX = 192;
        private const int StartY = 160;
        private const int TotalFrames = 4;
        public bool IsDetonating => Detonating;
        public void Initialize(Texture2D texture)
        {
            textureDet = texture;

            destinationRectangle = new Rectangle(
                (int)location.X,
                (int)location.Y,
                128,
                128
            );
        }

        public void UpdateSprite(GameTime gameTime)
        {
            if (Detonating)
            {
                animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

                if (animationTimer >= animationSpeed)
                {
                    animationTimer -= animationSpeed;
                    currentFrame++;

                    if (currentFrame >= TotalFrames)
                    {
                        currentFrame = TotalFrames - 1;
                        Detonating = false;
                    }
                }
            }

            sourceRectangle = new Rectangle(
                StartX + currentFrame * FrameWidth,
                StartY,
                FrameWidth,
                FrameHeight
            );

            destinationRectangle.X = (int)location.X - 16;
            destinationRectangle.Y = (int)location.Y - 32;
        }

        public void SetLocation(Vector2 location)
        {
            this.location = location;

            destinationRectangle = new Rectangle(
                (int)location.X - 64,
                (int)location.Y - 64,
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
