// Connor Fulton
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3902sprint0.Environment.Doors
{
    public abstract class DoorTile : Tile
    {
        private readonly DoorArt art;

        protected DoorTile(Texture2D spriteSheet, Rectangle bounds, DoorArt art)
            : base(spriteSheet, bounds.Center.ToVector2(), art.Closed, bounds.Width, bounds.Height)
        {
            this.art = art;
        }

        protected void ShowClosed()
        {
            sourceRectangle = art.Closed;
        }

        protected void ShowOpen()
        {
            sourceRectangle = art.Open;
        }
    }
}