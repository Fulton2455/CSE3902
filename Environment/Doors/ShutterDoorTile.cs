// Connor Fulton
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3902sprint0.Environment.Doors
{
    // A door that is shut while enemies are alive and open once the room is cleared.
    public class ShutterDoorTile : DoorTile
    {
        public bool IsOpen { get; private set; }
        public override bool IsSolid => !IsOpen;

        public ShutterDoorTile(Texture2D spriteSheet, Rectangle bounds, DoorArt art)
            : base(spriteSheet, bounds, art) { }

        public void Open()
        {
            IsOpen = true;
            ShowOpen();
        }

        public void Close()
        {
            IsOpen = false;
            ShowClosed();
        }
    }
}