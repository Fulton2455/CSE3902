using _3902sprint0.Environment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace _3902sprint0
{
    public class Fireball : Ientity
    {
        public Texture2D Texture { get; private set; }
        private Vector2 position;
        private double detonationTimer = 0;
        private FireballSprite fireballSprite;
        private detonateSprite detonateSprite;

        private Vector2 direction;

        private Room room;

        private IEnemyTileInteraction tileInteraction;

        private float speed = 400f;
        private bool isActive = false;

        private float detonationTime = 1.5f;

        private bool isHostile = false;

        public bool IsActive => isActive;
        public bool IsHostile => isHostile;
        public int Damage { get; set; } = 1;
        public Rectangle Bounds => GetBoundsAt(position);
        public Fireball(Texture2D texture, FireballSprite fireballSprite,
        detonateSprite detonateSprite, Room room)
        {
            Texture = texture;
            this.fireballSprite = fireballSprite;
            this.detonateSprite = detonateSprite;
            this.room = room;
            this.tileInteraction = new FlyingEnemyTileInteraction();
        }
        public void generateEntity(Vector2 position, Vector2 direction, bool isHostile)
        {
            SoundManager.PlayFireballShoot();

            this.position = position;
            this.direction = direction;
            this.direction.Normalize();
            isActive = true;
            detonationTimer = 0;
            fireballSprite.SetLocation(position);
            fireballSprite.SetDirection(this.direction);
            fireballSprite.activate();
            this.isHostile = isHostile;
        }
        public void Update(GameTime gameTime)
        {
            if (isActive)
            {
                move(gameTime);
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
            SoundManager.PlayExplosion();

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

        /// <summary>Called when an enemy reports a hit. Stops the fireball and plays its normal detonation effect.</summary>
public void Hit()
{
    if (!isActive) return;
    fireballSprite.deactivate();
    ApplyEffect(position);
    isActive = false;
}

private Rectangle GetBoundsAt(Vector2 pos) => new Rectangle((int)(pos.X + 64), (int)(pos.Y + 16), 3, 3);
        private void move(GameTime gameTime)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 attemptedPosition = position + direction * elapsedTime * speed;
            Rectangle bounds = GetBoundsAt(attemptedPosition);
            if (room.CanEnter(bounds, direction, null, tileInteraction))
            {
                position = attemptedPosition;

            }
            else
            {
                detonationTimer = 1.5f;
                speed = 0;
            }
        }
    }

}

