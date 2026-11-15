
namespace _3902sprint0.Environment.Rooms
{
    public class ExplosionWallBreaker
    {
        private readonly RoomManager rooms;
        private readonly BombManager bombs;

        public ExplosionWallBreaker(RoomManager rooms, BombManager bombs)
        {
            this.rooms = rooms;
            this.bombs = bombs;
        }

        public void Update()
        {
            foreach (var bomb in bombs.Bombs)
            {
                if (bomb.IsExplosionActive && !bomb.HasBrokenWalls)
                {
                    rooms.CurrentRoom.Detonate(bomb.ExplosionBounds);
                    bomb.HasBrokenWalls = true;
                }
            }
        }
    }
}
