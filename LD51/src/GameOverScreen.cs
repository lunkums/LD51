using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LD51
{
    public class GameOverScreen : IGoodUpdateable, IGoodDrawable
    {
        private static readonly float _layerDepth = Data.Get<int>("hudLayerDepth");

        private static Texture2D texture;

        private static Point bounds;
        private static Sprite sprite;

        public static Texture2D Texture
        {
            get => texture;
            set
            {
                texture = value;
                bounds = new Point(128, 128);
                sprite = new Sprite(Texture, bounds, Color.White, _layerDepth, 1 / 2f);
            }
        }

        public void Update(float deltaTime)
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
        }
    }
}
