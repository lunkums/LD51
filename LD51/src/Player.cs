using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace LD51
{
    public class Player : ICollider
    {
        private static Texture2D texture;
        private static Point bounds;
        private static Sprite sprite;

        private Vector2 position;
        private float speed;

        public Player(Vector2 startingPosition, float speed)
        {
            position = startingPosition;
            this.speed = speed;
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

        public void CollisionResponse(ICollider collider)
        {
            if (collider is Enemy)
            {
                Main.GameOver();
            }
        }

        public void Update(float deltaTime)
        {
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
            if (Input.LeftMousePressed())
            {
                Vector2 pointToMouse = Input.MouseWorldPosition - position;
                Bullet.Spawn(position, pointToMouse.Normalized(), speed * 4f);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, position);
        }
    }
}
