// Connor Fulton
using System.Collections.Generic;
using _3902sprint0.Environment.Doors;

namespace _3902sprint0.Environment.Rooms
{
    //The symbols allowed in a room file. The one place that maps characters to meaning.
    public static class RoomLegend
    {
        public const char Floor = '.';
        public const char PlayerSpawn = '@';
        public const char KeyholeKey = 'k';
        public const char DiamondKey = 'l';
        public const string HeaderSeparator = "---";

        public static readonly IReadOnlyDictionary<char, TileType> Tiles = new Dictionary<char, TileType>
        {
            { '#', TileType.Wall },
            { 'B', TileType.SquareBlock },
            { 'S', TileType.Statue },
            { 'P', TileType.PushableBlock },
            { 'F', TileType.Fire },
            { 'G', TileType.BlueGap },
            { 'X', TileType.Stairs }
        };

        public static readonly IReadOnlyDictionary<char, DoorKind> Doors = new Dictionary<char, DoorKind>
        {
            { 'D', DoorKind.Doorway },
            { 'K', DoorKind.Keyhole },
            { 'L', DoorKind.Diamond },
            { 'O', DoorKind.Bombable },
            { 'Z', DoorKind.Shutter }
        };

        public static bool IsKnown(char c)
        {
            return c == Floor || c == PlayerSpawn || c == KeyholeKey || c == DiamondKey
                || Tiles.ContainsKey(c) || Doors.ContainsKey(c);
        }
    }
}
