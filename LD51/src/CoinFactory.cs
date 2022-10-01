using Microsoft.Xna.Framework;

namespace LD51
{
    public class CoinFactory
    {
        private static Rand random = new Rand();

        public static void TryToSpawn(float percentDropChange, Vector2 position)
        {
            if ((random.NextInt(0, 1000) / 10) <= percentDropChange)
            {
                Coin.Spawn(position);
            }
        }
    }
}
