// Connor Fulton
namespace _3902sprint0.Environment.Doors
{
    
    // A door that can be opened without its normal trigger. Used when the player walks through a
    // door and arrives at its far side: the far side must open too, or the player is walled in.
   
    public interface IOpenable
    {
        void ForceOpen();
    }
}