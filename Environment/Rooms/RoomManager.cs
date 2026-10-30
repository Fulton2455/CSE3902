// Connor Fulton
using _3902sprint0.Environment.Pickups;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3902sprint0.Environment.Rooms
{
    
    // Owns the rooms of a dungeon: builds each one the first time it is entered, keeps it 
    public class RoomManager
    {
        private const int EntryMargin = 8;

        private readonly DungeonMap dungeon;
        private readonly RoomBuilder builder;
        private readonly Dictionary<string, RoomInstance> built = new Dictionary<string, RoomInstance>();

        private Player player;
        private IKeyHolder keyHolder;

        public RoomInstance Current { get; private set; }
        public Room CurrentRoom => Current.Room;

        /// <summary>Raised once per room, the first time it is built.</summary>
        public event Action<RoomInstance> RoomCreated;
        /// <summary>Raised every time the player arrives in a room (including the first).</summary>
        public event Action<RoomInstance> RoomEntered;

        public RoomManager(DungeonMap dungeon, RoomBuilder builder)
        {
            this.dungeon = dungeon;
            this.builder = builder;
        }

        /// <summary>
        /// Step 1 of startup: builds the first room so objects that need a Room (chests, player)
        /// can be created. Step 2 is Begin.
        /// </summary>
        public Room Preload(string startRoomId)
        {
            Current = GetOrBuild(startRoomId);
            return Current.Room;
        }

        /// <summary>Step 2 of startup: places the player in the preloaded room and announces it.</summary>
        public void Begin(Player player, IKeyHolder keyHolder)
        {
            this.player = player;
            this.keyHolder = keyHolder;
            PlacePlayerAtSpawn(Current);
            RoomEntered?.Invoke(Current);
        }

        public void Update()
        {
            Rectangle hitbox = player.Bounds;
            foreach (KeyPickup key in Current.Keys)
                key.Update(hitbox, keyHolder);

            foreach (RoomExit exit in Current.Exits)
            {
                if (exit.IsCrossedBy(hitbox))
                {
                    Travel(exit);
                    return;
                }
            }
        }

        public void DrawPickups(SpriteBatch spriteBatch)
        {
            foreach (KeyPickup key in Current.Keys)
                key.Draw(spriteBatch);
        }

        private void Travel(RoomExit exit)
        {
            Current = GetOrBuild(exit.TargetRoomId);
            PlaceAtEntrance(Current, exit.Side.Opposite());
            RoomEntered?.Invoke(Current);
        }

        private RoomInstance GetOrBuild(string id)
        {
            if (built.TryGetValue(id, out RoomInstance existing))
                return existing;

            RoomInstance created = builder.Build(dungeon.Get(id));
            built[id] = created;
            RoomCreated?.Invoke(created);
            return created;
        }

        private void PlacePlayerAtSpawn(RoomInstance instance)
        {
            Rectangle hitbox = player.Bounds;
            Vector2 inset = new Vector2(hitbox.X - player.Location.X, hitbox.Y - player.Location.Y);
            player.Location = instance.PlayerSpawn - new Vector2(hitbox.Width / 2f, hitbox.Height / 2f) - inset;
            player.Velocity = Vector2.Zero;
        }

        /// <summary>Puts the player just inside the door on 'entrySide', keeping their position along the wall.</summary>
        private void PlaceAtEntrance(RoomInstance instance, RoomSide entrySide)
        {
            RoomExit arrival = instance.Exits.FirstOrDefaultOnSide(entrySide);
            Rectangle doorBounds = arrival != null ? arrival.Door.Bounds : FallbackDoor(instance, entrySide);

            Rectangle hitbox = player.Bounds;
            Vector2 inset = new Vector2(hitbox.X - player.Location.X, hitbox.Y - player.Location.Y);
            int x = hitbox.X, y = hitbox.Y;

            switch (entrySide)
            {
                case RoomSide.North: y = doorBounds.Bottom + EntryMargin; x = ClampAlong(hitbox.X, hitbox.Width, doorBounds.Left, doorBounds.Right); break;
                case RoomSide.South: y = doorBounds.Top - hitbox.Height - EntryMargin; x = ClampAlong(hitbox.X, hitbox.Width, doorBounds.Left, doorBounds.Right); break;
                case RoomSide.West: x = doorBounds.Right + EntryMargin; y = ClampAlong(hitbox.Y, hitbox.Height, doorBounds.Top, doorBounds.Bottom); break;
                default: x = doorBounds.Left - hitbox.Width - EntryMargin; y = ClampAlong(hitbox.Y, hitbox.Height, doorBounds.Top, doorBounds.Bottom); break;
            }

            player.Location = new Vector2(x, y) - inset;
            player.Velocity = Vector2.Zero;
        }

        private static int ClampAlong(int start, int size, int low, int high)
        {
            return Math.Max(low, Math.Min(start, high - size));
        }

        private static Rectangle FallbackDoor(RoomInstance instance, RoomSide side)
        {
            // Dead-end door (no exit object): use the wall's door tile found by the run data.
            throw new InvalidOperationException(
                $"Room '{instance.Data.Id}' has no door on its {side} side to arrive through.");
        }
    }

    internal static class RoomExitListExtensions
    {
        public static RoomExit FirstOrDefaultOnSide(this IReadOnlyList<RoomExit> exits, RoomSide side)
        {
            foreach (RoomExit exit in exits)
                if (exit.Side == side) return exit;
            return null;
        }
    }
}
