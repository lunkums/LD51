using Microsoft.Xna.Framework;

namespace LD51
{
    public interface ICollider
    {
        Rectangle Hitbox { get; }
        void CollisionResponse(ICollider collider);
    }
}