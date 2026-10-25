// Connor Fulton
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using _3902sprint0.Environment;

namespace _3902sprint0
{
   
    // Builds a Room for the current screen size
    
    public static class RoomLoader
    {
        private const int TileSize = 64;

        public static Rectangle GetRoomBounds(int screenWidth, int screenHeight)
        {
            int cols = screenWidth / TileSize;
            int rows = screenHeight / TileSize;
            return new Rectangle(0, 0, cols * TileSize, rows * TileSize);
        }

        public static Room LoadRoom(Rectangle roomBounds, Texture2D environmentSpriteSheet)
        {
            Room room = new Room(tileSize: TileSize, worldBounds: roomBounds);
            LevelLoader loader = new LevelLoader();

            try
            {
                loader.LoadRoom("exampleRoom.txt", room, environmentSpriteSheet);
            }
            catch (Exception)
            {
                try
                {
                    loader.LoadRoom("Content/rooms/exampleRoom.txt", room, environmentSpriteSheet);
                }
                catch (Exception)
                {
                    GenerateFallbackRoom(room, environmentSpriteSheet, roomBounds);
                }
            }

            return room;
        }

        private static void GenerateFallbackRoom(Room room, Texture2D envSheet, Rectangle bounds)
        {
            int cols = bounds.Width / TileSize;
            int rows = bounds.Height / TileSize;

            for (int col = 0; col < cols; col++)
            {
                room.AddTile(TileFactory.CreateTile(TileType.Wall, envSheet, GridPosition(col, 0)));
                room.AddTile(TileFactory.CreateTile(TileType.Wall, envSheet, GridPosition(col, rows - 1)));
            }
            for (int row = 0; row < rows; row++)
            {
                room.AddTile(TileFactory.CreateTile(TileType.Wall, envSheet, GridPosition(0, row)));
                room.AddTile(TileFactory.CreateTile(TileType.Wall, envSheet, GridPosition(cols - 1, row)));
            }
        }

        private static Vector2 GridPosition(int col, int row) => new Vector2(32 + col * TileSize, 32 + row * TileSize);
    }
}