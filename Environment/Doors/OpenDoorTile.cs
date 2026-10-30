// Connor Fulton
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3902sprint0.Environment.Doors
{
    //An open doorway
    public class OpenDoorTile : Tile
    {
        public override bool IsSolid => false;

        // Single-cell doorway from the environment spritesheet.
        public OpenDoorTile(Texture2D spriteSheet, Vector2 position, Rectangle sourceRectangle, int width = 64, int height = 64)
            : base(spriteSheet, position, sourceRectangle, width, height) { }
    }
}