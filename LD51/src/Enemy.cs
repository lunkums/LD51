using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;
using System.Collections.Generic;

namespace LD51
{
    public class Enemy
    {
        public static Texture2D texture;

        private static Dictionary<uint, Enemy> enemies = new Dictionary<uint, Enemy>();
        private static uint lastInstanceNum = 0;

        private Vector2 position;
        private float speed;
        private uint instanceNum;

        private Enemy(Vector2 position, float speed)
        {
            this.position = position;
            this.speed = speed;

            Direction = new Vector2();
            instanceNum = lastInstanceNum++;
        }

        public static IEnumerable Enemies => enemies.Values;
        
        public Vector2 Direction { get; set; }
        public Vector2 Position => position;

        public static void Spawn(Vector2 position, float speed)
        {
            Enemy enemy = new Enemy(position, speed);
            enemies.Add(enemy.instanceNum, enemy);
        }

        public void Update(float deltaTime)
        {
            position += speed * Direction * deltaTime;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle((int)position.X, -(int)position.Y, 32, 32), Color.Red);
        }

        public void Despawn()
        {
            enemies.Remove(instanceNum);
        }
    }
}
