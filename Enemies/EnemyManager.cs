using _3902sprint0.Environment;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace _3902sprint0
{
	public class EnemyManager
	{
		private readonly List<Enemy> enemies;
		public IReadOnlyList<Enemy> Enemies => enemies;
		private readonly List<BossShockwave> bossShockwaves = new List<BossShockwave>();
		private Texture2D ShockwavePixel;
		private Texture2D toadTexture;

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
			Texture2D toadTexture,
			Texture2D shockwavePixel,
			fireballManager fireballManager)
		{
			this.player = player;
			this.slimeTexture = slimeTexture;
			this.zombieTexture = zombieTexture;
			this.mageTexture = mageTexture;
			this.batTexture = batTexture;
			this.toadTexture = toadTexture;
			this.ShockwavePixel = shockwavePixel;
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
		
		public void CreateBossShockwave(
			Vector2 position,
			float radius,
			float duration,
			int damage)
		{
			bossShockwaves.Add(
				new BossShockwave(position, radius, duration, damage, ShockwavePixel));
		}
		private void UpdateBossShockwave(GameTime gameTime)
		{
			foreach (BossShockwave shockwave in bossShockwaves)
			{
				shockwave.Update(gameTime, player);
			}
			bossShockwaves.RemoveAll(shockwave => shockwave.IsFinished);
		}
		public bool CanSpawnBossMinion()
		{
			int minionCount = 0;
			foreach (Enemy enemy in enemies)
			{
				if (enemy.Type == EnemyType.BossMinion && !enemy.IsDead)
				{
					minionCount++;
				}
				
			}
            return minionCount < 4;
        }
        private bool TryGetMinionSpawnPosition(
Vector2 bossPosition,
int minionSize,
out Vector2 spawnPosition)
        {
            const int attempts = 30;
            const float minimumDistanceFromBoss = 80f;

            for (int i = 0; i < attempts; i++)
            {
                
                float angle =
                (float)(random.NextDouble() * MathHelper.TwoPi);

                float distance =
                minimumDistanceFromBoss +
                (float)(random.NextDouble() * 80f);

                Vector2 candidate = bossPosition + new Vector2(
                (float)Math.Cos(angle) * distance,
                (float)Math.Sin(angle) * distance
                );

                Rectangle candidateBounds = new Rectangle(
                (int)(candidate.X - minionSize / 2f),
                (int)(candidate.Y - minionSize / 2f),
                minionSize,
                minionSize
                );

                
                if (candidateBounds.Left < movementBounds.Left ||
                candidateBounds.Right > movementBounds.Right ||
                candidateBounds.Top < movementBounds.Top ||
                candidateBounds.Bottom > movementBounds.Bottom)
                {
                    continue;
                }

                
                if (room == null || room.IsBlocked(candidateBounds))
                {
                    continue;
                }

                
                if (candidateBounds.Intersects(player.Bounds))
                {
                    continue;
                }

                
                bool overlapsEnemy = false;

                foreach (Enemy enemy in enemies)
                {
                    if (enemy.IsDead)
                    {
                        continue;
                    }

                    if (candidateBounds.Intersects(enemy.bounds))
                    {
                        overlapsEnemy = true;
                        break;
                    }
                }

                if (overlapsEnemy)
                {
                    continue;
                }

                spawnPosition = candidate;
                return true;
            }

            
            spawnPosition = Vector2.Zero;
            return false;
        }

        public void SpawnBossMinions(
			Vector2 bossPosition,
			int requestedCount)
        {
            const int minionSize = 48;

            for (int i = 0; i < requestedCount; i++)
            {
                if (!CanSpawnBossMinion())
                {
                    break;
                }

                if (!TryGetMinionSpawnPosition(
                bossPosition,
                minionSize,
                out Vector2 spawnPosition))
                {
                    continue;
                }
                Enemy minion = EnemyCreator.CreateEnemy(
                EnemyType.BossMinion,
                toadTexture,
                slimeTexture,
                zombieTexture,
                mageTexture,
                batTexture,
                movementBounds,
                room,
				player,
                fireballManager,
				this,
                spawnPosition);

                enemies.Add(minion);
            }
        }
        public void RespawnEnemies(params EnemyType[] enemyTypes)
		{
			enemies.Clear();
			foreach (EnemyType enemyType in enemyTypes)
			{
                Enemy enemy = EnemyCreator.CreateEnemy(enemyType,
                    toadTexture,
                    slimeTexture,
					zombieTexture,
					mageTexture,
					batTexture,
                    movementBounds,
					room,
					player,
					fireballManager,
					this);
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
				UpdateBossShockwave(gameTime);
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
			foreach (BossShockwave shockwave in bossShockwaves)
			{
				shockwave.draw(spriteBatch);
			}
		}
    }
	
}
