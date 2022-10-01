using Microsoft.Xna.Framework;

namespace LD51
{
    public class CollisionHandler
    {
        private CollisionHandler() { }

        // Return whether a collision has occurred between two colliders and if so, invoke a collision response
        public static void HandleCollision(ICollider collider, ICollider collidee)
        {
            Rectangle colliderBox = collider.Hitbox;
            Rectangle collideeBox = collidee.Hitbox;

            // Axis-Aligned Bounding Box collisions will not work on rotated colliders
            if (colliderBox.Intersects(collideeBox))
            {
                Rectangle overlap = Rectangle.Intersect(colliderBox, collideeBox);

                collider.CollisionResponse(new Collision(collidee, overlap));
                collidee.CollisionResponse(new Collision(collider, overlap));
            }
        }
    }
}
