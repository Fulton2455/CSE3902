using _3902sprint0.Environment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3902sprint0
{
    internal class chestManager
    {
        private List<Chest> chests;
        public IReadOnlyList<Chest> Chests => chests;
        private Texture2D chestTexture;
        private Inventory inventory;
        private itemDatabase database;
        private Player player;
        private Room room;

        public chestManager(
           Texture2D chestTexture,
           Inventory inventory,
           itemDatabase database,
           Player player)
        {
            chests = new List<Chest>();

            this.chestTexture = chestTexture;
            this.inventory = inventory;
            this.database = database;
            this.player = player;


        }
        public void CreateChest(Vector2 position)
        {
            Chest newChest = new Chest(
                position,
                inventory,
                database,
                player);

            newChest.InitializeSprite(chestTexture);

            chests.Add(newChest);
        }
        public void SetRoom(Room room)
        {
            this.room = room;
        }


       
        public void Update(GameTime gameTime)
        {
            foreach (Chest chest in chests)
            {
                chest.Update(gameTime);
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Chest chest in chests)
            {
                chest.Draw(spriteBatch);
            }
        }
    }
}
