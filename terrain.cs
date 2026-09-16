using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static _3902sprint0.PlayerSprite;

namespace _3902sprint0
{
    public class terrain
    {
        public float Acceleration;
        public Color terrainColor;
        private bool terrainChecked=false;

        public enum terrainState
        {
            Stone,
            Ice,
            Swamp
        }

        public void newTerrainAccessed(Player player)
        {
            terrainChecked = false;
            
            player.Acceleration = 500f;
            player.Speed = 200f;
            
        }
        public terrainState currentState { get; set; }

        public void getTerainEffect( Player player)
        {
            if(terrainChecked)
            {
                return;
            }
            switch (currentState)
            {
                case terrainState.Stone://must travel between stones to get to other terrains
                    Acceleration = player.Acceleration;
                    player.Speed =player.Speed;
                    terrainColor = new Color(75, 75, 70);
                    terrainChecked= true;
                    break;

                case terrainState.Ice:
                    Acceleration =player.Acceleration- 200f;
                    player.Speed = player.Speed+ 400f;
                    terrainColor = Color.LightBlue;
                    terrainChecked = true;

                    break;

                case terrainState.Swamp:
                    Acceleration = player.Acceleration+ 500f;
                    player.Speed = player.Speed- 100f;
                    terrainColor = Color.Olive;
                    terrainChecked = true;

                    break;
            }
        }
    }
}
