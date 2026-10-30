// Connor Fulton
using _3902sprint0.Environment.Doors;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace _3902sprint0.Environment.Rooms
{
    //A door found on a room border
    public class DoorRun
    {
        public RoomSide Side { get; }
        public DoorKind Kind { get; }
        public int StartIndex { get; }

        public DoorRun(RoomSide side, DoorKind kind, int startIndex)
        {
            Side = side;
            Kind = kind;
            StartIndex = startIndex;
        }
    }

    public class KeySpawn
    {
        public Point Cell { get; }
        public LockType Lock { get; }

        public KeySpawn(Point cell, LockType lockType)
        {
            Cell = cell;
            Lock = lockType;
        }
    }

    //Everything a room file says, as plain data. Knows nothing about textures or tiles.
    public class RoomData
    {
        public string Id { get; }
        public IReadOnlyList<string> Rows { get; }
        //Target room id per side. A null value means the door is a dead end.
        public IReadOnlyDictionary<RoomSide, string> Links { get; }
        public IReadOnlyList<EnemyType> Enemies { get; }
        public IReadOnlyList<DoorRun> Doors { get; }
        public IReadOnlyList<KeySpawn> KeySpawns { get; }
        public Point? PlayerSpawn { get; }
        //Terrain applied when the player enters this room; null keeps the current terrain.
        public terrain.terrainState? Terrain { get; }

        public int Columns => Rows.Count == 0 ? 0 : Rows[0].Length;

        public RoomData(string id, IReadOnlyList<string> rows,
            IReadOnlyDictionary<RoomSide, string> links, IReadOnlyList<EnemyType> enemies,
            IReadOnlyList<DoorRun> doors, IReadOnlyList<KeySpawn> keySpawns, Point? playerSpawn,
            terrain.terrainState? terrainState)
        {
            Id = id;
            Rows = rows;
            Links = links;
            Enemies = enemies;
            Doors = doors;
            KeySpawns = keySpawns;
            PlayerSpawn = playerSpawn;
            Terrain = terrainState;
        }

        public bool HasDoorOn(RoomSide side)
        {
            foreach (DoorRun door in Doors)
                if (door.Side == side) return true;
            return false;
        }
    }
}