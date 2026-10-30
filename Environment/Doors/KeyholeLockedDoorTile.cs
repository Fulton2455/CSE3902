// Connor Fulton
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3902sprint0.Environment.Doors
{
    public class KeyholeLockedDoorTile : LockedDoorTile
    {
        // Single-cell door using the environment spritesheet.
        public KeyholeLockedDoorTile(Texture2D spriteSheet, Vector2 position, int width = 64, int height = 64)
            : base(spriteSheet, position, LockType.Keyhole, TileSpriteSheet.GetSourceRect(TileType.KeyholeLockedDoor), width, height) { }

        // Sized door with its own closed and unlocked pictures (used for room border doors).
        public KeyholeLockedDoorTile(Texture2D spriteSheet, Vector2 position, Rectangle closedSource, Rectangle unlockedSource, int width, int height)
            : base(spriteSheet, position, LockType.Keyhole, closedSource, width, height, unlockedSource) { }
    }
}
