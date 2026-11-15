using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3902sprint0
{
    
    public class Game1 : Game, IGameSession
    {
        public static Player currentPlayer { get; private set; }

        public readonly static bool IsDesktop = OperatingSystem.IsMacOS() || OperatingSystem.IsLinux() || OperatingSystem.IsWindows();

        private readonly GraphicsDeviceManager deviceManager;
        private SpriteBatch spriteBatch;
        private RenderTarget2D sceneTarget;
        private bool applyingWindowSize;

        private ControlBindings controls;
        private GameAssets assets;
        private GameScreenManager screens;
        private GameWorld world;

        public Game1()
        {
            deviceManager = new GraphicsDeviceManager(this);
            DisplayMode desktop = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
            Point startSize = ScreenScaler.FitWindowSize((int)(desktop.Width * 0.9f), (int)(desktop.Height * 0.9f));
            deviceManager.PreferredBackBufferWidth = startSize.X;
            deviceManager.PreferredBackBufferHeight = startSize.Y;
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += OnClientSizeChanged;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            controls = new ControlBindings();
            IsMouseVisible = true;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            sceneTarget = new RenderTarget2D(GraphicsDevice, ScreenScaler.VirtualWidth, ScreenScaler.VirtualHeight);
            ScreenScaler.Update(GraphicsDevice.PresentationParameters.BackBufferWidth,
                GraphicsDevice.PresentationParameters.BackBufferHeight);

            SoundManager.LoadContent(Content);

            assets = new GameAssets();
            assets.Load(Content, GraphicsDevice);

            screens = new GameScreenManager(this, controls, assets.GameFont, assets.WhitePixel,
                new Point(ScreenScaler.VirtualWidth, ScreenScaler.VirtualHeight));

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            screens.Update();

            if (screens.IsPlaying)
            {
                world.Update(gameTime);
                if (world.IsDeathAnimationFinished) screens.ShowDeathScreen();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(sceneTarget);
            GraphicsDevice.Clear(screens.IsWorldVisible ? world.BackgroundColor : Color.Black);

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            if (screens.IsWorldVisible)
                world.Draw(spriteBatch, gameTime);
            screens.Draw(spriteBatch);
            spriteBatch.End();

            PresentScene();

            base.Draw(gameTime);
        }

        // IGameSession: what the menus ask of the game.
        public void StartNewGame()
        {
            world = new GameWorld(GraphicsDevice, Content, assets);
            currentPlayer = world.Player;
        }

        public void QuitGame()
        {
            Exit();
        }

       
        private void PresentScene()
        {
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.Black);

            bool wholeNumberScale = ScreenScaler.Scale == (float)Math.Floor(ScreenScaler.Scale);
            spriteBatch.Begin(samplerState: wholeNumberScale ? SamplerState.PointClamp : SamplerState.LinearClamp);
            spriteBatch.Draw(sceneTarget, ScreenScaler.Destination, Color.White);
            spriteBatch.End();
        }

        private void OnClientSizeChanged(object sender, EventArgs e)
        {
            if (applyingWindowSize)
                return;
            int width = Window.ClientBounds.Width;
            int height = Window.ClientBounds.Height;
            if (width <= 0 || height <= 0)
                return;

            applyingWindowSize = true;
            deviceManager.PreferredBackBufferWidth = width;
            deviceManager.PreferredBackBufferHeight = height;
            deviceManager.ApplyChanges();
            ScreenScaler.Update(width, height);
            applyingWindowSize = false;
        }
    }
}