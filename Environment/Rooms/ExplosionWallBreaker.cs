namespace _3902sprint0.Environment.Rooms
{
    //Lets active fireball explosions blow open bombable walls in the current room.
    public class ExplosionWallBreaker
    {
        private readonly RoomManager rooms;
        private readonly fireballManager fireballs;

        public ExplosionWallBreaker(RoomManager rooms, fireballManager fireballs)
        {
            this.rooms = rooms;
            this.fireballs = fireballs;
        }

        public void Update()
        {
            foreach (var fireball in fireballs.Fireballs)
            {
                if (fireball.IsExplosionActive)
                    rooms.CurrentRoom.Detonate(fireball.ExplosionBounds);
            }
        }
    }
}
