namespace _3902sprint0
{
    
    // Owns which screen is showing and the rules for moving between screens.
    public class GameScreenFlow
    {
        private readonly IGameSession session;
        private GameScreenState screenBeforeSettings = GameScreenState.Start;

        public GameScreenState Current { get; private set; } = GameScreenState.Start;

        public GameScreenFlow(IGameSession session)
        {
            this.session = session;
        }

        public void Apply(MenuAction action)
        {
            switch (action)
            {
                case MenuAction.StartGame:
                case MenuAction.Restart:
                    session.StartNewGame();
                    Current = GameScreenState.Playing;
                    break;

                case MenuAction.Resume:
                    Current = GameScreenState.Playing;
                    break;

                case MenuAction.OpenSettings:
                    // Settings can be opened from the start or pause menu; Back returns to whichever it was.
                    screenBeforeSettings = Current;
                    Current = GameScreenState.Settings;
                    break;

                case MenuAction.Back:
                    Current = screenBeforeSettings;
                    break;

                case MenuAction.MainMenu:
                    Current = GameScreenState.Start;
                    break;

                case MenuAction.Quit:
                    session.QuitGame();
                    break;
            }
        }

        /// <summary>Pauses while playing and resumes while paused; does nothing on other screens.</summary>
        public void TogglePause()
        {
            if (Current == GameScreenState.Playing)
                Current = GameScreenState.Paused;
            else if (Current == GameScreenState.Paused)
                Current = GameScreenState.Playing;
        }

        /// <summary>Shows the game-over screen if the player was playing.</summary>
        public void ShowDeathScreen()
        {
            if (Current == GameScreenState.Playing)
                Current = GameScreenState.Death;
        }
    }
}