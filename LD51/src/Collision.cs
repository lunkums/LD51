using Microsoft.Xna.Framework;

namespace LD51
{
    public class Collision
    {
        private Collision() { }

        // Return whether a collision has occurred between two colliders and if so, invoke a collision response
        public static void HandleCollision(ICollider collider, ICollider collidee)
        {
            Rectangle colliderBox = collider.Hitbox;
            Rectangle collideeBox = collidee.Hitbox;

            // Axis-Aligned Bounding Box collisions will not work on rotated colliders
            if (colliderBox.Intersects(collideeBox))
            {
                collider.CollisionResponse(collidee);
                collidee.CollisionResponse(collider);
            }
        }
    }
}
