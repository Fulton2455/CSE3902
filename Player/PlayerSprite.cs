// Connor Fulton
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3902sprint0
{
    // Draws the player and plays the animation clip for the current state and facing.
    public class PlayerSprite : ISprite
    {
        public enum AnimationState { Idle, Walk, Attack, UseItem, Pickup, Hurt, Death }

        private const int DrawSize = 128;

        private Texture2D texture;
        private Vector2 location;
        private AnimationState state = AnimationState.Idle;
        private Facing facing = Facing.Down;
        private AnimationClip clip;
        private int frame;
        private float timer;
        private bool finished;

        public Color Tint { get; set; } = Color.White;

        public void Initialize(Texture2D texture)
        {
            this.texture = texture;
            location = Vector2.Zero;
            clip = PlayerAnimations.Get(state, facing);
        }

        public void SetAnimation(AnimationState animation)
        {
            if (state == animation)
                return;

            state = animation;
            clip = PlayerAnimations.Get(state, facing);
            frame = 0;
            timer = 0f;
            finished = false;
        }

        // Switching facing keeps the current frame so a swing doesn't restart.
        public void SetFacing(Facing newFacing)
        {
            if (facing == newFacing)
                return;

            facing = newFacing;
            clip = PlayerAnimations.Get(state, facing);
        }

        public void UpdateSprite(GameTime gameTime)
        {
            if (finished)
                return;

            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            while (timer >= clip.FrameSeconds)
            {
                timer -= clip.FrameSeconds;
                frame++;

                if (frame < clip.Frames.Length)
                    continue;

                if (clip.Loops)
                {
                    frame = 0;
                }
                else
                {
                    frame = clip.Frames.Length - 1;
                    finished = true;
                    break;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var destination = new Rectangle((int)location.X, (int)location.Y, DrawSize, DrawSize);
            spriteBatch.Draw(texture, destination, GetSourceRectangle(), Tint, 0f, Vector2.Zero, clip.Effect, 0f);
        }

        public Rectangle GetSourceRectangle() => clip.Frames[frame];

        public void SetLocation(Vector2 newLocation) => location = newLocation;

        public bool IsAnimation(AnimationState animation) => state == animation;

        public bool IsAnimationFinished() => finished;
    }
}