// Connor Fulton

namespace _3902sprint0.Environment
{
    // Implement on Inventory so locked-door tiles can consume keys.
    public interface IKeyHolder
    {
        bool HasKey(LockType lockType);
        void ConsumeKey(LockType lockType);
    }
}