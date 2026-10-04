using Microsoft.Xna.Framework;
using System;

namespace _3902sprint0
{
    public class FleeingEnemyAI : IEnemyAI
    {
        private static readonly Random random = new Random();
        private fireballManager fireballManager;


        private bool isPaused;
        private float stateTimer;

        public bool IsPaused
        {
            get
            {
                return isPaused;
            }
        }
        public FleeingEnemyAI(fireballManager fireballManager)
        {
            this.fireballManager = fireballManager;

            isPaused = true;
            stateTimer = GetRandomMoveTime();
        }

        

        private float GetRandomMoveTime()
        {
            return (float)(1.0 + random.NextDouble() * 2.0);
        }

        private void shootFireball(fireballManager fireballManager, Player player, Enemy enemy)
        {
            Vector2 direction = player.getPosition() - enemy.Position;
            Vector2 enemyCenter = new Vector2(enemy.Position.X -40, enemy.Position.Y);
            if (direction != Vector2.Zero)
            {
                direction.Normalize();
                fireballManager.createFireball(2, enemyCenter, direction,true);
            }
        }

        public void Update(Enemy enemy, Player player, GameTime gameTime, float terrainAcceleration)
        {
            if (player == null)
                return;
            float elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            stateTimer -= elapsedSeconds;
            if (stateTimer <= 0)
            {
                ChangeBehavior(enemy, player);
                
            }
            if (!isPaused)
            {
                Vector2 direction = enemy.Position - player.Location;
                if (direction != Vector2.Zero)
                {
                    enemy.MoveInDirection(direction, gameTime, terrainAcceleration);
                }
            }
            else
            {
                enemy.MoveInDirection(Vector2.Zero, gameTime, terrainAcceleration);

            }
        }
        private void ChangeBehavior(Enemy enemy, Player player)
        {
            if (isPaused)
            {
                isPaused = false;
                enemy.SetDirection(GetRandomDirection());
                float distance = Vector2.Distance(player.getPosition(), enemy.Position);
                if (distance <= 950)
                    shootFireball(fireballManager, player, enemy);

                stateTimer = GetRandomMoveTime();
            }
            else
            {
                isPaused = true;
                stateTimer = GetRandomPauseTime();
                
            }

        }
        private Direction GetRandomDirection()
        {
            int directionNumber = random.Next(0, 8);
            return (Direction)directionNumber;
        }
        private float GetRandomPauseTime()
        {

            return (float)(0.5 + random.NextDouble() * 3.0);

        }

    }

}
