// Connor Fulton
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace _3902sprint0
{
    
    // Loads and holds every texture/font the game needs. 
    
    public class GameAssets
    {
        public Texture2D KnightTexture { get; private set; }
        public Texture2D MagicSwordTexture { get; private set; }
        public Texture2D FireScrollTexture { get; private set; }
        public Texture2D ChestTexture { get; private set; }
        public Texture2D FireballTexture { get; private set; }
        public Texture2D AcidTexture { get; private set; }
        public Texture2D DetonateTexture { get; private set; }
        public Texture2D ShieldOfInvulnerabilityTexture { get; private set; }
        public Texture2D RobeOfInvisibilityTexture { get; private set; }
        public Texture2D PanaceaTexture { get; private set; }
        public Texture2D MagicArmorTexture { get; private set; }
        public Texture2D DeckOfCardsTexture { get; private set; }
        public Texture2D RupeeTexture { get; private set; }
        public Texture2D HeartTexture { get; private set; }
        public Texture2D EnvironmentSpriteSheet { get; private set; }

        public Texture2D EnemySlimeTexture { get; private set; }
        public Texture2D EnemyZombieTexture { get; private set; }
        public Texture2D EnemyMageTexture { get; private set; }
        public Texture2D EnemyBatTexture { get; private set; }
        public Texture2D EnemyToadTexture { get; private set; }
        public Texture2D ShockwavePixel { get; private set; }

        public SpriteFont GameFont { get; private set; }

        public void Load(ContentManager content, GraphicsDevice graphicsDevice)
        {
            KnightTexture = content.Load<Texture2D>("Knight");
            MagicSwordTexture = content.Load<Texture2D>("items/MagicSword");
            FireScrollTexture = content.Load<Texture2D>("items/fireScroll");
            ChestTexture = content.Load<Texture2D>("Chest");
            FireballTexture = content.Load<Texture2D>("Fireball");
            AcidTexture = FireballTexture; // change when a dedicated acid texture exists
            DetonateTexture = content.Load<Texture2D>("Explosion");
            ShieldOfInvulnerabilityTexture = content.Load<Texture2D>("items/shieldOfInvulnerability");
            RobeOfInvisibilityTexture = content.Load<Texture2D>("items/robeOfInvisibility");
            PanaceaTexture = content.Load<Texture2D>("items/panacea");
            MagicArmorTexture = content.Load<Texture2D>("items/healthShield");
            DeckOfCardsTexture = content.Load<Texture2D>("items/deckOfCards");
            RupeeTexture = content.Load<Texture2D>("items/coin3_16x16");
            HeartTexture = content.Load<Texture2D>("Pixel Heart Sprite Sheet 32x32");
            EnvironmentSpriteSheet = content.Load<Texture2D>("environment/environment_spritesheet");

            EnemySlimeTexture = content.Load<Texture2D>("enemySlime");
            EnemyZombieTexture = content.Load<Texture2D>("enemyZombie");
            EnemyMageTexture = content.Load<Texture2D>("enemyMage");
            EnemyBatTexture = content.Load<Texture2D>("enemyBat");
            EnemyToadTexture = content.Load<Texture2D>("enemyToad");

            ShockwavePixel = new Texture2D(graphicsDevice, 1, 1);
            ShockwavePixel.SetData(new[] { Color.White });

            GameFont = content.Load<SpriteFont>("GameFont");
        }
    }
}