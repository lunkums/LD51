using System;

namespace LD51
{
    public interface ICollider
    {
        Hitbox Hitbox { get; }
        void CollisionResponse(ICollider collider);
    }
}