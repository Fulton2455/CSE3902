
using _3902sprint0.Environment;
using _3902sprint0.Environment.Tiles;
namespace _3902sprint0
{
    public class GroundEnemyTileInteraction : IEnemyTileInteraction
    {
        public bool InteractWith(ITile tile)
        {
            return true;
        }
        public bool CanPush(ITile tile)
        {
            return false;
        }
    }
}
