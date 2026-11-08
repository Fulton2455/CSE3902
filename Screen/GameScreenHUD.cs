using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _3902sprint0
{
    public class GameScreenHUD
    {
        private readonly SpriteFont font;
        private readonly Texture2D pixel;
        private MouseState previousMouse;

        public GameScreenHUD(GraphicsDevice graphicsDevice, SpriteFont font)
        {
            this.font = font;
            pixel = new Texture2D(graphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });
        }

        public MenuAction Update(GameScreenState state, GraphicsDevice gd)
        {
            MouseState mouse = Mouse.GetState();
            Vector2 mousePos = new Vector2(mouse.X, mouse.Y);

            bool clicked =
                mouse.LeftButton == ButtonState.Pressed &&
                previousMouse.LeftButton == ButtonState.Released;

            MenuAction action = MenuAction.None;

            if (clicked)
            {
                if (state == GameScreenState.Start)
                {
                    if (GetButton(gd, 0, 3).Contains(mousePos))
                        action = MenuAction.StartGame;
                    else if (GetButton(gd, 1, 3).Contains(mousePos))
                        action = MenuAction.Settings;
                    else if (GetButton(gd, 2, 3).Contains(mousePos))
                        action = MenuAction.Quit;
                }
                else if (state == GameScreenState.Settings)
                {
                    if (GetButton(gd, 0, 1).Contains(mousePos))
                        action = MenuAction.Back;
                }
                else if (state == GameScreenState.Paused)
                {
                    if (GetButton(gd, 0, 3).Contains(mousePos))
                        action = MenuAction.Restart;
                    else if (GetButton(gd, 1, 3).Contains(mousePos))
                        action = MenuAction.Settings;
                    else if (GetButton(gd, 2, 3).Contains(mousePos))
                        action = MenuAction.MainMenu;
                }
                else if (state == GameScreenState.Death)
                {
                    if (GetButton(gd, 0, 2).Contains(mousePos))
                        action = MenuAction.Restart;
                    else if (GetButton(gd, 1, 2).Contains(mousePos))
                        action = MenuAction.MainMenu;
                }
            }

            previousMouse = mouse;
            return action;
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice gd, GameScreenState state)
        {
            if (state == GameScreenState.Playing)
                return;

            Color background =
                state == GameScreenState.Start || state == GameScreenState.Settings
                ? Color.Black
                : Color.Black * 0.75f;

            spriteBatch.Draw(
                pixel,
                new Rectangle(0, 0, gd.Viewport.Width, gd.Viewport.Height),
                background);

            if (state == GameScreenState.Start)
            {
                DrawTitle(spriteBatch, gd, "THE LEGEND OF ZELDA");
                DrawButton(spriteBatch, gd, "START", 0, 3);
                DrawButton(spriteBatch, gd, "SETTINGS", 1, 3);
                DrawButton(spriteBatch, gd, "QUIT", 2, 3);
            }
            else if (state == GameScreenState.Settings)
            {
                DrawTitle(spriteBatch, gd, "SETTINGS");
                DrawButton(spriteBatch, gd, "BACK", 0, 1);
            }
            else if (state == GameScreenState.Paused)
            {
                DrawTitle(spriteBatch, gd, "PAUSED");
                DrawButton(spriteBatch, gd, "RESTART", 0, 3);
                DrawButton(spriteBatch, gd, "SETTINGS", 1, 3);
                DrawButton(spriteBatch, gd, "MAIN MENU", 2, 3);
            }
            else if (state == GameScreenState.Death)
            {
                DrawTitle(spriteBatch, gd, "GAME OVER");
                DrawButton(spriteBatch, gd, "RESTART", 0, 2);
                DrawButton(spriteBatch, gd, "MAIN MENU", 1, 2);
            }
        }

        private Rectangle GetButton(GraphicsDevice gd, int index, int count)
        {
            const int width = 320;
            const int height = 70;
            const int spacing = 20;

            int totalHeight = count * height + (count - 1) * spacing;
            int x = gd.Viewport.Width / 2 - width / 2;
            int y = gd.Viewport.Height / 2 - totalHeight / 2 + index * (height + spacing) + 70;

            return new Rectangle(x, y, width, height);
        }

        private void DrawButton(SpriteBatch spriteBatch, GraphicsDevice gd, string text, int index, int count)
        {
            Rectangle rect = GetButton(gd, index, count);

            MouseState mouse = Mouse.GetState();
            Vector2 mousePos = new Vector2(mouse.X, mouse.Y);

            bool hover = rect.Contains(mousePos);

            spriteBatch.Draw(pixel, rect, hover ? Color.DarkGray : Color.Gray);

            Vector2 size = font.MeasureString(text);

            spriteBatch.DrawString(
                font,
                text,
                new Vector2(rect.Center.X - size.X / 2, rect.Center.Y - size.Y / 2), Color.White);
        }

        private void DrawTitle(SpriteBatch spriteBatch, GraphicsDevice gd, string text)
        {
            Vector2 size = font.MeasureString(text);

            spriteBatch.DrawString(
                font,
                text,
                new Vector2(gd.Viewport.Width / 2f - size.X / 2, gd.Viewport.Height / 2f - 180), Color.White);
        }
    }
}