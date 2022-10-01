using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LD51
{
    public class Sprite : ISprite
    {
        private Texture2D texture;
        private Point bounds;
        private Color color;

        public Sprite(Texture2D texture, Point bounds, Color color)
        {
            this.texture = texture;
            this.bounds = bounds;
            this.color = color;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            // Flip the Y coord so we can imagine a positive Y being upwards
            // Subtract the Y bounds from the Y position to align the sprite
            spriteBatch.Draw(
                texture,
                new Rectangle((int)position.X, -(int)position.Y - bounds.Y, bounds.X, bounds.Y),
                color);
        }
    }
}
