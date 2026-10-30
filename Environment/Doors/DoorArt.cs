// Connor Fulton
using Microsoft.Xna.Framework;

namespace _3902sprint0.Environment.Doors
{
    //The door pictures on doors.png.
    public enum DoorArtType
    {
        Doorway,
        KeyClosed,
        DiamondClosed,
        ShutterClosed,
        BombableClosed,
        BombableOpen
    }

    // Knows where each door picture sits on Content/environment/doors.png and which pictures
    // each DoorKind uses.
    public static class DoorArt
    {
        public const int Cell = 64;
        public const int Span = 4;                 // every door is 4 cells long
        public const int Length = Cell * Span;     // 256px

        public static Rectangle Source(DoorArtType type, bool vertical)
        {
            int index = (int)type;
            return vertical
                ? new Rectangle(Length + index * Cell, 0, Cell, Length)
                : new Rectangle(0, index * Cell, Length, Cell);
        }

        public static DoorArtType ClosedArt(DoorKind kind)
        {
            switch (kind)
            {
                case DoorKind.Keyhole: return DoorArtType.KeyClosed;
                case DoorKind.Diamond: return DoorArtType.DiamondClosed;
                case DoorKind.Shutter: return DoorArtType.ShutterClosed;
                case DoorKind.Bombable: return DoorArtType.BombableClosed;
                default: return DoorArtType.Doorway;
            }
        }

        public static DoorArtType OpenArt(DoorKind kind)
        {
            return kind == DoorKind.Bombable ? DoorArtType.BombableOpen : DoorArtType.Doorway;
        }
    }
}
