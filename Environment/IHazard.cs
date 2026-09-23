// Connor Fulton
namespace _3902sprint0.Environment
{
    public interface IHazard
    {
        int Damage { get; }
        void ApplyEffect(IDamageable target);
    }
}