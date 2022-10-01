using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;

namespace LD51
{
    public class Bullet : ICollider, IEntity
    {
        private static EntityContainer<Bullet> instances = new EntityContainer<Bullet>();

        private static Texture2D texture;
        private static Point bounds;
        private static Sprite sprite;

        private const float lifeTimeInSeconds = 1;

        private Vector2 position;
        private Vector2 direction;
        private float speed;
        private float remainingLife;
        private bool active;

        private Bullet(Vector2 position, Vector2 direction, float speed)
        {
            this.position = position;
            this.direction = direction;
            this.speed = speed;

            remainingLife = lifeTimeInSeconds;
            active = true;
        }

        public static IEnumerable Instances => instances.List;

        public static Texture2D Texture
        {
            get => texture;
            set
            {
                texture = value;
                bounds = new Point(1, 1);
                sprite = new Sprite(texture, bounds, Color.Yellow);
            }
        }

        public Rectangle Hitbox => new Rectangle((int)position.X, (int)position.Y, bounds.X, bounds.Y);
        public uint Id { get; private set; }

        public static void Spawn(Vector2 position, Vector2 direction, float speed)
        {
            Bullet bullet = new Bullet(position, direction, speed);
            bullet.Id = instances.Spawn(bullet);
        }

        public static void DespawnAll()
        {
            foreach (Bullet bullet in Instances)
            {
                bullet.Despawn();
            }
        }

        public void CollisionResponse(ICollider collider)
        {
            if (collider is Enemy)
            {
                // Prevent the same bullet from affecting multiple enemies simultaneously
                if (!active) return;

                (collider as Enemy).TakeDamage(1);
                Despawn();
            }
        }

        public void Update(float deltaTime)
        {
            remainingLife -= deltaTime;

            if (remainingLife < 0)
                Despawn();

            position += speed * direction * deltaTime;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, position);
        }

        private void Despawn()
        {
            active = false;
            instances.Despawn(this);
        }
    }
}
