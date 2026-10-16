using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3902sprint0
{
    internal class PanaceaBottle : Iitem
    {
        private float useTime = 30f;
        private float useSpeed = 30f;


        
        public string Name { get; set; }

        public bool firstUse = true;
        public Texture2D Texture { get; }
        public bool IsOffCooldown => useTime >= useSpeed;
        public PanaceaBottle(Texture2D texture)
        {
            Name = "Panacea Bottle";
            Texture = texture;

        }
        public void cooldownReset()
        {

            useTime = 30f;
        }


        public void ApplyEffect(Player player)
        {

            player.heal(6);
            player.mana = 100;

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
            if (useTime >= useSpeed)
            {
                ApplyEffect(player);
                useTime = 0f;
            }
        }

    }
}
