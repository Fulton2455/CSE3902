using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace _3902sprint0
{
	public interface IEnemy
	{
		void Update(GameTime gameTime);
		void Draw(SpriteBatch spriteBatch);
	}
}
