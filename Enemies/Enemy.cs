using _3902sprint0.Environment;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;



namespace _3902sprint0
{
	public class Enemy : IEnemy
		{

        private enum AnimationState
        {
            Walk,
            Attack,
            Death,
            Idle,
            Telegraph,
            Leap,
            Spit,
            Summon,
            Recover
        }

        private const int StandardFrameSize = 64;
        private const int BossFrameSize = 128;
        private const int BossSpecialFrameCount = 4;
        private const int BossTelegraphStartColumn = 11;
        private const int BossLeapStartColumn = 15;
        private const int BossSpitStartColumn = 19;
        private const int BossSummonStartColumn = 23;
        private const int BossRecoverStartColumn = 27;
        private const int BossEnragedRowOffset = 10;
        private const int MovementFrameCount = 8;
        private const int AttackFrameCount = 3;
        private const int DeathFrameCount = 8;
        private const int IdleFrameCount = 8;
        private const int AttackStartColumn = 8;
        private const int DeathRow = 8;
        private const int IdleRow = 9;
        private const float AnimationSpeed = 0.1f;

        private float damageCooldown;
        private const float CoolDownTime = 0.8f;

        private float hitFlashTimer = 0f;
        private const float HitFlashTime = 0.15f;


        private readonly Texture2D spriteSheet;
        private readonly Rectangle movementBounds;
        private readonly float movementSpeed;
        private readonly float acceleration;
        private readonly int size;
        private readonly IEnemyAI enemyAI;
        private readonly EnemyType enemyType;
        private readonly IEnemyTileInteraction tileInteraction;
        private readonly Room room;

        private int health;
        private int maxHealth;
        private int damage;
        private Direction currentDirection;
        private int currentFrame;
        private float animationTimer;
        private AnimationState animationState;
        private bool animationFinished;
        private readonly int frameSize;
        private bool timedAnimationActive;
        private float timedAnimationDuration;
        private float timedAnimationElapsed;
        private Rectangle destinationRectangle;
        private Vector2 position;
        private Vector2 velocity;




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
        public int MaxHealth
        {
            get
            {
                return maxHealth;
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
                return damageCooldown <= 0f && !IsDying && !IsDead;

            }
        }
        public bool IsDying 
        {
            get
            {
               return animationState == AnimationState.Death;
            }
        }
        public bool IsDead 
        {
            get
            {
                return IsDying && animationFinished;
            }
        } 
        public bool IsAttacking
        {
            get
            {
                return animationState == AnimationState.Attack;
            }
        }
        public bool IsPlayingTimedAnimation
        {
            get
            {
                return timedAnimationActive;
            }
        }
        private bool IsEnraged
        {
            get
            {
                return enemyType == EnemyType.ToadBoss && health <= maxHealth / 2;
            }
        }


        public Enemy(
            EnemyType enemyType,
            IEnemyAI enemyAI,
            IEnemyTileInteraction tileInteraction,
			Texture2D spriteSheet,
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

			this.spriteSheet = spriteSheet;

            this.size = size;

			this.health = health;

            this.maxHealth = health;

			this.damage = damage;

			this.movementSpeed = movementSpeed;

			this.movementBounds = movementBounds;

            this.room = room;

            frameSize = enemyType == EnemyType.ToadBoss ? BossFrameSize : StandardFrameSize;

            currentDirection = Direction.Down;

			currentFrame = 0;
			animationTimer = 0f;
            damageCooldown = 0f;
            animationState = AnimationState.Idle;
            animationFinished = false;
            UpdateDestinationRectangle();
		}
        
        public void Update(GameTime gameTime, float terrainAcceleration)
        {
            if (hitFlashTimer > 0f)
            {
                hitFlashTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            Player player = Game1.currentPlayer;
            if (player == null || player.IsDead)
            {
                velocity = Vector2.Zero;
                if (!IsDying && !IsDead)
                {
                    timedAnimationActive = false;
                    animationState = AnimationState.Idle;
                }
                UpdateAnimation(gameTime);
                UpdateDestinationRectangle();
                return;
            }
                if (!IsDying && !IsDead)
                {

                    enemyAI.Update(this, player, gameTime, terrainAcceleration);
                }


                UpdateAnimation(gameTime);

                UpdateDamageCooldown(gameTime);
                if (!IsDying && !IsDead)
                {
                    KeepInsideBounds();
                }

                UpdateDestinationRectangle();
            
        }

		public void Move(GameTime gameTime, float terrainAcceleration)
		{
            Vector2 direction = GetDirection(currentDirection);
            MoveInDirection(direction, gameTime, terrainAcceleration);
        }
        
        public void StartDamageCoolDown()
        {
            damageCooldown = CoolDownTime;
        }
        public void StartAttack()
        {
            if (IsDying || IsDead || timedAnimationActive)
                return;
            Player player = Game1.currentPlayer;
            if (player != null)
            {
                Vector2 playerDirection = player.Bounds.Center.ToVector2() - position;

                if (playerDirection != Vector2.Zero)
                {
                    playerDirection.Normalize();
                    SetDirectionFromVector(playerDirection);
                }
            }
            animationState = AnimationState.Attack;
            currentFrame = 0;
            animationTimer = 0f;
            animationFinished = false;
        }

        public void TakeDamage(int damageAmount)
        {
            if (IsDying || IsDead)
                return;

            health -= damageAmount;
            hitFlashTimer = HitFlashTime;
            SoundManager.PlayEnemyHit();

            if (health <= 0)
            {
                health = 0;
                velocity = Vector2.Zero;
                timedAnimationActive = false;
                animationState = AnimationState.Death;
                currentFrame = 0;
                animationTimer = 0f;
                animationFinished = false;
            }
        }

        public void StopMovement()
        {
            velocity = Vector2.Zero;
            if (animationState == AnimationState.Walk && !timedAnimationActive)
            {
                animationState = AnimationState.Idle;
            }
        }
        public void PlayTimedAnimation(BossAnimation animation, float duration)
        {
            if (IsDying || IsDead)
                return;

            switch (animation)
            {
                case BossAnimation.Melee:
                    animationState = AnimationState.Attack;
                    break;
                case BossAnimation.Telegraph:
                    animationState = AnimationState.Telegraph;
                    break;
                case BossAnimation.Leap:
                    animationState = AnimationState.Leap;
                    break;
                case BossAnimation.Spit:
                    animationState = AnimationState.Spit;
                    break;
                case BossAnimation.Summon:
                    animationState = AnimationState.Summon;
                    break;
                default:
                    animationState = AnimationState.Recover;
                    break;
            }
            timedAnimationActive = true;
            timedAnimationDuration = System.Math.Max(duration, 0.01f);
            timedAnimationElapsed = 0f;
            currentFrame = 0;
            animationFinished = false;
        }

        public void EndTimedAnimation()
        {
            if (!timedAnimationActive)
                return;

            timedAnimationActive = false;
            animationState = AnimationState.Idle;
            currentFrame = 0;
            animationTimer = 0f;
        }

        public void FaceTowards(Vector2 target)
        {
            Vector2 direction = target - position;
            if (direction != Vector2.Zero)
            {
                SetDirectionFromVector(direction);
            }
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
            if (IsDying || IsDead || IsAttacking || timedAnimationActive)
                return;

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
                animationState = AnimationState.Walk;
            }
            else
            {
                SlowDown(terrainAcceleration, elapsedSeconds);
                animationState = AnimationState.Idle;
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
                if (velocity.LengthSquared() < 0.001f)
                {
                    velocity = Vector2.Zero;
                    if (!timedAnimationActive)
                    {
                        animationState = AnimationState.Idle;
                    }
                }
            }

        }

        public void LeapTowards(Vector2 target,
            GameTime gameTime,
            float leapSpeed)
        {
            Vector2 direction = target - position;

            if (direction == Vector2.Zero)
            {
                velocity = Vector2.Zero;
                return;
            }

            direction.Normalize();
            velocity = direction * leapSpeed;

            float elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 delta = velocity * elapsedSeconds;
            MoveAxis(new Vector2(delta.X, 0f));
            MoveAxis(new Vector2(0f, delta.Y));
        }
        public void SetDirection(Direction direction)
        {
            currentDirection = direction;
        }
        private int GetTimedFrameCount()
        {
            return animationState == AnimationState.Attack ? AttackFrameCount : BossSpecialFrameCount;
        }

        private int GetTimedStartColumn()
        {
            switch (animationState)
            {
                case AnimationState.Attack:
                    return AttackStartColumn;
                case AnimationState.Telegraph:
                    return BossTelegraphStartColumn;
                case AnimationState.Leap:
                    return BossLeapStartColumn;
                case AnimationState.Spit:
                    return BossSpitStartColumn;
                case AnimationState.Summon:
                    return BossSummonStartColumn;
                default:
                    return BossRecoverStartColumn;
            }
        }
        private void UpdateAnimation(GameTime gameTime)
        {
            if (timedAnimationActive)
            {
                timedAnimationElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
                int frameCount = GetTimedFrameCount();
                int frame = (int)(timedAnimationElapsed / timedAnimationDuration * frameCount);
                currentFrame = System.Math.Min(frame, frameCount - 1);
                return;
            }
            animationTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (animationTimer < AnimationSpeed)
                return;

            animationTimer -= AnimationSpeed;

            switch (animationState)
            {
                case AnimationState.Walk:
                    currentFrame++;
                    if (currentFrame >= MovementFrameCount)
                        currentFrame = 0;
                    break;

                case AnimationState.Idle:
                    currentFrame++;
                    if (currentFrame >= IdleFrameCount)
                        currentFrame = 0;
                    break;

                case AnimationState.Attack:
                    currentFrame++;
                    if (currentFrame >= AttackFrameCount)
                    {
                        currentFrame = AttackFrameCount - 1;
                        animationFinished = true;
                        animationState = AnimationState.Idle;
                    }
                    break;

                case AnimationState.Death:
                    currentFrame++;
                    if (currentFrame >= DeathFrameCount)
                    {
                        currentFrame = DeathFrameCount - 1;
                        animationFinished = true;
                    }
                    break;
            }

        }
        private void UpdateDestinationRectangle()
        {
            destinationRectangle = new Rectangle(
                (int)(position.X - size),
                (int)(position.Y - size),
                size * 2,
                size * 2);
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
                    directionRow = 4;
                    break;

                case Direction.UpRight:
                    directionRow = 3;
                    break;

                case Direction.Right:
                    directionRow = 2;
                    break;

                case Direction.DownRight:
                    directionRow = 1;
                    break;

                case Direction.Down:
                    directionRow = 0;
                    break;

                case Direction.DownLeft:
                    directionRow = 7;

                    break;

                case Direction.Left:
                    directionRow = 6;
                    break;

                case Direction.UpLeft:
                    directionRow = 5;
                    break;
                default:
                    directionRow = 0;
                    break;

                
            }
            return directionRow;
        }
        private Rectangle GetSourceRectangle()
        {
            int x = currentFrame * frameSize;
            int y;

            switch (animationState)
            {
                case AnimationState.Walk:
                    y = GetDirectionRow(currentDirection) * frameSize;
                    break;

                case AnimationState.Attack:
                    x = (AttackStartColumn + currentFrame) * frameSize;
                    y = GetDirectionRow(currentDirection) * frameSize;
                    break;

                case AnimationState.Telegraph:
                case AnimationState.Leap:
                case AnimationState.Spit:
                case AnimationState.Summon:
                case AnimationState.Recover:
                    x = (GetTimedStartColumn() + currentFrame) * frameSize;
                    y = GetDirectionRow(currentDirection) * frameSize;
                    break;

                case AnimationState.Death:
                    y = DeathRow * frameSize;
                    break;

                case AnimationState.Idle:
                    y = IdleRow * frameSize;
                    break;

                default:
                    y = IdleRow * frameSize;
                    break;
            }

            if (IsEnraged)
            {
                y += BossEnragedRowOffset * frameSize;
            }

            return new Rectangle(x, y, frameSize, frameSize);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Color drawColor = Color.White;

            if (hitFlashTimer > 0f)
                drawColor = Color.Red;

            spriteBatch.Draw(
                spriteSheet,
                destinationRectangle,
                GetSourceRectangle(),
                drawColor
            );
        }

    }
}
