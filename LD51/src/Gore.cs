using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace LD51
{
    public class Gore : IEntity
    {
        private static readonly float _lifeTimeInSeconds = Data.Get<float>("goreLifeTime");
        private static readonly float _speedInterpolation = Data.Get<float>("goreSpeedInterpolation");
        private static readonly float _layerDepth = Data.Get<float>("goreLayerDepth");

        public static Texture2D Texture;

        private static EntityContainer<Gore> instances = new EntityContainer<Gore>();

        private Vector2 position;
        private Vector2 direction;
        private Point bounds;
        private Sprite sprite;
        private float speed;
        private float remainingLife;

        private Gore(Vector2 position, Vector2 direction, float speed, Point size, Color color)
        {
            this.position = position;
            this.direction = direction;
            this.speed = speed;

            bounds = size;
            sprite = new Sprite(Texture, bounds, color, _layerDepth, 1 / 2f);

            remainingLife = _lifeTimeInSeconds;
        }

        public static IEnumerable<Gore> Instances => instances.List;

        public uint Id { get; private set; }

        public static void Spawn(Vector2 position, Vector2 direction, float speed, Point size, Color color)
        {
            Gore gore = new Gore(position, direction, speed, size, color);
            gore.Id = instances.Spawn(gore);
        }

        public void Update(float deltaTime)
        {
            // Lifetime

            remainingLife -= deltaTime;

            if (remainingLife < 0)
                Despawn();

            sprite.Alpha = remainingLife / _lifeTimeInSeconds;

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
    }
}
