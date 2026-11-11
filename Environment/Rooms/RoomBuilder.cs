using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using _3902sprint0.Environment.Doors;
using _3902sprint0.Environment.Pickups;

namespace _3902sprint0.Environment.Rooms
{
    /// <summary>Builds a RoomInstance (tiles, doors, key pickups) from parsed RoomData.</summary>
    public class RoomBuilder
    {
        private const int Cell = 64;

        private readonly RoomTextures textures;
        private readonly Rectangle worldBounds;

        public RoomBuilder(RoomTextures textures, Rectangle worldBounds)
        {
            this.textures = textures;
            this.worldBounds = worldBounds;
        }

        public RoomInstance Build(RoomData data)
        {
            int cols = data.Columns, rows = data.Rows.Count;
            if (cols * Cell > worldBounds.Width || rows * Cell > worldBounds.Height)
                throw new InvalidDataException($"Room '{data.Id}' is {cols}x{rows} cells but the world only fits " +
                    $"{worldBounds.Width / Cell}x{worldBounds.Height / Cell}.");

            var room = new Room(Cell, worldBounds);
            AddFixedTiles(data, room);

            var exits = new List<RoomExit>();
            var shutters = new List<ShutterDoorTile>();
            foreach (DoorRun run in data.Doors)
                AddDoor(data, run, room, exits, shutters);

            var keys = new List<KeyPickup>();
            foreach (KeySpawn spawn in data.KeySpawns)
                keys.Add(new KeyPickup(textures.SmallKey, CellCenter(spawn.Cell.X, spawn.Cell.Y), spawn.Lock));

            Point spawnCell = data.PlayerSpawn ?? new Point(cols / 2, rows / 2);
            return new RoomInstance(data, room, exits, shutters, keys, CellCenter(spawnCell.X, spawnCell.Y));
        }

        private void AddFixedTiles(RoomData data, Room room)
        {
            for (int r = 0; r < data.Rows.Count; r++)
            for (int c = 0; c < data.Columns; c++)
            {
                if (RoomLegend.Tiles.TryGetValue(data.Rows[r][c], out TileType type))
                    room.AddTile(TileFactory.CreateTile(type, textures.Environment, CellCenter(c, r)));
            }
        }

        private void AddDoor(RoomData data, DoorRun run, Room room, List<RoomExit> exits, List<ShutterDoorTile> shutters)
        {
            DoorArt art = DoorArt.For(run.Kind, run.Side.RunsVertically());
            DoorTile door = CreateDoor(run.Kind, DoorBounds(data, run), art);
            room.AddTile(door);

            if (door is ShutterDoorTile shutter) shutters.Add(shutter);

            string target = data.Links[run.Side];
            if (target != null) exits.Add(new RoomExit(door, run.Side, target));
        }

        private DoorTile CreateDoor(DoorKind kind, Rectangle bounds, DoorArt art)
        {
            switch (kind)
            {
                case DoorKind.Keyhole: return new KeyholeLockedDoorTile(textures.Doors, bounds, art);
                case DoorKind.Diamond: return new DiamondLockedDoorTile(textures.Doors, bounds, art);
                case DoorKind.Bombable: return new BombedWallOpeningTile(textures.Doors, bounds, art);
                case DoorKind.Shutter: return new ShutterDoorTile(textures.Doors, bounds, art);
                default: return new OpenDoorTile(textures.Doors, bounds, art);
            }
        }

        private static Vector2 CellCenter(int col, int row)
        {
            return new Vector2(col * Cell + Cell / 2f, row * Cell + Cell / 2f);
        }

        // The rectangle a door covers on its wall: 4 cells long, 1 cell thick.
        private static Rectangle DoorBounds(RoomData data, DoorRun run)
        {
            int along = run.StartIndex * Cell;
            int lastCol = (data.Columns - 1) * Cell;
            int lastRow = (data.Rows.Count - 1) * Cell;

            switch (run.Side)
            {
                case RoomSide.North: return new Rectangle(along, 0, DoorArt.Length, Cell);
                case RoomSide.South: return new Rectangle(along, lastRow, DoorArt.Length, Cell);
                case RoomSide.West: return new Rectangle(0, along, Cell, DoorArt.Length);
                default: return new Rectangle(lastCol, along, Cell, DoorArt.Length);
            }
        }
    }
}