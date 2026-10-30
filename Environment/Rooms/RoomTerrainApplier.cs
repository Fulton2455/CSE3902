// Connor Fulton
namespace _3902sprint0.Environment.Rooms
{
    //Applies a room's terrain (stone, ice or swamp) to the player whenever they arrive in it.
    public class RoomTerrainApplier
    {
        private readonly Player player;

        public RoomTerrainApplier(RoomManager rooms, Player player)
        {
            this.player = player;
            rooms.RoomEntered += OnRoomEntered;
        }

        private void OnRoomEntered(RoomInstance instance)
        {
            if (!(instance.Data.Terrain is terrain.terrainState state))
                return;

            player.Terrain.currentState = state;
            player.Terrain.newTerrainAccessed(player);
            player.Terrain.getTerainEffect(player);
        }
    }
}