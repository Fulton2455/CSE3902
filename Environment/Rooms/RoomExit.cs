// Connor Fulton
using Microsoft.Xna.Framework;

namespace _3902sprint0.Environment.Rooms
{
    //A door the player can walk through to reach another room.
    public class RoomExit
    {
        // How far the hitbox must be inside an open door before the room changes.
        private const int Depth = 24;

        public ITile Door { get; }
        public RoomSide Side { get; }
        public string TargetRoomId { get; }

        public RoomExit(ITile door, RoomSide side, string targetRoomId)
        {
            Door = door;
            Side = side;
            TargetRoomId = targetRoomId;
        }

        public bool IsCrossedBy(Rectangle hitbox)
        {
            if (Door.IsSolid || !Door.Bounds.Intersects(hitbox))
                return false;

            Rectangle d = Door.Bounds;
            switch (Side)
            {
                case RoomSide.North: return hitbox.Top <= d.Bottom - Depth;
                case RoomSide.South: return hitbox.Bottom >= d.Top + Depth;
                case RoomSide.West: return hitbox.Left <= d.Right - Depth;
                default: return hitbox.Right >= d.Left + Depth;
            }
        }
    }
}
