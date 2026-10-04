
using _3902sprint0.Environment;
using _3902sprint0.Environment.Tiles;
namespace _3902sprint0
{
    public class FlyingEnemyTileInteraction : IEnemyTileInteraction
    {
        public bool InteractWith(ITile tile)
        {
            if (tile is IHazard)
            {
                return false;
            }
            return true;
        }
        public bool CanPush(ITile tile)
        {
            return false;
        }
    }
}
