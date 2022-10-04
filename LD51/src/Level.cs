using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace LD51
{
    public class Level
    {
        private static readonly float _scrollDelay = Data.Get<float>("gameOverScrollDelay");

        private static string[] laughSfx = new string[] { "laughter1", "laughter2" };

        private Game game;
        private State state;
        private Player player;
        private Arena arena;

        // HUD
        private Countdown countdown;
        private CoinCounter coinCounter;
        private TitleScreen titleScreen;
        private GameOverScreen gameOverScreen;

        public Level(Game game)
        {
            this.game = game;
        }

        public void Initialize()
        {
            state = State.GameOver;
            player = new Player();
            arena = new Arena();

            countdown = new Countdown();
            coinCounter = new CoinCounter();
            titleScreen = new TitleScreen();
            gameOverScreen = new GameOverScreen();

            titleScreen.OnDisappear += Play;
            gameOverScreen.OnDisappear += Play;
            gameOverScreen.OnFullyAppear += () =>
            {
                state = State.GameOver;
            };

            countdown.OnCountdownEnd += arena.BeginNextRound;
        }
        
        private enum State
        {
            Playing, // Indicates the game logical is still updating
            Resetting, // Indicates going from "GameOver" state to "Playing" state
            GameOver
        }

        private IPlayer Player => player == null ? NullPlayer.Instance : player;

        private IEnumerable<IEntity> Entities =>
            (Player as IEntity).Yield()
            .Concat(Coin.Instances)
            .Concat(Enemy.Instances)
            .Concat(Gore.Instances)
            .Concat(Grenade.Instances)
            .Concat(Bullet.Instances);

        /*
         * Main loops
         */

        public void Update(float deltaTime)
        {
            // Persistent actions
            if (Input.IsKeyPressed(Keys.Escape))
                game.Exit();

            if (Input.IsKeyPressed(Keys.OemPlus))
                Audio.IncreaseVolume();

            if (Input.IsKeyPressed(Keys.OemMinus))
                Audio.DecreaseVolume();

            UpdateHUD(deltaTime);
            
            switch (state)
            {
                case State.GameOver:
                    if (Input.IsKeyPressed(Keys.Enter) || Input.LeftMousePressed())
                    {
                        Audio.Play("clack");
                        Reset();
                    }
                    break;
                case State.Playing:
                    UpdateGameLogic(deltaTime);
                    UpdateCollisions();
                    break;
                case State.Resetting:
                    // Don't update the game or take input if the level is resetting
                    break;
                default:
                    // Shouldn't happen
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw order is determined by layerDepth
            foreach (IEntity entity in Entities)
            {
                entity.Draw(spriteBatch);
            }

            // HUD
            countdown.Draw(spriteBatch);
            coinCounter.Draw(spriteBatch);
            titleScreen.Draw(spriteBatch);
            gameOverScreen.Draw(spriteBatch);
        }

        /*
         * Game state
         */

        public void Play()
        {
            state = State.Playing;
            countdown.Start();
        }

        public void Reset()
        {
            state = State.Resetting;

            titleScreen.ScrollUp();
            gameOverScreen.Active = false;

            // Reset entities
            foreach (IEntity entity in Entities)
            {
                entity.Despawn();
            }
            player = new Player();

            countdown.Reset();
            arena.Reset();
        }

        public void GameOver()
        {
            if (state == State.GameOver) return;

            GoreFactory.SpawnRandomGoreExplosion(Player);
            player = null;
            countdown.Stop();

            Coroutine.InvokeDelayed(() =>
            {
                Audio.PlayRandom(laughSfx);
                gameOverScreen.Active = true;
            }, _scrollDelay);
        }

        /*
         * Update methods
         */

        private void UpdateGameLogic(float deltaTime)
        {
            foreach (Enemy enemy in Enemy.Instances)
            {
                enemy.Direction = (Player.Position - enemy.Position).Normalized();
            }

            foreach (IEntity entity in Entities)
            {
                entity.Update(deltaTime);
            }
        }

        private void UpdateCollisions()
        {
            foreach (Bullet bullet in Bullet.Instances)
            {
                foreach (Enemy enemy in Enemy.Instances)
                {
                    CollisionHandler.HandleCollision(bullet, enemy);
                }
            }

            foreach (Enemy enemy in Enemy.Instances)
            {
                CollisionHandler.HandleCollision(enemy, Player);
            }

            foreach (Coin coin in Coin.Instances)
            {
                CollisionHandler.HandleCollision(coin, Player);
            }

            foreach (Grenade grenade in Grenade.Instances)
            {
                CollisionHandler.HandleCollision(grenade, Player);
            }

            // This is an n^2 collision checked... probably not the best
            foreach (Enemy enemy in Enemy.Instances)
            {
                foreach (Enemy otherEnemy in Enemy.Instances)
                {
                    CollisionHandler.HandleCollision(enemy, otherEnemy);
                }

                foreach (Grenade grenade in Grenade.Instances)
                {
                    CollisionHandler.HandleCollision(grenade, enemy);
                }
            }
        }

        private void UpdateHUD(float deltaTime)
        {
            countdown.Update(deltaTime);

            coinCounter.NumberOfCoins = Player.NumberOfCoins;
            coinCounter.Update();

            gameOverScreen.Update(deltaTime);
            titleScreen.Update(deltaTime);
        }
    }
}
