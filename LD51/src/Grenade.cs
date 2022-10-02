using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;

namespace LD51
{
    public class Grenade : IEntity
    {
        private static readonly float _lifeTimeInSeconds = Data.Get<float>("grenadeLifeTime");
        private static readonly float _initialSpeed = Data.Get<float>("grenadeInitialSpeed");
        private static readonly float _speedInterpolation = Data.Get<float>("grenadeSpeedInterpolation");

        public static Texture2D Texture;

        private static EntityContainer<Grenade> instances = new EntityContainer<Grenade>();

        private Point bounds;
        private Sprite sprite;

        private Vector2 position;
        private Vector2 direction;
        private float speed;
        private float remainingLife;
        private bool active;

        private Grenade(Vector2 position, Vector2 direction)
        {
            this.position = position;
            this.direction = direction;

            bounds = new Point(2, 2);
            sprite = new Sprite(Texture, bounds, Color.ForestGreen);

            speed = _initialSpeed;
            remainingLife = _lifeTimeInSeconds;
            active = true;
        }

        public static IEnumerable Instances => instances.List;

        public Rectangle Hitbox => RectToHitbox.Translate(position, bounds);
        public uint Id { get; private set; }

        public static void Spawn(Vector2 position, Vector2 direction)
        {
            Grenade grenade = new Grenade(position, direction);
            grenade.Id = instances.Spawn(grenade);
        }

        public static void DespawnAll()
        {
            foreach (Grenade grenade in Instances)
            {
                grenade.Despawn();
            }
        }

        public void CollisionResponse(Collision collision)
        {
            if (collision.Other is Enemy)
            {
                // Prevent the same bullet from affecting multiple enemies simultaneously
                if (!active) return;

                (collision.Other as Enemy).TakeDamage(1);
                Despawn();
            }
        }

        public void Update(float deltaTime)
        {
            // Lifetime

            remainingLife -= deltaTime;

            if (remainingLife < 0)
                Despawn();

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

        private void Despawn()
        {
            active = false;
            instances.Despawn(this);
        }
    }
}
