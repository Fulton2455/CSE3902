// Connor Fulton
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using _3902sprint0.Environment.Tiles;
using _3902sprint0.Environment.Doors;

namespace _3902sprint0.Environment.Collision
{
    
    // Pure collision-resolution rules for tiles: blocking, pushing, door unlocking,
    // hazards, stairs, detonation. Room owns the tiles and
    // the QuadTree and delegates resolution here.
   
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

                if (enemyInteraction != null && !enemyInteraction.InteractWith(tile))
                    continue;

                if (tile is LockedDoorTile door && !door.IsUnlocked)
                {
                    if (keyHolder != null && door.TryUnlock(keyHolder))
                        continue;
                    return false;
                }

                if (tile is PushableBlockTile pushable)
                {
                    if (enemyInteraction != null && !enemyInteraction.CanPush(pushable))
                        return false;

                    bool moved = pushable.TryPush(direction, tileSize, target =>
                    {
                        Rectangle targetBounds = new Rectangle(
                            (int)(target.X - tileSize / 2),
                            (int)(target.Y - tileSize / 2),
                            (int)tileSize,
                            (int)tileSize);
                        return !IsBlocked(targetBounds, pushable);
                    });

                    if (moved)
                        continue;
                    return false;
                }

                if (tile.IsSolid)
                    return false;
            }
            return true;
        }

        public void ApplyHazards(Rectangle bounds, IDamageable target)
        {
            foreach (var tile in queryNearby(bounds))
            {
                if (tile is IHazard hazard && tile.Bounds.Intersects(bounds))
                    hazard.ApplyEffect(target);
            }
        }

        public void CheckStairs(Rectangle bounds)
        {
            foreach (var tile in queryNearby(bounds))
            {
                if (tile is StairsTile stairs && stairs.Bounds.Intersects(bounds))
                    stairs.NotifyTraversed();
            }
        }

        public void Detonate(Rectangle bounds)
        {
            foreach (var tile in queryNearby(bounds))
            {
                if (tile is BombedWallOpeningTile bombed && bombed.Bounds.Intersects(bounds))
                    bombed.Detonate();
            }
        }
    }
}