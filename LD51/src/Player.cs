using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace LD51
{
    public class Player : IPlayer
    {
        private static readonly float _maxSpeed = Data.Get<float>("playerMaxSpeed");
        private static readonly float _secondsBetweenShots = Data.Get<float>("playerSecondsBetweenShots");
        private static readonly int _maxCoins = Data.Get<int>("playerMaxCoins");
        private static readonly Vector2 _startingPosition = new Vector2(
            Data.Get<float>("playerStartingPositionX"), Data.Get<float>("playerStartingPositionY"));
        private static readonly float _layerDepth = Data.Get<float>("playerLayerDepth");

        private static string[] dyingSfx = new string[] { "headexploding1", "headexploding2", "headexploding3" };

        private static Texture2D texture;
        private static Point bounds;
        private static Sprite sprite;

        private Vector2 position;
        private float speed;
        private float shootCooldown;
        private bool hasReloaded;
        private int numberOfCoins;
        private bool dead;

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
                sprite = new Sprite(texture, bounds, Color.White, _layerDepth);
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

        public int Size => 2;
        public Color DebrisColor => Color.DarkRed;
        public Vector2 Center => Hitbox.Center.ToVector2();

        public void CollisionResponse(Collision collision)
        {
            if (collision.Other is Enemy)
            {
                Die();
            }
            else if (collision.Other is Coin coin)
            {
                if (NumberOfCoins == _maxCoins) return;

                coin.Pickup();
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

        public void Die()
        {
            if (dead) return;

            Audio.PlayRandom(dyingSfx);
            Main.GameOver();
            dead = true;
        }

        // This method is hijacked as a "reset" method, doesn't really despawn the player
        public void Despawn()
        {
            dead = false;
            position = _startingPosition;
            speed = _maxSpeed;
            shootCooldown = _secondsBetweenShots;
            hasReloaded = false;
            NumberOfCoins = 0;
        }
    }
}
