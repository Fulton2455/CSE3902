using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3902sprint0
{
    internal class Fireball : Ientity
    {
        public Texture2D Texture { get; private set; }
        private Vector2 position;
        private double detonationTimer = 0;
        private FireballSprite fireballSprite;
        private detonateSprite detonateSprite;

        private Vector2 direction;

       
        private float speed = 400f;
        private bool isActive = false;

        private float detonationTime = 1.5f;
        public Fireball(Texture2D texture, FireballSprite fireballSprite,
        detonateSprite detonateSprite)
        {
            Texture = texture;
            this.fireballSprite = fireballSprite;
            this.detonateSprite = detonateSprite;
        }
        public void generateEntity(Vector2 position, Vector2 direction)
        {
            this.position = position;
            this.direction = direction;
            this.direction.Normalize();
            isActive = true;
            detonationTimer = 0;
            fireballSprite.SetLocation(position);
            fireballSprite.SetDirection(this.direction);
            fireballSprite.activate();
        }
        public void Update(GameTime gameTime)
        {
            if (isActive)
            {
                position += direction * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                detonationTimer += gameTime.ElapsedGameTime.TotalSeconds;
                fireballSprite.SetLocation(position);


                if (detonationTimer >= detonationTime)
                {
                    fireballSprite.deactivate();
                    ApplyEffect(position);
                    isActive = false;
                   
                }
          


            }
            fireballSprite.UpdateSprite(gameTime);
            detonateSprite.UpdateSprite(gameTime);
        }
        public void ApplyEffect(Vector2 position)
        {
            this.position = position;
            detonateSprite.SetLocation(position);
            detonateSprite.activate();
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            fireballSprite.Draw(spriteBatch);
            detonateSprite.Draw(spriteBatch);
        }
        public void Reset()
        {
            isActive = false;
            fireballSprite.deactivate();
        }
    }

}

