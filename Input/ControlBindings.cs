// Connor Fulton
using Microsoft.Xna.Framework.Input;

namespace _3902sprint0
{
    // Mouse buttons that can be bound to an action 
    public enum MouseButton
    {
        Left,
        Right,
        Middle
    }

    
    // Every key and mouse button the game readss. 
    public class ControlBindings
    {
        // General/Testing Purposes
        public Keys QuitKey { get; set; } = Keys.Escape;
        public Keys DieKey { get; set; } = Keys.K;
        public Keys ReviveKey { get; set; } = Keys.Q;
        public Keys ForceItemKey { get; set; } = Keys.G;

        // Movement: each direction has a primary and an alternate key
        public Keys MoveUpKey { get; set; } = Keys.W;
        public Keys MoveUpAltKey { get; set; } = Keys.Up;
        public Keys MoveDownKey { get; set; } = Keys.S;
        public Keys MoveDownAltKey { get; set; } = Keys.Down;
        public Keys MoveLeftKey { get; set; } = Keys.A;
        public Keys MoveLeftAltKey { get; set; } = Keys.Left;
        public Keys MoveRightKey { get; set; } = Keys.D;
        public Keys MoveRightAltKey { get; set; } = Keys.Right;

        // Terrain debug keys
        public Keys IceTerrainKey { get; set; } = Keys.I;
        public Keys SwampTerrainKey { get; set; } = Keys.F;
        public Keys StoneTerrainKey { get; set; } = Keys.T;

        //Inventory slots 1-8 in order. Index 0 selects the first item.
        public Keys[] ItemSlotKeys { get; } =
        {
            Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8
        };

        // Mouse
        public MouseButton AttackButton { get; set; } = MouseButton.Left;
        public MouseButton SecondaryButton { get; set; } = MouseButton.Right;
    }
}