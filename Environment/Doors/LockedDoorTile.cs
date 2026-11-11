// Connor Fulton
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3902sprint0.Environment.Doors
{
    // A door that stays solid until unlocked with the matching key type.
    public abstract class LockedDoorTile : DoorTile, IOpenable
    {
        public LockType RequiredLock { get; }
        public bool IsUnlocked { get; private set; }
        public override bool IsSolid => !IsUnlocked;

        protected LockedDoorTile(Texture2D spriteSheet, Rectangle bounds, DoorArt art, LockType requiredLock)
            : base(spriteSheet, bounds, art)
        {
            RequiredLock = requiredLock;
        }

        public bool TryUnlock(IKeyHolder keyHolder)
        {
            if (IsUnlocked)
                return true;

            if (!keyHolder.HasKey(RequiredLock))
                return false;

            keyHolder.ConsumeKey(RequiredLock);
            Unlock();
            return true;
        }

        // If the player came through the other side.
        public void ForceOpen()
        {
            Unlock();
        }

        private void Unlock()
        {
            IsUnlocked = true;
            ShowOpen();
        }
    }
}