// Connor Fulton
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3902sprint0.Environment.Doors
{
    // A wall that looks solid until an explosion opens it.
    public class BombedWallOpeningTile : DoorTile, IOpenable
    {
        public bool IsOpened { get; private set; }
        public override bool IsSolid => !IsOpened;

        public BombedWallOpeningTile(Texture2D spriteSheet, Rectangle bounds, DoorArt art)
            : base(spriteSheet, bounds, art) { }

        public void Detonate()
        {
            if (IsOpened)
                return;

            IsOpened = true;
            ShowOpen();
        }

        public void ForceOpen()
        {
            Detonate();
        }
    }
}