using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3902sprint0
{
    internal class FireScroll :Iitem
    {
        private float useTime = 3f;
        private float useSpeed = 3f;

        bool fireEffect = false;
        private Fireball fireball;

        public string Name { get; set; }

        public bool firstUse = true;
        public Texture2D Texture { get; }
    public FireScroll(Texture2D texture, Fireball fireball)
    {
        Name = "Fire Scroll";
        Texture = texture;
        this.fireball = fireball;

        }
        public void cooldownReset()
        {
          
            useTime = 3f;
        }


        public void ApplyEffect(Player player)
        {
            
            
           
            fireball.generateEntity(
            player.Location,
           player.AimDirection
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
            if (useTime >= useSpeed)
            {
                ApplyEffect(player);
                useTime = 0f;
            }
        }

    }
}
