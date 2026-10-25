// Connor Fulton
using Microsoft.Xna.Framework;
using _3902sprint0.Environment.Tiles;

namespace _3902sprint0
{
    
    // Tracks the current dungeon level and debounces stairs traversal so walking back
    // and forth over a StairsTile doesn't advance the level multiple times per visit.
    public class LevelProgression
    {
        private const float StairsCooldownTime = 1.0f;

        private float stairsCooldown;

        public int CurrentLevel { get; private set; } = 1;

        public void Update(GameTime gameTime)
        {
            if (stairsCooldown > 0)
                stairsCooldown -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void OnStairsTraversed(StairsTile stairs)
        {
            if (stairsCooldown <= 0)
            {
                CurrentLevel++;
                stairsCooldown = StairsCooldownTime;
            }
        }
    }
}