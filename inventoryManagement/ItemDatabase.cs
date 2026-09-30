using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.Xna.Framework.MathHelper;

namespace _3902sprint0
{
    internal class itemDatabase
    {

        public List<Iitem> items = new List<Iitem>();

        public itemDatabase(Texture2D magicSwordTexture, Texture2D fireScrollTexture, fireballManager fireballManager, Texture2D shieldOfInvulnerabilityTexture)
        {
            items.Add(new MagicSword(magicSwordTexture));
            items.Add(new fireScroll(fireScrollTexture, fireballManager));
            items.Add(new shieldOfInvulnerability(shieldOfInvulnerabilityTexture));
        }
        public Iitem RandomItem()
        {
            Random random = new Random();

            int result = random.Next(0, items.Count);

            return items[result];
        }

    }
}
