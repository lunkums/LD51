using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LD51
{
    public class CoinCounter
    {
        private static readonly Point _positionOfFirstDigit = new Point(
            Data.Get<int>("countdownSpritesheetLastDigitPositionX"),
            Data.Get<int>("countdownSpritesheetLastDigitPositionY"));

        private static Texture2D texture;
        private static Point bounds;
        private static Sprite sprite;

        private Player player;

        public CoinCounter(Player player)
        {
            this.player = player;
        }

        public static Texture2D Texture
        {
            get => texture;
            set
            {
                texture = value;
                bounds = new Point(8, 8);
                sprite = new Sprite(texture, bounds, Color.Yellow, 1 / 2f)
                {
                    TexturePosition = new Point(200, 8)
                };
            }
        }

        public void Update()
        {
            SetTextureOffset();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 position = new Vector2(120, -128 + 2)
                * sprite.LocalScale * Sprite.GLOBAL_SCALE;
            sprite.Draw(spriteBatch, position);
        }

        private void SetTextureOffset()
        {
            sprite.TexturePosition = _positionOfFirstDigit + new Point((player.NumberOfCoins % 10) * bounds.X, 0);
        }
    }
}
