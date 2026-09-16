using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3902sprint0
{
    public interface Ientity
    {
        Texture2D Texture { get; }
        void Update(GameTime gameTime);
        void ApplyEffect(Vector2 position);

        void generateEntity(Vector2 position, Vector2 direction);

        void Reset();
        void Draw(SpriteBatch spriteBatch);
    }
}
