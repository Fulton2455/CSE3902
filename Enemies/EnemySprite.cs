using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace _3902sprint0
{
    /// <summary>
    /// Owns an enemy's visual state, sprite-sheet animation, and drawing.
    /// Enemy remains responsible for movement, collision, health, and AI.
    /// </summary>
    internal sealed class EnemySprite
    {
        internal enum AnimationState
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
        private const float HitFlashTime = 0.15f;

        private readonly Texture2D spriteSheet;
        private readonly EnemyType enemyType;
        private readonly int frameSize;
        private int currentFrame;
        private float animationTimer;
        private bool animationFinished;
        private bool timedAnimationActive;
        private float timedAnimationDuration;
        private float timedAnimationElapsed;
        private float hitFlashTimer;

        internal AnimationState State 
        { 
            get; private set; 
        } = AnimationState.Idle;
        internal bool IsDying => State == AnimationState.Death;
        internal bool IsDead => IsDying && animationFinished;
        internal bool IsAttacking => State == AnimationState.Attack;
        internal bool IsPlayingTimedAnimation => timedAnimationActive;
        private bool IsEnraged(int health, int maxHealth) => enemyType == EnemyType.ToadBoss && health <= maxHealth / 2;

        internal EnemySprite(Texture2D spriteSheet, EnemyType enemyType)
        {
            this.spriteSheet = spriteSheet;
            this.enemyType = enemyType;
            frameSize = enemyType == EnemyType.ToadBoss ? BossFrameSize : StandardFrameSize;
        }

        internal void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (hitFlashTimer > 0f)
                hitFlashTimer = System.Math.Max(0f, hitFlashTimer - elapsed);

            if (timedAnimationActive)
            {
                timedAnimationElapsed += elapsed;
                int frameCount = GetTimedFrameCount();
                currentFrame = Math.Min((int)(timedAnimationElapsed / timedAnimationDuration * frameCount), frameCount - 1);
                return;
            }

            animationTimer += elapsed;
            if (animationTimer < AnimationSpeed)
                return;

            animationTimer -= AnimationSpeed;
            switch (State)
            {
                case AnimationState.Walk:
                    currentFrame = (currentFrame + 1) % MovementFrameCount;
                    break;
                case AnimationState.Idle:
                    currentFrame = (currentFrame + 1) % IdleFrameCount;
                    break;
                case AnimationState.Attack:
                    currentFrame++;
                    if (currentFrame >= AttackFrameCount)
                    {
                        currentFrame = AttackFrameCount - 1;
                        animationFinished = true;
                        State = AnimationState.Idle;
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

        internal void SetState(AnimationState state)
        {
            if (State == state)
                return;
            State = state;
            currentFrame = 0;
            animationTimer = 0f;
            animationFinished = false;
        }

        internal void StartAttack()
        {
            if (IsDying || IsDead || timedAnimationActive)
                return;
            SetState(AnimationState.Attack);
        }

        internal void StartDeath()
        {
            timedAnimationActive = false;
            SetState(AnimationState.Death);
        }

        internal void RegisterHit() => hitFlashTimer = HitFlashTime;

        internal void PlayTimedAnimation(BossAnimation animation, float duration)
        {
            if (IsDying || IsDead)
                return;

            switch (animation)
            {
                case BossAnimation.Melee: 
                    SetState(AnimationState.Attack); 
                    break;
                case BossAnimation.Telegraph: 
                    SetState(AnimationState.Telegraph); 
                    break;
                case BossAnimation.Leap: 
                    SetState(AnimationState.Leap); 
                    break;
                case BossAnimation.Spit: 
                    SetState(AnimationState.Spit); 
                    break;
                case BossAnimation.Summon: 
                    SetState(AnimationState.Summon); 
                    break;
                default: 
                    SetState(AnimationState.Recover); 
                    break;
            }
            timedAnimationActive = true;
            timedAnimationDuration = System.Math.Max(duration, 0.01f);
            timedAnimationElapsed = 0f;
            currentFrame = 0;
            animationFinished = false;
        }

        internal void EndTimedAnimation()
        {
            if (!timedAnimationActive)
                return;
            timedAnimationActive = false;
            SetState(AnimationState.Idle);
        }

        internal void Draw(
            SpriteBatch spriteBatch, 
            Rectangle destination, 
            Direction direction, 
            int health, 
            int maxHealth)
        {
            Color drawColor = hitFlashTimer > 0f ? Color.Red : Color.White;
            spriteBatch.Draw(spriteSheet, destination, GetSourceRectangle(direction, IsEnraged(health, maxHealth)), drawColor);
        }

        private int GetTimedFrameCount() => State == AnimationState.Attack ? AttackFrameCount : BossSpecialFrameCount;

        private int GetTimedStartColumn()
        {
            switch (State)
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

        private Rectangle GetSourceRectangle(Direction direction, bool enraged)
        {
            int x = currentFrame * frameSize;
            int y;
            switch (State)
            {
                case AnimationState.Walk: 
                    y = GetDirectionRow(direction) * frameSize; break;
                case AnimationState.Attack:
                    x = (AttackStartColumn + currentFrame) * frameSize;
                    y = GetDirectionRow(direction) * frameSize;
                    break;
                case AnimationState.Telegraph:
                case AnimationState.Leap:
                case AnimationState.Spit:
                case AnimationState.Summon:
                case AnimationState.Recover:
                    x = (GetTimedStartColumn() + currentFrame) * frameSize;
                    y = GetDirectionRow(direction) * frameSize;
                    break;
                case AnimationState.Death: 
                    y = DeathRow * frameSize; break;
                default: 
                    y = IdleRow * frameSize; break;
            }

            if (enraged)
            {
                y += BossEnragedRowOffset * frameSize;
            }
            return new Rectangle(x, y, frameSize, frameSize);
        }

        private static int GetDirectionRow(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up: 
                    return 4;
                case Direction.UpRight: 
                    return 3;
                case Direction.Right: 
                    return 2;
                case Direction.DownRight: 
                    return 1;
                case Direction.Down: 
                    return 0;
                case Direction.DownLeft: 
                    return 7;
                case Direction.Left:
                    return 6;
                case Direction.UpLeft: 
                    return 5;
                default:
                    return 0;
            }
        }
    }
}
