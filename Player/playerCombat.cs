using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using _3902sprint0.Environment;

namespace _3902sprint0
{
    public class PlayerCombat
    {
        private readonly Player p;

        public PlayerCombat(Player player)
        {
            p = player;
        }

        public void Heal(int amount)
        {
            if (p.PlayerSprite1.IsAnimation(PlayerSprite.AnimationState.Death))
                return;
            if (amount > 6)
                amount = 6;
            p.Health = Math.Min(p.Health + amount, 6);

        }

        public void ActivateShield()
        {
            p.ShieldHealth = p.MaxShieldHealth;
        }

        public void RemoveShield()
        {
            p.ShieldHealth = 0;
        }

        public void TakeDamage(int amount)
        {
            if (p.PlayerSprite1.IsAnimation(PlayerSprite.AnimationState.Death))
                return;

            if (p.invulnerable || amount <= 0)
                return;

            if (p.ShieldHealth > 0)
            {
                p.ShieldHealth -= amount;

                if (p.ShieldHealth < 0)
                {
                    amount = -p.ShieldHealth;
                    p.ShieldHealth = 0;
                }
                else
                {

                    return;
                }
            }

            p.Health -= amount;

            if (p.Health <= 0)
            {
                p.Health = 0;
                p.Die();
            }
            else
            {
                SoundManager.PlayPlayerHurt();
            }
        }

        public void BecomeInvulnerable()
        {
            p.invulnerable = true;
            p.invulnerableTimer = 3.0;
        }

        public void BecomeInvisible()
        {
            p.invisible = true;
            p.invisibleTimer = 3.0;
        }

        public void UpdateInvisibility(GameTime gameTime)
        {
            if (p.invisible)
            {
                p.invisibleTimer -= gameTime.ElapsedGameTime.TotalSeconds;

                if (p.invisibleTimer <= 0)
                {
                    p.invisible = false;
                }
            }
        }

        public void FireballHitCheck()
        {
            if (p.currentRoom == null)
                return;

            if (p.fireballManager.Fireballs == null)
                return;
            if (p.fireballManager.Fireballs2 == null)
                return;
            foreach (Fireball2 fireball2 in p.fireballManager.Fireballs2)
            {
                if (fireball2.IsActive && fireball2.IsHostile && p.Bounds.Intersects(fireball2.Bounds))
                {
                    p.TakeDamage(fireball2.Damage);
                    fireball2.Hit();
                }
            }
            foreach (Fireball fireball in p.fireballManager.Fireballs)
            {
                if (fireball.IsActive && fireball.IsHostile && p.Bounds.Intersects(fireball.Bounds))
                {
                    p.TakeDamage(fireball.Damage);
                    fireball.Hit();
                }
            }
        }

        public void Attack()
        {

            p.PlayerSprite1.SetAnimation(PlayerSprite.AnimationState.Attack);
            SoundManager.PlaySwordSwing();

        }

        public void Die()
        {
            SoundManager.PlayPlayerDeath();
            p.PlayerSprite1.SetAnimation(PlayerSprite.AnimationState.Death);
            p.Velocity = Vector2.Zero;

        }

        public void Revive()
        {

            p.PlayerSprite1.SetAnimation(PlayerSprite.AnimationState.Walk);
            p.Health = 6;
            p.RemoveShield();

        }

    }
}