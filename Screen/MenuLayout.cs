using Microsoft.Xna.Framework;

namespace _3902sprint0
{
    // Where menu pieces go on screen. 
    public static class MenuLayout
    {
        private const int ButtonWidth = 320;
        private const int ButtonHeight = 70;
        private const int ButtonSpacing = 20;
        private const int ButtonBlockOffsetY = 70;  
        private const int TitleOffsetAboveCenter = 180;

        //The rectangle for button number 'index' in a column of 'count' buttons centered on the screen.
        public static Rectangle ButtonBounds(Point screenSize, int index, int count)
        {
            int totalHeight = count * ButtonHeight + (count - 1) * ButtonSpacing;
            int x = screenSize.X / 2 - ButtonWidth / 2;
            int y = screenSize.Y / 2 - totalHeight / 2 + index * (ButtonHeight + ButtonSpacing) + ButtonBlockOffsetY;

            return new Rectangle(x, y, ButtonWidth, ButtonHeight);
        }

        // The y position of the top of the title text.
        public static float TitleTop(Point screenSize)
        {
            return screenSize.Y / 2f - TitleOffsetAboveCenter;
        }
    }
}