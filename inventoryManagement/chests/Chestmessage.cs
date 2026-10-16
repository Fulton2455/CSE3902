using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace _3902sprint0
{
    public static class ChestMessage
    {
        private class Message
        {
            public string Text;
            public Color Color;
            public Vector2 Position;
            public float Timer;
        }

        private static SpriteFont font;
        private static readonly List<Message> messages = new List<Message>();

        private const float Duration = 2.5f;
        private const float FadeTime = 0.7f;
        private const float RiseSpeed = 20f;

        public static void Load(SpriteFont spriteFont)
        {
            font = spriteFont;
        }

        // position = the point the text is centered on (just above the chest)
        public static void Show(string text, Color color, Vector2 position)
        {
            messages.Add(new Message
            {
                Text = text,
                Color = color,
                Position = position,
                Timer = Duration
            });
        }

        public static void Clear()
        {
            messages.Clear();
        }

        public static void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            for (int i = messages.Count - 1; i >= 0; i--)
            {
                messages[i].Timer -= dt;
                if (messages[i].Timer <= 0f)
                    messages.RemoveAt(i);
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            if (font == null)
                return;

            foreach (Message m in messages)
            {
                float alpha = MathHelper.Clamp(m.Timer / FadeTime, 0f, 1f);
                float rise = (Duration - m.Timer) * RiseSpeed;

                Vector2 size = font.MeasureString(m.Text);
                Vector2 pos = new Vector2(m.Position.X - size.X / 2f, m.Position.Y - rise);

                spriteBatch.DrawString(font, m.Text, pos + new Vector2(2, 2), Color.Black * alpha);
                spriteBatch.DrawString(font, m.Text, pos, m.Color * alpha);
            }
        }
    }
}