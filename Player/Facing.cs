// Connor Fulton
using System;
using Microsoft.Xna.Framework;

namespace _3902sprint0
{
    public enum Facing { Down, Up, Left, Right }

    public static class FacingExtensions
    {
        // The dominant axis of a direction.
        public static Facing FromDirection(Vector2 direction, Facing current)
        {
            if (direction == Vector2.Zero)
                return current;

            if (Math.Abs(direction.X) > Math.Abs(direction.Y))
                return direction.X > 0 ? Facing.Right : Facing.Left;

            return direction.Y > 0 ? Facing.Down : Facing.Up;
        }
    }
}