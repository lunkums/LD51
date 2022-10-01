using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;

namespace LD51
{
    public class Enemy : ICollider, IEntity
    {
        private static EntityContainer<Enemy> instances = new EntityContainer<Enemy>();

        private static Texture2D texture;
        private static Point bounds;
        private static Sprite sprite;

        private Vector2 position;
        private float speed;

        private Enemy(Vector2 position, float speed)
        {
            this.position = position;
            this.speed = speed;

            Direction = new Vector2();
        }

        public static IEnumerable Instances => instances.List;

        public static Texture2D Texture
        {
            get => texture;
            set
            {
                texture = value;
                bounds = new Point(32, 32);
                sprite = new Sprite(texture, bounds, Color.Red);
            }
        }

        public Vector2 Direction { get; set; }
        public Vector2 Position => position;
        public Hitbox Hitbox => new Hitbox(Position, bounds);
        public uint Id { get; private set; }
        public float Speed { set => speed = value; }

        public static void Spawn(Vector2 position, float speed)
        {
            Enemy enemy = new Enemy(position, speed);
            enemy.Id = instances.Spawn(enemy);
        }

        public static void DespawnAll()
        {
            foreach (Enemy enemy in Instances)
            {
                enemy.Despawn();
            }
        }

        public void CollisionResponse(Type type)
        {
            if (type == typeof(Bullet))
            {
                Despawn();
            }
        }

        public void Update(float deltaTime)
        {
            position += speed * Direction * deltaTime;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, position);
        }

        public void Despawn()
        {
            instances.Despawn(this);
        }
    }
}
