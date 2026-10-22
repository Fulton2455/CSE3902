using System;
using Microsoft.Xna.Framework;

namespace _3902sprint0
{
    public static class ScreenScaler
    {
        public const int VirtualWidth = 1792;
        public const int VirtualHeight = 1280;

        public static Rectangle Destination
        {
            get ; private set;
        } = new Rectangle(0, 0, VirtualWidth, VirtualHeight);

        public static float Scale {  get; private set; } = 1f;

        public static Point FitWindowSize(int availableWidth, int availableHeight)
        {
            float scale = Math.Min(1f, Math.Min(availableWidth / (float)VirtualWidth, availableHeight / (float)VirtualHeight));
            return new Point((int)(VirtualWidth * scale), (int)(VirtualHeight * scale));
        }
        public static void Update(int windowWidth, int windowHeight)
        {
            if (windowHeight <= 0 || windowWidth <= 0)
                return;
            Scale = Math.Min(windowWidth /  (float)VirtualWidth, windowHeight / (float)VirtualHeight);
            int width = (int)Math.Round(VirtualWidth * Scale);
            int height = (int)Math.Round(VirtualHeight * Scale);
            Destination = new Rectangle((windowWidth - width) / 2, (windowHeight - height) / 2, width, height);
        }
        public static Vector2 ToVirtual(int windowX, int windowY)
        {
            return new Vector2((windowX - Destination.X) / Scale, (windowY - Destination.Y) / Scale);
        }
    }

}
