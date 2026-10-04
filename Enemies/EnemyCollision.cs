using System;
using Microsoft.Xna.Framework;
using _3902sprint0.Environment;


namespace _3902sprint0
{
    public static class EnemyCollision
    {
        public static bool CanEnter(
            Room room,
            Rectangle bounds,
            Vector2 direction,
            IEnemyTileInteraction tileInteraction)
        {
            if (room == null) 
                return true;

            return room.CanEnter(bounds, direction, null, tileInteraction);
        }
    }
}
