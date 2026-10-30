// Connor Fulton
using _3902sprint0.Environment.Doors;
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;

namespace _3902sprint0.Environment.Rooms
{
    
    // Turns the text of one room file into RoomData and reports every format mistake with the
    // room id and cell. 
    public static class RoomFileParser
    {
        public static RoomData Parse(string id, IEnumerable<string> lines)
        {
            var header = new List<KeyValuePair<string, string>>();
            var rows = new List<string>();
            bool inGrid = false;
            int lineNumber = 0;

            foreach (string raw in lines)
            {
                lineNumber++;
                string line = raw.Trim();
                if (line.Length == 0 || line.StartsWith("//")) continue;

                if (!inGrid && line == RoomLegend.HeaderSeparator) { inGrid = true; continue; }
                if (inGrid) { rows.Add(line); continue; }

                int colon = line.IndexOf(':');
                if (colon < 0) throw Error(id, $"line {lineNumber}: expected 'key: value' or '---', got '{line}'");
                header.Add(new KeyValuePair<string, string>(
                    line.Substring(0, colon).Trim().ToLowerInvariant(), line.Substring(colon + 1).Trim()));
            }

            if (!inGrid) throw Error(id, "missing '---' line before the grid");
            if (rows.Count == 0) throw Error(id, "grid is empty");

            for (int r = 0; r < rows.Count; r++)
                if (rows[r].Length != rows[0].Length)
                    throw Error(id, $"row {r} is {rows[r].Length} wide but row 0 is {rows[0].Length}");

            var links = new Dictionary<RoomSide, string>();
            var enemies = new List<EnemyType>();
            ParseHeader(id, header, links, enemies);

            var keys = new List<KeySpawn>();
            Point? spawn = null;
            ScanInterior(id, rows, keys, ref spawn);
            List<DoorRun> doors = ScanDoors(id, rows);

            return new RoomData(id, rows, links, enemies, doors, keys, spawn);
        }

        private static void ParseHeader(string id, List<KeyValuePair<string, string>> header,
            Dictionary<RoomSide, string> links, List<EnemyType> enemies)
        {
            foreach (var pair in header)
            {
                if (pair.Key == "enemies")
                {
                    foreach (string name in pair.Value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (!Enum.TryParse(name.Trim(), true, out EnemyType type))
                            throw Error(id, $"unknown enemy '{name.Trim()}'");
                        enemies.Add(type);
                    }
                }
                else if (Enum.TryParse(pair.Key, true, out RoomSide side))
                {
                    if (links.ContainsKey(side)) throw Error(id, $"'{pair.Key}' is listed twice");
                    bool none = pair.Value == "-" || pair.Value.Equals("none", StringComparison.OrdinalIgnoreCase);
                    links[side] = none ? null : pair.Value;
                }
                else
                {
                    throw Error(id, $"unknown header key '{pair.Key}'");
                }
            }
        }

        private static void ScanInterior(string id, List<string> rows, List<KeySpawn> keys, ref Point? spawn)
        {
            int cols = rows[0].Length;
            for (int r = 0; r < rows.Count; r++)
            for (int c = 0; c < cols; c++)
            {
                char ch = rows[r][c];
                if (!RoomLegend.IsKnown(ch)) throw Error(id, $"unknown symbol '{ch}' at ({c},{r})");

                bool onBorder = r == 0 || c == 0 || r == rows.Count - 1 || c == cols - 1;
                if (RoomLegend.Doors.ContainsKey(ch) && !onBorder)
                    throw Error(id, $"door symbol '{ch}' at ({c},{r}) must be on the room border");

                if (ch == RoomLegend.PlayerSpawn)
                {
                    if (spawn.HasValue) throw Error(id, $"second player spawn at ({c},{r})");
                    spawn = new Point(c, r);
                }
                else if (ch == RoomLegend.KeyholeKey) keys.Add(new KeySpawn(new Point(c, r), LockType.Keyhole));
                else if (ch == RoomLegend.DiamondKey) keys.Add(new KeySpawn(new Point(c, r), LockType.Diamond));
            }
        }

        private static List<DoorRun> ScanDoors(string id, List<string> rows)
        {
            int cols = rows[0].Length, rowCount = rows.Count;
            var doors = new List<DoorRun>();
            AddRun(id, doors, RoomSide.North, rows[0]);
            AddRun(id, doors, RoomSide.South, rows[rowCount - 1]);

            var west = new char[rowCount];
            var east = new char[rowCount];
            for (int r = 0; r < rowCount; r++) { west[r] = rows[r][0]; east[r] = rows[r][cols - 1]; }
            AddRun(id, doors, RoomSide.West, new string(west));
            AddRun(id, doors, RoomSide.East, new string(east));
            return doors;
        }

        private static void AddRun(string id, List<DoorRun> doors, RoomSide side, string border)
        {
            int start = -1, count = 0;
            for (int i = 0; i < border.Length; i++)
            {
                if (!RoomLegend.Doors.ContainsKey(border[i])) continue;
                if (start >= 0 && i != start + count)
                    throw Error(id, $"{side} wall has more than one door");
                if (start >= 0 && border[i] != border[start])
                    throw Error(id, $"{side} wall door mixes symbols '{border[start]}' and '{border[i]}'");
                if (start < 0) start = i;
                count++;
            }
            if (start < 0) return;

            if (count != DoorArt.Span)
                throw Error(id, $"{side} door must be exactly {DoorArt.Span} cells, found {count}");
            if (start < 1 || start + count > border.Length - 1)
                throw Error(id, $"{side} door touches a corner");

            doors.Add(new DoorRun(side, RoomLegend.Doors[border[start]], start));
        }

        private static InvalidDataException Error(string id, string message)
        {
            return new InvalidDataException($"Room '{id}': {message}");
        }
    }
}
