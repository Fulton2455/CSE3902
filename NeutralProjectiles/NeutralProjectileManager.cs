using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using _3902sprint0.Environment.Rooms;

namespace _3902sprint0.NeutralProjectiles
{
    /// <summary>Manages neutral candle-flame projectiles created by the player.</summary>
    public sealed class NeutralProjectileManager
    {
        private sealed class Flame
        {
            public Vector2 Position;
            public Vector2 Velocity;
            public float Remaining;
            public float AnimationTimer;
            public int Frame;
        }

        private readonly Player player;
        private readonly RoomManager rooms;
        private readonly Texture2D fireballSpriteSheet;
        private readonly List<Flame> flames = new List<Flame>();
        private bool previousCandle;

      
        private const int FrameWidth = 64;
        private const int FrameHeight = 16;
        private const int FrameCount = 15;
        private const float FrameDuration = 0.075f;
        private const float FlameSpeed = 420f;
        private const float FlameLifetime = 0.85f;

        public NeutralProjectileManager(Player player, RoomManager rooms, Texture2D fireballSpriteSheet)
        {
            this.player = player;
            this.rooms = rooms;
            this.fireballSpriteSheet = fireballSpriteSheet;
        }

        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            KeyboardState keys = Keyboard.GetState();
            bool candle = keys.IsKeyDown(Keys.C);

            if (candle && !previousCandle)
                //FireCandle();
            previousCandle = candle;

            for (int i = flames.Count - 1; i >= 0; i--)
            {
                Flame flame = flames[i];
                flame.Remaining -= dt;
                flame.AnimationTimer += dt;

                while (flame.AnimationTimer >= FrameDuration)
                {
                    flame.AnimationTimer -= FrameDuration;
                    flame.Frame = (flame.Frame + 1) % FrameCount;
                }

                Vector2 next = flame.Position + flame.Velocity * dt;
                Rectangle nextBounds = new Rectangle((int)next.X - 10, (int)next.Y - 10, 20, 20);
                if (flame.Remaining <= 0 || rooms.CurrentRoom.IsBlocked(nextBounds))
                    flames.RemoveAt(i);
                else
                    flame.Position = next;
            }
        }

        private void FireCandle()
        {
            Vector2 direction = player.AimDirection;
            if (direction.LengthSquared() < 0.001f)
                direction = Vector2.UnitX;
            direction.Normalize();

            flames.Add(new Flame
            {
                Position = player.getPosition() + direction * 55f,
                Velocity = direction * FlameSpeed,
                Remaining = FlameLifetime
            });
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Flame flame in flames)
            {
                Rectangle source = new Rectangle(flame.Frame * FrameWidth, 0, FrameWidth, FrameHeight);
                Vector2 origin = new Vector2(FrameWidth / 2f, FrameHeight / 2f);
                spriteBatch.Draw(
                    fireballSpriteSheet,
                    flame.Position,
                    source,
                    Color.White,
                    (float)System.Math.Atan2(flame.Velocity.Y, flame.Velocity.X),
                    origin,
                    new Vector2(0.75f, 1.5f),
                    SpriteEffects.None,
                    0f);
            }
        }
    }
}
