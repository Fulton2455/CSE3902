using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3902sprint0
{
    public class Fireball2 : Ientity
    {
        public Texture2D Texture { get; private set; }
        private Vector2 position;
        private double detonationTimer = 0;
        private FireballSprite fireballSprite;

        private Vector2 direction;

       
        private float speed = 600f;
        private bool isActive = false;

        private float detonationTime = 1.5f;
        public Fireball2(Texture2D texture, FireballSprite fireballSprite
        )
        {
            Texture = texture;
            this.fireballSprite = fireballSprite;
        }
        public void generateEntity(Vector2 position, Vector2 direction, bool isHostile)
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
               
                    isActive = false;
                   
                }
          


            }
            fireballSprite.UpdateSprite(gameTime);
        }
        public void ApplyEffect(Vector2 position)
        {
            this.position = position;

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            fireballSprite.Draw(spriteBatch);
        }
        public void Reset()
        {
            isActive = false;
            fireballSprite.deactivate();
        }
    }

}

