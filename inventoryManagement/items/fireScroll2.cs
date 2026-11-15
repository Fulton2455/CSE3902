
using Microsoft.Xna.Framework.Graphics;

namespace _3902sprint0
{
    internal class FireScroll2 : FireScrollBase
    {
        private const int ManaCost = 8;
        private const int FireballType = 2;

        public FireScroll2(
            Texture2D texture,
            fireballManager fireballManager)
            : base(
                texture,
                fireballManager,
                ManaCost,
                FireballType,2f,
                "Fire Scroll 2")
        {
        }
    }
}
