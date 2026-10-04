using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace _3902sprint0
{
	public interface IEnemy
	{
		void Update(GameTime gameTime, float terrainAccelaration);
		void Draw(SpriteBatch spriteBatch);
	}
}
