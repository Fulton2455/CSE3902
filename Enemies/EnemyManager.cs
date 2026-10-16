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

        private readonly Texture2D slimeTexture;
        private readonly Texture2D zombieTexture;
        private readonly Texture2D mageTexture;
        private readonly Texture2D batTexture;

        private readonly Player player;

		private Room room;

		private Rectangle movementBounds;

		private static readonly System.Random random = new System.Random();

		public EnemyManager(
			Player player,

            Texture2D slimeTexture,
			Texture2D zombieTexture,
			Texture2D mageTexture,
			Texture2D batTexture,
			fireballManager fireballManager)
		{
			this.player = player;
			this.slimeTexture = slimeTexture;
			this.zombieTexture = zombieTexture;
			this.mageTexture = mageTexture;
			this.batTexture = batTexture;
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
                    slimeTexture,
					zombieTexture,
					mageTexture,
					batTexture,
                    movementBounds,
					room,
					player,
					fireballManager);
                enemies.Add(enemy);
            }
		}
		public void update(GameTime gameTime, float terrainAcceleration)
		{
    		for (int i = enemies.Count - 1; i >= 0; i--)
    		{
        		Enemy enemy = enemies[i];
        		enemy.Update(gameTime, terrainAcceleration);

        		if (enemy.IsDead)
        		{
            		enemies.RemoveAt(i);
            		continue;
        		}

        		if (enemy.bounds.Intersects(player.Bounds) && enemy.CanDamagePlayer)
        		{
           		 enemy.StartAttack();
            		player.TakeDamage(enemy.Damage);
            		enemy.StartDamageCoolDown();
        		}

        		CheckFireballHits(enemy);
    		}
		}


        private void CheckFireballHits(Enemy enemy)
        {
            if (enemy.IsDying || enemy.IsDead)
                return;

            foreach (Fireball fireball in fireballManager.Fireballs)
            {
                if (fireball.IsHostile)
                    continue;

                if (fireball.IsActive &&
                    fireball.Bounds.Intersects(enemy.bounds))
                {
                    enemy.TakeDamage(fireball.Damage);
                    fireball.Hit();
                }

                if (fireball.IsExplosionActive &&
                    !fireball.HasExplosionDamaged(enemy) &&
                    fireball.ExplosionBounds.Intersects(enemy.bounds))
                {
                    fireball.MarkExplosionDamage(enemy);
                    enemy.TakeDamage(fireball.ExplosionDamage);
                }
            }

            foreach (Fireball2 fireball2 in fireballManager.Fireballs2)
            {
                if (!fireball2.IsActive || fireball2.IsHostile)
                    continue;

                if (fireball2.Bounds.Intersects(enemy.bounds))
                {
                    enemy.TakeDamage(fireball2.Damage);
                    fireball2.Hit();
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
