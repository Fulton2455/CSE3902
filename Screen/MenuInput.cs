using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace _3902sprint0
{
    
    public class MenuInput
    {
        private readonly ControlBindings controls;
        private MouseState previousMouse;
        private KeyboardState previousKeyboard;

        public Point Pointer { get; private set; }
        public bool Clicked { get; private set; }
        public bool PausePressed { get; private set; }

        public MenuInput(ControlBindings controls)
        {
            this.controls = controls;
        }

        public void Update()
        {
            MouseState mouse = Mouse.GetState();
            KeyboardState keyboard = Keyboard.GetState();

            Clicked = mouse.LeftButton == ButtonState.Pressed
                && previousMouse.LeftButton == ButtonState.Released;

            PausePressed = keyboard.IsKeyDown(controls.PauseKey)
                && previousKeyboard.IsKeyUp(controls.PauseKey);

            Vector2 virtualPosition = ScreenScaler.ToVirtual(mouse.X, mouse.Y);
            Pointer = new Point((int)virtualPosition.X, (int)virtualPosition.Y);

            previousMouse = mouse;
            previousKeyboard = keyboard;
        }
    }
}