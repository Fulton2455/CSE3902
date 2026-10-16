using _3902sprint0.Environment;
using Microsoft.Xna.Framework;
using System;

namespace _3902sprint0
{
    public static class ChestCreator
    {
        public const float MinimumSpawnDistance = 300f;
        private const int MaxSpawnAttempts = 500;

        public static Vector2 GetSpawnBehavior(
            Rectangle movementBounds,
            Room room,
            Player player,
            int chestWidth,
            int chestHeight)
        {
            float minimumX = movementBounds.Left + chestWidth / 2f;
            float maximumX = movementBounds.Right - chestWidth / 2f;
            float minimumY = movementBounds.Top + chestHeight / 2f;
            float maximumY = movementBounds.Bottom - chestHeight / 2f;

            Vector2 playerCenter = player != null
                ? player.Bounds.Center.ToVector2()
                : Vector2.Zero;

            for (int attempt = 0; attempt < MaxSpawnAttempts; attempt++)
            {
                float x = Random.Shared.NextSingle() *
                    (maximumX - minimumX) + minimumX;

                float y = Random.Shared.NextSingle() *
                    (maximumY - minimumY) + minimumY;

                Vector2 candidate = new Vector2(x, y);

                Rectangle candidateBounds = new Rectangle(
                    (int)(candidate.X - chestWidth / 2f),
                    (int)(candidate.Y - chestHeight / 2f),
                    chestWidth,
                    chestHeight);

                if (player != null &&
                    Vector2.Distance(candidate, playerCenter) < MinimumSpawnDistance)
                {
                    continue;
                }

                if (room != null && IsOverlappingAnyTile(room, candidateBounds))
                {
                    continue;
                }

                return candidate;
            }

            throw new InvalidOperationException(
                "Could not find a valid chest spawn position.");
        }

        private static bool IsOverlappingAnyTile(
            Room room,
            Rectangle bounds)
        {
            foreach (ITile tile in room.Tiles)
            {
                if (tile.Bounds.Intersects(bounds))
                {
                    return true;
                }
            }

            return false;
        }
    }
}