using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace LD51
{
    public class FpsCounter
    {
        private static readonly Point _positionOfFirstDigit = new Point(
            Data.Get<int>("countdownSpritesheetLastDigitPositionX"),
            Data.Get<int>("countdownSpritesheetLastDigitPositionY"));
        private static readonly float _layerDepth = Data.Get<float>("hudLayerDepth");

        private static Texture2D texture;
        private static Point bounds;

        private static Sprite firstDigitSprite;
        private static Sprite secondDigitSprite;
        private static Sprite thirdDigitSprite;

        private int frameRate;

        public FpsCounter()
        {
            frameRate = 0;
            Enabled = false;
        }

        public static Texture2D Texture
        {
            get => texture;
            set
            {
                texture = value;
                bounds = new Point(8, 8);
                firstDigitSprite = new Sprite(texture, bounds, Color.Lime, _layerDepth, 1 / 2f)
                {
                    TexturePosition = new Point(200, 8)
                };
                secondDigitSprite = new Sprite(texture, bounds, Color.Lime, _layerDepth, 1 / 2f)
                {
                    TexturePosition = new Point(200, 8)
                };
                thirdDigitSprite = new Sprite(texture, bounds, Color.Lime, _layerDepth, 1 / 2f)
                {
                    TexturePosition = new Point(200, 8)
                };
            }
        }

        private bool Enabled { get; set; }

        public void Update(float frameRate)
        {
            this.frameRate = (int)MathF.Round(frameRate);

            SetTextureOffsets();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!Enabled) return;

            firstDigitSprite.Draw(spriteBatch, GetDigitPosition(0));
            secondDigitSprite.Draw(spriteBatch, GetDigitPosition(1));
            thirdDigitSprite.Draw(spriteBatch, GetDigitPosition(2));
        }

        public void Toggle()
        {
            Enabled = !Enabled;
        }

        private static Vector2 GetDigitPosition(int digit)
        {
            // This is awful. I know.
            return (new Vector2(120, -(bounds.X + 2)) - Vector2.UnitX * (8 * digit))
                   * firstDigitSprite.LocalScale * Sprite.GLOBAL_SCALE;
        }

        private Point GetNextDigitTextureOffset(bool allowZero = false)
        {
            int remainder = frameRate % 10;
            frameRate /= 10;
            return remainder == 0 && !allowZero ?
                Point.Zero : _positionOfFirstDigit + new Point(remainder * bounds.X, 0);
        } 

        private void SetTextureOffsets()
        {
            firstDigitSprite.TexturePosition = GetNextDigitTextureOffset(true);
            secondDigitSprite.TexturePosition = GetNextDigitTextureOffset();
            thirdDigitSprite.TexturePosition = GetNextDigitTextureOffset();
        }
    }
}
