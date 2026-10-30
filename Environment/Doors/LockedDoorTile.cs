using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using _3902sprint0.Environment.Doors;
namespace _3902sprint0.Environment.Doors
{
   
    // A door that stays solid until unlocked with the matching key type.
    public abstract class LockedDoorTile : Tile, IOpenable
    {
        private readonly Rectangle? unlockedSource;

        public LockType RequiredLock { get; }
        public bool IsUnlocked { get; private set; }
        public override bool IsSolid => !IsUnlocked;

        protected LockedDoorTile(
            Texture2D texture,
            Vector2 position,
            LockType requiredLock,
            Rectangle sourceRectangle,
            int width = 64,
            int height = 64,
            Rectangle? unlockedSource = null)
            : base(texture, position, sourceRectangle, width, height)
        {
            RequiredLock = requiredLock;
            this.unlockedSource = unlockedSource;
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

        //Opens the door without spending a key (the player came through from the other side).
        public void ForceOpen()
        {
            Unlock();
        }

        private void Unlock()
        {
            IsUnlocked = true;

            if (unlockedSource.HasValue)
                sourceRectangle = unlockedSource.Value;
        }
    }
}