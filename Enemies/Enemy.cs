using _3902sprint0.Environment;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;



namespace _3902sprint0
{
	public class Enemy : IEnemy
		{

        private float damageCooldown;
        private float coolDownTime = .8f;
		private Texture2D runningTexture;
		private Texture2D idleTexture;

		private Rectangle movementBounds;

		private float movementSpeed;
        private float acceleration;
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
        private IEnemyTileInteraction tileInteraction;
        private Vector2 position;
        private Vector2 velocity;
        private Room room;

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
        public Vector2 Velocity
        {
            get
            {
                return velocity;
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
        public float Acceleration
        {
            get
            {
                return acceleration;
            }
        }
        public Direction CurrentDirection
        {
            get
            {
                return currentDirection;
            }
        }
        public Rectangle bounds
        {
            get
            {
                return destinationRectangle;
            }
        }
        public IEnemyTileInteraction TileInteraction
        {
            get
            {
                return tileInteraction;
            }

        }
        public bool CanDamagePlayer
        {
            get
            {
                return damageCooldown <= 0f;
            }
        }


		public Enemy(
            EnemyType enemyType,
            IEnemyAI enemyAI,
            IEnemyTileInteraction tileInteraction,
			Texture2D runningTexture,
			Texture2D idleTexture,
			Vector2 position,
			int size,
			int health,
			int damage,
			float movementSpeed,
            float acceleration,
			Rectangle movementBounds,
            Room room)
		{ 
            this.enemyType = enemyType;

            this.acceleration = acceleration;

            this.velocity = Vector2.Zero;
            
            this.position = position;

            this.enemyAI = enemyAI;

            this.tileInteraction = tileInteraction;

			this.runningTexture = runningTexture;

			this.idleTexture = idleTexture;

            this.size = size;

			this.health = health;

			this.damage = damage;

			this.movementSpeed = movementSpeed;

			this.movementBounds = movementBounds;

            this.room = room;

			currentDirection = Direction.Down;

			currentFrame = 0;
			animationTimer = 0f;
            damageCooldown = 0f;
            UpdateDestinationRectangle();
		}
        
        public void Update(GameTime gameTime, float terrainAcceleration)
        {
            Player player = Game1.currentPlayer;
            enemyAI.Update(this, player, gameTime, terrainAcceleration);
            

            UpdateAnimation(gameTime);

            UpdateDamageCooldown(gameTime);

            KeepInsideBounds();

            UpdateDestinationRectangle();

        }

		public void Move(GameTime gameTime, float terrainAcceleration)
		{
            Vector2 direction = GetDirection(currentDirection);
            MoveInDirection(direction, gameTime, terrainAcceleration);
        }
        
        public void StartDamageCoolDown()
        {
            damageCooldown = coolDownTime;
        }
        private void UpdateDamageCooldown(GameTime gameTime)
        {
            if (damageCooldown > 0f)
            {
                damageCooldown -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (damageCooldown < 0f)
                    damageCooldown = 0f;
            }
        }
        public void MoveInDirection(Vector2 direction, GameTime gametime, float terrainAcceleration)
        {
            float elapsedSeconds = (float)gametime.ElapsedGameTime.TotalSeconds;
            if (direction != Vector2.Zero)
            {
                

                direction.Normalize();
                velocity += direction * terrainAcceleration * elapsedSeconds;
                if (velocity.Length() > movementSpeed)
                {
                    velocity.Normalize();
                    velocity *= movementSpeed;
                }
                
                
                SetDirectionFromVector(direction);
            }
            else
            {
                SlowDown(terrainAcceleration, elapsedSeconds);
            }
            Vector2 delta = velocity * elapsedSeconds;
            MoveAxis(new Vector2(delta.X, 0));
            MoveAxis(new Vector2(0, delta.Y));
        }
        private void SlowDown(float terrainAcceleration, float elapsedSeconds)
        {
            if (velocity.Length() <= 0f)
            {
                velocity = Vector2.Zero;
                return;
            }

            float slowdown = terrainAcceleration * elapsedSeconds;

            if (velocity.Length() <= slowdown)
            {
                velocity = Vector2.Zero;
                return;
            }
            velocity -= Vector2.Normalize(Velocity) * slowdown;
            
            
            
        }
        private void MoveAxis(Vector2 delta)
        {
            if (delta == Vector2.Zero)
            {
                return;
            }
            Vector2 attemptedLocation = position + delta;
            Rectangle attemptedBounds = new Rectangle((int)(attemptedLocation.X - size /2f), (int)(attemptedLocation.Y - size / 2f ), size, size);
            Vector2 direction = Vector2.Normalize(delta);
            bool canEnter = EnemyCollision.CanEnter(
                room, 
                attemptedBounds, 
                direction, 
                tileInteraction);
            if (canEnter)
            {
                position = attemptedLocation;
            }
            else
            {
                if (delta.X != 0)
                {
                    velocity.X = 0;
                }
                if (delta.Y != 0)
                {
                    velocity.Y = 0;
                }
            }

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
    }
}
