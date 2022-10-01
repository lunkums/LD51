﻿using Microsoft.Xna.Framework;

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
            if (colliderBox.X < collideeBox.X + collideeBox.Width * Sprite.GLOBAL_SCALE &&
                colliderBox.X + colliderBox.Width * Sprite.GLOBAL_SCALE > collideeBox.X &&
                colliderBox.Y < collideeBox.Y + collideeBox.Height * Sprite.GLOBAL_SCALE &&
                colliderBox.Height * Sprite.GLOBAL_SCALE + colliderBox.Y > collideeBox.Y)
            {
                collider.CollisionResponse(collidee);
                collidee.CollisionResponse(collider);
            }
        }
    }
}
