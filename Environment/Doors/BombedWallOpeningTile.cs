// Connor Fulton

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3902sprint0.Environment.Doors
{
   
    // A wall segment that looks solid until Detonate() is called.
    // Then it becomes a walkable opening.
    
     public class BombedWallOpeningTile : Tile
    {
        private readonly Rectangle closedSource;
        private readonly Rectangle openSource;

        public bool IsOpened { get; private set; }
        public override bool IsSolid => !IsOpened;

        public BombedWallOpeningTile(Texture2D spriteSheet, Vector2 position, int width = 64, int height = 64)
            : base(spriteSheet, position, TileSpriteSheet.GetSourceRect(TileType.BombedWallOpening), width, height)
        {
            closedSource = TileSpriteSheet.GetSourceRect(TileType.BombedWallOpening);
            openSource = TileSpriteSheet.GetSourceRect(TileSpriteSheet.BombedWallOpenCell);
        }

        public void Detonate()
        {
            if (IsOpened)
                return;

            IsOpened = true;
            sourceRectangle = openSource;
        }
    }
}