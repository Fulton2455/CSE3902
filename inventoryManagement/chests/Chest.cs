using _3902sprint0;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3902sprint0
{
    public class Chest
    {
        public Vector2 Location;
        public bool isOpen = false;

        private Player player;
        private Mousecontroller Mouse;
        private ChestSprite ChestSprite1;
        private Inventory inventory;
        private itemDatabase database;
        private chestManager chestManager;

        public Chest(Vector2 startingLocation, Inventory inventory, itemDatabase database, Player player, chestManager chestManager)
        {
            Location = startingLocation;
            ChestSprite1 = new ChestSprite();
            Mouse = new Mousecontroller();
            this.player = player;
            this.chestManager = chestManager;

            this.inventory = inventory;
            this.database = database;
        }

        public void Update(GameTime gameTime)
        {
            

            ChestSprite1.UpdateSprite(gameTime);
        
            generateItem();
              
        }
        public void InitializeSprite(Texture2D texture)
        {
            ChestSprite1.SetLocation(Location);

            ChestSprite1.Initialize(texture);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            ChestSprite1.Draw(spriteBatch);
        }
        public void generateItem()
        {
            if(database.items.Count == 0)
            {
                chestManager.deleteChest(this);
                return;
            }
            Vector2 mousePosition = Mouse.RightClickPressed();
           
            if (mousePosition != Vector2.Zero && !isOpen)
            {
                float distance = Vector2.Distance(player.getPosition(), Location);
                if (distance <= 300)
                {
                    if (mousePosition.X >= Location.X &&
                    mousePosition.X <= Location.X + 48 &&
                    mousePosition.Y >= Location.Y &&
                    mousePosition.Y <= Location.Y + 32)
                    {
                        Debug.WriteLine("3");

                        Iitem item = database.RandomItem();
                        inventory.AddItem(item);
                        database.items.Remove(item);
                        ChestMessage.Show(item.Name, Color.Gold, Location);
                        //must add to deleted list in future

                        Debug.WriteLine("Item generated at chest location: " + Location);

                        isOpen = true;

                        // Tell the sprite to play the opening animation
                        ChestSprite1.Open();
                    }
                }
            }
        }
    }
}
