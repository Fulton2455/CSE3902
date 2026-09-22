using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace _3902sprint0
{
	public class Enemy : Sprite, IEnemy
		{


		private Texture2D runningTexture;
		private Texture2D idleTexture;

		private Rectangle movementBounds;

		private float movementSpeed;
		private int health;
		private int damage;

		private Direction currentDirection;

        private int currentFrame;

        private float animationTimer;


		private const float AnimationSpeed = 0.1f;

        private IEnemyAI enemyAI;
        public bool IsPaused
        {
            get
            {
                return enemyAI.IsPaused;
            }
        }


		public Enemy(
            IEnemyAI enemyAI,
			Texture2D runningTexture,
			Texture2D idleTexture,
			Vector2 position,
			int size,
			int health,
			int damage,
			float movementSpeed,
			Rectangle movementBounds)
            : base(runningTexture,
				  position,
				  size,
				  size)
		{ 
            this.enemyAI = enemyAI;

			this.runningTexture = runningTexture;

			this.idleTexture = idleTexture;

			this.health = health;

			this.damage = damage;

			this.movementSpeed = movementSpeed;

			this.movementBounds = movementBounds;

			currentDirection = Direction.Down;

			currentFrame = 0;
			animationTimer = 0f;
		}
        public void Update(GameTime gameTime)
        {
            enemyAI.Update(this, gameTime);

            UpdateAnimation(gameTime);

            KeepInsideBounds();

            base.UpdateSprite(gameTime);
        }

		public void Move(GameTime gameTime)
		{
            float elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 movementDirection = GetDirection(currentDirection);

            Position += movementDirection * movementSpeed * elapsedSeconds;
        }
        public void SetDirection(Direction direction)
        {
            currentDirection = direction;
        }

        private void UpdateAnimation(GameTime gameTime)
        {

            animationTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (animationTimer >= AnimationSpeed)
            {
                animationTimer -= AnimationSpeed;

                currentFrame++;

                if (currentFrame >= 8)
                {
                    currentFrame = 0;
                }
            }


        }
		private Vector2 GetDirection(Direction direction)
		{
			Vector2 vector;

			switch (direction) {
				case Direction.Up:
					vector = new Vector2(0, -1);
					break;
				case Direction.UpRight:
                    vector = new Vector2(1, -1);
                    break;

                case Direction.Right:
                    vector = new Vector2(1, 0);
                    break;

                case Direction.DownRight:
                    vector = new Vector2(1, 1);
                    break;

                case Direction.Down:
                    vector = new Vector2(0, 1);


                    break;

                case Direction.DownLeft:
                    vector = new Vector2(-1, 1);

                    break;

                case Direction.Left:
                    vector = new Vector2(-1, 0);
                    break;

                case Direction.UpLeft:
                    vector = new Vector2(-1, -1);
                    break;
                default:
                    vector = Vector2.Zero;
                    break;
            }
            if (vector != Vector2.Zero)
            {
                vector.Normalize();
            }
            return vector;
        }
        private void KeepInsideBounds()
        {
            float minimumX = movementBounds.Left + width / 2f;
            float maximumX = movementBounds.Right - width / 2f;
            float minimumY = movementBounds.Top + height / 2f;
            float maximumY = movementBounds.Bottom - height / 2f;


            Position = new Vector2(
                MathHelper.Clamp(Position.X,
                    minimumX,
                    maximumX
                ),

                MathHelper.Clamp(
                    Position.Y,
                    minimumY,
                    maximumY
                )
            );
        }

        private int GetDirectionRow(Direction direction)
        {
            int directionRow;
            switch (currentDirection)
            {
                case Direction.Up:
                    directionRow = 0;
                    break;

                case Direction.UpRight:
                    directionRow = 1;
                    break;

                case Direction.Right:
                    directionRow = 2;
                    break;

                case Direction.DownRight:
                    directionRow = 3;
                    break;

                case Direction.Down:
                    directionRow = 4;
                    break;

                case Direction.DownLeft:
                    directionRow = 5;

                    break;

                case Direction.Left:
                    directionRow = 6;
                    break;

                case Direction.UpLeft:
                    directionRow = 7;
                    break;
                default:
                    directionRow = 4;
                    break;

                
            }
            return directionRow;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            int directionRow = GetDirectionRow(currentDirection);

            Rectangle sourceRectangle = new Rectangle(
                    currentFrame * 32,
                    directionRow * 32,
                    32,
                    32

                );
            Texture2D currentTexture;
            if (IsPaused)
            {
                currentTexture = idleTexture;
            }
            else
            {
                currentTexture = runningTexture;
            }
            spriteBatch.Draw(
                currentTexture,
                destinationRectangle,
                sourceRectangle,
                Color.White
            );
        }
    }
}
