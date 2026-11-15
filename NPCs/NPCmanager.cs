using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using _3902sprint0.Environment.Rooms;

namespace _3902sprint0.NPCs
{
    
    public sealed class NPCmanager
    {
        private sealed class ShopOffer
        {
            public string Name { get; }
            public int Price { get; }
            public Func<Iitem> CreateItem { get; }

            public ShopOffer(string name, int price, Func<Iitem> createItem)
            {
                Name = name;
                Price = price;
                CreateItem = createItem;
            }
        }

        private readonly Player player;
        private readonly Inventory inventory;
        private readonly RoomManager rooms;
        private readonly Texture2D pixel;
        private readonly SpriteFont font;
        private readonly OldMan oldMan = new OldMan(new Vector2(900, 480));
        private readonly ShopOffer[] offers;
        private bool previousInteract;
        private bool previousBuy1;
        private bool previousBuy2;
        private bool previousBuy3;
        private bool shopOpen;
        private string statusMessage = "";
        private float statusTimer;
        private const float InteractionDistance = 145f;

        public NPCmanager(Player player, Inventory inventory, RoomManager rooms,
            Texture2D pixel, SpriteFont font, Texture2D panaceaTexture,
            Texture2D shieldTexture, Texture2D fireScrollTexture, fireballManager fireballs)
        {
            this.player = player;
            this.inventory = inventory;
            this.rooms = rooms;
            this.pixel = pixel;
            this.font = font;

 
            offers = new[]
            {
                new ShopOffer("option 1)", 1,
                    () => new PanaceaBottle(panaceaTexture)),
                new ShopOffer("option 2", 2,
                    () => new shieldOfInvulnerability(shieldTexture)),
                new ShopOffer("option 3", 3,
                    () => new fireScroll(fireScrollTexture, fireballs))
            };
        }

        public void Update(GameTime gameTime)
        {
            bool inShopRoom = rooms.Current.Data.Id == "d1_c6";
            bool nearby = inShopRoom && Vector2.Distance(player.getPosition(), oldMan.Position) <= InteractionDistance;
            KeyboardState keys = Keyboard.GetState();
            bool interact = keys.IsKeyDown(Keys.E);
            bool buy1 = keys.IsKeyDown(Keys.D1) || keys.IsKeyDown(Keys.NumPad1);
            bool buy2 = keys.IsKeyDown(Keys.D2) || keys.IsKeyDown(Keys.NumPad2);
            bool buy3 = keys.IsKeyDown(Keys.D3) || keys.IsKeyDown(Keys.NumPad3);

            if (nearby && interact && !previousInteract)
                shopOpen = !shopOpen;
            if (!nearby || !inShopRoom)
                shopOpen = false;

            if (shopOpen)
            {
                if (buy1 && !previousBuy1) Purchase(0);
                if (buy2 && !previousBuy2) Purchase(1);
                if (buy3 && !previousBuy3) Purchase(2);
            }

            if (statusTimer > 0)
                statusTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            previousInteract = interact;
            previousBuy1 = buy1;
            previousBuy2 = buy2;
            previousBuy3 = buy3;
        }

        private void Purchase(int offerIndex)
        {
            ShopOffer offer = offers[offerIndex];
            
            
            if (player.RupeeCount < offer.Price)
            {
                ShowStatus("Not enough rupees! This costs " + offer.Price + ".");
                return;
            }

            Iitem item = offer.CreateItem();
            inventory.AddItem(item);

            player.RupeeCount -= offer.Price;
            ShowStatus("Purchased " + offer.Name + "!");
        }

        private void ShowStatus(string message)
        {
            statusMessage = message;
            statusTimer = 2.5f;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (rooms.Current.Data.Id != "d1_c6") return;

            oldMan.Draw(spriteBatch, pixel);
           

            bool nearby = Vector2.Distance(player.getPosition(), oldMan.Position) <= InteractionDistance;
            if (nearby && !shopOpen)
                spriteBatch.DrawString(font, "Press E to browse shop", oldMan.Position + new Vector2(-95, -55), Color.White);

            if (shopOpen)
            {
                Rectangle box = new Rectangle(300, 785, 1220, 340);
                spriteBatch.Draw(pixel, box, new Color(15, 15, 25, 245));
                spriteBatch.Draw(pixel, new Rectangle(box.X, box.Y, box.Width, 4), Color.Goldenrod);
                spriteBatch.DrawString(font, "OLD MAN'S SHOP", new Vector2(box.X + 28, box.Y + 18), Color.Gold);
                spriteBatch.DrawString(font, "Your rupees: " + player.RupeeCount,
                    new Vector2(box.X + 850, box.Y + 18), Color.LightGreen);

                for (int i = 0; i < offers.Length; i++)
                {
                    ShopOffer offer = offers[i];
                    string line = (i + 1) + ". " + offer.Name + "  -  " + offer.Price + " rupee(s)";
                    spriteBatch.DrawString(font, line,
                        new Vector2(box.X + 35, box.Y + 75 + i * 55), Color.White);
                }

                spriteBatch.DrawString(font, "Press 1 / 2 / 3 to purchase. Press E to leave.",
                    new Vector2(box.X + 35, box.Bottom - 55), Color.LightGray);
                if (statusTimer > 0)
                    spriteBatch.DrawString(font, statusMessage,
                        new Vector2(box.X + 620, box.Y + 125), Color.Yellow);
            }
        }
    }
}
