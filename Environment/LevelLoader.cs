using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using _3902sprint0.Environment.Tiles;
using _3902sprint0.Environment.Doors;

namespace _3902sprint0.Environment
{
    public class LevelLoader
    {
        public void LoadRoom(string filePath, Room room, Texture2D spriteSheet)
        {
            string[] lines = File.ReadAllLines(filePath);
            
            for (int y = 0; y < lines.Length; y++)
            {
                // No longer split by commas. A string would just be an array of chars
                string row = lines[y];
                
                for (int x = 0; x < row.Length; x++)
                {
                    char tileChar = row[x];
                    
                    // Add + 32 because Tile.cs calculates bounds from the center of the tile
                    Vector2 position = new Vector2(x * 64 + 32, y * 64 + 32); 

                    switch (tileChar)
                    {
                        case '#': // Wall
                            room.AddTile(TileFactory.CreateTile(TileType.Wall, spriteSheet, position));
                            break;
                        case '.': // Floor
                            // Do nothing (empty space)
                            break;
                        case 'B': // SquareBlock
                            room.AddTile(TileFactory.CreateTile(TileType.SquareBlock, spriteSheet, position));
                            break;
                        case 'S': // Statue
                            room.AddTile(TileFactory.CreateTile(TileType.Statue, spriteSheet, position));
                            break;
                        case 'P': // PushableBlock
                            room.AddTile(TileFactory.CreateTile(TileType.PushableBlock, spriteSheet, position));
                            break;
                        case 'F': // Fire
                            room.AddTile(TileFactory.CreateTile(TileType.Fire, spriteSheet, position));
                            break;
                        case 'G': // BlueGap
                            room.AddTile(TileFactory.CreateTile(TileType.BlueGap, spriteSheet, position));
                            break;
                        case 'X': // Stairs
                            room.AddTile(TileFactory.CreateTile(TileType.Stairs, spriteSheet, position));
                            break;
                        case 'D': // OpenDoor
                            room.AddTile(TileFactory.CreateTile(TileType.OpenDoor, spriteSheet, position));
                            break;
                        case 'O': // BombedWallOpening
                            room.AddTile(new BombedWallOpeningTile(spriteSheet, position));
                            break;
                        case 'K': // KeyholeLockedDoor
                            room.AddTile(new KeyholeLockedDoorTile(spriteSheet, position));
                            break;
                        case 'L': // DiamondLockedDoor
                            room.AddTile(new DiamondLockedDoorTile(spriteSheet, position));
                            break;
                    }
                }
            }
        }
    }
}