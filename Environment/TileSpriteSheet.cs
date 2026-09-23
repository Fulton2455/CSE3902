// Connor Fulton

using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace _3902sprint0.Environment
{
    //
    /// Maps each TileType to its cell in environment sprite sheet.
    //
    public static class TileSpriteSheet
    {
        public const int CellSize = 64;

        private static readonly Dictionary<TileType, Point> Cells = new Dictionary<TileType, Point>
        {
            { TileType.Wall, new Point(0, 0) },
            { TileType.Statue, new Point(1, 0) },
            { TileType.SquareBlock, new Point(2, 0) },
            { TileType.PushableBlock, new Point(3, 0) },
            { TileType.Fire, new Point(0, 1) },
            { TileType.BlueGap, new Point(1, 1) },
            { TileType.Stairs, new Point(2, 1) },
            { TileType.OpenDoor, new Point(3, 1) },
            { TileType.BombedWallOpening, new Point(0, 2) }, // closed state
            { TileType.KeyholeLockedDoor, new Point(2, 2) },
            { TileType.DiamondLockedDoor, new Point(3, 2) },
        };

        public static readonly Point BombedWallOpenCell = new Point(1, 2);

        public static Rectangle GetSourceRect(TileType type) => GetSourceRect(Cells[type]);

        public static Rectangle GetSourceRect(Point cell) =>
            new Rectangle(cell.X * CellSize, cell.Y * CellSize, CellSize, CellSize);
    }
}