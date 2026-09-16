using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3902sprint0
{
    public class FireballSprite : ISprite

{
        private Rectangle sourceRectangle;

        private Vector2 direction = Vector2.UnitX;
        private float rotation = 0f;
        public void SetDirection(Vector2 direction)
        {
            if (direction != Vector2.Zero)
            {
                this.direction = Vector2.Normalize(direction);
                rotation = (float)Math.Atan2(direction.Y, direction.X);
            }
        }
        private bool isActive = false;
        private Rectangle destinationRectangle;
        private Texture2D textureProj;
        public void Initialize(Texture2D textureProj)
        {
            destinationRectangle = new Rectangle(
                (int)location.X,
                (int)location.Y,
                128,
                32
            );
            this.textureProj = textureProj;
        }
        public void deactivate()
        {
            isActive = false;
        }
        private Vector2 location;
        private int currentFrame = 0;
        private double animationTimer = 0;
        private float animationSpeed = 0.1f;

        public void UpdateSprite(GameTime gameTime)
        {
            if (isActive)
            {
                animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

                if (animationTimer >= animationSpeed)
                {
                    animationTimer -= animationSpeed;

                    currentFrame++;

                    if (currentFrame >= 16)
                    {
                        currentFrame = 0;
                    }
                }
            }

            sourceRectangle = new Rectangle(
            currentFrame * 65,
                    0,
                    65,
                    16
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
                128,
                32
            );
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (isActive)
            {
                Vector2 center = new Vector2(
                destinationRectangle.X + destinationRectangle.Width / 2f,
                destinationRectangle.Y + destinationRectangle.Height / 2f
                );
                spriteBatch.Draw(
                textureProj,
                center,
                sourceRectangle,
                Color.White,
                rotation,
                new Vector2(
                sourceRectangle.Width / 2f,
                sourceRectangle.Height / 2f
                ),
                new Vector2(
               (float)destinationRectangle.Width / sourceRectangle.Width,
               (float)destinationRectangle.Height / sourceRectangle.Height
                ),
                SpriteEffects.None,
                0f
                );
            }
        }
        public void activate()
        {
            isActive = true;
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
