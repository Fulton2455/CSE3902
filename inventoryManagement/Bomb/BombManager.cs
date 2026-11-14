
using _3902sprint0.Environment;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace _3902sprint0
{
    public class BombManager
    {
        private readonly List<Bomb> activeBombs = new List<Bomb>();
        private readonly Texture2D bombTexture;
        private readonly Texture2D detonateTexture;
        private readonly itemDatabase database;
        private readonly Room room;
        private readonly Rectangle movementBounds;
        private Player player;

        public List<Bomb> Bombs => activeBombs;

        public BombManager(Texture2D bombTexture, Texture2D detonateTexture,
            Inventory inventory, itemDatabase database, Room room,
            Rectangle movementBounds)
        {
            this.bombTexture = bombTexture;
            this.detonateTexture = detonateTexture;
            this.database = database;
            this.room = room;
            this.movementBounds = movementBounds;
        }

        public void SetPlayer(Player player)
        {
            this.player = player;
        }

        public void CreateBomb(Vector2 location)
        {
            activeBombs.Add(new Bomb(location, bombTexture, detonateTexture, false));
        }

        public void SpawnActiveBomb()
        {
            if (player == null || player.BombCount <= 0)
                return;

            player.BombCount--;
            activeBombs.Add(new Bomb(player.Location, bombTexture, detonateTexture, true));
        }

        public void Update(GameTime gameTime)
        {
            foreach (Bomb bomb in activeBombs.ToList())
            {
                bomb.Update(gameTime, player);

                if (bomb.IsCollected)
                    activeBombs.Remove(bomb);
            }
        }
        public void Clear()
        {
            activeBombs.Clear();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Bomb bomb in activeBombs)
                bomb.Draw(spriteBatch);
        }
    }
}
