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
                string[] columns = lines[y].Split(',');
                
                for (int x = 0; x < columns.Length; x++)
                {
                    if (string.IsNullOrWhiteSpace(columns[x])) continue;

                    int objectID = int.Parse(columns[x].Trim());
                    
                    // Add + 32 because your Tile.cs calculates bounds from the CENTER of the tile!
                    Vector2 position = new Vector2(x * 64 + 32, y * 64 + 32); 

                    switch (objectID)
                    {
                        case 1:
                            room.AddTile(TileFactory.CreateTile(TileType.Wall, spriteSheet, position));
                            break;
                        case 2:
                            // Floor: Do nothing (empty space)
                            break;
                        case 3:
                            room.AddTile(TileFactory.CreateTile(TileType.SquareBlock, spriteSheet, position));
                            break;
                        case 4:
                            room.AddTile(TileFactory.CreateTile(TileType.Statue, spriteSheet, position));
                            break;
                        case 5:
                            room.AddTile(TileFactory.CreateTile(TileType.PushableBlock, spriteSheet, position));
                            break;
                        case 6:
                            room.AddTile(TileFactory.CreateTile(TileType.Fire, spriteSheet, position));
                            break;
                        case 7:
                            room.AddTile(TileFactory.CreateTile(TileType.BlueGap, spriteSheet, position));
                            break;
                        case 8:
                            room.AddTile(TileFactory.CreateTile(TileType.Stairs, spriteSheet, position));
                            break;
                        case 9:
                            room.AddTile(TileFactory.CreateTile(TileType.OpenDoor, spriteSheet, position));
                            break;
                        case 10:
                            room.AddTile(new BombedWallOpeningTile(spriteSheet, position));
                            break;
                        case 11:
                            room.AddTile(new KeyholeLockedDoorTile(spriteSheet, position));
                            break;
                        case 12:
                            room.AddTile(new DiamondLockedDoorTile(spriteSheet, position));
                            break;
                    }
                }
            }
        }
    }
}