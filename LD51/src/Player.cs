using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace LD51
{
    public class Player : ICollider, IEntity
    {
        private readonly static float _maxSpeed = Data.Get<float>("playerMaxSpeed");
        private readonly static float _secondsBetweenShots = Data.Get<float>("playerSecondsBetweenShots");
        private readonly static float _maxCoins = Data.Get<float>("playerMaxCoins");
        private readonly static Vector2 _startingPosition = new Vector2(
            Data.Get<float>("playerStartingPositionX"), Data.Get<float>("playerStartingPositionY"));

        private static Texture2D texture;
        private static Point bounds;
        private static Sprite sprite;

        private Vector2 position;
        private float speed;
        private float shootCooldown;
        private bool hasReloaded;
        private int numberOfCoins;

        public Player()
        {
            Despawn();
        }

        public static Texture2D Texture
        {
            get => texture;
            set
            {
                texture = value;
                bounds = new Point(4, 4);
                sprite = new Sprite(texture, bounds, Color.White);
            }
        }

        public Vector2 Position { get => position; set => position = value; }
        public Rectangle Hitbox => RectToHitbox.Translate(position, bounds);
        public int NumberOfCoins
        {
            get => numberOfCoins;
            private set => numberOfCoins = (int)MathF.Min(value, _maxCoins);
        }
        public uint Id => 999;

        private Vector2 Center => Hitbox.Center.ToVector2();

        public void CollisionResponse(Collision collision)
        {
            if (collision.Other is Enemy)
            {
                Main.GameOver();
            }
            else if (collision.Other is Coin)
            {
                if (NumberOfCoins == _maxCoins) return;

                (collision.Other as Coin).Pickup();
                NumberOfCoins++;
            }
        }

        public void Update(float deltaTime)
        {
            shootCooldown = MathF.Max(shootCooldown - deltaTime, -1);

            // Movement

            Vector2 moveDirection = new Vector2();

            if (Input.IsKeyDown(Keys.W))
            {
                moveDirection += Vector2.UnitY;
            }
            if (Input.IsKeyDown(Keys.S))
            {
                moveDirection -= Vector2.UnitY;
            }
            if (Input.IsKeyDown(Keys.D))
            {
                moveDirection += Vector2.UnitX;
            }
            if (Input.IsKeyDown(Keys.A))
            {
                moveDirection -= Vector2.UnitX;
            }

            position += moveDirection.Normalized() * speed * deltaTime;

            // Shooting
            Vector2 directionToMouse = position.DirectionToMouse();

            if (Input.LeftMousePressed() && shootCooldown <= 0)
            {
                // Play clip
                Audio.Play("shoot");

                // Spawn bullet
                Bullet.Spawn(Center, directionToMouse, speed * 4f);
                Bullet.Spawn(Center, directionToMouse.Rotate(15), speed * 4f);
                Bullet.Spawn(Center, directionToMouse.Rotate(-15), speed * 4f);

                // Start cooldown
                shootCooldown = _secondsBetweenShots;
                hasReloaded = false;
            }

            if (Input.RightMousePressed())
            {
                Grenade.Spawn(Center, directionToMouse);
            }

            if (!hasReloaded && shootCooldown <= _secondsBetweenShots / 2)
            {
                Audio.Play("cock");
                hasReloaded = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, position);
        }

        // This method is hijacked as a "reset" method, doesn't really despawn the player
        public void Despawn()
        {
            position = _startingPosition;
            speed = _maxSpeed;
            shootCooldown = 0;
            hasReloaded = true;
            NumberOfCoins = 0;
        }
    }
}
