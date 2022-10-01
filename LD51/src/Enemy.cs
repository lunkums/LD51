using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;

namespace LD51
{
    public class Enemy : ICollider, IEntity
    {
        public static Texture2D texture;

        private static EntityContainer<Enemy> instances = new EntityContainer<Enemy>();
        private static Point bounds = new Point(32, 32);

        private Vector2 position;
        private float speed;

        private Enemy(Vector2 position, float speed)
        {
            this.position = position;
            this.speed = speed;

            Direction = new Vector2();
        }

        public static IEnumerable Instances => instances.List;

        public Vector2 Direction { get; set; }
        public Vector2 Position => position;
        public Hitbox Hitbox => new Hitbox(Position, bounds);
        public uint Id { get; private set; }
        public float Speed { set => speed = value; }

        public void InvokeResponse(Type type)
        {
            if (type == typeof(Bullet))
            {
                Despawn();
            }
        }

        public static void Spawn(Vector2 position, float speed)
        {
            Enemy enemy = new Enemy(position, speed);
            enemy.Id = instances.Spawn(enemy);
        }

        public void Update(float deltaTime)
        {
            position += speed * Direction * deltaTime;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle((int)position.X, -(int)position.Y - bounds.Y, bounds.X, bounds.Y), Color.Red);
        }

        public void Despawn()
        {
            instances.Despawn(this);
        }
    }
}
