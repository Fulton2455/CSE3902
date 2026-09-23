// Connor Fulton

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3902sprint0.Environment.Tiles
{
    
    // A block the player can push one tile at a time. The push attempt is validated
    // by the caller via canOccupy, since this tile doesn't know about
    // other tiles or the grid.
    
     public class PushableBlockTile : Tile
    {
        public PushableBlockTile(Texture2D texture, Vector2 position, Rectangle sourceRectangle, int width = 64, int height = 64)
            : base(texture, position, sourceRectangle, width, height)
        {
            IsSolid = true;
        }

        public bool TryPush(Vector2 direction, float tileSize, Func<Vector2, bool> canOccupy)
        {
            Vector2 target = Position + direction * tileSize;
            if (!canOccupy(target))
                return false;

            Position = target;
            UpdateDestinationRectangle();
            return true;
        }
    }
}