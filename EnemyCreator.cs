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
			Rectangle movementBounds
			)
		{
			IEnemyAI enemyAI;
			int health;
			int damage;
			int size;
			float movementSpeed;
			Texture2D runningTexture;
			Texture2D idleTexture;

			switch (enemyType)
			{
				case EnemyType.Basic:
					enemyAI = new BasicEnemyAI();
					health = 2;
					damage = 1;
					movementSpeed = 100f;
					size = 64;
                    runningTexture = basicRunningTexture;

					idleTexture = basicIdleTexture;

					break;
				case EnemyType.Chaser:
					enemyAI = new ChaserEnemyAI();
					health = 1;
					damage = 2;
					movementSpeed = 150f;
					size = 48;
                    runningTexture = basicRunningTexture;

                    idleTexture = basicIdleTexture;
					break;
				case EnemyType.Fleeing:
					enemyAI = new FleeingEnemyAI();
                    health = 1;
                    damage = 3;
                    movementSpeed = 190f;
                    size = 80;
                    runningTexture = basicRunningTexture;

                    idleTexture = basicIdleTexture;
					break;
				case EnemyType.Erratic:
					enemyAI= new ErraticEnemyAI();
                    health = 3;
                    damage = 3;
                    movementSpeed = 125f;
                    size = 32;
                    runningTexture = basicRunningTexture;

                    idleTexture = basicIdleTexture;
					break;
                default:
					enemyAI = new BasicEnemyAI();
                    health = 1;
                    damage = 1;
                    movementSpeed = 100f;
                    size = 64;
                    runningTexture = basicRunningTexture;

					idleTexture = basicIdleTexture;
					break;
			}
			Vector2 spawnPosition = GetSpawnBehavior(enemyType, movementBounds);

			return new Enemy(
				enemyType,
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
				case EnemyType.Chaser:
					return GetRandomSpawnPosition(movementBounds); //will change this to spawn at walls and important objects eventually
				case EnemyType.Fleeing:
					return GetRandomSpawnPosition(movementBounds);
				case EnemyType.Erratic:
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
