// Connor Fulton

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3902sprint0.Environment.Tiles
{
    
    // Triggers a room transition when something walks over it. Fires an event
    // rather than owning transition logic, since that belongs to a Room manager.
   
    public class StairsTile : Tile
    {
        public event Action<StairsTile> Traversed;

        public StairsTile(Texture2D texture, Vector2 position, Rectangle sourceRectangle, int width = 64, int height = 64)
            : base(texture, position, sourceRectangle, width, height)
        {
            IsSolid = false;
        }

        public void NotifyTraversed() => Traversed?.Invoke(this);
    }
}