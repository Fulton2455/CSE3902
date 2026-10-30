// Connor Fulton
using System.Linq;

namespace _3902sprint0.Environment.Rooms
{
    //Fills each room with its enemies whenever the player arrives.
    public class RoomPopulator
    {
        private readonly EnemyManager enemyManager;

        public RoomPopulator(RoomManager rooms, EnemyManager enemyManager)
        {
            this.enemyManager = enemyManager;
            rooms.RoomEntered += OnRoomEntered;
        }

        private void OnRoomEntered(RoomInstance instance)
        {
            enemyManager.SetRoom(instance.Room);
            enemyManager.RespawnEnemies(instance.Data.Enemies.ToArray());
        }
    }
}
