// Connor Fulton

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using _3902sprint0.Environment.Collision;

namespace _3902sprint0.Environment
{

    // Owns a room's tiles and their spatial index (QuadTree, rebuilt once per frame).
    // Collision rules themselves live in TileCollisionResolver - this class just
    // supplies it with tile storage and a nearby-tile query.

    public class Room
    {
        private readonly List<ITile> tiles = new List<ITile>();
        private readonly Rectangle worldBounds;
        private QuadTree<ITile> quadTree;
        private readonly TileCollisionResolver collision;

        public IReadOnlyList<ITile> Tiles => tiles;

        public Room(float tileSize, Rectangle worldBounds)
        {
            this.worldBounds = worldBounds;
            collision = new TileCollisionResolver(QueryNearby, tileSize);
        }

        public void AddTile(ITile tile) => tiles.Add(tile);

        public void Update(GameTime gameTime)
        {
            foreach (var tile in tiles)
                tile.Update(gameTime);

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

        public bool IsBlocked(Rectangle bounds, ITile ignore = null) => collision.IsBlocked(bounds, ignore);
        public bool CanEnter(Rectangle bounds, Vector2 direction, IKeyHolder keyHolder = null, IEnemyTileInteraction enemyInteraction = null)
            => collision.CanEnter(bounds, direction, keyHolder, enemyInteraction);
        public void ApplyHazards(Rectangle bounds, IDamageable target) => collision.ApplyHazards(bounds, target);
        public void CheckStairs(Rectangle bounds) => collision.CheckStairs(bounds);
        public void Detonate(Rectangle bounds) => collision.Detonate(bounds);
    }
}