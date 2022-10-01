using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LD51
{
    public class Level
    {
        private const float countdownLengthInSeconds = 10f;

        private Player player;
        private Countdown countdown;
        private Rand rand;

        // Starting position is set in Main
        private Vector2 startingPosition;
        private float playerMovementSpeed;
        private int maxNumOfEnemies;

        public Vector2 StartingPosition { set => startingPosition = value; }

        public void Initialize()
        {
            playerMovementSpeed = 512 / 2f;
            Enemy.MaxSpeed = playerMovementSpeed;
            maxNumOfEnemies = 3;

            player = new Player(startingPosition, playerMovementSpeed);
            countdown = new Countdown(countdownLengthInSeconds);
            rand = new Rand();

            countdown.OnCountdownEnd += AttackEvent;
        }

        public void Update(float deltaTime)
        {
            // Player

            player.Update(deltaTime);

            // Enemy

            foreach (Enemy enemy in Enemy.Instances)
            {
                enemy.Direction = (player.Position - enemy.Position).Normalized();
                enemy.Update(deltaTime);
            }

            // Bullets

            foreach (Bullet bullet in Bullet.Instances)
            {
                bullet.Update(deltaTime);
            }

            // Collisions

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

            // This is an n^2 collision checked... probably not the best
            foreach (Enemy enemy in Enemy.Instances)
            {
                foreach (Enemy otherEnemy in Enemy.Instances)
                {
                    CollisionHandler.HandleCollision(enemy, otherEnemy);
                }
            }

            // HUD

            countdown.Update(deltaTime);

            // Debug

            if (Input.IsKeyPressed(Keys.F12))
                Enemy.Spawn(Vector2.Zero, 1);

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
            player.Draw(spriteBatch);

            foreach (Enemy enemy in Enemy.Instances)
            {
                enemy.Draw(spriteBatch);
            }

            foreach (Bullet bullet in Bullet.Instances)
            {
                bullet.Draw(spriteBatch);
            }

            // HUD

            countdown.Draw(spriteBatch);
        }

        public void GameOver()
        {
            Main.TimeScale = 0f;
        }

        public void Reset()
        {
            Main.TimeScale = 1f;

            // Reset locals
            player.Position = startingPosition;
            maxNumOfEnemies = 3;

            // Reset entities
            Bullet.DespawnAll();
            Enemy.DespawnAll();

            // Reset countdown
            countdown.Reset();
        }

        private void AttackEvent()
        {
            SpawnEnemies(maxNumOfEnemies);
            maxNumOfEnemies += 3;
        }

        private void SpawnEnemies(int maxNumOfEnemies)
        {
            int numOfEnemies = rand.NextInt(maxNumOfEnemies / 2, maxNumOfEnemies);

            for (int i = 0; i < numOfEnemies; i++)
            {
                Enemy.Spawn(new Vector2(rand.NextInt(0, 512), rand.NextInt(0, 32)), rand.NextInt(1, 4));
            }
        }
    }
}
