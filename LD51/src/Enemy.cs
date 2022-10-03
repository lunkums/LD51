using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace LD51
{
    public class Enemy : ICollider, IEntity, IExplodable
    {
        private static readonly float _recentDamageTimeThreshold = Data.Get<float>("enemyRecentDamageTimeThreshold");
        private static readonly float _basePercentDropChance = Data.Get<float>("enemyBasePercentDropChance");
        private static readonly float _maxSpeed = Data.Get<float>("enemyMaxSpeed");
        private static readonly float _layerDepth = Data.Get<float>("enemyLayerDepth");

        public static Texture2D Texture;

        private static EntityContainer<Enemy> instances = new EntityContainer<Enemy>();
        private static string[] dyingSfx = new string[] { "headexploding1", "headexploding2", "headexploding3" };

        private Vector2 position;
        private float speed;
        private Point bounds;
        private Sprite sprite;

        private int health;
        private bool alive;
        private int recentDamageTaken;
        private float recentDamageTimer;
        private float percentDropChance;

        private Enemy(Vector2 position, float speed, int size)
        {
            this.position = position;
            this.speed = speed;

            Size = size;
            bounds = new Point(size * 2, size * 2);
            sprite = new Sprite(Texture, bounds, Color.Red, _layerDepth);
            health = size;
            alive = true;
            recentDamageTaken = 0;
            recentDamageTimer = 0f;
            percentDropChance = _basePercentDropChance * MathF.Pow(2, size - 1);

            Direction = new Vector2();
        }

        public static IEnumerable<Enemy> Instances => instances.List;

        public Vector2 Direction { get; set; }
        public Vector2 Position => position;
        public Rectangle Hitbox => RectToHitbox.Translate(position, bounds);
        public uint Id { get; private set; }
        public float Speed { set => speed = value; }
        public int Size { get; private set; }
        public Vector2 Center => Hitbox.Center.ToVector2();
        public Color DebrisColor => Color.DarkRed;

        private bool CriticalDeath => recentDamageTaken > Math.Max(Size - 1, 1);

        public static void Spawn(Vector2 position, int size)
        {
            size = Math.Clamp(size, 1, 3);
            Enemy enemy = new Enemy(position, _maxSpeed / size, size);
            enemy.Id = instances.Spawn(enemy);
        }

        public void CollisionResponse(Collision collision)
        {
            if (collision.Other is Enemy enemy && enemy.Size >= Size)
            {
                while (Hitbox.Intersects(enemy.Hitbox))
                {
                    position += collision.Direction(this);
                }
            }
        }

        public void Update(float deltaTime)
        {
            position += speed * Direction * deltaTime;

            // The "alive" check may be redundant, but I wanted to cover possible edge cases where the enemy doesn't
            // despawn right away
            if (health < 1 && alive)
                Die();

            recentDamageTimer += deltaTime;

            if (recentDamageTimer >= _recentDamageTimeThreshold && health > 0)
            {
                recentDamageTaken = 0;
                recentDamageTimer = 0f;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, position);
        }

        public void Despawn()
        {
            instances.Despawn(this);
        }

        public void TakeDamage(int damage)
        {
            health -= damage;
            recentDamageTaken += damage;
        }

        private void Die()
        {
            // "Critical" hit feedback
            if (CriticalDeath)
            {
                Audio.PlayRandom(dyingSfx);
                GoreFactory.SpawnRandomGoreExplosion(this);
                percentDropChance *= 2;
            }

            CoinFactory.TryToSpawn(percentDropChance, Hitbox.Center.ToVector2());

            instances.Despawn(this);
            alive = false;
        }
    }
}
