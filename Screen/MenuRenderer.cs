using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3902sprint0
{
    //Draws a menu: background, title and buttons. Reads nothing but what it is given.
    public class MenuRenderer
    {
        private static readonly Color OpaqueBackground = Color.Black;
        private static readonly Color OverlayBackground = Color.Black * 0.75f;
        private static readonly Color ButtonColor = Color.Gray;
        private static readonly Color ButtonHoverColor = Color.DarkGray;
        private static readonly Color TextColor = Color.White;

        private readonly Texture2D pixel;
        private readonly SpriteFont font;

        public MenuRenderer(Texture2D pixel, SpriteFont font)
        {
            this.pixel = pixel;
            this.font = font;
        }

        public void Draw(SpriteBatch spriteBatch, Point screenSize, MenuDefinition menu, int hoveredIndex)
        {
            Color background = menu.HasOpaqueBackground ? OpaqueBackground : OverlayBackground;
            spriteBatch.Draw(pixel, new Rectangle(0, 0, screenSize.X, screenSize.Y), background);

            DrawTitle(spriteBatch, screenSize, menu.Title);

            for (int i = 0; i < menu.Buttons.Count; i++)
            {
                Rectangle bounds = MenuLayout.ButtonBounds(screenSize, i, menu.Buttons.Count);
                DrawButton(spriteBatch, bounds, menu.Buttons[i].Label, i == hoveredIndex);
            }
        }

        private void DrawTitle(SpriteBatch spriteBatch, Point screenSize, string title)
        {
            Vector2 size = font.MeasureString(title);
            Vector2 position = new Vector2(screenSize.X / 2f - size.X / 2f, MenuLayout.TitleTop(screenSize));
            spriteBatch.DrawString(font, title, position, TextColor);
        }

        private void DrawButton(SpriteBatch spriteBatch, Rectangle bounds, string label, bool hovered)
        {
            spriteBatch.Draw(pixel, bounds, hovered ? ButtonHoverColor : ButtonColor);

            Vector2 size = font.MeasureString(label);
            Vector2 position = new Vector2(bounds.Center.X - size.X / 2f, bounds.Center.Y - size.Y / 2f);
            spriteBatch.DrawString(font, label, position, TextColor);
        }
    }
}