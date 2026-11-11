using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3902sprint0
{
    
    //The single object the game talks to for menus. 
    
    public class GameScreenManager
    {
        private readonly Point screenSize;
        private readonly MenuInput input;
        private readonly MenuController controller = new MenuController();
        private readonly MenuRenderer renderer;
        private readonly GameScreenFlow flow;

        public GameScreenState State => flow.Current;
        public bool IsPlaying => flow.Current == GameScreenState.Playing;

        /// <summary>
        /// False on full-screen menus (start, settings), where the game world is hidden and may not
        /// exist yet. True while playing and on the pause and death overlays.
        /// </summary>
        public bool IsWorldVisible =>
            !(GameMenus.TryGet(flow.Current, out MenuDefinition menu) && menu.HasOpaqueBackground);

        public GameScreenManager(IGameSession session, ControlBindings controls, SpriteFont font,
            Texture2D pixel, Point screenSize)
        {
            this.screenSize = screenSize;
            input = new MenuInput(controls);
            renderer = new MenuRenderer(pixel, font);
            flow = new GameScreenFlow(session);
        }

        /// <summary>Call every frame, even while playing.</summary>
        public void Update()
        {
            input.Update();

            if (input.PausePressed)
            {
                flow.TogglePause();
                return;
            }

            if (!GameMenus.TryGet(flow.Current, out MenuDefinition menu))
                return;

            MenuAction action = controller.Update(menu, screenSize, input.Pointer, input.Clicked);
            flow.Apply(action);
        }

        /// <summary>Draw after the game and HUD, inside the same pass, so menus appear on top.</summary>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (!GameMenus.TryGet(flow.Current, out MenuDefinition menu))
                return;

            renderer.Draw(spriteBatch, screenSize, menu, controller.HoveredIndex);
        }

        //Call when the player's health reaches zero.
        public void ShowDeathScreen()
        {
            flow.ShowDeathScreen();
        }
    }
}