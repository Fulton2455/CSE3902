using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace _3902sprint0
{
    public static class SoundManager
    {
        // Sound effect references
        private static SoundEffect swordSwing;
        private static SoundEffect fireballShoot;
        private static SoundEffect explosion;
        private static SoundEffect playerHurt;
        private static SoundEffect playerDeath;
        private static SoundEffect enemyHit;

        public static void LoadContent(ContentManager content)
        {
            swordSwing = content.Load<SoundEffect>("sounds/SwordSwing");
            fireballShoot = content.Load<SoundEffect>("sounds/FireballShoot");
            explosion = content.Load<SoundEffect>("sounds/Explosion");
            playerHurt = content.Load<SoundEffect>("sounds/PlayerHurt");
            playerDeath = content.Load<SoundEffect>("sounds/PlayerDeath");
            enemyHit = content.Load<SoundEffect>("sounds/EnemyHit");
        }

        public static void PlaySwordSwing()
        {
            swordSwing?.Play();
        }

        public static void PlayFireballShoot()
        {
            fireballShoot?.Play();
        }

        public static void PlayExplosion()
        {
            explosion?.Play();
        }

        public static void PlayPlayerHurt()
        {
            playerHurt?.Play();
        }

        public static void PlayPlayerDeath()
        {
            playerDeath?.Play();
        }

        public static void PlayEnemyHit()
        {
            enemyHit?.Play();
        }
    }
}