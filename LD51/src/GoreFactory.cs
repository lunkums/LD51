
using Microsoft.Xna.Framework;

namespace LD51
{
    public class GoreFactory
    {
        private static Rand random = new Rand();

        public static void SpawnRandomGoreExplosion(IExplodeable explodeable, float initialDebrisSpeed)
        {
            int numOfGoryBits = random.NextInt(explodeable.Size * 4, explodeable.Size * 8);
            for (int i = 0; i < numOfGoryBits; i++)
            {
                Gore.Spawn(
                    explodeable.Center,
                    new Vector2(random.NextInt(-10, 10), random.NextInt(-10, 10)).Normalized(),
                    random.NextInt((int)initialDebrisSpeed / 2, (int)initialDebrisSpeed),
                    new Point(random.NextInt(1, 4), random.NextInt(1, 4)),
                    explodeable.DebrisColor);
            }
        }
    }
}
