using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

        public bool IsFinished => elapsedTime >= duration;

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
        }
        public void Update(GameTime gameTime, Player player)
        {
            if (IsFinished)
            {
                return;
            }
            previousRadius = Radius;

            elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (elapsedTime > duration)
            {
                elapsedTime = duration;
            }
            if (player == null || player.IsDead || hasHitPlayer)
            {
                return;
            }
            float currentRadius = Radius;
            float distance = Vector2.Distance(
                center,
                player.Location);
            float playerRadius = player.Bounds.Width / 2f;
            bool waveReachedPlayer =
                distance + playerRadius >= previousRadius &&
                distance - playerRadius <= currentRadius;
            if (waveReachedPlayer)
            {
                player.TakeDamage(damage);
                hasHitPlayer = true;
            }
        }
        public void draw(SpriteBatch spriteBatch)
        {
            if (IsFinished)
            {
                return;
            }
            float radius = Radius;
            int thickness = 6;
            int diameter = (int)(radius * 2f);
            if (diameter < 0)
            {
                return;
            }
            Rectangle top = new Rectangle(
                (int)(center.X - radius),
                (int)(center.Y - radius),
                diameter,
                thickness);
            Rectangle bottom = new Rectangle(
                (int)(center.X - radius),
                (int)(center.Y + radius - thickness),
                diameter,
                thickness);
            Rectangle left = new Rectangle(
                (int)(center.X - radius),
                (int)(center.Y - radius),
                diameter,
                thickness);
            Rectangle right = new Rectangle(
                (int)(center.X + radius - thickness),
                (int)(center.Y - radius),
                diameter,
                thickness);
            Color waveColor = Color.LimeGreen * 0.8f;

            spriteBatch.Draw(pixel, top, waveColor);
            spriteBatch.Draw(pixel, bottom, waveColor);
            spriteBatch.Draw(pixel, left, waveColor);
            spriteBatch.Draw(pixel, right, waveColor);
        }
    }
}
