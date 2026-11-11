// Connor Fulton
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using _3902sprint0.Environment.Tiles;
using _3902sprint0.Environment.Doors;

namespace _3902sprint0.Environment.Collision
{

    // Pure collision-resolution rules for tiles

    public class TileCollisionResolver
    {
        private readonly Func<Rectangle, List<ITile>> queryNearby;
        private readonly float tileSize;

        public TileCollisionResolver(Func<Rectangle, List<ITile>> queryNearby, float tileSize)
        {
            this.queryNearby = queryNearby;
            this.tileSize = tileSize;
        }

        public bool IsBlocked(Rectangle bounds, ITile ignore = null)
        {
            foreach (var tile in queryNearby(bounds))
            {
                if (ReferenceEquals(tile, ignore))
                    continue;
                if (tile.IsSolid && tile.Bounds.Intersects(bounds))
                    return true;
            }
            return false;
        }

        public bool CanEnter(Rectangle bounds, Vector2 direction, IKeyHolder keyHolder = null, IEnemyTileInteraction enemyInteraction = null)
        {
            foreach (var tile in queryNearby(bounds))
            {
                if (!tile.Bounds.Intersects(bounds))
                    continue;

                if (EnemyIgnores(tile, enemyInteraction))
                    continue;

                if (!CanPassThrough(tile, direction, keyHolder, enemyInteraction))
                    return false;
            }
            return true;
        }

        public void ApplyHazards(Rectangle bounds, IDamageable target)
        {
            ForEachTouching<IHazard>(bounds, hazard => hazard.ApplyEffect(target));
        }

        public void CheckStairs(Rectangle bounds)
        {
            ForEachTouching<StairsTile>(bounds, stairs => stairs.NotifyTraversed());
        }

        public void Detonate(Rectangle bounds)
        {
            ForEachTouching<BombedWallOpeningTile>(bounds, wall => wall.Detonate());
        }

        // An enemy that has declared it does not interact with this kind of tile walks straight through it.
        private static bool EnemyIgnores(ITile tile, IEnemyTileInteraction enemyInteraction)
        {
            return enemyInteraction != null && !enemyInteraction.InteractWith(tile);
        }

        // Decides whether the mover may occupy a tile it overlaps. Locked doors and pushable blocks
        // have special rules; any other tile just follows its IsSolid flag.
        private bool CanPassThrough(ITile tile, Vector2 direction, IKeyHolder keyHolder, IEnemyTileInteraction enemyInteraction)
        {
            if (tile is LockedDoorTile door && !door.IsUnlocked)
                return TryUnlockDoor(door, keyHolder);

            if (tile is PushableBlockTile block)
                return TryPushBlock(block, direction, enemyInteraction);

            return !tile.IsSolid;
        }

        private static bool TryUnlockDoor(LockedDoorTile door, IKeyHolder keyHolder)
        {
            return keyHolder != null && door.TryUnlock(keyHolder);
        }

        private bool TryPushBlock(PushableBlockTile block, Vector2 direction, IEnemyTileInteraction enemyInteraction)
        {
            if (enemyInteraction != null && !enemyInteraction.CanPush(block))
                return false;

            return block.TryPush(direction, tileSize, target => !IsBlocked(CellBoundsAt(target), block));
        }

        private Rectangle CellBoundsAt(Vector2 center)
        {
            return new Rectangle(
                (int)(center.X - tileSize / 2),
                (int)(center.Y - tileSize / 2),
                (int)tileSize,
                (int)tileSize);
        }

        private void ForEachTouching<T>(Rectangle bounds, Action<T> action) where T : class
        {
            foreach (var tile in queryNearby(bounds))
            {
                if (tile is T match && tile.Bounds.Intersects(bounds))
                    action(match);
            }
        }
    }
}