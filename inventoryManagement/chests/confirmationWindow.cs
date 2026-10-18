using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _3902sprint0
{
    public class ConfirmationWindow
    {
        private Texture2D pixel;
        private SpriteFont font;

        private Rectangle window;
        private Rectangle yesButton;
        private Rectangle noButton;

        private MouseState previousMouse;

        public bool IsOpen { get; private set; }
        public bool Answered { get; private set; }
        public bool Confirmed { get; private set; }

        public ConfirmationWindow(Texture2D pixel, SpriteFont font)
        {
            this.pixel = pixel;
            this.font = font;

            window = new Rectangle(600, 400, 500, 250);

            yesButton = new Rectangle(660, 530, 150, 60);
            noButton = new Rectangle(890, 530, 150, 60);
        }

        public void Open()
        {
            IsOpen = true;
            Answered = false;
            Confirmed = false;
        }

        public void Update()
        {
            if (!IsOpen)
                return;

            MouseState mouse = Mouse.GetState();

            bool clicked =
                mouse.LeftButton == ButtonState.Pressed &&
                previousMouse.LeftButton == ButtonState.Released;

            if (clicked)
            {
                if (yesButton.Contains(mouse.Position))
                {
                    Confirmed = true;
                    Answered = true;
                    IsOpen = false;
                }
                else if (noButton.Contains(mouse.Position))
                {
                    Confirmed = false;
                    Answered = true;
                    IsOpen = false;
                }
            }

            previousMouse = mouse;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!IsOpen)
                return;

            spriteBatch.Draw(pixel, window, Color.DarkSlateGray);

            string message = "Do you want this item?";

            Vector2 textSize = font.MeasureString(message);

            spriteBatch.DrawString(
                font,
                message,
                new Vector2(
                    window.Center.X - textSize.X / 2,
                    window.Y + 50),
                Color.White);

            spriteBatch.Draw(pixel, yesButton, Color.ForestGreen);
            spriteBatch.Draw(pixel, noButton, Color.Firebrick);

            spriteBatch.DrawString(
                font, "Yes",
                new Vector2(yesButton.X + 50, yesButton.Y + 18),
                Color.White);

            spriteBatch.DrawString(
                font, "No",
                new Vector2(noButton.X + 55, noButton.Y + 18),
                Color.White);
        }
    }
}