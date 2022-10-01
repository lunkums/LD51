using Microsoft.Xna.Framework;

namespace LD51
{
    public struct Collision
    {
        private ICollider other;
        private Rectangle overlap;

        public Collision()
        {
            other = null;
            overlap = Rectangle.Empty;
        }

        public Collision(ICollider other, Rectangle overlap)
        {
            this.other = other;
            this.overlap = overlap;
        }

        public ICollider Other => other;
        public Rectangle Overlap => overlap;

        public Vector2 Direction(ICollider collider)
        {
            Vector2 direction;

            if (overlap.Width < overlap.Height)
            {
                if (collider.Hitbox.Center.X < other.Hitbox.Center.X)
                    direction = new Vector2(-1, 0);
                else
                    direction = new Vector2(1, 0);
            }
            else
            {
                if (collider.Hitbox.Center.Y < other.Hitbox.Center.Y)
                    direction = new Vector2(0, -1);
                else
                    direction = new Vector2(0, 1);
            }

            return direction;
        }
    }
}