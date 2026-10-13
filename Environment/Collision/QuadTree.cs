// Connor Fulton
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace _3902sprint0.Environment.Collision
{
   
    
    // type works for tiles today and entities later without needing a shared interface.
    
    public class QuadTree<T>
    {
        private const int DefaultCapacity = 8;
        private const int MaxDepth = 6;

        private readonly Rectangle boundary;
        private readonly int capacity;
        private readonly int depth;
        private readonly Func<T, Rectangle> getBounds;

        private readonly List<T> items = new List<T>();
        private QuadTree<T>[] children;

        public QuadTree(Rectangle boundary, Func<T, Rectangle> getBounds, int capacity = DefaultCapacity, int depth = 0)
        {
            this.boundary = boundary;
            this.getBounds = getBounds;
            this.capacity = capacity;
            this.depth = depth;
        }

        //Inserts an item. Items outside the root boundary entirely are silently dropped.
        public void Insert(T item)
        {
            Rectangle itemBounds = getBounds(item);
            if (!boundary.Intersects(itemBounds))
                return;

            if (children != null)
            {
                QuadTree<T> target = ChildFor(itemBounds);
                if (target != null)
                {
                    target.Insert(item);
                    return;
                }
                // Is in more than one child quadrant keep it at this level
                // rather than duplicating it into several children.
                items.Add(item);
                return;
            }

            items.Add(item);

            if (items.Count > capacity && depth < MaxDepth)
                Subdivide();
        }

        //Appends every item whose bounds might overlap range. 
        public void Query(Rectangle range, List<T> results)
        {
            if (!boundary.Intersects(range))
                return;

            foreach (var item in items)
            {
                if (getBounds(item).Intersects(range))
                    results.Add(item);
            }

            if (children != null)
            {
                foreach (var child in children)
                    child.Query(range, results);
            }
        }

        private void Subdivide()
        {
            int halfWidth = boundary.Width / 2;
            int halfHeight = boundary.Height / 2;
            int x = boundary.X;
            int y = boundary.Y;

            children = new[]
            {
                new QuadTree<T>(new Rectangle(x, y, halfWidth, halfHeight), getBounds, capacity, depth + 1),
                new QuadTree<T>(new Rectangle(x + halfWidth, y, boundary.Width - halfWidth, halfHeight), getBounds, capacity, depth + 1),
                new QuadTree<T>(new Rectangle(x, y + halfHeight, halfWidth, boundary.Height - halfHeight), getBounds, capacity, depth + 1),
                new QuadTree<T>(new Rectangle(x + halfWidth, y + halfHeight, boundary.Width - halfWidth, boundary.Height - halfHeight), getBounds, capacity, depth + 1),
            };

            List<T> remaining = new List<T>();
            foreach (var item in items)
            {
                Rectangle itemBounds = getBounds(item);
                QuadTree<T> target = ChildFor(itemBounds);
                if (target != null)
                    target.Insert(item);
                else
                    remaining.Add(item);
            }

            items.Clear();
            items.AddRange(remaining);
        }

        private QuadTree<T> ChildFor(Rectangle itemBounds)
        {
            foreach (var child in children)
            {
                if (child.boundary.Contains(itemBounds))
                    return child;
            }
            return null;
        }
    }
}