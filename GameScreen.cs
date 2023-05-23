using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace AnotherLib
{
    public class GameScreen
    {
        public readonly GameWindow Window;
        public readonly GraphicsDeviceManager GraphicsDeviceManager;
        public readonly GraphicsDevice GraphicsDevice;
        public static GameScreen ActiveGameScreen;

        public static int resolutionWidth;
        public static int resolutionHeight;
        public static int halfScreenWidth;
        public static int halfScreenHeight;
        public static Rectangle ScreenRectangle;

        public static Vector2 topLeft;
        public static Vector2 topRight;
        public static Vector2 center;
        public static Vector2 bottomLeft;
        public static Vector2 bottomRight;

        /// <summary>
        /// Initializes and sets up the Screen.
        /// </summary>
        /// <param name="window">The MonoGame GameWindow instance.</param>
        /// <param name="graphicsDeviceManager">The graphics device manager.</param>
        public GameScreen(GameWindow window, GraphicsDeviceManager graphicsDeviceManager, int customWidth = 640, int customHeight = 512)
        {
            Window = window;
            GraphicsDeviceManager = graphicsDeviceManager;
            GraphicsDevice = graphicsDeviceManager.GraphicsDevice;
            resolutionWidth = customWidth;
            resolutionHeight = customHeight;
            SetupScreen();
            SetupScreenPositions();
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += ResizeScreen;
            ActiveGameScreen = this;
        }

        public void SetupScreen()
        {
            GraphicsDeviceManager.PreferredBackBufferWidth = resolutionWidth;
            GraphicsDeviceManager.PreferredBackBufferHeight = resolutionHeight;
            GraphicsDeviceManager.ApplyChanges();
            ScreenRectangle = new Rectangle(0, 0, resolutionWidth, resolutionHeight);
            halfScreenWidth = resolutionWidth / 2;
            halfScreenHeight = resolutionHeight / 2;
        }

        public void SetupScreenPositions()
        {
            topLeft = Vector2.Zero;
            topRight = new Vector2(resolutionWidth, 0f);
            center = new Vector2(resolutionWidth, resolutionHeight) / 2f;
            bottomLeft = new Vector2(0f, resolutionHeight);
            bottomRight = new Vector2(resolutionWidth, resolutionHeight);
        }

        public void ResizeScreen(object sender, EventArgs args)
        {
            resolutionWidth = Window.ClientBounds.Width;
            resolutionHeight = Window.ClientBounds.Height;
            halfScreenWidth = resolutionWidth / 2;
            halfScreenHeight = resolutionHeight / 2;
            ScreenRectangle = new Rectangle(0, 0, resolutionWidth, resolutionHeight);
            SetupScreenPositions();
        }
    }
}
