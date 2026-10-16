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

        public deckOfCards(Texture2D texture, fireballManager fireballManager)
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

        private void ShootRandom(int count, int fireballType, bool hostile, Vector2 origin)
        {
            for (int i = 0; i < count; i++)
            {
                float angle = (float)(rng.NextDouble() * MathHelper.TwoPi);
                Vector2 direction = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));

                fireballManager.createFireball(fireballType, origin, direction, hostile);
            }
        }

        public void ApplyEffect(Player player)
        {
            int roll = rng.Next(EffectCount); // 0 to 20
            Vector2 playerCeneter = new Vector2(player.Location.X, player.Location.Y + 40);
            SoundManager.PlaySwordSwing();
            switch (roll)
            {
                case 0:
                    player.becomeInvulnerable();
                    EffectMessage.Show("Invulnerable!", Color.Gold);
                    break;
                case 1:
                    player.mana -= 10;
                    EffectMessage.Show("-10 Mana", Color.OrangeRed);
                    break;
                case 2:
                    player.mana += 12;
                    EffectMessage.Show("+12 Mana", Color.CornflowerBlue);
                    break;
                case 3:
                    player.heal(3);
                    EffectMessage.Show("+3 Health", Color.LimeGreen);
                    break;
                case 4:
                    player.heal(4);
                    EffectMessage.Show("+4 Health", Color.LimeGreen);
                    break;
                case 5:
                    player.heal(5);
                    EffectMessage.Show("+5 Health", Color.LimeGreen);
                    break;
                case 6:
                    player.TakeDamage(1);
                    EffectMessage.Show("-1 Health", Color.Red);
                    break;
                case 7:
                    player.TakeDamage(2);
                    EffectMessage.Show("-2 Health", Color.Red);
                    break;
                case 8:
                    player.damage += 1; //will need reversing effect
                    EffectMessage.Show("+1 Damage", Color.Orange);
                    break;
                case 9:
                    player.damage -= .5f; //will need reversing effect
                    EffectMessage.Show("-0.5 Damage", Color.Red);
                    break;
                case 10:
                    player.damage += .5f; //will need reversing effect
                    EffectMessage.Show("+0.5 Damage", Color.Orange);
                    break;
                case 11:
                    ShootRandom(4, 2, false, playerCeneter);
                    EffectMessage.Show("Fireball Burst!", Color.OrangeRed);
                    break;
                case 12:
                    ShootRandom(1, 1, true, player.Location+new Vector2(10, 10));
                    ShootRandom(1, 1, true, playerCeneter);

                    EffectMessage.Show("Cursed Fireballs!", Color.Red);
                    break;
                case 13:
                    ShootRandom(3, 1, false, playerCeneter);
                    EffectMessage.Show("Triple Fireball", Color.OrangeRed);
                    break;
                case 14:
                    ShootRandom(2, 2, false, playerCeneter);
                    EffectMessage.Show("Double Fireball", Color.OrangeRed);
                    break;
                case 15:
                    ShootRandom(6, 2, false, playerCeneter);
                    EffectMessage.Show("Fireball Storm!", Color.OrangeRed);
                    break;
                case 16:
                    player.Speed += 100;
                    EffectMessage.Show("+100 Speed", Color.Cyan);
                    break;
                case 17:
                    player.Speed -= 100;
                    EffectMessage.Show("-100 Speed", Color.Red);
                    break;
                case 18:
                    player.Speed += 100;
                    EffectMessage.Show("+100 Speed", Color.Cyan);
                    break;
                case 19:
                    player.becomeInvulnerable();
                    EffectMessage.Show("Invulnerable!", Color.Gold);
                    break;
                case 20:
                    player.becomeInvisible();
                    EffectMessage.Show("Invisible!", Color.LightGray);
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