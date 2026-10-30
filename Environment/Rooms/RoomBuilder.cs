// Connor Fulton
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using _3902sprint0.Environment.Doors;
using _3902sprint0.Environment.Pickups;

namespace _3902sprint0.Environment.Rooms
{
    //Builds a RoomInstance (tiles, doors, key pickups) from parsed RoomData.
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
            bool vertical = run.Side.RunsVertically();
            Rectangle closed = DoorArt.Source(DoorArt.ClosedArt(run.Kind), vertical);
            Rectangle open = DoorArt.Source(DoorArt.OpenArt(run.Kind), vertical);
            Vector2 center = DoorCenter(data, run);
            int width = vertical ? Cell : DoorArt.Length;
            int height = vertical ? DoorArt.Length : Cell;

            Tile door = CreateDoor(run.Kind, center, closed, open, width, height);
            room.AddTile(door);

            if (door is ShutterDoorTile shutter) shutters.Add(shutter);

            string target = data.Links[run.Side];
            if (target != null) exits.Add(new RoomExit(door, run.Side, target));
        }

        private Tile CreateDoor(DoorKind kind, Vector2 center, Rectangle closed, Rectangle open, int w, int h)
        {
            switch (kind)
            {
                case DoorKind.Keyhole: return new KeyholeLockedDoorTile(textures.Doors, center, closed, open, w, h);
                case DoorKind.Diamond: return new DiamondLockedDoorTile(textures.Doors, center, closed, open, w, h);
                case DoorKind.Bombable: return new BombedWallOpeningTile(textures.Doors, center, closed, open, w, h);
                case DoorKind.Shutter: return new ShutterDoorTile(textures.Doors, center, closed, open, w, h);
                default: return new OpenDoorTile(textures.Doors, center, closed, w, h);
            }
        }

        private static Vector2 CellCenter(int col, int row)
        {
            return new Vector2(col * Cell + Cell / 2f, row * Cell + Cell / 2f);
        }

        private static Vector2 DoorCenter(RoomData data, DoorRun run)
        {
            float along = (run.StartIndex + DoorArt.Span / 2f) * Cell;
            switch (run.Side)
            {
                case RoomSide.North: return new Vector2(along, Cell / 2f);
                case RoomSide.South: return new Vector2(along, (data.Rows.Count - 1) * Cell + Cell / 2f);
                case RoomSide.West: return new Vector2(Cell / 2f, along);
                default: return new Vector2((data.Columns - 1) * Cell + Cell / 2f, along);
            }
        }
    }
}
