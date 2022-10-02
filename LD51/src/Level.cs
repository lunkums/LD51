using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace LD51
{
    public class Level
    {
        private Player player;
        private Countdown countdown;
        private CoinCounter coinCounter;
        private Rand rand;

        // Starting position is set in Main
        private int maxNumOfEnemies;

        public void Initialize()
        {
            maxNumOfEnemies = 3;

            player = new Player();
            countdown = new Countdown();
            coinCounter = new CoinCounter(player);
            rand = new Rand();

            countdown.OnCountdownEnd += AttackEvent;
        }

        public IEnumerable<IEntity> Entities =>
            Bullet.Instances
            .Concat(Coin.Instances)
            .Concat(Enemy.Instances)
            .Concat(Gore.Instances)
            .Concat(Grenade.Instances)
            .Concat((player as IEntity).Yield());

        public void Update(float deltaTime)
        {
            UpdateGameLogic(deltaTime);
            UpdateCollisions();
            UpdateHUD(deltaTime);

            // Debug

            if (Input.IsKeyPressed(Keys.F12))
                Enemy.Spawn(Vector2.Zero, 3);

            if (Input.IsKeyPressed(Keys.F11))
                foreach (Enemy enemy in Enemy.Instances)
                {
                    enemy.Speed = 0;
                }

            if (Input.IsKeyPressed(Keys.R))
                Reset();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw order is based on the order in which these methods are called (i.e. the HUD is drawn on top of
            // everything because its draw method is called last)

            foreach (Gore gore in Gore.Instances)
            {
                gore.Draw(spriteBatch);
            }

            foreach (Coin coin in Coin.Instances)
            {
                coin.Draw(spriteBatch);
            }

            foreach (Enemy enemy in Enemy.Instances)
            {
                enemy.Draw(spriteBatch);
            }

            player.Draw(spriteBatch);

            foreach (Grenade grenade in Grenade.Instances)
            {
                grenade.Draw(spriteBatch);
            }

            foreach (Bullet bullet in Bullet.Instances)
            {
                bullet.Draw(spriteBatch);
            }

            // HUD

            countdown.Draw(spriteBatch);
            coinCounter.Draw(spriteBatch);
        }

        public void GameOver()
        {
            Main.TimeScale = 0f;
        }

        public void Reset()
        {
            Main.TimeScale = 1f;

            // Reset locals
            maxNumOfEnemies = 3;

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
                enemy.Direction = (player.Position - enemy.Position).Normalized();
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
                CollisionHandler.HandleCollision(enemy, player);
            }

            foreach (Coin coin in Coin.Instances)
            {
                CollisionHandler.HandleCollision(coin, player);
            }

            // This is an n^2 collision checked... probably not the best
            foreach (Enemy enemy in Enemy.Instances)
            {
                foreach (Enemy otherEnemy in Enemy.Instances)
                {
                    CollisionHandler.HandleCollision(enemy, otherEnemy);
                }
            }
        }

        private void UpdateHUD(float deltaTime)
        {
            countdown.Update(deltaTime);
            coinCounter.Update();
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
