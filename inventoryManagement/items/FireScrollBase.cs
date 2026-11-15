
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3902sprint0
{
    internal abstract class FireScrollBase : Iitem
    {
        protected float useTime = 3f;
        protected float useSpeed = 3f;

        protected int manaCost;
        protected int fireballType;

        protected fireballManager fireballManager;

        public string Name { get; set; }
        public Texture2D Texture { get; }

        public bool IsOffCooldown => useTime >= useSpeed;

        protected FireScrollBase(
            Texture2D texture,
            fireballManager fireballManager,
            int manaCost,
            int fireballType,
            float cooldown,
            string name)
        {
            Texture = texture;
            this.fireballManager = fireballManager;
            this.manaCost = manaCost;
            this.fireballType = fireballType;
            useSpeed = cooldown;
            useTime = cooldown;
            Name = name;
        }

        public void cooldownReset()
        {
            useTime = useSpeed;
        }

        public void ApplyEffect(Player player)
        {
            Vector2 playerCenter = new Vector2(
                player.Location.X,
                player.Location.Y + 40
            );

            fireballManager.createFireball(
                fireballType,
                playerCenter,
                player.AimDirection,
                false
            );
        }

        public void Update(GameTime gameTime)
        {
            useTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (useTime > useSpeed)
            {
                useTime = useSpeed;
            }
        }

        public void Use(Player player, GameTime gameTime)
        {
            if (!IsOffCooldown || player.mana < manaCost)
            {
                EffectMessage.Show("Out of Mana!", Color.LightGray);
                return;
            }

            ApplyEffect(player);
            player.mana -= manaCost;
            useTime = 0f;
        }
    }
}
