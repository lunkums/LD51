using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LD51
{
    public sealed class NullPlayer : IPlayer
    {
        private static readonly NullPlayer instance = new NullPlayer();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static NullPlayer() {}

        private NullPlayer() {}

        public static NullPlayer Instance => instance;

        public Rectangle Hitbox => Rectangle.Empty;

        // This shouldn't happen
        public void CollisionResponse(Collision collision) { }
        public void Draw(SpriteBatch spriteBatch) { }

        public void Update(float deltaTime) { }

        public uint Id => 0;
        public void Despawn() { }

        public int Size => 0;
        public Vector2 Center => Vector2.Zero;
        public Color DebrisColor => Color.White;
        public Vector2 Position { get; } = new Vector2(-3825968, 3825968);
    }
}