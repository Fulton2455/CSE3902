// Connor Fulton
using _3902sprint0.Environment.Doors;
using System;
using Microsoft.Xna.Framework;

namespace _3902sprint0.Environment.Rooms
{
    
    // Opens shutter doors when the room has no living enemies and closes them otherwise
    
    public class ShutterController
    {
        private readonly RoomManager rooms;
        private readonly Func<bool> hasLivingEnemies;
        private readonly Func<Rectangle> playerBounds;

        public ShutterController(RoomManager rooms, Func<bool> hasLivingEnemies, Func<Rectangle> playerBounds)
        {
            this.rooms = rooms;
            this.hasLivingEnemies = hasLivingEnemies;
            this.playerBounds = playerBounds;
        }

        public void Update()
        {
            bool fight = hasLivingEnemies();
            Rectangle player = playerBounds();

            foreach (ShutterDoorTile shutter in rooms.Current.Shutters)
            {
                if (!fight) shutter.Open();
                else if (!shutter.Bounds.Intersects(player)) shutter.Close();
            }
        }
    }
}
