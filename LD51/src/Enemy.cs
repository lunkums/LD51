using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;

namespace LD51
{
    public class Enemy : ICollider, IEntity
    {
        public static float MaxSpeed;
        public static Texture2D Texture;

        private static EntityContainer<Enemy> instances = new EntityContainer<Enemy>();

        private Vector2 position;
        private float speed;
        private Point bounds;
        private Sprite sprite;

        private int health;
        private bool alive;

        private Enemy(Vector2 position, float speed, int size)
        {
            this.position = position;
            this.speed = speed;

            bounds = new Point(size * 2, size * 2);
            sprite = new Sprite(Texture, bounds, Color.Red);
            health = size;
            alive = true;

            Direction = new Vector2();
        }

        public static IEnumerable Instances => instances.List;
        public static (IEnumerable, IEnumerable) SplitInstances => instances.SplitLists;

        public Vector2 Direction { get; set; }
        public Vector2 Position => position;
        public Hitbox Hitbox => new Hitbox(Position, bounds);
        public uint Id { get; private set; }
        public float Speed { set => speed = value; }

        public static void Spawn(Vector2 position, int size)
        {
            size = Math.Clamp(size, 1, 3);
            Enemy enemy = new Enemy(position, MaxSpeed / size, size);
            enemy.Id = instances.Spawn(enemy);
        }

        public static void DespawnAll()
        {
            foreach (Enemy enemy in Instances)
            {
                instances.Despawn(enemy);
            }
        }

        public void CollisionResponse(ICollider collider)
        {
            if (collider is Enemy)
            {

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

        public void TakeDamage(int damage)
        {
            health -= damage;

            // The "alive" check may be redundant, but I wanted to cover possible edge cases where the enemy doesn't
            // despawn right away
            if (health < 1 && alive)
            {
                instances.Despawn(this);
                alive = false;
            }
        }
    }
}
