using Microsoft.Xna.Framework.Input;


namespace _3902sprint0 {
    /// <summary>
    /// Controller class that handles mouse input for aiming and actions. It implements the IController interface and provides methods to update the player's aim direction based on mouse position and check for left click actions.
    /// </summary>
    public class Mousecontroller : IController
    {
        /// <summary>
        /// Gets the current aim direction based on the mouse position relative to the player's position. The aim direction is normalized to ensure consistent aiming behavior.
        /// </summary>
        public Vector2 AimDirection { get; private set; }

        /// <summary>
        /// Checks if the left mouse button is currently pressed. This method can be used to trigger actions such as shooting or interacting with objects in the game.
        /// </summary>
        /// <returns></returns>
        public bool IsLeftClickPressed()
        {
            MouseState state = Mouse.GetState();

            return state.LeftButton == ButtonState.Pressed;
        }
        public Vector2 RightClickPressed()
        {
            MouseState state = Mouse.GetState();
            if (state.RightButton == ButtonState.Pressed)
            {
                Vector2 MousePosition = new Vector2(state.X, state.Y);
                return MousePosition;
            }
            else
            {
                return Vector2.Zero;
            }
            
        }

        /// <summary>
        /// Updates the aim direction based on the current mouse position and the player's position. This method calculates the vector from the player to the mouse cursor and normalizes it to determine the direction in which the player is aiming.
        /// </summary>
        /// <param name="playerPosition"></param>
        public void update(Vector2 playerPosition)
        {
            MouseState state = Mouse.GetState();

            Vector2 mousePosition = new Vector2(state.X, state.Y);
            AimDirection = mousePosition - playerPosition;

            //gives a unit vector for the direction of the mouse from the player
            if (AimDirection != Vector2.Zero)
                AimDirection.Normalize();
        }
    }

}
