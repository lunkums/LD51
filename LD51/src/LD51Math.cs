using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LD51
{
    public static class LD51Math
    {
        public static Vector2 Normalized(this Vector2 vector)
        {
            return IsZero(vector.Length()) ? vector : Vector2.Normalize(vector);
        }

        public static bool IsZero(float num)
        {
            return Math.Abs(num) <= float.Epsilon;
        }

        public static Point ToPoint(this Vector2 vector)
        {
            return new Point((int)vector.X, (int)vector.Y);
        }

        public static (IEnumerable<V>, IEnumerable<V>) Split<K, V>(this Dictionary<K, V> source)
        {
            if (source.Count < 2) return (source.Values, Array.Empty<V>());
            Dictionary<K, V>[] result = source
                .Select((kvp, n) => new { kvp, k = n % 2 })
                .GroupBy(x => x.k, x => x.kvp)
                .Select(x => x.ToDictionary(y => y.Key, y => y.Value))
                .ToArray();
            return (result[0].Values, result[1].Values);
        }
    }
}
