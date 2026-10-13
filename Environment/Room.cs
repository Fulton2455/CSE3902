// Connor Fulton

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using _3902sprint0.Environment.Tiles;
using _3902sprint0.Environment.Doors;
using _3902sprint0.Environment.Collision;

namespace _3902sprint0.Environment
{

    // Owns a room's tiles. Updates/draws them, resolves movement collision
    // including pushing blocks and unlocking doors, and applies hazards.
    // Collision queries are broad-phased through a QuadTree rebuilt once per
    // frame, so Player/Enemy/Fireball movement checks don't each walk every tile.

    public class Room
    {
        private readonly List<ITile> tiles = new List<ITile>();
        private readonly float tileSize;
        private readonly Rectangle worldBounds;
        private QuadTree<ITile> quadTree;

        public IReadOnlyList<ITile> Tiles => tiles;

        public Room(float tileSize, Rectangle worldBounds)
        {
            this.tileSize = tileSize;
            this.worldBounds = worldBounds;
        }

        public void AddTile(ITile tile) => tiles.Add(tile);

        public void Update(GameTime gameTime)
        {
            foreach (var tile in tiles)
                tile.Update(gameTime);

            // Rebuilt once per frame, before movers (Player/Enemy/Fireball) call
            // CanEnter below. Cheap at this tile count and avoids having to track
            // pushable-block moves or door/wall state changes incrementally.
            RebuildQuadTree();
        }

        private void RebuildQuadTree()
        {
            quadTree = new QuadTree<ITile>(worldBounds, t => t.Bounds);
            foreach (var tile in tiles)
                quadTree.Insert(tile);
        }

        private List<ITile> QueryNearby(Rectangle bounds)
        {
            if (quadTree == null)
                RebuildQuadTree();

            var results = new List<ITile>();
            quadTree.Query(bounds, results);
            return results;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var tile in tiles)
                tile.Draw(spriteBatch);
        }

        // True if any solid tile other than ignore overlaps bounds.
        public bool IsBlocked(Rectangle bounds, ITile ignore = null)
        {
            foreach (var tile in QueryNearby(bounds))
            {
                if (ReferenceEquals(tile, ignore))
                    continue;
                if (tile.IsSolid && tile.Bounds.Intersects(bounds))
                    return true;
            }
            return false;
        }


        // Checks whether an entity moving in `direction` can occupy `bounds`.
        // Pushable blocks in the way are pushed if there's room. Locked doors are
        // unlocked automatically if keyHolder has the right key.

        //NEEDS TO BE REFACTORED FOR FEWER LINES. BROKEN INTO MULTIPLE FUNCTIONS

        public bool CanEnter(Rectangle bounds, Vector2 direction, IKeyHolder keyHolder = null, IEnemyTileInteraction enemyInteraction = null)
        {
            foreach (var tile in QueryNearby(bounds))
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
                    {
                        return false;
                    }
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

        // Applies any hazard tiles overlapping bounds to the given target.
        public void ApplyHazards(Rectangle bounds, IDamageable target)
        {
            foreach (var tile in QueryNearby(bounds))
            {
                if (tile is IHazard hazard && tile.Bounds.Intersects(bounds))
                    hazard.ApplyEffect(target);
            }
        }

        // Fires Traversed on any StairsTile the given bounds overlaps.
        public void CheckStairs(Rectangle bounds)
        {
            foreach (var tile in QueryNearby(bounds))
            {
                if (tile is StairsTile stairs && stairs.Bounds.Intersects(bounds))
                    stairs.NotifyTraversed();
            }
        }

        // Detonates the first BombedWallOpeningTile overlapping bounds.
        public void Detonate(Rectangle bounds)
        {
            foreach (var tile in QueryNearby(bounds))
            {
                if (tile is BombedWallOpeningTile bombed && bombed.Bounds.Intersects(bounds))
                    bombed.Detonate();
            }
        }
    }
}