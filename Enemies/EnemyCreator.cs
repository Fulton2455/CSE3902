using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using _3902sprint0.Environment;
using System;
namespace _3902sprint0
{
	public static class EnemyCreator
	{
        public const float MinimumSpawnDistance = 300f;
        private const int MaxSpawnAttempts = 500;
        public static Enemy CreateEnemy(

			EnemyType enemyType,
            Texture2D slimeTexture,
            Texture2D zombieTexture,
            Texture2D mageTexture,
            Texture2D batTexture,
            Rectangle movementBounds,
			Room room,
			Player player,
            fireballManager fireballManager)
		{
			IEnemyAI enemyAI;
			IEnemyTileInteraction tileInteraction;
			int health;
			int damage;
			int size;
			float movementSpeed;
			float acceleration;
			Texture2D spriteSheet;

			switch (enemyType)
			{
				case EnemyType.Basic:
					enemyAI = new BasicEnemyAI();
					tileInteraction = new GroundEnemyTileInteraction();
					health = 2;
					damage = 1;
					movementSpeed = 100f;
					acceleration = 500f;
					size = 64;
					spriteSheet = slimeTexture;

					break;
				case EnemyType.Chaser:
					enemyAI = new ChaserEnemyAI();
                    tileInteraction = new GroundEnemyTileInteraction();
                    health = 1;
					damage = 2;
					movementSpeed = 150f;
					acceleration = 600f;
					size = 48;
                    spriteSheet = zombieTexture;
					break;
				case EnemyType.Fleeing:
					enemyAI = new FleeingEnemyAI(fireballManager);
                    tileInteraction = new GroundEnemyTileInteraction();
                    health = 1;
                    damage = 3;
                    movementSpeed = 190f;
					acceleration = 700f;
                    size = 80;
					spriteSheet = mageTexture;
					break;
				case EnemyType.Erratic:
					enemyAI= new ErraticEnemyAI();
                    tileInteraction = new FlyingEnemyTileInteraction();
                    health = 3;
                    damage = 3;
                    movementSpeed = 125f;
					acceleration = 600f;
                    size = 32;
					spriteSheet = batTexture;
					break;
                default:
					enemyAI = new BasicEnemyAI();
                    tileInteraction = new GroundEnemyTileInteraction();
                    health = 1;
                    damage = 1;
                    movementSpeed = 100f;
					acceleration = 500f;
                    size = 64;
					spriteSheet = slimeTexture;
					break;
			}
			Vector2 spawnPosition = GetSpawnBehavior( 
				movementBounds,
				room,
				player,
				size);

			return new Enemy(
				enemyType,
				enemyAI,
				tileInteraction,
				spriteSheet,
				spawnPosition,
				size,
				health,
				damage,
				movementSpeed,
				acceleration,
				movementBounds,
				room
			);



		}
		private static Vector2 GetSpawnBehavior(
			
			Rectangle movementBounds,
            Room room,
            Player player,
            int enemySize)
		{
            float minimumX = movementBounds.Left + enemySize / 2f;
            float maximumX = movementBounds.Right - enemySize / 2f;
            float minimumY = movementBounds.Top + enemySize / 2f;
            float maximumY = movementBounds.Bottom - enemySize / 2f;
            
            Vector2 playerCenter = player != null
                ? player.Bounds.Center.ToVector2()
                : new Vector2(float.MinValue, float.MinValue);

            for (int attempt = 0; attempt < MaxSpawnAttempts; attempt++)
            {
                float x = Random.Shared.NextSingle() * (maximumX - minimumX) + minimumX;
                float y = Random.Shared.NextSingle() * (maximumY - minimumY) + minimumY;
                Vector2 candidate = new Vector2(x, y);

                Rectangle candidateBounds = new Rectangle(
                    (int)(candidate.X - enemySize / 2f),
                    (int)(candidate.Y - enemySize / 2f),
                    enemySize,
                    enemySize);

                if (player != null &&
                    Vector2.Distance(candidate, playerCenter) < MinimumSpawnDistance)
                {
                    continue;
                }

                if (room != null && IsOverlappingAnyTile(room, candidateBounds))
                {
                    continue;
                }

                return candidate;
            }

            
            for (int attempt = 0; attempt < MaxSpawnAttempts; attempt++)
            {
                float x = Random.Shared.NextSingle() * (maximumX - minimumX) + minimumX;
                float y = Random.Shared.NextSingle() * (maximumY - minimumY) + minimumY;
                Vector2 candidate = new Vector2(x, y);

                Rectangle candidateBounds = new Rectangle(
                    (int)(candidate.X - enemySize / 2f),
                    (int)(candidate.Y - enemySize / 2f),
                    enemySize,
                    enemySize);

                if (room == null || !IsOverlappingAnyTile(room, candidateBounds))
                    return candidate;
            }

            
            return new Vector2(
                movementBounds.Center.X,
                movementBounds.Center.Y);
        }

        private static bool IsOverlappingAnyTile(Room room, Rectangle bounds)
        {
            foreach (ITile tile in room.Tiles)
            {
                if (tile.Bounds.Intersects(bounds))
                    return true;
            }

            return false;
        }
    }
}
