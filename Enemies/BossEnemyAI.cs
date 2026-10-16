
using Microsoft.Xna.Framework;
using System;
using System.Runtime.CompilerServices;

namespace _3902sprint0
{
    public class BossEnemyAI : IEnemyAI
    {
        private enum BossState
        {
            Approach,
            TelegraphLeap,
            Leap,
            Recover,
            Melee,
            AcidSpit,
            Summon
        }
        private readonly EnemyManager enemyManager;
        private readonly fireballManager fireballManager;
        private BossState state = BossState.Approach;

        private float stateTimer;
        private float attackCooldown;
        private float summonCooldown;

        private Vector2 leapTarget;

        private bool phase2;

        private static readonly Random random = new Random();

        public bool IsPaused =>
            state != BossState.Approach &&
            state != BossState.Leap;

        public BossEnemyAI(EnemyManager enemyManager, fireballManager fireballManager)
        {
            this.enemyManager = enemyManager;
            this.fireballManager = fireballManager;

            stateTimer = 0f;
            attackCooldown = 1f;
            summonCooldown = 8f;
        }
        public void Update(
            Enemy enemy,
            Player player,
            GameTime gameTime,
            float terrainAcceleration)
        {
            if (player == null || player.IsDead || enemy.IsDead)
            {
                enemy.StopMovement();
                return;
            }
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            stateTimer -= elapsed;
            attackCooldown -= elapsed;
            summonCooldown -= elapsed;

            if (!phase2 && enemy.Health <= enemy.MaxHealth / 2)
            {
                phase2 = true;

                enemy.StopMovement();
                state = BossState.Recover;
                stateTimer = 1.25f;
                attackCooldown = 0.5f;
            }
            switch (state)
            {
                case BossState.Approach:
                    updateApproach(
                        enemy, player, gameTime, terrainAcceleration);
                    break;
                case BossState.TelegraphLeap:
                    updateTelegraph(enemy);
                    break;
                case BossState.Leap:
                    updateLeap(enemy, player, gameTime, terrainAcceleration);
                    break;
                case BossState.Recover:
                    updateRecover(enemy);
                    break;
                case BossState.Melee:
                    updateMelee(enemy, player);
                    break;
                case BossState.AcidSpit:
                    updateAcidSpit(enemy, player);
                    break;
                case BossState.Summon:
                    updateSummon(enemy);
                    break;
            }
            
        }
        private void updateApproach(
            Enemy enemy,
            Player player,
            GameTime gameTime, float terrainAcceleration)
        {
            return;
        }
        private void updateTelegraph(Enemy enemy)
        {
            return;
        }
        private void updateLeap(Enemy enemy, Player player, GameTime gameTime, float terrainAcceleration)
        {
            return;
        }
        private void updateRecover(Enemy enemy)
        {
            return;
        }
        private void updateMelee(Enemy enemy, Player player)
        {
            return;
        }
        private void updateAcidSpit(Enemy enemy, Player player)
        {
            return;
        }
        private void updateSummon(Enemy enemy)
        {
            return;
        }
    }
}
