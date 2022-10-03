using Microsoft.Xna.Framework;

namespace LD51
{
    public class Arena
    {
        private Rand rand;
        private int maxNumOfEnemies;

        public Arena()
        {
            rand = new Rand();
            Reset();
        }

        public void Reset()
        {
            maxNumOfEnemies = 3;
        }
        
        public void BeginNextRound()
        {
            SpawnEnemies(maxNumOfEnemies++);
        }

        private void SpawnEnemies(int maxNumOfEnemies)
        {
            int numOfEnemies = rand.NextInt(maxNumOfEnemies / 2, maxNumOfEnemies);

            for (int i = 0; i < numOfEnemies; i++)
            {
                Enemy.Spawn(GetRandomSpawnPosition(), rand.NextInt(1, 4));
            }
        }

        private Vector2 GetRandomSpawnPosition()
        {
            int randomSide = rand.NextInt(0, 4);

            switch (randomSide)
            {
                case 0:
                    // Top
                    return new Vector2(rand.NextInt(0, 512), rand.NextInt(0, 32));
                case 1:
                    // Bottom
                    return new Vector2(rand.NextInt(0, 512), -rand.NextInt(512 + 32, 512 + 64));
                case 2:
                    // Left
                    return new Vector2(rand.NextInt(-64, -32), -rand.NextInt(0, 512));
                case 3:
                    // Right
                    return new Vector2(rand.NextInt(512, 512 + 32), -rand.NextInt(0, 512));
                default:
                    // Shouldn't happen
                    return new Vector2(
                        Data.Get<float>("playerStartingPositionX"),
                        Data.Get<float>("playerStartingPositionY"));
            }
        }
    }
}
