using Microsoft.Xna.Framework.Input;



namespace _3902sprint0
{

    /// <summary>
    /// Controller class that handles keyboard input for player movement and actions. It implements the IController interface and provides methods to update the player's direction based on keyboard input, check for quit and die actions.
    /// </summary>
    public class KeyboardController : IController
    {
        // stores the current direction of movement based on keyboard input.
        public Vector2 direction  { get; private set; }
        private KeyboardState currentState;
        private KeyboardState previousState;



        // starts the proccess of exiting the game if the escape key is pressed
        public bool isQuit()
        {
            KeyboardState state = Keyboard.GetState();
            return state.IsKeyDown(Keys.Escape);
        }

        // starts the proccess of dying if the K key is pressed
        public bool die() {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.K))
            {
                return true;
            }
            return false;
            
        }
        public bool revive()
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Q))
            {
                return true;
            }
            return false;

        }
        public string terrain()
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.I))
            {
                return "Ice";
            }
            else if (state.IsKeyDown(Keys.F))
            {
                return "Swamp";
            }
            else if (state.IsKeyDown(Keys.T))
            {
                return "Stone";
            }
            return "null";
        }



        public int items()
        {
            if (currentState.IsKeyDown(Keys.D1) &&
                previousState.IsKeyUp(Keys.D1))
                return 0;

            if (currentState.IsKeyDown(Keys.D2) &&
                previousState.IsKeyUp(Keys.D2))
                return 1;

            if (currentState.IsKeyDown(Keys.D3) &&
                previousState.IsKeyUp(Keys.D3))
                return 2;

            if (currentState.IsKeyDown(Keys.D4) &&
                previousState.IsKeyUp(Keys.D4))
                return 3;

            if (currentState.IsKeyDown(Keys.D5) &&
                previousState.IsKeyUp(Keys.D5))
                return 4;

            if (currentState.IsKeyDown(Keys.D6) &&
                previousState.IsKeyUp(Keys.D6))
                return 5;

            if (currentState.IsKeyDown(Keys.D7) &&
                previousState.IsKeyUp(Keys.D7))
                return 6;

            if (currentState.IsKeyDown(Keys.D8) &&
                previousState.IsKeyUp(Keys.D8))
                return 7;

            return -1;
        }
        /// <summary>
        /// Updates the controller's state based on keyboard input.
        /// </summary>
        public void update()
        {
            previousState = currentState;
             currentState = Keyboard.GetState();
            direction = Vector2.Zero;
            if (currentState.IsKeyDown(Keys.W))
                direction += new Vector2(0, -1);

            if (currentState.IsKeyDown(Keys.S))
                direction += new Vector2(0, 1);

            if (currentState.IsKeyDown(Keys.A))
                direction += new Vector2(-1, 0);

            if (currentState.IsKeyDown(Keys.D))
                direction += new Vector2(1, 0);
            // Normalize the direction vector to ensure consistent movement speed in all directions.
            if (direction != Vector2.Zero)
                direction.Normalize();

            

        }
    }

}
