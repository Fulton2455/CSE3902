using _3902sprint0.Environment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3902sprint0
{
    public class chestManager
    {
        public List<Chest> chests;
        public IReadOnlyList<Chest> Chests => chests;
        private Texture2D chestTexture;
        public const float MinimumSpawnDistance = 300f;
        private const int MaxSpawnAttempts = 500;

        private Rectangle movementBounds;
        private Inventory inventory;
        private itemDatabase database;
        private Player player;
        private Room room;

        public chestManager(
            Texture2D chestTexture,
            Inventory inventory,
            itemDatabase database,
             Room room,
            Rectangle movementBounds)
        {
            chests = new List<Chest>();

            this.chestTexture = chestTexture;
            this.inventory = inventory;
            this.database = database;
            this.room = room;
            this.movementBounds = movementBounds;
        }

        public void SetPlayer(Player player)
        {
            this.player = player;
        }
        public void SetMovementBounds(Rectangle movementBounds)
        {
            this.movementBounds = movementBounds;
        }
        public void deleteChest(Chest chest)
        {
            chests.Remove(chest);
        }
        public void CreateChest(Vector2 position)
        {
            Chest newChest = new Chest(
                position,
                inventory,
                database,
                player,
                this);

            newChest.InitializeSprite(chestTexture);

            chests.Add(newChest);
        }



        public void CreateRandomChest()
        {
            Vector2 spawnPosition = ChestCreator.GetSpawnBehavior(
                movementBounds,
                room,
                player,
                48,
                32);

            CreateChest(spawnPosition);
        }



        

       

        public void Update(GameTime gameTime)
        {
            foreach (Chest chest in chests.ToList())
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
