using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

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
        public static Vector2 RotateRadians(this Vector2 v, float radians)
        {
            float ca = MathF.Cos(radians);
            float sa = MathF.Sin(radians);
            return new Vector2(ca * v.X - sa * v.Y, sa * v.X + ca * v.Y);
        }

        public static Vector2 DirectionToMouse(this Vector2 position)
        {
            return (Input.MouseWorldPosition - position).Normalized();
        }

        public static float Area(this Rectangle rectangle)
        {
            return rectangle.Width * rectangle.Height;
        }

        public static float Lerp(float firstFloat, float secondFloat, float by)
        {
            return firstFloat * (1 - by) + secondFloat * by;
        }


        public static bool IsZero(float num)
        {
            return MathF.Abs(num) <= float.Epsilon;
        }

        public static IEnumerable<T> Yield<T>(this T item)
        {
            yield return item;
        }
    }
}
