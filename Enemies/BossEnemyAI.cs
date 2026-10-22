
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

                enemy.FaceTowards(player.Location);
                enemy.PlayTimedAnimation(BossAnimation.Summon, stateTimer);
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
            GameTime gameTime, float acceleration)
        {
            Vector2 toPlayer = player.Location - enemy.Position;

            float distance = toPlayer.Length();

            if (distance > 90f)
            {
                enemy.MoveInDirection(
                toPlayer,
                gameTime,
                acceleration
                );
            }
            else
            {
                enemy.StopMovement();
            }

            if (stateTimer > 0f)
            {
                return;
            }

            if (summonCooldown <= 0f &&
            enemyManager.CanSpawnBossMinion())
            {
                state = BossState.Summon;
                stateTimer = 0.75f;
                summonCooldown = phase2 ? 7f : 12f;
                enemy.StopMovement();
                enemy.FaceTowards(player.Location);
                enemy.PlayTimedAnimation(BossAnimation.Summon, stateTimer);
                return;
            }

            if (attackCooldown > 0f)
            {
                return;
            }

            
            float roll = (float)random.NextDouble();

            if (distance <= 120f && roll < 0.4f)
            {
                state = BossState.Melee;
                stateTimer = 0.45f;
                enemy.StopMovement();
                enemy.FaceTowards(player.Location);
                enemy.PlayTimedAnimation(BossAnimation.Melee, stateTimer);
            }
            else if (roll < (phase2 ? 0.55f : 0.35f))
            {
                leapTarget = player.Location;
                state = BossState.TelegraphLeap;
                stateTimer = phase2 ? 0.55f : 0.8f;
                enemy.StopMovement();
                enemy.FaceTowards(leapTarget);
                enemy.PlayTimedAnimation(BossAnimation.Telegraph, stateTimer);


            }
            else
            {
                state = BossState.AcidSpit;
                stateTimer = 0.5f;
                enemy.StopMovement();
                enemy.FaceTowards(player.Location);
                enemy.PlayTimedAnimation(BossAnimation.Spit, stateTimer);
            }

            enemy.StopMovement();
            attackCooldown = phase2 ? 0.8f : 1.5f;
        }
        private void updateTelegraph(Enemy enemy)
        {
            enemy.StopMovement();
            if (stateTimer <= 0f)
            {
                state = BossState.Leap;
                stateTimer = phase2 ? 0.45f : 0.55f;
                enemy.FaceTowards(leapTarget);
                enemy.PlayTimedAnimation(BossAnimation.Leap, stateTimer);
            }
        }
        private void updateLeap(Enemy enemy, Player player, GameTime gameTime, float acceleration)
        {
            float leapSpeed = phase2 ? 700f : 500f;
            enemy.LeapTowards(
                leapTarget,
                gameTime,
                leapSpeed);

            if (stateTimer <= 0f)
            {
                enemy.StopMovement();
                SoundManager.PlayExplosion();
                enemyManager.CreateBossShockwave(
                enemy.Position,
                phase2 ? 420f : 300f,
                2f,
                phase2 ? 3 : 2
                );

                EnterRecover(enemy, phase2 ? 0.35f : 0.7f);
            }
        }
        private void updateRecover(Enemy enemy)
        {
            enemy.StopMovement();

            if (stateTimer <= 0f)
            {
                enemy.EndTimedAnimation();
                state = BossState.Approach;
                stateTimer = phase2 ? 0.2f : 0.5f;
            }
        }
        private void EnterRecover(Enemy enemy, float duration)
        {
            state = BossState.Recover;
            stateTimer = duration;
            enemy.PlayTimedAnimation(BossAnimation.Recover, duration);
        }
        private void updateMelee(Enemy enemy, Player player)
        {
            enemy.StopMovement();

            if (stateTimer <= 0f)
            {
                float distance = Vector2.Distance(
                enemy.Position,
                player.Location);

                if (distance <= (phase2 ? 110f : 90f))
                {
                    player.TakeDamage(phase2 ? 4 : 3);
                }

                EnterRecover(enemy, phase2 ? 0.35f : 0.65f);
            }
        }
        private void updateAcidSpit(Enemy enemy, Player player)
        {
            enemy.StopMovement();

            if (stateTimer <= 0f)
            {
                Vector2 direction =
                player.Location - enemy.Position;

                if (direction != Vector2.Zero)
                {
                    direction.Normalize();

                    Vector2 projectilePosition =
                    enemy.Position + direction * 45f;
                    SoundManager.PlayFireballShoot();
                    fireballManager.CreateAcidProjectile(
                    projectilePosition,
                    direction
                    );
                }

                EnterRecover(enemy, phase2 ? 0.3f : 0.6f);
            }
        }
        private void updateSummon(Enemy enemy)
        {
            enemy.StopMovement();

            if (stateTimer <= 0f)
            {
                enemyManager.SpawnBossMinions(
                enemy.Position,
                phase2 ? 3 : 2
                );

                EnterRecover(enemy, phase2 ? 0.3f : 0.6f);
            }
        }
    }
}
