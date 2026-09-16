
namespace _3902sprint0 {
    /// <summary>
    /// Interface for controllers that manage player input and actions. Implementing classes should define the update method to handle input and update the player's state based on their position.
    /// </summary>
    public interface IController
    {
        //it updates what the individual controller does and can use the players position.
        public void update(Vector2 playerPosition) { }
}
    

}
