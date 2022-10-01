using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace LD51
{
    public class Bullet
    {
        public static Texture2D texture;

        private static Dictionary<uint, Bullet> bullets = new Dictionary<uint, Bullet>();
        private static uint lastInstanceNum = 0;

        private const float lifeTimeInSeconds = 1;

        private Vector2 position;
        private Vector2 direction;
        private float speed;
        private float remainingLife;
        private uint instanceNum;

        private Bullet(Vector2 position, Vector2 direction, float speed)
        {
            this.position = position;
            this.direction = direction;
            this.speed = speed;

            remainingLife = lifeTimeInSeconds;
            instanceNum = lastInstanceNum++;
        }

        public static IEnumerable Bullets => bullets.Values;

        public static void Spawn(Vector2 position, Vector2 direction, float speed)
        {
            Bullet bullet = new Bullet(position, direction, speed);
            bullets.Add(bullet.instanceNum, bullet);
        }

        public void Update(float deltaTime)
        {
            remainingLife -= deltaTime;

            if (remainingLife < 0)
                Despawn();

            position += speed * direction * deltaTime;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle((int)position.X, -(int)position.Y, 16, 16), Color.Yellow);
        }

        private void Despawn()
        {
            bullets.Remove(instanceNum);
        }
    }
}
