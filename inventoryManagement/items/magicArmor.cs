
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3902sprint0
{
    internal class magicArmor : Iitem
    {
        private float useTime = 15f;
        private float useSpeed = 15f;

        private bool shieldActive = false;
        private Player shieldPlayer;

        private const int ManaCost = 15;
        public string Name { get; set; }
        public bool firstUse = true;
        public Texture2D Texture { get; }

        public bool IsOffCooldown => !shieldActive && useTime >= useSpeed;

        public magicArmor(Texture2D texture)
        {
            Name = "Magic Armor";
            Texture = texture;
        }

        public void cooldownReset()
        {
            if (shieldPlayer != null)
            {
                shieldPlayer.RemoveShield();
            }

            shieldActive = false;
            shieldPlayer = null;
            useTime = useSpeed;
        }

        public void ApplyEffect(Player player)
        {
            player.ActivateShield();
            shieldPlayer = player;
            shieldActive = true;
            useTime = 0f;
            player.mana -= 15; 
        }

        public void Update(GameTime gameTime)
        {
            if (shieldActive)
            {
                if (shieldPlayer == null || shieldPlayer.ShieldHealth <= 0)
                {
                    shieldActive = false;
                    shieldPlayer = null;
                    useTime = 0f;
                }

                return;
            }

            useTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (useTime > useSpeed)
            {
                useTime = useSpeed;
            }
        }

        public void Use(Player player, GameTime gameTime)
        {
            if (!IsOffCooldown || player.mana < ManaCost)
                return;
            if (IsOffCooldown)
            {
                ApplyEffect(player);
            }
        }
    }
}
