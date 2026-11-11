using System.Collections.Generic;

namespace _3902sprint0
{
    
    // The one place that says which menu belongs to which screen. To add or reorder a button,
    // change it here.
    public static class GameMenus
    {
        private static readonly Dictionary<GameScreenState, MenuDefinition> menus =
            new Dictionary<GameScreenState, MenuDefinition>
            {
                [GameScreenState.Start] = new MenuDefinition("THE LEGEND OF ZELDA", true,
                    new MenuButton("START", MenuAction.StartGame),
                    new MenuButton("SETTINGS", MenuAction.OpenSettings),
                    new MenuButton("QUIT", MenuAction.Quit)),

                [GameScreenState.Settings] = new MenuDefinition("SETTINGS", true,
                    new MenuButton("BACK", MenuAction.Back)),

                [GameScreenState.Paused] = new MenuDefinition("PAUSED", false,
                    new MenuButton("RESUME", MenuAction.Resume),
                    new MenuButton("RESTART", MenuAction.Restart),
                    new MenuButton("SETTINGS", MenuAction.OpenSettings),
                    new MenuButton("MAIN MENU", MenuAction.MainMenu)),

                [GameScreenState.Death] = new MenuDefinition("GAME OVER", false,
                    new MenuButton("RESTART", MenuAction.Restart),
                    new MenuButton("MAIN MENU", MenuAction.MainMenu))
            };

        //Finds the menu for a screen. Returns false for screens without one (Playing).
        public static bool TryGet(GameScreenState state, out MenuDefinition menu)
        {
            return menus.TryGetValue(state, out menu);
        }
    }
}