using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3902sprint0
{
    public static class EffectMessage
    {
        private static SpriteFont font;
        private static string text = "";
        private static Color color = Color.White;
        private static float timer = 0f;
        private const float Duration = 2f;
        private const float FadeTime = 0.5f;

        public static void Load(SpriteFont spriteFont)
        {
            font = spriteFont;
        }

        public static void Show(string message, Color messageColor)
        {
            text = message;
            color = messageColor;
            timer = Duration;
        }

        public static void Update(GameTime gameTime)
        {
            if (timer > 0f)
                timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public static void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            if (timer <= 0f || font == null)
                return;

            float alpha = MathHelper.Clamp(timer / FadeTime, 0f, 1f);
            float rise = (Duration - timer) * 20f; // drifts upward over time

            Vector2 size = font.MeasureString(text);
            Vector2 pos = new Vector2(position.X - size.X / 2f, position.Y - rise);

            spriteBatch.DrawString(font, text, pos + new Vector2(2, 2), Color.Black * alpha); // shadow
            spriteBatch.DrawString(font, text, pos, color * alpha);
        }
    }
}