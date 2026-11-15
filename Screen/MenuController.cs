using Microsoft.Xna.Framework;

namespace _3902sprint0
{
   
    // The menu's logic: which button is under the pointer, and which action a click produces.
  
    public class MenuController
    {
        public const int NoSelection = -1;

        //Index of the button under the pointer after the last Update, or NoSelection.
        public int HoveredIndex { get; private set; } = NoSelection;

        public MenuAction Update(MenuDefinition menu, Point screenSize, Point pointer, bool clicked)
        {
            HoveredIndex = FindButtonAt(menu, screenSize, pointer);

            if (HoveredIndex == NoSelection || !clicked)
                return MenuAction.None;

            return menu.Buttons[HoveredIndex].Action;
        }

        private static int FindButtonAt(MenuDefinition menu, Point screenSize, Point pointer)
        {
            for (int i = 0; i < menu.Buttons.Count; i++)
            {
                if (MenuLayout.ButtonBounds(screenSize, i, menu.Buttons.Count).Contains(pointer))
                    return i;
            }

            return NoSelection;
        }
    }
}