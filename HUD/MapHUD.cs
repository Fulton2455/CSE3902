// Erik Zhang
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using _3902sprint0.Environment.Rooms;

namespace _3902sprint0
{
    /// <summary>
    /// Draws the dungeon minimap in the top-right corner of the screen.
    /// Without the map item only rooms the player has visited are shown.
    /// With the map item every reachable room is shown.
    /// With the compass the room holding the diamond key gets a red dot.
    /// The player's current room blinks green.
    /// </summary>
    public class MapHUD
    {
        private const int CellSize = 28;
        private const int Gap = 6;                  // space between cells; doors are drawn across it
        private const int Pitch = CellSize + Gap;
        private const int Padding = 10;             // panel border around the grid
        private const int DoorThickness = 6;

        private static readonly Color PanelColor = new Color(0, 0, 0, 150);
        private static readonly Color VisitedColor = new Color(90, 140, 220);
        private static readonly Color UnvisitedColor = new Color(70, 70, 100);
        private static readonly Color DoorColor = new Color(170, 170, 190);
        private static readonly Color PlayerColor = Color.LimeGreen;
        private static readonly Color TreasureColor = Color.Red;

        private readonly DungeonMap dungeon;
        private readonly DungeonLayout layout;
        private readonly RoomVisitTracker tracker;
        private readonly Texture2D pixel;
        private readonly string treasureRoomId;     // null if the dungeon has no diamond key
        private readonly Rectangle panel;
        private readonly Point origin;

        /// <param name="topRight">Screen position of the panel's top-right corner.</param>
        public MapHUD(DungeonMap dungeon, DungeonLayout layout, RoomVisitTracker tracker,
            Texture2D whitePixel, Point topRight, string treasureRoomId)
        {
            this.dungeon = dungeon;
            this.layout = layout;
            this.tracker = tracker;
            this.pixel = whitePixel;
            this.treasureRoomId = treasureRoomId;

            int gridWidth = layout.Width * Pitch - Gap;
            int gridHeight = layout.Height * Pitch - Gap;
            panel = new Rectangle(
                topRight.X - gridWidth - Padding * 2,
                topRight.Y,
                gridWidth + Padding * 2,
                gridHeight + Padding * 2);
            origin = new Point(panel.X + Padding, panel.Y + Padding);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, bool hasMap, bool hasCompass)
        {
            spriteBatch.Draw(pixel, panel, PanelColor);

            foreach (RoomData room in dungeon.Rooms)
            {
                if (!layout.TryGetCell(room.Id, out Point cell)) continue;

                bool visited = tracker.HasVisited(room.Id);
                if (!hasMap && !visited) continue;

                Rectangle r = CellRect(cell);
                spriteBatch.Draw(pixel, r, visited ? VisitedColor : UnvisitedColor);

                foreach (var link in room.Links)
                    if (link.Value != null) DrawDoor(spriteBatch, r, link.Key);
            }

            if (hasCompass && treasureRoomId != null && layout.TryGetCell(treasureRoomId, out Point t))
                DrawMarker(spriteBatch, CellRect(t), TreasureColor, 10);

            // Blink the player's room so it stands out.
            bool on = (int)(gameTime.TotalGameTime.TotalSeconds * 4) % 2 == 0;
            if (on && tracker.CurrentRoomId != null && layout.TryGetCell(tracker.CurrentRoomId, out Point c))
                DrawMarker(spriteBatch, CellRect(c), PlayerColor, 14);
        }

        private Rectangle CellRect(Point cell)
        {
            return new Rectangle(
                origin.X + cell.X * Pitch,
                origin.Y + cell.Y * Pitch,
                CellSize, CellSize);
        }

        // A short bar across the gap on the side the link leaves from.
        private void DrawDoor(SpriteBatch spriteBatch, Rectangle r, RoomSide side)
        {
            int mid = (CellSize - DoorThickness) / 2;
            Rectangle d;
            switch (side)
            {
                case RoomSide.North: d = new Rectangle(r.X + mid, r.Y - Gap, DoorThickness, Gap); break;
                case RoomSide.South: d = new Rectangle(r.X + mid, r.Bottom, DoorThickness, Gap); break;
                case RoomSide.West: d = new Rectangle(r.X - Gap, r.Y + mid, Gap, DoorThickness); break;
                default: d = new Rectangle(r.Right, r.Y + mid, Gap, DoorThickness); break;
            }
            spriteBatch.Draw(pixel, d, DoorColor);
        }

        private void DrawMarker(SpriteBatch spriteBatch, Rectangle cell, Color color, int size)
        {
            var m = new Rectangle(
                cell.X + (CellSize - size) / 2,
                cell.Y + (CellSize - size) / 2,
                size, size);
            spriteBatch.Draw(pixel, m, color);
        }
    }
}
