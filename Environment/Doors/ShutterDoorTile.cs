// Connor Fulton
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3902sprint0.Environment.Doors
{
    
    // A door that is shut while enemies are alive and open once the room is cleared.
  
    public class ShutterDoorTile : Tile
    {
        private readonly Rectangle closedSource;
        private readonly Rectangle openSource;

        public bool IsOpen { get; private set; }
        public override bool IsSolid => !IsOpen;

        public ShutterDoorTile(
            Texture2D spriteSheet,
            Vector2 position,
            Rectangle closedSource,
            Rectangle openSource,
            int width,
            int height)
            : base(spriteSheet, position, closedSource, width, height)
        {
            this.closedSource = closedSource;
            this.openSource = openSource;
        }

        public void Open()
        {
            IsOpen = true;
            sourceRectangle = openSource;
        }

        public void Close()
        {
            IsOpen = false;
            sourceRectangle = closedSource;
        }
    }
}
