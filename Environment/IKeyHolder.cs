namespace _3902sprint0.Environment
{
    /// <summary>Implemented by Inventory so doors can check/consume keys and pickups can add them.</summary>
    public interface IKeyHolder
    {
        void AddKey(LockType lockType);
        bool HasKey(LockType lockType);
        void ConsumeKey(LockType lockType);
    }
}
