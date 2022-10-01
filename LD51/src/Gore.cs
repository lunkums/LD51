using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;

namespace LD51
{
    public class Gore : IEntity
    {
        public static Texture2D Texture;

        private static EntityContainer<Gore> instances = new EntityContainer<Gore>();

        private const float lifeTimeInSeconds = 10;
        // This value should fall within 0 and 1
        private const float speedInterpolation = .25f;

        private Vector2 position;
        private Vector2 direction;
        private Point bounds;
        private Sprite sprite;
        private float speed;
        private float remainingLife;

        private Gore(Vector2 position, Vector2 direction, float speed, Point size)
        {
            this.position = position;
            this.direction = direction;
            this.speed = speed;

            bounds = size;
            sprite = new Sprite(Texture, bounds, Color.DarkRed, 1 / 2f);

            remainingLife = lifeTimeInSeconds;
        }

        public static IEnumerable Instances => instances.List;

        public uint Id { get; private set; }

        public static void Spawn(Vector2 position, Vector2 direction, float speed, Point size)
        {
            Gore gore = new Gore(position, direction, speed, size);
            gore.Id = instances.Spawn(gore);
        }

        public static void DespawnAll()
        {
            foreach (Gore gore in Instances)
            {
                gore.Despawn();
            }
        }

        public void Update(float deltaTime)
        {
            // Lifetime

            remainingLife -= deltaTime;

            if (remainingLife < 0)
                Despawn();

            sprite.Alpha = remainingLife / lifeTimeInSeconds;

            // Movement

            position += speed * direction * deltaTime;

            speed = LD51Math.Lerp(speed, 0, (float)(1 - Math.Pow(speedInterpolation, deltaTime)));

            if (speed < 0.01f)
                speed = 0;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, position);
        }

        private void Despawn()
        {
            instances.Despawn(this);
        }
    }
}
