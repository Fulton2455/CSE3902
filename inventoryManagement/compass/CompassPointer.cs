using System;
using System.Collections.Generic;

namespace _3902sprint0.Environment.Rooms
{
    public class CompassPointer
    {
        private readonly DungeonMap dungeon;
        public string TargetRoomId { get; }

        public CompassPointer(DungeonMap dungeon, string startRoomId)
        {
            this.dungeon = dungeon;
            TargetRoomId = FarthestFrom(startRoomId);
        }

        // Which door to take from 'currentId' to head toward the target. Null if already there.
        public RoomSide? DirectionFrom(string currentId)
        {
            if (currentId == TargetRoomId) return null;

            var firstStep = new Dictionary<string, RoomSide>();
            var seen = new HashSet<string> { currentId };
            var queue = new Queue<string>();
            queue.Enqueue(currentId);

            while (queue.Count > 0)
            {
                string id = queue.Dequeue();
                foreach (RoomSide side in Enum.GetValues(typeof(RoomSide)))
                {
                    if (!dungeon.Get(id).Links.TryGetValue(side, out string next) || next == null || !seen.Add(next))
                        continue;

                    firstStep[next] = id == currentId ? side : firstStep[id];
                    if (next == TargetRoomId) return firstStep[next];
                    queue.Enqueue(next);
                }
            }
            return null; // target unreachable
        }

        private string FarthestFrom(string startId)
        {
            var seen = new HashSet<string> { startId };
            var queue = new Queue<string>();
            queue.Enqueue(startId);
            string last = startId;

            while (queue.Count > 0)
            {
                last = queue.Dequeue();
                foreach (RoomSide side in Enum.GetValues(typeof(RoomSide)))
                    if (dungeon.Get(last).Links.TryGetValue(side, out string next) && next != null && seen.Add(next))
                        queue.Enqueue(next);
            }
            return last; // last room dequeued = farthest by door count
        }
    }
}