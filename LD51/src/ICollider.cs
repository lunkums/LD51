using Microsoft.Xna.Framework;

namespace LD51
{
    public interface ICollider
    {
        Rectangle Hitbox { get; }
        void CollisionResponse(Collision collision);
    }
}