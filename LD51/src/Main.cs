using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace LD51
{
    public class Main : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private Texture2D player;
        private Vector2 playerPosition = new Vector2();
        private float playerMovementSpeed = 250f;

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            player = new Texture2D(GraphicsDevice, 1, 1);
            player.SetData(new[] { Color.White });
        }

        protected override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            KeyboardState state = Keyboard.GetState();
            Vector2 moveDelta = new Vector2();

            if (state.IsKeyDown(Keys.W))
            {
                moveDelta += Vector2.UnitY;
            }
            if (state.IsKeyDown(Keys.S))
            {
                moveDelta -= Vector2.UnitY;
            }
            if (state.IsKeyDown(Keys.D))
            {
                moveDelta += Vector2.UnitX;
            }
            if (state.IsKeyDown(Keys.A))
            {
                moveDelta -= Vector2.UnitX;
            }

            moveDelta.SafeNormalize();
            moveDelta *= playerMovementSpeed;

            playerPosition += moveDelta * deltaTime;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            // TODO: Add your drawing code here
            spriteBatch.Draw(player, new Rectangle((int)playerPosition.X, -(int)playerPosition.Y, 25, 25), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }

    public static class LunkumsMath
    {
        public static void SafeNormalize(this Vector2 vector)
        {
            if (IsZero(vector.Length()))
                return;
            vector.Normalize();
        }

        public static bool IsZero(float num)
        {
            return Math.Abs(num) <= float.Epsilon;
        }
    }
}