using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace LD51
{
    public class Main : Game
    {
        public static event Action OnUpdateEnd;

        private static float timeScale = 1f;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private Player player;
        private float playerMovementSpeed = 512 / 2f;

        private Point screenSize;

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            screenSize = new Point(512, 512);
            graphics.PreferredBackBufferWidth = screenSize.X;
            graphics.PreferredBackBufferHeight = screenSize.Y;
            graphics.ApplyChanges();

            // Register this listener so the "Invoke" will never be null
            OnUpdateEnd += () => { };
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            player = new Player(new Vector2(screenSize.X / 2f, -screenSize.Y / 2f), playerMovementSpeed);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Player.Texture = new Texture2D(GraphicsDevice, 1, 1);
            Player.Texture.SetData(new[] { Color.White });

            Enemy.Texture = new Texture2D(GraphicsDevice, 1, 1);
            Enemy.Texture.SetData(new[] { Color.White });

            Bullet.Texture = new Texture2D(GraphicsDevice, 1, 1);
            Bullet.Texture.SetData(new[] { Color.Yellow });
        }

        protected override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds * timeScale;

            /// Input

            Input.Update();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Input.IsKeyDown(Keys.Escape))
                Exit();

            /// Player

            player.Update(deltaTime);

            /// Enemy
            
            foreach (Enemy enemy in Enemy.Instances)
            {
                enemy.Direction = (player.Position - enemy.Position).Normalized();
                enemy.Update(deltaTime);
            }

            /// Bullets

            foreach (Bullet bullet in Bullet.Instances)
            {
                bullet.Update(deltaTime);
            }

            /// Collisions

            foreach (Bullet bullet in Bullet.Instances)
            {
                foreach (Enemy enemy in Enemy.Instances)
                {
                    Collision.HandleCollision(bullet, enemy);
                }
            }

            foreach (Enemy enemy in Enemy.Instances)
            {
                Collision.HandleCollision(enemy, player);
            }

            /// Debug

            if (Input.IsKeyPressed(Keys.F12))
                Enemy.Spawn(Vector2.Zero, playerMovementSpeed / 2f);

            if (Input.IsKeyPressed(Keys.F11))
                foreach (Enemy enemy in Enemy.Instances)
                {
                    enemy.Speed = 0;
                }

            OnUpdateEnd.Invoke();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            player.Draw(spriteBatch);

            foreach (Enemy enemy in Enemy.Instances)
            {
                enemy.Draw(spriteBatch);
            }

            foreach (Bullet bullet in Bullet.Instances)
            {
                bullet.Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public static void GameOver()
        {
            timeScale = 0f;
        }
    }
}