// Connor Fulton
using Microsoft.Xna.Framework.Graphics;

namespace _3902sprint0.Environment.Rooms
{
    public class RoomTextures
    {
        public Texture2D Environment { get; }
        public Texture2D Doors { get; }
        public Texture2D SmallKey { get; }

        public RoomTextures(Texture2D environment, Texture2D doors, Texture2D smallKey)
        {
            Environment = environment;
            Doors = doors;
            SmallKey = smallKey;
        }
    }
}
