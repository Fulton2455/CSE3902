// Connor Fulton
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3902sprint0.Environment.Doors
{
    
    public class OpenDoorTile : DoorTile
    {
        public override bool IsSolid => false;

        public OpenDoorTile(Texture2D spriteSheet, Rectangle bounds, DoorArt art)
            : base(spriteSheet, bounds, art) { }
    }
}