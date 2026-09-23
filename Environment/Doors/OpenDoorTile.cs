// Connor Fulton

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3902sprint0.Environment.Doors
{
    //An already-open doorway. Never blocks movement.
     public class OpenDoorTile : Tile
    {
        public OpenDoorTile(Texture2D texture, Vector2 position, Rectangle sourceRectangle, int width = 64, int height = 64)
            : base(texture, position, sourceRectangle, width, height)
        {
            IsSolid = false;
        }
    }
}