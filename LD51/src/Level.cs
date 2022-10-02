using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace LD51
{
    public class Level
    {
        private static string[] laughSfx = new string[] { "laughter1", "laughter2" };

        private Player player;
        private Rand rand;

        // HUD
        private Countdown countdown;
        private CoinCounter coinCounter;
        private TitleScreen titleScreen;
        private GameOverScreen gameOverScreen;

        private int maxNumOfEnemies;
        private bool gameOver;

        public void Initialize()
        {
            maxNumOfEnemies = 3;

            player = new Player();
            rand = new Rand();
            gameOver = true;

            countdown = new Countdown();
            coinCounter = new CoinCounter();
            titleScreen = new TitleScreen();
            gameOverScreen = new GameOverScreen();

            titleScreen.OnDisappear += Reset;
            gameOverScreen.OnDeactivate += () =>
            {
                Reset();
                countdown.Stopped = true;
            };
            gameOverScreen.OnDisappear += () => { countdown.Stopped = false; };

            countdown.OnCountdownEnd += AttackEvent;
        }

        private IPlayer Player => player == null ? NullPlayer.Instance : player;

        private IEnumerable<IEntity> Entities =>
            (Player as IEntity).Yield()
            .Concat(Coin.Instances)
            .Concat(Enemy.Instances)
            .Concat(Gore.Instances)
            .Concat(Grenade.Instances)
            .Concat(Bullet.Instances);

        public void Update(float deltaTime)
        {
            // Persistent actions
            if (Input.IsKeyPressed(Keys.OemPlus))
                Audio.IncreaseVolume();

            if (Input.IsKeyPressed(Keys.OemMinus))
                Audio.DecreaseVolume();

            UpdateHUD(deltaTime);

            // Can't hit retry if the game over screen hasn't fully loaded
            if (!(gameOverScreen.Active && !gameOverScreen.FullyVisible))
            {
                if ((Input.IsKeyPressed(Keys.Enter) || Input.LeftMousePressed()) && gameOver)
                {
                    Audio.Play("clack");
                    titleScreen.ScrollUp();
                    gameOverScreen.Active = false;
                }
            }

            if (titleScreen.Visible || (!gameOverScreen.Active && gameOverScreen.Visible)) return;

            UpdateGameLogic(deltaTime);
            UpdateCollisions();
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

        public void GameOver()
        {
            if (gameOver) return;

            Audio.PlayRandom(laughSfx);
            GoreFactory.SpawnRandomGoreExplosion(Player);
            player = null;
            gameOver = true;
            countdown.Stopped = true;
            gameOverScreen.Active = true;
        }

        public void Reset()
        {
            Main.TimeScale = 1f;

            player = new Player();

            // Reset locals
            maxNumOfEnemies = 3;
            gameOver = false;

            // Reset entities
            foreach (IEntity entity in Entities)
            {
                entity.Despawn();
            }

            // Reset countdown
            countdown.Reset();
        }

        /*
         * Update
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

            coinCounter.NumberOfCoins = player?.NumberOfCoins ?? 0;
            coinCounter.Update();

            gameOverScreen.Update(deltaTime);
            titleScreen.Update(deltaTime);
        }

        /*
         * Events
         */

        private void AttackEvent()
        {
            SpawnEnemies(maxNumOfEnemies++);
        }

        private void SpawnEnemies(int maxNumOfEnemies)
        {
            int numOfEnemies = rand.NextInt(maxNumOfEnemies / 2, maxNumOfEnemies);

            for (int i = 0; i < numOfEnemies; i++)
            {
                Enemy.Spawn(GetRandomSpawnPosition(), rand.NextInt(1, 4));
            }
        }

        private Vector2 GetRandomSpawnPosition()
        {
            int randomSide = rand.NextInt(0, 4);

            switch (randomSide)
            {
                case 0:
                    // Top
                    return new Vector2(rand.NextInt(0, 512), rand.NextInt(0, 32));
                case 1:
                    // Bottom
                    return new Vector2(rand.NextInt(0, 512), -rand.NextInt(512 + 32, 512 + 64));
                case 2:
                    // Left
                    return new Vector2(rand.NextInt(-64, -32), -rand.NextInt(0, 512));
                case 3:
                    // Right
                    return new Vector2(rand.NextInt(512, 512 + 32), -rand.NextInt(0, 512));
                default:
                    // Shouldn't happen
                    return new Vector2(
                        Data.Get<float>("playerStartingPositionX"),
                        Data.Get<float>("playerStartingPositionY"));
            }
        }
    }
}
