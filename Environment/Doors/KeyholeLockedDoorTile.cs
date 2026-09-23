// Connor Fulton

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3902sprint0.Environment.Doors
{
   public class KeyholeLockedDoorTile : LockedDoorTile
    {
        public KeyholeLockedDoorTile(Texture2D spriteSheet, Vector2 position, int width = 64, int height = 64)
            : base(spriteSheet, position, LockType.Keyhole, TileSpriteSheet.GetSourceRect(TileType.KeyholeLockedDoor), width, height) { }
    }
}