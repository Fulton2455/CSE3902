// Connor Fulton

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3902sprint0.Environment.Doors
{
    public class DiamondLockedDoorTile : LockedDoorTile
    {
        public DiamondLockedDoorTile(Texture2D spriteSheet, Vector2 position, int width = 64, int height = 64)
            : base(spriteSheet, position, LockType.Diamond, TileSpriteSheet.GetSourceRect(TileType.DiamondLockedDoor), width, height) { }
    }
}