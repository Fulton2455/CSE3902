// Connor Fulton
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using _3902sprint0.Environment;

namespace _3902sprint0
{
    
    // Draws collision-box outlines for the player, tiles, enemies, fireballs, and chests
    // while F1 is held. 
    
    public class DebugOverlay
    {
        private readonly Texture2D pixelTexture;

        public DebugOverlay(GraphicsDevice graphicsDevice)
        {
            pixelTexture = new Texture2D(graphicsDevice, 1, 1);
            pixelTexture.SetData(new[] { Color.White });
        }

        public void Draw(
            SpriteBatch spriteBatch,
            Player player,
            Room room,
            EnemyManager enemyManager,
            fireballManager fireballManager,
            chestManager chestManager)
        {
            if (!Keyboard.GetState().IsKeyDown(Keys.F1))
                return;

            DrawRectOutline(spriteBatch, player.Bounds, Color.Red);

            foreach (var tile in room.Tiles)
                DrawRectOutline(spriteBatch, tile.Bounds, tile.IsSolid ? Color.Cyan : Color.Yellow);

            foreach (Enemy enemy in enemyManager.Enemies)
                DrawRectOutline(spriteBatch, enemy.bounds, Color.Magenta);

            foreach (Fireball fireball in fireballManager.Fireballs)
            {
                if (fireball.IsActive)
                    DrawRectOutline(spriteBatch, fireball.Bounds, Color.White);
                if (fireball.IsExplosionActive)
                    DrawRectOutline(spriteBatch, fireball.ExplosionBounds, Color.Purple);
            }

            foreach (Fireball2 fireball2 in fireballManager.Fireballs2)
            {
                if (fireball2.IsActive)
                    DrawRectOutline(spriteBatch, fireball2.Bounds, Color.White);
            }

            foreach (Chest chest in chestManager.Chests)
                DrawRectOutline(spriteBatch, chest.GetBounds(), Color.Gold);
        }

        private void DrawRectOutline(SpriteBatch spriteBatch, Rectangle rect, Color color)
        {
            const int t = 2;
            spriteBatch.Draw(pixelTexture, new Rectangle(rect.Left, rect.Top, rect.Width, t), color);
            spriteBatch.Draw(pixelTexture, new Rectangle(rect.Left, rect.Bottom - t, rect.Width, t), color);
            spriteBatch.Draw(pixelTexture, new Rectangle(rect.Left, rect.Top, t, rect.Height), color);
            spriteBatch.Draw(pixelTexture, new Rectangle(rect.Right - t, rect.Top, t, rect.Height), color);
        }
    }
}