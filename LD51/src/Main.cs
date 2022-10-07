using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            IsMouseVisible = true;

            graphics.PreferredBackBufferWidth = _screenSize.X;
            graphics.PreferredBackBufferHeight = _screenSize.Y;
            graphics.ApplyChanges();

            // Register this listener so the "Invoke" will never be null
            OnUpdateEnd += () => { };
        }

        public float FrameRate { get; private set; }

        protected override void Initialize()
        {
            // Initialize the level
            level = new Level(this);
            level.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Content.RootDirectory = "Content";

            // Add textures
            Texture2D font = Content.LoadTexture("font");
            Texture2D baseTexture = new Texture2D(GraphicsDevice, 1, 1);
            baseTexture.SetData(new[] { Color.White });

            // It doesn't seem to matter that these guys all share a base texture, so long as they specify their own
            // color for it
            Bullet.Texture = baseTexture;
            Coin.Texture = baseTexture;
            Enemy.Texture = baseTexture;
            Gore.Texture = baseTexture;
            Grenade.Texture = baseTexture;
            Player.Texture = baseTexture;

            Countdown.Texture = font;
            CoinCounter.Texture = font;
            FpsCounter.Texture = font;

            GameOverScreen.Texture = Content.LoadTexture("gameoverscreen");
            TitleScreen.Texture = Content.LoadTexture("titlescreen");

            // Add sounds
            string[] soundEffects =
            {
                "cock", "clack", "click", "coinpickup", "grenadeexploding", "grenadepriming", "headexploding1",
                "headexploding2", "headexploding3", "laughter1", "laughter2", "shoot"
            };

            foreach (string effect in soundEffects)
            {
                Audio.AddSoundEffect(effect, Content.LoadSoundEffect(effect));
            }

            // Add music
            string[] musicTracks =
            {
                "exhilarate"
            };

            // Comment this loop out if you don't have access to the tracks
            foreach (string track in musicTracks)
            {
                //Audio.AddMusicTrack(track, Content.LoadMusicTrack(track));
            }
        }

        protected override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds * TimeScale;
            FrameRate = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            // Update input
            Input.Update(IsActive);

            // Update game logic
            level.Update(deltaTime);

            OnUpdateEnd.Invoke();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // BackToFront means a sprite with layerDepth 0 is drawn on top of a sprite with layerDepth 1
            // NonPremultiplied allows for sprite alphas
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied, SamplerState.PointClamp,
                DepthStencilState.None, RasterizerState.CullNone);
            level.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public static void GameOver()
        {
            level.GameOver();
        }
    }
}