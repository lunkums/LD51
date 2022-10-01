using Microsoft.Xna.Framework;

namespace LD51
{
    public static class RectToHitbox
    {
        // Translates a position vector and sprite bounds to a hitbox
        public static Rectangle Translate(Vector2 position, Point bounds)
        {
            return new Rectangle(
                (int)position.X,
                (int)position.Y, 
                bounds.X * Sprite.GLOBAL_SCALE, 
                bounds.Y * Sprite.GLOBAL_SCALE);
        }
    }
}
