using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace _3902sprint0
{
	public static class EnemyCreator
	{
		public static Enemy CreateEnemy(

			EnemyType enemyType,
			Texture2D basicRunningTexture,
			Texture2D basicIdleTexture,
			Rectangle movementBounds,
			int health,
			int damage,
			float movementSpeed,
			int size)
		{
			IEnemyAI enemyAI;

			Texture2D runningTexture;
			Texture2D idleTexture;

			switch (enemyType)
			{
				case EnemyType.Basic:
					enemyAI = new BasicEnemyAI();

					runningTexture = basicRunningTexture;

					idleTexture = basicIdleTexture;

					break;
				default:
					enemyAI = new BasicEnemyAI();
					runningTexture = basicRunningTexture;

					idleTexture = basicIdleTexture;
					break;
			}
			Vector2 spawnPosition = GetSpawnBehavior(enemyType, movementBounds);

			return new Enemy(
				enemyAI,
				runningTexture,
				idleTexture,
				spawnPosition,
				size,
				health,
				damage,
				movementSpeed,
				movementBounds
			);



		}
		private static Vector2 GetSpawnBehavior(
			EnemyType enemyType,
			Rectangle movementBounds)
		{
			switch (enemyType)
			{
				case EnemyType.Basic:
					return GetRandomSpawnPosition(movementBounds);
				default:
					return GetRandomSpawnPosition(movementBounds);
			}
		}

		private static Vector2 GetRandomSpawnPosition(Rectangle movementBounds) 
		{
			int x = System.Random.Shared.Next(movementBounds.Left, movementBounds.Right);

			int y = System.Random.Shared.Next(movementBounds.Top, movementBounds.Bottom);

			return new Vector2( x, y );
		}
	}
}
