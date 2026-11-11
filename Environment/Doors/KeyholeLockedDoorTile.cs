// Connor Fulton
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3902sprint0.Environment.Doors
{
    public class KeyholeLockedDoorTile : LockedDoorTile
    {
        public KeyholeLockedDoorTile(Texture2D spriteSheet, Rectangle bounds, DoorArt art)
            : base(spriteSheet, bounds, art, LockType.Keyhole) { }
    }
}