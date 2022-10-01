using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace LD51
{
    public class Timer
    {
        private static Texture2D texture;
        private static Point bounds;
        private static Sprite sprite;

        private readonly float lengthInSeconds;

        private float timer;

        public Timer(float lengthInSeconds)
        {
            this.lengthInSeconds = lengthInSeconds;
            timer = lengthInSeconds;
        }

        public static Texture2D Texture
        {
            get => texture;
            set
            {
                texture = value;
                bounds = new Point(8, 8);
                sprite = new Sprite(texture, bounds, Color.White, 1 / 2f)
                {
                    TexturePosition = new Point(128, 8)
                };
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 position = new Vector2(2, -(bounds.X + 2)) * Sprite.GLOBAL_SCALE * sprite.LocalScale;
            sprite.Draw(spriteBatch, position);
        }
    }
}
