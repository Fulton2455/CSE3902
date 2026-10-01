using _3902sprint0.Environment;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace _3902sprint0
{
	public class EnemyManager
	{
		private readonly List<Enemy> enemies;

        private  fireballManager fireballManager;

        private readonly Texture2D runningTexture;
        private readonly Texture2D idleTexture;

		private readonly Player player;

		private Room room;

		private Rectangle movementBounds;

		private static readonly System.Random random = new System.Random();

		public EnemyManager(
			Player player,
			Texture2D runningTexture,
			Texture2D idleTexture,
			fireballManager fireballManager)
		{
			this.player = player;
			this.runningTexture = runningTexture;
			this.idleTexture = idleTexture;
			this.fireballManager = fireballManager;

			enemies = new List<Enemy>();
		}
		public void SetMovementBounds(Rectangle movementBounds)
		{
			this.movementBounds = movementBounds;
		}
		public void SetRoom(Room room)
		{
			this.room = room;
		}
		

		
		public void RespawnEnemies(params EnemyType[] enemyTypes)
		{
			enemies.Clear();
			foreach (EnemyType enemyType in enemyTypes)
			{
                Enemy enemy = EnemyCreator.CreateEnemy(enemyType, 
					runningTexture, 
					idleTexture, 
					movementBounds,
					room,
					fireballManager);
                enemies.Add(enemy);
            }
		}
		public void update(GameTime gameTime, float terrainAcceleration)
		{
			foreach (Enemy enemy in enemies)
			{
				enemy.Update(gameTime, terrainAcceleration);
				if (enemy.bounds.Intersects(player.Bounds) && enemy.CanDamagePlayer)
				{
					player.TakeDamage(enemy.Damage);
					enemy.StartDamageCoolDown();
				}
			}
		}
		public void draw(SpriteBatch spriteBatch)
		{
			foreach(Enemy enemy in enemies)
			{
				enemy.Draw(spriteBatch);
			}
		}
    }
	
}
