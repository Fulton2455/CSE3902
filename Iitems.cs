using _3902sprint0;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3902sprint0
{
    public interface Iitem
    {
        string Name { get; set; }
        Texture2D Texture { get; }
        void ApplyEffect(Player player);
        void Use(Player player, GameTime gameTime);

        void cooldownReset();
        void Update(GameTime gameTime);

    }
}
