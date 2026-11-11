using System.Collections.Generic;

namespace _3902sprint0
{
    //Everything that defines one menu screen
    public class MenuDefinition
    {
        public string Title { get; }
        public IReadOnlyList<MenuButton> Buttons { get; }

        // True for full-screen menus (start, settings) and false for overlays drawn over the game.
        public bool HasOpaqueBackground { get; }

        public MenuDefinition(string title, bool hasOpaqueBackground, params MenuButton[] buttons)
        {
            Title = title;
            HasOpaqueBackground = hasOpaqueBackground;
            Buttons = buttons;
        }
    }
}