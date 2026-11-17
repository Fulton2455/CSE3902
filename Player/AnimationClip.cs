// Connor Fulton
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3902sprint0
{
    public sealed class AnimationClip
    {
        public Rectangle[] Frames { get; }
        public float FrameSeconds { get; }
        public bool Loops { get; }
        public SpriteEffects Effect { get; }

        public AnimationClip(Rectangle[] frames, float frameSeconds, bool loops, SpriteEffects effect)
        {
            Frames = frames;
            FrameSeconds = frameSeconds;
            Loops = loops;
            Effect = effect;
        }
    }
}