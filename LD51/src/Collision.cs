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
            if (colliderBox.X < collideeBox.X + collideeBox.Width &&
                colliderBox.X + colliderBox.Width > collideeBox.X &&
                colliderBox.Y < collideeBox.Y + collideeBox.Height &&
                colliderBox.Height + colliderBox.Y > collideeBox.Y)
            {
                collider.InvokeResponse(collidee.GetType());
                collidee.InvokeResponse(collider.GetType());
            }
        }
    }
}
