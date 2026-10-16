using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3902sprint0
{
    internal class RobeOfInvisibility : Iitem
    {
        private float useTime = 10f;
        private float useSpeed = 10f;

        private const int ManaCost = 10;



        public string Name { get; set; }

        public bool firstUse = true;
        public Texture2D Texture { get; }
        public bool IsOffCooldown => useTime >= useSpeed;
        public RobeOfInvisibility(Texture2D texture)
        {
            Name = "Robe of Invisibility";
            Texture = texture;

        }
        public void cooldownReset()
        {

            useTime = 10f;
        }


        public void ApplyEffect(Player player)
        {

            player.becomeInvisible();
            player.mana -= 10;

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
            if (!IsOffCooldown || player.mana < ManaCost)
                return;
            if (useTime >= useSpeed)
            {
                ApplyEffect(player);
                useTime = 0f;
            }
        }

    }
}
