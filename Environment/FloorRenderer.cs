// Connor Fulton
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3902sprint0.Environment
{
   
    // Draws the room's floor for the current terrain.
   
    public class FloorRenderer
    {
        private const int CellSize = 64;
        private const int AccentChance = 5; // roughly 1 in 5 cells gets the accent variant

        private readonly Texture2D floorSheet;
        private readonly Rectangle bounds;

        public FloorRenderer(Texture2D floorSheet, Rectangle roomBounds)
        {
            this.floorSheet = floorSheet;
            this.bounds = roomBounds;
        }

        public void Draw(SpriteBatch spriteBatch, terrain.terrainState state)
        {
            int column = ColumnFor(state);
            int cols = bounds.Width / CellSize;
            int rows = bounds.Height / CellSize;

            for (int col = 0; col < cols; col++)
            {
                for (int row = 0; row < rows; row++)
                {
                    Rectangle source = new Rectangle(
                        column * CellSize,
                        VariantRow(col, row) * CellSize,
                        CellSize,
                        CellSize);
                    Rectangle destination = new Rectangle(
                        bounds.X + col * CellSize,
                        bounds.Y + row * CellSize,
                        CellSize,
                        CellSize);

                    spriteBatch.Draw(floorSheet, destination, source, Color.White);
                }
            }
        }

        private static int ColumnFor(terrain.terrainState state)
        {
            switch (state)
            {
                case terrain.terrainState.Ice: return 1;
                case terrain.terrainState.Swamp: return 2;
                default: return 0; // Stone
            }
        }

        // Deterministic per-cell pick, so the accents stay put instead of flickering each frame.
        private static int VariantRow(int col, int row)
        {
            return (col * 31 + row * 17) % AccentChance == 0 ? 1 : 0;
        }
    }
}