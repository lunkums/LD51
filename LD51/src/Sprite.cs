using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LD51
{
    public class Sprite : ISprite
    {
        public static readonly int GLOBAL_SCALE = Data.Get<int>("spriteGlobalScale");

        private Texture2D texture;
        private Point bounds;
        private float layerDepth;
        private float localScale;

        public Sprite(Texture2D texture, Point bounds, Color color, float layerDepth, float localScale = 1f)
        {
            this.texture = texture;
            this.bounds = bounds;
            Color = color;
            this.layerDepth = layerDepth;
            this.localScale = localScale;

            TexturePosition = Point.Zero;
        }

        // Use these to form a "cutout" of the texture2D
        public Point TexturePosition { private get; set; }
        public float LocalScale => localScale;
        public float Alpha { set => Color = new Color(Color, value); }
        public Point Bounds { set => bounds = value; }
        public Color Color { get; set; }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            // Flip the Y coord so we can imagine a positive Y being upwards
            // Subtract the Y bounds from the Y position to align the sprite
            spriteBatch.Draw(
                texture,
                new Rectangle(
                    (int)position.X,
                    -1 * (int)(position.Y + (bounds.Y * GLOBAL_SCALE * localScale)),
                    (int)(bounds.X * GLOBAL_SCALE * localScale),
                    (int)(bounds.Y * GLOBAL_SCALE * localScale)),
                new Rectangle(TexturePosition, bounds),
                Color, 0, Vector2.Zero, SpriteEffects.None, layerDepth);
        }
    }
}
