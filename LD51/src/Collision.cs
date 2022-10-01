namespace LD51
{
    public class Collision
    {
        private Collision() { }

        public static void HandleCollision(ICollider collider, ICollider collidee)
        {
            Hitbox colliderBox = collider.Hitbox;
            Hitbox collideeBox = collidee.Hitbox;

            // Axis-Aligned Bounding Box collisions will not work on rotated colliders
            if (colliderBox.X < collideeBox.X + collideeBox.Width * Sprite.GLOBAL_SCALE &&
                colliderBox.X + colliderBox.Width * Sprite.GLOBAL_SCALE > collideeBox.X &&
                colliderBox.Y < collideeBox.Y + collideeBox.Height * Sprite.GLOBAL_SCALE &&
                colliderBox.Height * Sprite.GLOBAL_SCALE + colliderBox.Y > collideeBox.Y)
            {
                collider.CollisionResponse(collidee.GetType());
                collidee.CollisionResponse(collider.GetType());
            }
        }
    }
}
