using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _3902sprint0.Environment.Rooms
{
    /// <summary>All room files of a dungeon, parsed and checked against each other.</summary>
    public class DungeonMap
    {
        private readonly Dictionary<string, RoomData> rooms;

        private DungeonMap(Dictionary<string, RoomData> rooms)
        {
            this.rooms = rooms;
        }

        public RoomData Get(string id)
        {
            if (!rooms.TryGetValue(id, out RoomData data))
                throw new KeyNotFoundException($"No room file named '{id}'.");
            return data;
        }

        public static DungeonMap LoadFromDirectory(string directory)
        {
            if (!Directory.Exists(directory))
                throw new DirectoryNotFoundException($"Room directory not found: {directory}");

            var rooms = new Dictionary<string, RoomData>();
            foreach (string path in Directory.GetFiles(directory, "*.txt"))
            {
                string id = Path.GetFileNameWithoutExtension(path);
                rooms[id] = RoomFileParser.Parse(id, File.ReadAllLines(path));
            }

            var map = new DungeonMap(rooms);
            map.Validate();
            return map;
        }

        /// <summary>Checks every link in both directions and reports all problems at once.</summary>
        private void Validate()
        {
            var problems = new List<string>();

            foreach (RoomData room in rooms.Values)
            {
                foreach (DoorRun door in room.Doors)
                {
                    if (!room.Links.ContainsKey(door.Side))
                        problems.Add($"{room.Id}: has a door on the {door.Side} but no '{door.Side.ToString().ToLower()}:' line");
                }

                foreach (var link in room.Links)
                {
                    if (!room.HasDoorOn(link.Key))
                        problems.Add($"{room.Id}: links {link.Key} but has no door there");
                    if (link.Value == null) continue;

                    if (!rooms.TryGetValue(link.Value, out RoomData target))
                    {
                        problems.Add($"{room.Id}: {link.Key} links to missing room '{link.Value}'");
                        continue;
                    }

                    RoomSide back = link.Key.Opposite();
                    if (!target.Links.TryGetValue(back, out string returnId) || returnId != room.Id)
                        problems.Add($"{room.Id}: {link.Key} -> {target.Id}, but {target.Id} has no '{back.ToString().ToLower()}: {room.Id}' link back");
                }
            }

            if (problems.Count > 0)
                throw new InvalidDataException("Dungeon has problems:" + System.Environment.NewLine
                    + string.Join(System.Environment.NewLine, problems.OrderBy(p => p)));
        }
    }
}
