using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace LD51
{
    public class Main : Game
    {
        public static event Action OnUpdateEnd;
        public static float TimeScale = 1f;

        private static Level level;
        private static float frameRate;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

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

        public static float FrameRate => frameRate;

        protected override void Initialize()
        {
            // Initialize the level
            level = new Level();
            level.StartingPosition = new Vector2(screenSize.X / 2f, -screenSize.Y / 2f);
            level.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Content.RootDirectory = "Content";

            Player.Texture = new Texture2D(GraphicsDevice, 1, 1);
            Player.Texture.SetData(new[] { Color.White });

            Enemy.Texture = new Texture2D(GraphicsDevice, 1, 1);
            Enemy.Texture.SetData(new[] { Color.White });

            Bullet.Texture = new Texture2D(GraphicsDevice, 1, 1);
            Bullet.Texture.SetData(new[] { Color.Yellow });

            Countdown.Texture = Content.Load<Texture2D>("font");
        }

        protected override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds * TimeScale;
            float frameRate = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Debug.WriteLine(frameRate + " FPS");

            // Update input

            Input.Update();

            if (Input.IsKeyDown(Keys.Escape))
                Exit();

            // Update game logic

            level.Update(deltaTime);

            OnUpdateEnd.Invoke();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                DepthStencilState.None,
                RasterizerState.CullNone,
                null);
            level.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public static void GameOver()
        {
            level.GameOver();
        }

        public static void Reset()
        {
            level.Reset();
        }
    }
}