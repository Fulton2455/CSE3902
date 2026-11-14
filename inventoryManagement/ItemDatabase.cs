using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.Xna.Framework.MathHelper;

namespace _3902sprint0
{
    public class itemDatabase
    {

        public List<Iitem> items = new List<Iitem>();

        public itemDatabase(Texture2D magicSwordTexture, Texture2D fireScrollTexture, fireballManager fireballManager, Texture2D shieldOfInvulnerabilityTexture, Texture2D robeOfInvisibilityTexture, 
            Texture2D panaceaTexture, Texture2D magicArmorTexture, Texture2D deckOfcardsTexture)
        {
            items.Add(new MagicSword(magicSwordTexture));
            items.Add(new FireScroll(fireScrollTexture, fireballManager));
            items.Add(new ShieldOfInvulnerability(shieldOfInvulnerabilityTexture));
            items.Add(new FireScroll2(fireScrollTexture, fireballManager));
            items.Add(new magicAttackSword(magicSwordTexture));
            items.Add(new RobeOfInvisibility(robeOfInvisibilityTexture));
            items.Add(new PanaceaBottle(panaceaTexture));
            items.Add(new magicArmor(magicArmorTexture));
            items.Add(new deckOfCards(deckOfcardsTexture, fireballManager));
        }
        public Iitem RandomItem()
        {
            Random random = new Random();

            int result = random.Next(0, items.Count);

            return items[result];
        }

    }
}
