using System;

using Microsoft.Xna.Framework;

namespace _3902sprint0
{
	public class BasicEnemyAI : IEnemyAI
	{
		private static readonly Random random = new Random();

		private bool isPaused;
		private float stateTimer;

		public bool IsPaused
		{
			get
			{
				return isPaused;
			}
		}
		public BasicEnemyAI()
		{
			isPaused = true;
			stateTimer = GetRandomMoveTime();
		}

		private float GetRandomMoveTime()
		{
			return (float)(1.0 + random.NextDouble() * 3.0);
		}

		public void Update(Enemy enemy, Player player, GameTime gameTime, float terrainAcceleration)
		{
			float elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

			stateTimer -= elapsedSeconds;
			if (stateTimer <= 0)
			{
				ChangeBehavior(enemy);
			}
			if (!isPaused)
			{
				enemy.Move(gameTime, terrainAcceleration);
			}
			else
			{
				enemy.MoveInDirection(Vector2.Zero, gameTime, terrainAcceleration);
			}
		}
		private void ChangeBehavior(Enemy enemy)
		{
			if (isPaused)
			{
				isPaused = false;
				enemy.SetDirection(GetRandomDirection());

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
            
			return (float)(0.5 + random.NextDouble() * 2.0);
            
        }

	}
}
