using Microsoft.Xna.Framework;

namespace LD51
{
    public interface IExplodable
    {
        int Size { get; }
        Vector2 Center { get; }
        Color DebrisColor { get; }
    }
}