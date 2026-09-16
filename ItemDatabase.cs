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

        //database of all items and entitys they use
    public itemDatabase(Texture2D magicSwordTexture, Texture2D fireScrollTexture,
        Fireball fireball)
        {
        items.Add(new MagicSword(magicSwordTexture));
        items.Add(new FireScroll(fireScrollTexture, fireball));
    }
    public Iitem RandomItem()
    {
        Random random = new Random();

        int result = random.Next(0, items.Count);

        return items[result];
    }

}
}
