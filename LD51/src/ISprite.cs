using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LD51
{
    public interface ISprite
    {
        void Draw(SpriteBatch spriteBatch, Vector2 position);
    }
}