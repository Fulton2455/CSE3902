using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3902sprint0
{
    internal class MagicSword :Iitem
    {
        private float useTime = 0f;
        private float useSpeed = 1.5f;
        public bool IsOffCooldown => true;
        private float toggleTime = 0f;
        private float toggleSpeed = .05f;

        public string Name { get; set; }

        public bool toggleOff = true;
        public Texture2D Texture { get; }
    public MagicSword(Texture2D texture)
    {
        Name = "Magic Sword";
        Texture = texture;
    }
        public void cooldownReset()
        {
            toggleOff = true;
        }
        public void Update(GameTime gameTime)
        {
            useTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (useTime > useSpeed)
            {
                useTime = useSpeed;
            }
        }

        public void ApplyEffect(Player player)
        {
           
            if (toggleOff)
            {
                player.Speed += 300;
                player.Acceleration += 2500;
                toggleOff = false;
                SoundManager.PlaySwordSwing();

            }
            else if (!toggleOff)
            {
                player.Speed -= 300;
                player.Acceleration -= 2500;
                toggleOff = true;

            }

        }
    
    public void Use(Player player, GameTime gameTime)
        {


            ApplyEffect(player);
        }
        // Implement the active ability of the magic sword
        // For example, perform a special attack or cast a spell
    }


}

