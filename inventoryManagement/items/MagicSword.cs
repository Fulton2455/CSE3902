using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace _3902sprint0
{
    internal class MagicSword : Iitem
    {
        private float useTime = 0f;
        private float useSpeed = 1.5f;
        public bool IsOffCooldown => true;

        private Player swordPlayer;
        private float manaDrainTimer = 0f;
        private const float ManaDrainInterval = 1f;
        private const int ManaDrainAmount = 2;

        public string Name { get; set; }

        public bool toggleOff = true;
        public Texture2D Texture { get; }

        public MagicSword(Texture2D texture)
        {
            Name = "Magic Speed Sword";
            Texture = texture;
        }

        public void cooldownReset()
        {
            Deactivate();
        }

        public void Update(GameTime gameTime)
        {
            useTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (useTime > useSpeed)
            {
                useTime = useSpeed;
            }

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
                if (player.mana <= 1)
                {
                    EffectMessage.Show("Out of Mana!", Color.LightGray);
                    return;
                }

                player.Speed += 300;
                player.Acceleration += 2500;
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
                swordPlayer.Speed -= 300;
                swordPlayer.Acceleration -= 2500;
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