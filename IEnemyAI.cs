using Microsoft.Xna.Framework;

namespace _3902sprint0
{
    public interface IEnemyAI
    {
        bool IsPaused { get; }

        void Update(Enemy enemy, GameTime gameTime);
	}
}
