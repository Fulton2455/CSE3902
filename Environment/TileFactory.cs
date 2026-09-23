// Connor Fulton
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using _3902sprint0.Environment.Tiles;
using _3902sprint0.Environment.Doors;

namespace _3902sprint0.Environment
{
   
    // Creates simple tiles. Locked doors and bombed-wall openings need extra
    // parameters so construct those directly.
    
     public static class TileFactory
    {
        public static ITile CreateTile(TileType type, Texture2D spriteSheet, Vector2 position, int width = 64, int height = 64)
        {
            Rectangle sourceRect = TileSpriteSheet.GetSourceRect(type);

            switch (type)
            {
                case TileType.Statue:
                case TileType.SquareBlock:
                case TileType.Wall:
                case TileType.BlueGap:
                    return new StaticTile(spriteSheet, position, isSolid: true, sourceRect, width, height);

                case TileType.PushableBlock:
                    return new PushableBlockTile(spriteSheet, position, sourceRect, width, height);

                case TileType.Fire:
                    return new FireTile(spriteSheet, position, sourceRect, width: width, height: height);

                case TileType.Stairs:
                    return new StairsTile(spriteSheet, position, sourceRect, width, height);

                case TileType.OpenDoor:
                    return new OpenDoorTile(spriteSheet, position, sourceRect, width, height);

                default:
                    throw new ArgumentException(
                        $"{type} needs extra parameters — construct it directly (KeyholeLockedDoorTile, DiamondLockedDoorTile, BombedWallOpeningTile).");
            }
        }
    }
}