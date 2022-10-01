using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LD51
{
    public class Level
    {
        private Player player;
        private Vector2 startingPosition = Vector2.Zero;
        private float playerMovementSpeed = 512 / 2f;

        public Vector2 StartingPosition { set => startingPosition = value; }

        public void Initialize()
        {
            player = new Player(startingPosition, playerMovementSpeed);
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
                    Collision.HandleCollision(bullet, enemy);
                }
            }

            foreach (Enemy enemy in Enemy.Instances)
            {
                Collision.HandleCollision(enemy, player);
            }

            // Debug

            if (Input.IsKeyPressed(Keys.F12))
                Enemy.Spawn(Vector2.Zero, playerMovementSpeed / 2f);

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
        }

        public void GameOver()
        {
            Main.TimeScale = 0f;
        }

        public void Reset()
        {
            Main.TimeScale = 1f;

            // Reset player
            player.Position = startingPosition;

            // Reset entities
            Bullet.DespawnAll();
            Enemy.DespawnAll();
        }
    }
}
