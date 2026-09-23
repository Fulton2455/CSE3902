// Connor Fulton

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3902sprint0.Environment.Doors
{
    // A door that stays solid until unlocked with the matching key type.
    
    public abstract class LockedDoorTile : Tile
    {
        public LockType RequiredLock { get; }
        public bool IsUnlocked { get; private set; }
        public override bool IsSolid => !IsUnlocked;

        protected LockedDoorTile(Texture2D texture, Vector2 position, LockType requiredLock, Rectangle sourceRectangle, int width = 64, int height = 64)
            : base(texture, position, sourceRectangle, width, height)
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
            IsUnlocked = true;
            return true;
        }
    }
}