
using Microsoft.Xna.Framework;

namespace LD51
{
    public class GoreFactory
    {
        private static readonly float _initialGoreSpeed = Data.Get<float>("goreInitialSpeed");

        private static Rand random = new Rand();

        public static void SpawnRandomGoreExplosion(IExplodable explodable)
        {
            SpawnRandomGoreExplosion(explodable, _initialGoreSpeed);
        }

        public static void SpawnRandomGoreExplosion(IExplodable explodable, float initialDebrisSpeed)
        {
            int numOfGoryBits = random.NextInt(explodable.Size * 4, explodable.Size * 8);
            for (int i = 0; i < numOfGoryBits; i++)
            {
                Gore.Spawn(
                    explodable.Center,
                    new Vector2(random.NextInt(-10, 10), random.NextInt(-10, 10)).Normalized(),
                    random.NextInt((int)initialDebrisSpeed / 2, (int)initialDebrisSpeed),
                    new Point(random.NextInt(1, 4), random.NextInt(1, 4)),
                    explodable.DebrisColor);
            }
        }
    }
}
