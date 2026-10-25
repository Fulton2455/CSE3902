using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace _3902sprint0
{
    internal class MagicSword2 : Iitem
    {
        public bool IsOffCooldown => true;

        private Player swordPlayer;
        private float manaDrainTimer = 0f;
        private const float ManaDrainInterval = 1f;
        private const int ManaDrainAmount = 2;

        public string Name { get; set; }

        public bool toggleOff = true;
        public Texture2D Texture { get; }

        public MagicSword2(Texture2D texture)
        {
            Name = "Magic Attack Sword";
            Texture = texture;
        }

        public void cooldownReset()
        {
            Deactivate();
        }

        public void Update(GameTime gameTime)
        {
            if (!toggleOff && swordPlayer != null)
            {
                manaDrainTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                while (manaDrainTimer >= ManaDrainInterval)
                {
                    manaDrainTimer -= ManaDrainInterval;
                    swordPlayer.mana -= ManaDrainAmount;
                }

                if (swordPlayer.mana <= 0)
                {
                    swordPlayer.mana = 0;
                    Deactivate();
                }
            }
        }

        public void ApplyEffect(Player player)
        {
            if (toggleOff)
            {
                if (player.mana < ManaDrainAmount)
                     {
                    EffectMessage.Show("Out of Mana!", Color.LightGray);
                    return;
                }

                player.damage += 1;
                toggleOff = false;
                swordPlayer = player;
                manaDrainTimer = 0f;
                SoundManager.PlaySwordSwing();
            }
            else
            {
                Deactivate();
            }
        }

        private void Deactivate()
        {
            if (!toggleOff && swordPlayer != null)
            {
                swordPlayer.damage -= 1;
            }

            toggleOff = true;
            swordPlayer = null;
            manaDrainTimer = 0f;
        }

        public void Use(Player player, GameTime gameTime)
        {
            ApplyEffect(player);
        }
    }
}