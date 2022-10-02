using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LD51
{
    public class CoinCounter
    {
        private static readonly Point _positionOfFirstDigit = new Point(
            Data.Get<int>("countdownSpritesheetLastDigitPositionX"),
            Data.Get<int>("countdownSpritesheetLastDigitPositionY"));
        private static readonly Point _positionOfDollarSign = new Point(
            Data.Get<int>("fontSpritesheetDollarSignPositionX"),
            Data.Get<int>("fontSpritesheetDollarSignPositionY"));
        private static readonly float _layerDepth = Data.Get<int>("hudLayerDepth");

        private static Texture2D texture;

        // These fields need to be static so that they are initialized after this class is instantiated but before the
        // first draw call occurs
        private static Point digitBounds;
        private static Sprite digitSprite;
        private static Point dollarSignBounds;
        private static Sprite dollarSignSprite;

        public static Texture2D Texture
        {
            get => texture;
            set
            {
                texture = value;

                digitBounds = new Point(8, 8);
                digitSprite = new Sprite(Texture, digitBounds, Color.Yellow, _layerDepth, 1 / 2f);

                dollarSignBounds = new Point(8, 10);
                dollarSignSprite = new Sprite(Texture, dollarSignBounds, Color.Yellow, _layerDepth, 1 / 2f)
                {
                    TexturePosition = _positionOfDollarSign
                };
            }
        }

        public int NumberOfCoins { get; set; } = 0;

        public void Update()
        {
            SetTextureOffset(NumberOfCoins);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 digitPosition = new Vector2(120, -128 + 2)
                * digitSprite.LocalScale * Sprite.GLOBAL_SCALE;
            Vector2 dollarSignPosition = new Vector2(112, -128 + 1)
                * digitSprite.LocalScale * Sprite.GLOBAL_SCALE;

            digitSprite.Draw(spriteBatch, digitPosition);
            dollarSignSprite.Draw(spriteBatch, dollarSignPosition);
        }

        private void SetTextureOffset(int numOfCoins)
        {
            digitSprite.TexturePosition = _positionOfFirstDigit
                + new Point((numOfCoins % 10) * digitBounds.X, 0);
        }
    }
}
