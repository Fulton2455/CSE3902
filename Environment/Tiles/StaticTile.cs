// Connor Fulton

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3902sprint0.Environment.Tiles
{
    public class StaticTile : Tile
    {
        public StaticTile(Texture2D texture, Vector2 position, bool isSolid, Rectangle sourceRectangle, int width = 64, int height = 64)
            : base(texture, position, sourceRectangle, width, height)
        {
            IsSolid = isSolid;
        }
    }
}