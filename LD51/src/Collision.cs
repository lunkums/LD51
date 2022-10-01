namespace LD51
{
    public class Collision
    {
        private Collision() { }

        public static void HandleCollision(ICollider collider, ICollider collidee)
        {
            Hitbox colliderBox = collider.Hitbox;
            Hitbox collideeBox = collidee.Hitbox;

            if (colliderBox.X < collideeBox.X + collideeBox.Width &&
                colliderBox.X + colliderBox.Width > collideeBox.X &&
                colliderBox.Y < collideeBox.Y + collideeBox.Height &&
                colliderBox.Height + colliderBox.Y > collideeBox.Y)
            {
                collidee.InvokeResponse(collider.GetType());
            }
        }
    }
}
