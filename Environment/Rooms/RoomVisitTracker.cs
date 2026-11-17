// Erik Zhang
using System.Collections.Generic;

namespace _3902sprint0.Environment.Rooms
{
    /// <summary>
    /// Remembers every room the player has entered and which one they are in now.
    /// Subscribes to RoomManager.RoomEntered the same way RoomPopulator does.
    /// </summary>
    public class RoomVisitTracker
    {
        private readonly HashSet<string> visited = new HashSet<string>();

        public string CurrentRoomId { get; private set; }

        public RoomVisitTracker(RoomManager rooms)
        {
            rooms.RoomEntered += OnRoomEntered;
        }

        public bool HasVisited(string id) => visited.Contains(id);

        private void OnRoomEntered(RoomInstance instance)
        {
            CurrentRoomId = instance.Data.Id;
            visited.Add(instance.Data.Id);
        }
    }
}
