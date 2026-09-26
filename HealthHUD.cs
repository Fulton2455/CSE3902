namespace _3902sprint0
{
    public class HealthHUD
    {
        private Texture2D heartTexture;

        private int heartWidth = 40;
        private int heartHeight = 40;
        private int spacing = 5;

        private Rectangle fullHeart;
        private Rectangle halfHeart;
        private Rectangle emptyHeart;

        public HealthHUD(Texture2D heartTexture)
        {
            this.heartTexture = heartTexture;

            fullHeart = new Rectangle(0, 0, 32, 32);
            halfHeart = new Rectangle(32, 0, 32, 32);
            emptyHeart = new Rectangle(64, 0, 32, 32);
        }

        public void Draw(SpriteBatch spriteBatch, Player player)
        {
            int totalHearts = 3;

            for (int i = 0; i < totalHearts; i++)
            {
                int x = 20 + i * (heartWidth + spacing);
                int y = 20;

                Rectangle heartBox = new Rectangle(
                    x,
                    y,
                    heartWidth,
                    heartHeight
                );

                int heartHealth = player.Health - i * 2;

                if (heartHealth >= 2)
                {
                    spriteBatch.Draw(
                        heartTexture,
                        heartBox,
                        fullHeart,
                        Color.White
                    );
                }
                else if (heartHealth == 1)
                {
                    spriteBatch.Draw(
                        heartTexture,
                        heartBox,
                        halfHeart,
                        Color.White
                    );
                }
                else
                {
                    spriteBatch.Draw(
                        heartTexture,
                        heartBox,
                        emptyHeart,
                        Color.White
                    );
                }
            }
        }
    }
}