using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;

namespace LD51
{
    public class Coin : IEntity, ICollider
    {
        private readonly float _lifeTimeInSeconds = Data.Get<float>("coinLifeTime");

        private static Texture2D texture;
        private static Point bounds;
        private static Sprite sprite;

        private static EntityContainer<Coin> instances = new EntityContainer<Coin>();

        private Vector2 position;
        private float remainingLife;

        private Coin(Vector2 position)
        {
            this.position = position;
            remainingLife = _lifeTimeInSeconds;
        }

        public static IEnumerable Instances => instances.List;

        public static Texture2D Texture
        {
            get => texture;
            set
            {
                texture = value;
                bounds = new Point(1, 2);
                sprite = new Sprite(texture, bounds, Color.Yellow);
            }
        }

        public Rectangle Hitbox => RectToHitbox.Translate(position, bounds);
        public uint Id { get; private set; }

        // Player will check if he picks up a coin
        public void CollisionResponse(Collision collision) { }

        public static void Spawn(Vector2 position)
        {
            Coin coin = new Coin(position);
            coin.Id = instances.Spawn(coin);
        }

        public static void DespawnAll()
        {
            foreach (Coin coin in Instances)
            {
                coin.Despawn();
            }
        }

        public void Update(float deltaTime)
        {
            // Lifetime

            remainingLife -= deltaTime;

            if (remainingLife < 0)
                Despawn();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, position);
        }

        public void Pickup()
        {
            Audio.Play("coinpickup");
            Despawn();
        }

        private void Despawn()
        {
            instances.Despawn(this);
        }
    }
}
