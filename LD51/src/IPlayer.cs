using Microsoft.Xna.Framework;

namespace LD51
{
    public interface IPlayer : ICollider, IEntity, IExplodable
    {
        Vector2 Position { get; }
        int NumberOfCoins { get; }
    }
}