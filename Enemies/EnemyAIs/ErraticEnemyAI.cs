using System;
using Microsoft.Xna.Framework;

namespace _3902sprint0
{
    public class ErraticEnemyAI : IEnemyAI
    {
        private static readonly Random random = new Random();
        private Direction direction;
        private float directionTime;

        private bool isPaused;
        private float stateTimer;

        public bool IsPaused
        {
            get
            {
                return false;
            }
        }
        public ErraticEnemyAI()
        {
            direction = GetRandomDirection();
            stateTimer = GetRandomMoveTime();
        }

        private float GetRandomMoveTime()
        {
            return (float)(.25 + random.NextDouble() * .75);
        }

        public void Update(Enemy enemy, Player player, GameTime gameTime, float terrainAcceleration)
        {
            float elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            stateTimer -= elapsedSeconds;
            if (stateTimer <= 0)
            {
                direction = GetRandomDirection();
                stateTimer = GetRandomMoveTime();
            }
            enemy.SetDirection(direction);
            enemy.Move(gameTime, terrainAcceleration);
        }
        
        private Direction GetRandomDirection()
        {
            
            return (Direction)random.Next(0, 8);
        }
        

    }

}
