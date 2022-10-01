using Microsoft.Xna.Framework;

namespace LD51
{
    public struct Hitbox
    {
        public float X;
        public float Y;
        public int Width;
        public int Height;

        public Hitbox(float x, float y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public Hitbox(Vector2 position, Point bounds)
        {
            X = position.X;
            Y = position.Y;
            Width = bounds.X;
            Height = bounds.Y;
        }
    }
}
