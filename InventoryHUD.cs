using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3902sprint0
{
    public class InventoryHUD
    {
        private Texture2D pixel;

        private int boxWidth = 100;
        private int boxHeight = 100;
        private int spacing = 10;
       
        private int itemNum=0;
        private float flashTimer = 0f;
        private int flashSlot = -1;

        public void FlashSlot(int slot)
        {
            flashSlot = slot;
            flashTimer = 0.2f;
        }

        public InventoryHUD(GraphicsDevice graphicsDevice)
        {
            pixel = new Texture2D(graphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, Inventory inventory, GameTime gameTime)
        {
            int y = graphicsDevice.Viewport.Height - boxHeight - 20;

            for (int i = 0; i < 8; i++)
            {
                int x = 20 + i * (boxWidth + spacing);

                Rectangle box = new Rectangle(
                    x,
                    y,
                    boxWidth,
                    boxHeight
                );
                Color boxColor = Color.Black * 0.5f;
                if (i == flashSlot && flashTimer > 0)
                {
                    boxColor = Color.White;
                }

                spriteBatch.Draw(
                    pixel,
                    box,
                    boxColor
                );
            }
            Update(inventory, spriteBatch, graphicsDevice, gameTime);
        }
        public void Update(Inventory inventory, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, GameTime gameTime)
        {
            if (flashTimer > 0)
            {
                flashTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            itemNum = 0;
            foreach (Iitem item in inventory.items)
            {
                int x = 20 + itemNum * (boxWidth + spacing);
                int y = graphicsDevice.Viewport.Height - boxHeight - 20;
                Rectangle itemBox = new Rectangle(
                 x,
                 y,
                 boxWidth,
                 boxHeight
                 );
                spriteBatch.Draw(item.Texture, itemBox, Color.White);
                itemNum++;
            }
        }
    }
}
