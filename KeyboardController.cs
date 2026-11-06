using Microsoft.Xna.Framework.Input;

namespace _3902sprint0
{

    /// <summary>
    /// Controller class that handles keyboard input for player movement and actions. It implements the IController interface and provides methods to update the player's direction based on keyboard input, check for quit and die actions.
    /// Which keys trigger each action comes from ControlBindings, so they can be changed at runtime.
    /// </summary>
    public class KeyboardController : IController
    {
        private readonly ControlBindings bindings;

        // stores the current direction of movement based on keyboard input.
        public Vector2 direction  { get; private set; }
        private KeyboardState currentState;
        private KeyboardState previousState;

        public KeyboardController() : this(new ControlBindings())
        {
        }

        public KeyboardController(ControlBindings bindings)
        {
            this.bindings = bindings;
        }

        // starts the proccess of exiting the game if the quit key is pressed
        public bool isQuit()
        {
            return Keyboard.GetState().IsKeyDown(bindings.QuitKey);
        }

        // starts the proccess of dying if the die key is pressed
        public bool die()
        {
            return Keyboard.GetState().IsKeyDown(bindings.DieKey);
        }

        public bool forceItem()
        {
            return Keyboard.GetState().IsKeyDown(bindings.ForceItemKey);
        }

        public bool revive()
        {
            return Keyboard.GetState().IsKeyDown(bindings.ReviveKey);
        }

        public string terrain()
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(bindings.IceTerrainKey))
            {
                return "Ice";
            }
            else if (state.IsKeyDown(bindings.SwampTerrainKey))
            {
                return "Swamp";
            }
            else if (state.IsKeyDown(bindings.StoneTerrainKey))
            {
                return "Stone";
            }
            return "null";
        }

        // returns the 0-based inventory slot whose key was just pressed this frame, or -1 if none
        public int items()
        {
            for (int slot = 0; slot < bindings.ItemSlotKeys.Length; slot++)
            {
                Keys key = bindings.ItemSlotKeys[slot];
                if (currentState.IsKeyDown(key) && previousState.IsKeyUp(key))
                    return slot;
            }

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

            if (IsEitherDown(bindings.MoveUpKey, bindings.MoveUpAltKey))
                direction += new Vector2(0, -1);

            if (IsEitherDown(bindings.MoveDownKey, bindings.MoveDownAltKey))
                direction += new Vector2(0, 1);

            if (IsEitherDown(bindings.MoveLeftKey, bindings.MoveLeftAltKey))
                direction += new Vector2(-1, 0);

            if (IsEitherDown(bindings.MoveRightKey, bindings.MoveRightAltKey))
                direction += new Vector2(1, 0);

            // Normalize the direction vector to ensure consistent movement speed in all directions.
            if (direction != Vector2.Zero)
                direction.Normalize();
        }

        private bool IsEitherDown(Keys primary, Keys alternate)
        {
            return currentState.IsKeyDown(primary) || currentState.IsKeyDown(alternate);
        }
    }

}