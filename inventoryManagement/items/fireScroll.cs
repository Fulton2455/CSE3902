
using Microsoft.Xna.Framework.Graphics;

namespace _3902sprint0
{
    internal class FireScroll : FireScrollBase
    {
        private const int ManaCost = 12;
        private const int FireballType = 1;

        public FireScroll(
            Texture2D texture,
            fireballManager fireballManager)
            : base(
                texture,
                fireballManager,
                ManaCost,
                FireballType,3f,
                "Fire Scroll")
        {
        }
    }
}
