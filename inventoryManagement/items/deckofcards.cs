using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace _3902sprint0
{
    internal class deckOfCards : Iitem
    {
        public const int ManaCost = 8;
        private const int EffectCount = 21;
        private fireballManager fireballManager;

        private float useTime = 1.5f;
        private float useSpeed = 1.5f;

        private static readonly Random rng = new Random();

        public string Name { get; set; }
        public Texture2D Texture { get; }
        public bool IsOffCooldown => useTime >= useSpeed;

        public deckOfCards(Texture2D texture,fireballManager fireballManager)
        {
            Name = "Deck of Cards";
            Texture = texture;
            this.fireballManager = fireballManager;

        }

        public void cooldownReset()
        {
            useTime = useSpeed;
            // Undo any active timed effects here once you add them.
        }

        public void Update(GameTime gameTime)
        {
            useTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (useTime > useSpeed)
            {
                useTime = useSpeed;
            }

            // Tick timers for timed effects here.
        }

        public void ApplyEffect(Player player)
        {
            int roll = rng.Next(EffectCount); // 0 to 19
            Vector2 playerCeneter = new Vector2(player.Location.X, player.Location.Y + 40);
            SoundManager.PlaySwordSwing();

            switch (roll)
            {
                case 0:
                    player.becomeInvulnerable();
                    break;
                case 1:
                    player.mana -= 10; // Example effect: reduce mana by 10
                    break;
                case 2:
                    player.mana += 12; // Example effect: increase mana by 10
                    break;
                case 3:
                    player.heal(3);
                    break;
                case 4:
                    player.heal(4);
                    break;
                case 5:
                    player.heal(5);
                    break;
                case 6:
                    player.TakeDamage(1);
                    break;
                case 7:
                    player.TakeDamage(2);
                    break;
                case 8:
                    player.damage += 1;//will need reversing effect
                    break;
                case 9:
                    player.damage -= .5f;//will need reversing effect
                    break;
                case 10:
                    player.damage += .5f;//will need reversing effect
                    break;
                case 11:
                    for (int i = 0; i < 4; i++)
                    {
                        float angle = (float)(rng.NextDouble() * MathHelper.TwoPi);
                        Vector2 direction = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));

                        fireballManager.createFireball(2, playerCeneter, direction, false);
                    }
                    break;
                case 12:
                    for (int i = 0; i < 2; i++)
                    {
                        float angle = (float)(rng.NextDouble() * MathHelper.TwoPi);
                        Vector2 direction = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));

                        fireballManager.createFireball(1, playerCeneter, direction, true);
                    }
                    break;
                case 13:
                    for (int i = 0; i < 3; i++)
                    {
                        float angle = (float)(rng.NextDouble() * MathHelper.TwoPi);
                        Vector2 direction = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));

                        fireballManager.createFireball(1, playerCeneter, direction, false);
                    }
                    break;
                case 14:
                    for (int i = 0; i < 2; i++)
                    {
                        float angle = (float)(rng.NextDouble() * MathHelper.TwoPi);
                        Vector2 direction = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));

                        fireballManager.createFireball(2, playerCeneter, direction, false);
                    }
                    break;
                case 15:
                    for (int i = 0; i < 6; i++)
                    {
                        float angle = (float)(rng.NextDouble() * MathHelper.TwoPi);
                        Vector2 direction = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));

                        fireballManager.createFireball(2, playerCeneter, direction, false);
                    }
                    break;
                case 16:
                    player.Speed += 100; // Example effect: increase speed by 100
                    break;
                case 17:
                    player.Speed -= 100;
                    break;
                case 18:
                    player.Speed += 100;
                    break;
                case 19:
                   player.becomeInvulnerable();
                    break;
                case 20:
                    player.becomeInvisible();
                    break;
            }
        }

        public void Use(Player player, GameTime gameTime)
        {
            if (!IsOffCooldown || player.mana < ManaCost)
                return;

            player.mana -= ManaCost;
            ApplyEffect(player);
            useTime = 0f;
        }
    }
}