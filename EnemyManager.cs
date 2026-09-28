using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace _3902sprint0
{
	public class EnemyManager
	{
		private readonly List<Enemy> enemies;

		private readonly Texture2D runningTexture;
        private readonly Texture2D idleTexture;

		private readonly Player player;

		private Rectangle movementBounds;

		private static readonly System.Random random = new System.Random();

		public EnemyManager(
			Player player,
			Texture2D runningTexture,
			Texture2D idleTexture)
		{
			this.player = player;
			this.runningTexture = runningTexture;
			this.idleTexture = idleTexture;

			enemies = new List<Enemy>();
		}
		public void SetMovementBounds(Rectangle movementBounds)
		{
			this.movementBounds = movementBounds;
		}
		

		
		public void RespawnEnemies(params EnemyType[] enemyTypes)
		{
			enemies.Clear();
			foreach (EnemyType enemyType in enemyTypes)
			{
                Enemy enemy = EnemyCreator.CreateEnemy(enemyType, 
					runningTexture, 
					idleTexture, 
					movementBounds);
                enemies.Add(enemy);
            }
		}
		public void update(GameTime gameTime)
		{
			foreach (Enemy enemy in enemies)
			{
				enemy.Update(gameTime);
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
