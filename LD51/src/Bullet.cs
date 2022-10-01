using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;

namespace LD51
{
    public class Bullet : ICollider, IEntity
    {
        public static Texture2D texture;

        private static EntityContainer<Bullet> instances = new EntityContainer<Bullet>();
        private static Point bounds = new Point(8, 8);

        private const float lifeTimeInSeconds = 1;

        private Vector2 position;
        private Vector2 direction;
        private float speed;
        private float remainingLife;

        private Bullet(Vector2 position, Vector2 direction, float speed)
        {
            this.position = position;
            this.direction = direction;
            this.speed = speed;

            remainingLife = lifeTimeInSeconds;
        }

        public static IEnumerable Instances => instances.List;

        public Hitbox Hitbox => new Hitbox(position, bounds);

        public uint Id { get; private set; }

        public void InvokeResponse(Type type)
        {
            if (type == typeof(Enemy))
            {
                Despawn();
            }
        }

        public static void Spawn(Vector2 position, Vector2 direction, float speed)
        {
            Bullet bullet = new Bullet(position, direction, speed);
            bullet.Id = instances.Spawn(bullet);
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
            spriteBatch.Draw(
                texture,
                new Rectangle((int)position.X, -(int)position.Y - bounds.Y, bounds.X, bounds.Y),
                Color.Yellow);
        }

        private void Despawn()
        {
            instances.Despawn(this);
        }
    }
}
