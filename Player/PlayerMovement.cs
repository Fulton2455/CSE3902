using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using _3902sprint0.Environment;

namespace _3902sprint0
{
    public class PlayerMovement
    {
        private readonly Player p;

        public PlayerMovement(Player player)
        {
            p = player;
        }

        public float GetTerrainAcceleration()
        {
            p.Terrain.getTerainEffect(p);
            return p.Terrain.Acceleration;
        }

        public void Move(GameTime gameTime, Vector2 movementDirection)
        {
            float deltaTime =
                (float)gameTime.ElapsedGameTime.TotalSeconds;
            float acceleration = p.GetTerrainAcceleration();
            if (movementDirection != Vector2.Zero)
            {
                p.Velocity += movementDirection * acceleration * deltaTime;
                if (p.Velocity.Length() > p.Speed)
                {
                    p.Velocity.Normalize();
                    p.Velocity *= p.Speed;
                }

            }
            else
            {
                if (p.Velocity.Length() > 0)
                {
                    p.Velocity -= Vector2.Normalize(p.Velocity) * acceleration * deltaTime;
                    if (p.Velocity.Length() < acceleration * deltaTime)

                        p.Velocity = Vector2.Zero;
                }
            }

            Vector2 delta = p.Velocity * deltaTime;
            p.MoveAxis(new Vector2(delta.X, 0));
            p.MoveAxis(new Vector2(0, delta.Y));

            if (p.currentRoom != null)
            {
                p.currentRoom.ApplyHazards(p.Bounds, p);
                p.currentRoom.CheckStairs(p.Bounds);
            }
        }

        public void MoveAxis(Vector2 delta)
        {
            if (delta == Vector2.Zero)
                return;

            Vector2 attemptedLocation = p.Location + delta;
            Rectangle attemptedBounds = p.Bounds;
            attemptedBounds = new Rectangle((int)attemptedLocation.X + 32, (int)attemptedLocation.Y + 16, Player.Width - 32 - 32, Player.Height - 16);
            if (p.currentRoom == null || p.currentRoom.CanEnter(attemptedBounds, Vector2.Normalize(delta), p.inventory))
            {
                p.Location = attemptedLocation;
            }
            else
            {
                if (delta.X != 0) p.Velocity.X = 0;
                if (delta.Y != 0) p.Velocity.Y = 0;
            }
        }

    }
}