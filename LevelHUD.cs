namespace _3902sprint0
{
    public class LevelHUD
    {
        private SpriteFont font;

        public LevelHUD(SpriteFont font)
        {
            this.font = font;
        }

        public void Draw(SpriteBatch spriteBatch, int level)
        {
            spriteBatch.DrawString(
                font,
                "LEVEL " + level,
                new Vector2(20, 70),
                Color.White
            );
        }
    }
}