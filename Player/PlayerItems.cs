using Microsoft.Xna.Framework;
using System;

namespace _3902sprint0
{
    public class PlayerItems
    {
        private readonly Player p;

        public PlayerItems(Player player)
        {
            p = player;
        }

        public void Update(GameTime gameTime)
        {
            RegenerateMana(gameTime);
        }

        internal void RegenerateMana(GameTime gameTime)
        {
            if (p.mana >= Player.MaxMana)
            {
                p.mana = Player.MaxMana;
                p.manaRegenerationTimer = 0f;
                return;
            }

            p.manaRegenerationTimer +=
                (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (p.manaRegenerationTimer >= 1f)
            {
                p.mana = Math.Min(
                    p.mana + (int)(
                        p.manaRegenerationTimer *
                        Player.ManaRegenerationRate),
                    Player.MaxMana);

                p.manaRegenerationTimer %= 1f;
            }
        }

        public void UseItem(GameTime gameTime)
        {
            int itemNumber = p.Keyboard.items();

            if (itemNumber >= 0 && itemNumber < p.inventory.items.Count)
            {
                Iitem item = p.inventory.items[itemNumber];

                if (item.IsOffCooldown)
                {
                    item.Use(p, gameTime);
                    p.inventoryHUD.FlashSlot(itemNumber);
                }
            }
        }

        public void ForceChestItem()
        {
            if (p.chestManager.chests.Count == 0)
                return;

            p.chestManager.chests[0].ForceGenerateItem();
        }
    }
}