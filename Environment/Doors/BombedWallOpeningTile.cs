// Connor Fulton
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3902sprint0.Environment.Doors
{
    public class BombedWallOpeningTile : Tile
    {
        private readonly Rectangle closedSource;
        private readonly Rectangle openSource;

        public bool IsOpened { get; private set; }
        public override bool IsSolid => !IsOpened;

        // Single-cell wall using the environment spritesheet.
        public BombedWallOpeningTile(Texture2D spriteSheet, Vector2 position, int width = 64, int height = 64)
            : this(
                spriteSheet,
                position,
                TileSpriteSheet.GetSourceRect(TileType.BombedWallOpening),
                TileSpriteSheet.GetSourceRect(TileSpriteSheet.BombedWallOpenCell),
                width,
                height)
        {
        }

        // Sized wall with its own closed and opened pictures (used for room border doors).
        public BombedWallOpeningTile(
            Texture2D spriteSheet,
            Vector2 position,
            Rectangle closedSource,
            Rectangle openSource,
            int width,
            int height)
            : base(spriteSheet, position, closedSource, width, height)
        {
            this.closedSource = closedSource;
            this.openSource = openSource;
        }

        public void Detonate()
        {
            if (IsOpened)
                return;

            IsOpened = true;
            sourceRectangle = openSource;
        }
    }
}
