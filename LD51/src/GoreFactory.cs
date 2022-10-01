
using Microsoft.Xna.Framework;

namespace LD51
{
    public class GoreFactory
    {
        private static Rand random = new Rand();

        public static void SpawnRandomGoreExplosion(Enemy enemy)
        {
            int numOfGoryBits = random.NextInt(enemy.Size * 4, enemy.Size * 8);
            for (int i = 0; i < numOfGoryBits; i++)
            {
                Gore.Spawn(
                    enemy.Position,
                    new Vector2(random.NextInt(-10, 10), random.NextInt(-10, 10)).Normalized(),
                    random.NextInt(512 / 8, 512 / 4),
                    new Point(random.NextInt(1, 4), random.NextInt(1, 4)));
            }
        }
    }
}
