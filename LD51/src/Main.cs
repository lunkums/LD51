﻿using Microsoft.Xna.Framework;
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

        protected override void Initialize()
        {
            // Initialize the level
            level = new Level();
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
            Bullet.Texture = baseTexture;
            Coin.Texture = baseTexture;
            Enemy.Texture = baseTexture;
            Gore.Texture = baseTexture;
            Grenade.Texture = baseTexture;
            Player.Texture = baseTexture;

            Countdown.Texture = font;
            CoinCounter.Texture = font;

            GameOverScreen.Texture = Content.Load<Texture2D>("gameoverscreen");
            TitleScreen.Texture = Content.Load<Texture2D>("titlescreen");

            // Add sounds
            Audio.AddSoundEffect("cock", Content.Load<SoundEffect>("cock"));
            Audio.AddSoundEffect("clack", Content.Load<SoundEffect>("clack"));
            Audio.AddSoundEffect("click", Content.Load<SoundEffect>("click"));
            Audio.AddSoundEffect("coinpickup", Content.Load<SoundEffect>("coinpickup")); 
            Audio.AddSoundEffect("grenadeexploding", Content.Load<SoundEffect>("grenadeexploding"));
            Audio.AddSoundEffect("grenadepriming", Content.Load<SoundEffect>("grenadepriming"));
            Audio.AddSoundEffect("headexploding1", Content.Load<SoundEffect>("headexploding1"));
            Audio.AddSoundEffect("headexploding2", Content.Load<SoundEffect>("headexploding2"));
            Audio.AddSoundEffect("headexploding3", Content.Load<SoundEffect>("headexploding3"));
            Audio.AddSoundEffect("laughter1", Content.Load<SoundEffect>("laughter1"));
            Audio.AddSoundEffect("laughter2", Content.Load<SoundEffect>("laughter2"));
            Audio.AddSoundEffect("shoot", Content.Load<SoundEffect>("shoot"));
        }

        protected override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds * TimeScale;
            frameRate = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;

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

        public static void Reset()
        {
            level.Reset();
        }
    }
}