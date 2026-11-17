// Erik Zhang
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace _3902sprint0.Environment.Rooms
{
    /// <summary>
    /// Assigns every room reachable from the start room a grid cell by walking the
    /// dungeon's links (north = up one cell, east = right one cell, and so on).
    /// The minimap uses this so it never has to parse room ids like "d1_c6".
    /// </summary>
    public class DungeonLayout
    {
        private readonly Dictionary<string, Point> cells = new Dictionary<string, Point>();

        /// <summary>Number of grid columns the dungeon spans.</summary>
        public int Width { get; }
        /// <summary>Number of grid rows the dungeon spans.</summary>
        public int Height { get; }

        public DungeonLayout(DungeonMap dungeon, string startRoomId)
        {
            var queue = new Queue<string>();
            cells[startRoomId] = Point.Zero;
            queue.Enqueue(startRoomId);

            // Breadth-first walk: each neighbour sits one cell over from the room that links to it.
            while (queue.Count > 0)
            {
                string id = queue.Dequeue();
                Point cell = cells[id];

                foreach (var link in dungeon.Get(id).Links)
                {
                    if (link.Value == null || cells.ContainsKey(link.Value)) continue;
                    cells[link.Value] = cell + Offset(link.Key);
                    queue.Enqueue(link.Value);
                }
            }

            // Shift everything so the top-left room is at (0,0).
            int minX = cells.Values.Min(p => p.X);
            int minY = cells.Values.Min(p => p.Y);
            foreach (string id in cells.Keys.ToList())
                cells[id] = new Point(cells[id].X - minX, cells[id].Y - minY);

            Width = cells.Values.Max(p => p.X) + 1;
            Height = cells.Values.Max(p => p.Y) + 1;
        }

        /// <summary>Grid cell of a room, or false if the room cannot be reached from the start room.</summary>
        public bool TryGetCell(string id, out Point cell) => cells.TryGetValue(id, out cell);

        private static Point Offset(RoomSide side)
        {
            switch (side)
            {
                case RoomSide.North: return new Point(0, -1);
                case RoomSide.South: return new Point(0, 1);
                case RoomSide.West: return new Point(-1, 0);
                default: return new Point(1, 0);
            }
        }
    }
}
