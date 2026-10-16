using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace _3902sprint0
{
    public class BossShockwave
    {
        private readonly Vector2 center;
        private readonly float maximumRadius;
        private readonly float duration;
        private readonly int damage;

        private readonly Texture2D pixel;
        

        private float elapsedTime;
        private float previousRadius;
        private bool hasHitPlayer;
        private bool isFinished;

        public bool IsFinished => isFinished;

        public float Radius => MathHelper.Lerp(
            0f,
            maximumRadius,
            MathHelper.Clamp(elapsedTime / duration, 0f, 1f)
            );
        public BossShockwave(
            Vector2 center,
            float maximumRadius,
            float duration,
            int damage,
            Texture2D pixel)
        {
            this.center = center;
            this.maximumRadius = maximumRadius;
            this.duration = duration;
            this.damage = damage;
            this.pixel = pixel;
            elapsedTime = 0f;
            previousRadius = 0f;
            hasHitPlayer = false;
            isFinished = false;
        }
        public void Update(GameTime gameTime, Player player)
        {
            if (IsFinished)
            {
                return;
            }

            float previousRadius = Radius;

            float elapsedSeconds =
                (float)gameTime.ElapsedGameTime.TotalSeconds;

            elapsedTime += elapsedSeconds;

            
            float progress = MathHelper.Clamp(
                elapsedTime / duration,
                0f,
                1f);
            

            CheckPlayerCollision(player, previousRadius, Radius);

            if (elapsedTime >= duration)
            {
                isFinished = true;
            }
        }
        private void CheckPlayerCollision(Player player, float previousRadius, float currentRadius)
        {
            if (hasHitPlayer || player == null || player.IsDead)
            {
                return;
            }

            Rectangle playerBounds = player.Bounds;

            
            float closestX = MathHelper.Clamp(
                center.X,
                playerBounds.Left,
                playerBounds.Right);

            float closestY = MathHelper.Clamp(
                center.Y,
                playerBounds.Top,
                playerBounds.Bottom);

            Vector2 closestPoint = new Vector2(closestX, closestY);

           
            float minDistance = Vector2.Distance(center, closestPoint);

            
            Vector2 topLeft = new Vector2(playerBounds.Left, playerBounds.Top);
            Vector2 topRight = new Vector2(playerBounds.Right, playerBounds.Top);
            Vector2 bottomLeft = new Vector2(playerBounds.Left, playerBounds.Bottom);
            Vector2 bottomRight = new Vector2(playerBounds.Right, playerBounds.Bottom);

            float maxDistance = Math.Max(
                Math.Max(
                    Vector2.Distance(center, topLeft),
                    Vector2.Distance(center, topRight)),
                Math.Max(
                    Vector2.Distance(center, bottomLeft),
                    Vector2.Distance(center, bottomRight)));

            
            bool waveReachedPlayer =
                currentRadius >= minDistance &&
                previousRadius <= maxDistance;

            if (waveReachedPlayer)
            {
                player.TakeDamage(damage);
                hasHitPlayer = true;
            }
        }
        public void draw(SpriteBatch spriteBatch)
        {
            if (IsFinished || pixel == null)
            {
                return;
            }

            float radius = Radius;

            if (radius <= 0f)
            {
                return;
            }

            const int segments = 96;
            const float thickness = 3f;

            Color waveColor = Color.LimeGreen * 0.8f;

            float angleStep = MathHelper.TwoPi / segments;

            for (int i = 0; i < segments; i++)
            {
                float angle1 = i * angleStep;
                float angle2 = (i + 1) * angleStep;

                Vector2 start = center + new Vector2(
                (float)Math.Cos(angle1) * radius,
                (float)Math.Sin(angle1) * radius
                );

                Vector2 end = center + new Vector2(
                (float)Math.Cos(angle2) * radius,
                (float)Math.Sin(angle2) * radius
                );

                Vector2 difference = end - start;

                float length = difference.Length();
                float rotation = (float)Math.Atan2(
                difference.Y,
                difference.X
                );

                spriteBatch.Draw(
                pixel,
                start,
                null,
                waveColor,
                rotation,
                Vector2.Zero,
                new Vector2(length, thickness),
                SpriteEffects.None,
                0f
                );
            }
        }
    }
}
