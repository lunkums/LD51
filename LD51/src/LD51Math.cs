using Microsoft.Xna.Framework;
using System;

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
    }
}
