// Connor Fulton
using System;

namespace _3902sprint0.Environment.Rooms
{
    public enum RoomSide { North, South, East, West }

    public static class RoomSideExtensions
    {
        public static RoomSide Opposite(this RoomSide side)
        {
            switch (side)
            {
                case RoomSide.North: return RoomSide.South;
                case RoomSide.South: return RoomSide.North;
                case RoomSide.East: return RoomSide.West;
                case RoomSide.West: return RoomSide.East;
                default: throw new ArgumentOutOfRangeException(nameof(side));
            }
        }

        //East and west walls run top to bottom, so their doors are vertical.
        public static bool RunsVertically(this RoomSide side)
        {
            return side == RoomSide.East || side == RoomSide.West;
        }
    }
}
