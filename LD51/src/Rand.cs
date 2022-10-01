using System;

namespace LD51
{
    // Why did I write my own RNG? Great question.
    public class Rand
    {
        private readonly int m = (int)Math.Pow(2, 31) - 1;
        private const int a = 48271;

        private long seed;

        public Rand()
        {
            seed = DateTime.Now.Ticks % int.MaxValue;
        }

        public int Seed { get => (int)seed; set => seed = value; }

        public int NextInt()
        {
            seed = (a * seed) % m;
            return Convert.ToInt32(seed);
        }

        public int NextInt(int min, int max)
        {
            return (NextInt() % (max - min)) + min;
        }
    }
}
