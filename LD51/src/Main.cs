﻿using Microsoft.Xna.Framework;
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
        private Vector2 playerPosition;
        private float playerMovementSpeed = 512 / 2f;

        private float enemyMovementSpeed;

        private MouseState previousMouseState;

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
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            playerPosition = new Vector2(screenSize.X / 2f, -screenSize.Y / 2f);
            enemyMovementSpeed = playerMovementSpeed / 2f;
            previousMouseState = Mouse.GetState();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            player = new Texture2D(GraphicsDevice, 1, 1);
            player.SetData(new[] { Color.White });

            Enemy.texture = new Texture2D(GraphicsDevice, 1, 1);
            Enemy.texture.SetData(new[] { Color.White });

            Bullet.texture = new Texture2D(GraphicsDevice, 1, 1);
            Bullet.texture.SetData(new[] { Color.Yellow });
        }

        protected override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            /// Input

            Input.Update();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            /// Player

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

            playerPosition += moveDirection.Normalized() * playerMovementSpeed * deltaTime;

            // Shooting

            MouseState currentMouseState = Mouse.GetState();
            if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
            {
                Vector2 pointToMouse = new Vector2(currentMouseState.X, -currentMouseState.Y) - playerPosition;
                Bullet.Spawn(playerPosition, pointToMouse.Normalized(), playerMovementSpeed * 4);
            }
            previousMouseState = currentMouseState;

            /// Enemy
            
            foreach (Enemy enemy in Enemy.Enemies)
            {
                enemy.Direction = (playerPosition - enemy.Position).Normalized();
                enemy.Update(deltaTime);
            }

            /// Bullets

            foreach (Bullet bullet in Bullet.Bullets)
            {
                bullet.Update(deltaTime);
            }

            /// Debug

            if (Input.IsKeyPressed(Keys.F12))
                Enemy.Spawn(Vector2.Zero, enemyMovementSpeed);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            spriteBatch.Draw(player, new Rectangle((int)playerPosition.X, -(int)playerPosition.Y, 32, 32), Color.White);

            foreach (Enemy enemy in Enemy.Enemies)
            {
                enemy.Draw(spriteBatch);
            }

            foreach (Bullet bullet in Bullet.Bullets)
            {
                bullet.Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }

    public static class LunkumsMath
    {
        public static Vector2 Normalized(this Vector2 vector)
        {
            return IsZero(vector.Length()) ? vector : Vector2.Normalize(vector);
        }

        public static bool IsZero(float num)
        {
            return Math.Abs(num) <= float.Epsilon;
        }
    }
}