using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace LD51
{
    public class Main : Game
    {
        private static readonly Point _screenSize = new Point(
            Data.Get<int>("screenWidth"),
            Data.Get<int>("screenHeight"));

        public static event Action OnUpdateEnd;
        public static float TimeScale = 1f;

        private static Level level;
        private static float frameRate;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            graphics.PreferredBackBufferWidth = _screenSize.X;
            graphics.PreferredBackBufferHeight = _screenSize.Y;
            graphics.ApplyChanges();

            // Register this listener so the "Invoke" will never be null
            OnUpdateEnd += () => { };
        }

        public static float FrameRate => frameRate;

        protected override void Initialize()
        {
            // Initialize the level
            level = new Level();
            level.StartingPosition = new Vector2(_screenSize.X / 2f, -_screenSize.Y / 2f);
            level.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Content.RootDirectory = "Content";

            // Add textures
            Texture2D baseTexture = new Texture2D(GraphicsDevice, 1, 1);
            baseTexture.SetData(new[] { Color.White });
            Texture2D font = Content.Load<Texture2D>("font");

            // It doesn't seem to matter that these guys all share a base texture, so long as they specify their own
            // color for it
            Gore.Texture = baseTexture;
            Grenade.Texture = baseTexture;
            Player.Texture = baseTexture;
            Enemy.Texture = baseTexture;
            Bullet.Texture = baseTexture;
            Gore.Texture = baseTexture;
            Coin.Texture = baseTexture;

            Countdown.Texture = font;
            CoinCounter.Texture = font;

            // Add sounds

            Audio.AddSoundEffect("shoot", Content.Load<SoundEffect>("shoot"));
            Audio.AddSoundEffect("cock", Content.Load<SoundEffect>("cock"));
            Audio.AddSoundEffect("headexploding1", Content.Load<SoundEffect>("headexploding1"));
            Audio.AddSoundEffect("headexploding2", Content.Load<SoundEffect>("headexploding2"));
            Audio.AddSoundEffect("headexploding3", Content.Load<SoundEffect>("headexploding3"));
            Audio.AddSoundEffect("coinpickup", Content.Load<SoundEffect>("coinpickup"));
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
                BlendState.NonPremultiplied,
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