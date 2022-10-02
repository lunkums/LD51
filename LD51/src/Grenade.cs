using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace LD51
{
    public class Grenade : IEntity, IExplodable, ICollider
    {
        private static readonly float _lifeTimeInSeconds = Data.Get<float>("grenadeLifeTime");
        private static readonly float _initialSpeed = Data.Get<float>("grenadeInitialSpeed");
        private static readonly float _speedInterpolation = Data.Get<float>("grenadeSpeedInterpolation");
        private static readonly float _explosionFlashAlpha = Data.Get<float>("grenadeExplosionFlashAlpha");
        private static readonly int _explosionRadius = Data.Get<int>("grenadeExplosionRadius");
        private static readonly int _explosionSize = Data.Get<int>("grenadeExplosionSize");
        private static readonly float _hitboxLifeTime = Data.Get<float>("grenadeHitboxLifeTime");
        private static readonly float _initialDebrisSpeed = Data.Get<float>("grenadeInitialDebrisSpeed");
        private static readonly float _layerDepth = Data.Get<float>("grenadeLayerDepth");

        public static Texture2D Texture;

        private static EntityContainer<Grenade> instances = new EntityContainer<Grenade>();

        private HashSet<ICollider> affectedColliders;

        private Point bounds;
        private Sprite sprite;

        private Vector2 position;
        private Vector2 direction;
        private float speed;
        private float remainingLife;
        private bool active;
        private bool exploding;

        private Grenade(Vector2 position, Vector2 direction)
        {
            this.position = position;
            this.direction = direction;

            bounds = new Point(2, 2);
            sprite = new Sprite(Texture, bounds, Color.ForestGreen, _layerDepth);

            affectedColliders = new HashSet<ICollider>();

            speed = _initialSpeed;
            remainingLife = _lifeTimeInSeconds;
            active = true;
            exploding = false;
        }

        public static IEnumerable<Grenade> Instances => instances.List;

        public Rectangle Hitbox => RectToHitbox.Translate(position, bounds);
        public uint Id { get; private set; }
        public int Size => _explosionSize;
        public Vector2 Center => Hitbox.Center.ToVector2();
        public Color DebrisColor => Color.DarkSlateGray;

        public static void Spawn(Vector2 position, Vector2 direction)
        {
            Grenade grenade = new Grenade(position, direction);
            grenade.Id = instances.Spawn(grenade);
            Audio.Play("grenadepriming");
        }

        public void CollisionResponse(Collision collision)
        {
            if (!exploding || affectedColliders.Contains(collision.Other)) return;

            float overlapArea = collision.Overlap.Area();
            float collideeArea = collision.Other.Hitbox.Area();
            
            // Always damage enemies whose hitboxes overlap the grenade's
            if (collision.Other is Enemy enemy)
            {
                if (overlapArea >= collideeArea)
                    enemy.TakeDamage(3);
                else if (overlapArea >= collideeArea / 2f)
                    enemy.TakeDamage(2);
                else
                    enemy.TakeDamage(1);
            }
            else if (collision.Other is Player player)
            {
                // If the player isn't close enough to the explosion, return before adding him to list of affected
                // colliders so he may still be killed by one
                if (overlapArea < collideeArea / 2f) return;

                player.Die();
            }

            affectedColliders.Add(collision.Other);
        }

        public void Update(float deltaTime)
        {
            // Lifetime

            remainingLife -= deltaTime;

            if (remainingLife < 0)
            {
                if (active)
                    Explode();
                else if (exploding)
                    Despawn();
            }

            // Movement

            position += speed * direction * deltaTime;

            speed = LD51Math.Lerp(speed, 0, 1 - MathF.Pow(_speedInterpolation, deltaTime));

            if (speed < 0.01f)
                speed = 0;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, position);
        }

        public void Despawn()
        {
            instances.Despawn(this);
        }

        private void Explode()
        {
            Audio.Play("grenadeexploding");
            GoreFactory.SpawnRandomGoreExplosion(this, _initialDebrisSpeed);

            active = false;
            exploding = true;
            remainingLife = _hitboxLifeTime;

            sprite.Color = Color.DarkSlateGray * _explosionFlashAlpha;
            sprite.Bounds = new Point(_explosionRadius, _explosionRadius);
            bounds = new Point(_explosionRadius, _explosionRadius);
            position -= new Vector2(_explosionRadius, _explosionRadius) * Sprite.GLOBAL_SCALE / 2;
        }
    }
}
