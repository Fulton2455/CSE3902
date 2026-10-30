// Connor Fulton
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using _3902sprint0.Environment.Doors;
using _3902sprint0.Environment.Pickups;

namespace _3902sprint0.Environment.Rooms
{
    //One built room: its tiles plus the things built alongside them.
    public class RoomInstance
    {
        public RoomData Data { get; }
        public Room Room { get; }
        public IReadOnlyList<RoomExit> Exits { get; }
        public IReadOnlyList<ShutterDoorTile> Shutters { get; }
        public IReadOnlyList<KeyPickup> Keys { get; }
        public Vector2 PlayerSpawn { get; }

        public RoomInstance(RoomData data, Room room, IReadOnlyList<RoomExit> exits,
            IReadOnlyList<ShutterDoorTile> shutters, IReadOnlyList<KeyPickup> keys, Vector2 playerSpawn)
        {
            Data = data;
            Room = room;
            Exits = exits;
            Shutters = shutters;
            Keys = keys;
            PlayerSpawn = playerSpawn;
        }
    }
}
