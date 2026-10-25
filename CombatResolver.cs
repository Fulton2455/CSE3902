// Connor Fulton
using System.Collections.Generic;

namespace _3902sprint0
{
    
    // Resolves melee sword hits between the player and enemies. 
    
    public class CombatResolver
    {
        private readonly Player player;
        private readonly EnemyManager enemyManager;
        private readonly HashSet<Enemy> swordHitEnemies = new HashSet<Enemy>();

        public CombatResolver(Player player, EnemyManager enemyManager)
        {
            this.player = player;
            this.enemyManager = enemyManager;
        }

        public void Update()
        {
            if (!player.IsAttacking)
            {
                swordHitEnemies.Clear();
                return;
            }

            foreach (Enemy enemy in enemyManager.Enemies)
            {
                if (enemy.IsDying || enemy.IsDead)
                    continue;

                if (swordHitEnemies.Contains(enemy))
                    continue;

                if (player.SwordBounds.Intersects(enemy.bounds))
                {
                    enemy.TakeDamage((int)player.damage);
                    swordHitEnemies.Add(enemy);
                }
            }
        }
    }
}