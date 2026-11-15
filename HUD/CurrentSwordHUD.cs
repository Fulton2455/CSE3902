namespace _3902sprint0
{
    public class CurrentSwordHUD
    {
        private Texture2D pixel;
        private SpriteFont font;

        private int boxWidth = 70;
        private int boxHeight = 70;

        public CurrentSwordHUD(GraphicsDevice graphicsDevice, SpriteFont font)
        {
            this.font = font;

            pixel = new Texture2D(graphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });
        }

        public void Draw(SpriteBatch spriteBatch, Inventory inventory)
        {
            int x = 20;
            int y = 120;

            spriteBatch.DrawString(
                font,
                "SWORD",
                new Vector2(x, y),
                Color.White
            );

            Rectangle swordBox = new Rectangle(
                x,
                y + 35,
                boxWidth,
                boxHeight
            );

            spriteBatch.Draw(
                pixel,
                swordBox,
                Color.Black * 0.5f
            );

            if (inventory.CurrentSword != null)
            {
                spriteBatch.Draw(
                    inventory.CurrentSword.Texture,
                    swordBox,
                    Color.White
                );
            }
        }
    }
}