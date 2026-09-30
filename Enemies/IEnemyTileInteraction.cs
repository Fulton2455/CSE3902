using System;
using Microsoft.Xna.Framework;
using _3902sprint0.Environment.Tiles;
using _3902sprint0.Environment;


namespace _3902sprint0
{
    public interface IEnemyTileInteraction
    {
        bool InteractWith(ITile tile);
        bool CanPush(ITile tile);
    }
}