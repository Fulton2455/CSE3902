// Connor Fulton
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using State = _3902sprint0.PlayerSprite.AnimationState;

namespace _3902sprint0
{
    // The player's sprite-sheet layout 
    public static class PlayerAnimations
    {
        private const int Cell = 32;
        private const float Step = 0.15f;
        private const int DownRow = 0, UpRow = 1, SideRow = 2, SharedRow = 3;

        private static readonly Dictionary<(State, Facing), AnimationClip> clips = Build();

        public static AnimationClip Get(State state, Facing facing)
        {
            return clips[(state, facing)];
        }

        private static Dictionary<(State, Facing), AnimationClip> Build()
        {
            var table = new Dictionary<(State, Facing), AnimationClip>();

            foreach (Facing facing in Enum.GetValues(typeof(Facing)))
            {
                int row = RowFor(facing);
                SpriteEffects effect = facing == Facing.Left
                    ? SpriteEffects.FlipHorizontally
                    : SpriteEffects.None;

                table[(State.Idle, facing)]    = new AnimationClip(Strip(row, 0, 1), Step, true, effect);
                table[(State.Walk, facing)]    = new AnimationClip(Strip(row, 0, 4), Step, true, effect);
                table[(State.Attack, facing)]  = new AnimationClip(Strip(row, 4, 3), Step, false, effect);

                // Single poses: the frame time is how long the pose lasts.
                table[(State.UseItem, facing)] = new AnimationClip(Strip(row, 7, 1), 0.25f, false, effect);
                table[(State.Hurt, facing)]    = new AnimationClip(Strip(row, 8, 1), 0.25f, false, effect);
                table[(State.Pickup, facing)]  = new AnimationClip(Strip(SharedRow, 0, 1), 1.0f, false, SpriteEffects.None);
                table[(State.Death, facing)]   = new AnimationClip(Strip(SharedRow, 1, 4), Step, false, SpriteEffects.None);
            }

            return table;
        }

        private static int RowFor(Facing facing)
        {
            switch (facing)
            {
                case Facing.Up: return UpRow;
                case Facing.Down: return DownRow;
                default: return SideRow;
            }
        }

        private static Rectangle[] Strip(int row, int firstColumn, int count)
        {
            var frames = new Rectangle[count];
            for (int i = 0; i < count; i++)
                frames[i] = new Rectangle((firstColumn + i) * Cell, row * Cell, Cell, Cell);
            return frames;
        }
    }
}