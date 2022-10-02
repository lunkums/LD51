using Microsoft.Xna.Framework;

namespace LD51
{
    public interface IExplodeable
    {
        int Size { get; }
        Vector2 Center { get; }
        Color DebrisColor { get; }
    }
}