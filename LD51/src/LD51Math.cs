using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LD51
{
    public static class LD51Math
    {
        private const float DegToRad = MathF.PI / 180;

        public static Vector2 Normalized(this Vector2 vector)
        {
            return IsZero(vector.Length()) ? vector : Vector2.Normalize(vector);
        }

        public static Vector2 Rotate(this Vector2 v, float degrees)
        {
            return v.RotateRadians(degrees * DegToRad);
        }

        public static float Lerp(float firstFloat, float secondFloat, float by)
        {
            return firstFloat * (1 - by) + secondFloat * by;
        }

        public static Vector2 RotateRadians(this Vector2 v, float radians)
        {
            float ca = MathF.Cos(radians);
            float sa = MathF.Sin(radians);
            return new Vector2(ca * v.X - sa * v.Y, sa * v.X + ca * v.Y);
        }

        public static bool IsZero(float num)
        {
            return MathF.Abs(num) <= float.Epsilon;
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
