using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace _3902sprint0
{
	public class Enemy : IEnemy
		{


		private Texture2D runningTexture;
		private Texture2D idleTexture;

		private Rectangle movementBounds;

		private float movementSpeed;
		private int health;
		private int damage;
        private int size;


		private Direction currentDirection;

        private int currentFrame;

        private float animationTimer;
        private Rectangle destinationRectangle;
        private const int FrameWidth = 32;
        private const int FrameHeight = 32;
        private const int FrameCount = 8;



        private const float AnimationSpeed = 0.1f;

        private IEnemyAI enemyAI;
        private EnemyType enemyType;
        private Vector2 position;

        public bool IsPaused
        {
            get
            {
                return enemyAI.IsPaused;
            }
        }

        public EnemyType Type
        {
            get
            {
                return enemyType;
            }
        }
        public Vector2 Position
        {
            get
            {
                return position;
            }
        }
        public int Health
        {
            get
            {
                return health;
            }
        }
        public int Damage
        {
            get
            {
                return damage;
            }
        }
        public float MovementSpeed
        {
            get
            {
                return movementSpeed;
            }
        }
        public Direction CurrentDirection
        {
            get
            {
                return currentDirection;
            }
        }


		public Enemy(
            EnemyType enemyType,
            IEnemyAI enemyAI,
			Texture2D runningTexture,
			Texture2D idleTexture,
			Vector2 position,
			int size,
			int health,
			int damage,
			float movementSpeed,
			Rectangle movementBounds)
		{ 
            this.enemyType = enemyType;
            
            this.position = position;

            this.enemyAI = enemyAI;

			this.runningTexture = runningTexture;

			this.idleTexture = idleTexture;

            this.size = size;

			this.health = health;

			this.damage = damage;

			this.movementSpeed = movementSpeed;

			this.movementBounds = movementBounds;

			currentDirection = Direction.Down;

			currentFrame = 0;
			animationTimer = 0f;

            UpdateDestinationRectangle();
		}
        
        public void Update(GameTime gameTime)
        {
            Player player = Game1.currentPlayer;
            if (player != null) 
            {
                enemyAI.Update(this, player, gameTime);
            }
            

            UpdateAnimation(gameTime);

            KeepInsideBounds();

            UpdateDestinationRectangle();

        }

		public void Move(GameTime gameTime)
		{
            Vector2 direction = GetDirection(currentDirection);
            MoveInDirection(direction, gameTime);
        }
        public void MoveInDirection(Vector2 direction, GameTime gametime)
        {
            if (direction == Vector2.Zero)
            {
                return;
            }
            direction.Normalize();

            float elapsedSeconds = (float)gametime.ElapsedGameTime.TotalSeconds;
            position += direction * movementSpeed * elapsedSeconds;
            SetDirectionFromVector(direction);
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
        private void UpdateDestinationRectangle()
        {
            destinationRectangle = new Rectangle(
                (int)(position.X - size / 2f),
                (int)(position.Y - size / 2f),
                size,
                size);
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
        private void SetDirectionFromVector(Vector2 direction)
        {
            float angle = MathHelper.ToDegrees((float)System.Math.Atan2(direction.Y, direction.X));
            if (angle < 0)
            {
                angle += 360;
            }
            if (angle >= 337.5f ||
                angle < 22.5f)
            {
                currentDirection = Direction.Right;
            }
            else if (angle < 67.5f)
            {
                currentDirection = Direction.DownRight;
            }
            else if (angle < 112.5f)
            {
                currentDirection = Direction.Down;
            }
            else if (angle < 157.5f)
            {
                currentDirection = Direction.DownLeft;
            }
            else if (angle < 202.5f)
            {
                currentDirection = Direction.Left;
            }
            else if (angle < 247.5f)
            {
                currentDirection = Direction.UpLeft;
            }
            else if (angle < 292.5f)
            {
                currentDirection = Direction.Up;
            }
            else
            {
                currentDirection = Direction.UpRight;
            }
        }
        
        private void KeepInsideBounds()
        {
            float minimumX = movementBounds.Left + size / 2f;
            float maximumX = movementBounds.Right - size / 2f;
            float minimumY = movementBounds.Top + size / 2f;
            float maximumY = movementBounds.Bottom - size / 2f;


            position = new Vector2(
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
        public void Draw(SpriteBatch spriteBatch)
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

        public void TakeDamage(int damageAmount)
        {
            health -= damageAmount;
            SoundManager.PlayEnemyHit();
        }
    }
}
