using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace LD51
{
    public class Countdown
    {
        private static readonly Point _positionOfLastDigit = new Point(
            Data.Get<int>("countdownSpritesheetLastDigitPositionX"),
            Data.Get<int>("countdownSpritesheetLastDigitPositionY"));
        private static readonly float _lengthInSeconds = Data.Get<float>("countdownLengthInSeconds");
        private static readonly float _layerDepth = Data.Get<float>("hudLayerDepth");

        public event Action OnCountdownEnd;

        private static Texture2D texture;
        private static Point bounds;
        private static Sprite sprite;

        private float countdown;
        private bool stopped;

        public Countdown()
        {
            countdown = 0;
            stopped = true;
            OnCountdownEnd += () => { };
        }

        public static Texture2D Texture
        {
            get => texture;
            set
            {
                texture = value;
                bounds = new Point(8, 8);
                sprite = new Sprite(texture, bounds, Color.White, _layerDepth, 1 / 2f)
                {
                    TexturePosition = new Point(200, 8)
                };
            }
        }

        public void Update(float deltaTime)
        {
            if (stopped) return;

            countdown -= deltaTime;

            if (countdown < 0)
            {
                countdown = _lengthInSeconds;
                OnCountdownEnd.Invoke();
            }

            SetTextureOffset();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 position = new Vector2(2, -(bounds.X + 2)) * sprite.LocalScale * Sprite.GLOBAL_SCALE;
            sprite.Draw(spriteBatch, position);
        }

        public void Reset()
        {
            stopped = true;
            countdown = _lengthInSeconds;
            // Needed for edge cases of resetting the timer while gameTime is frozen
            SetTextureOffset();
        }

        public void Start()
        {
            stopped = false;
        }

        public void Stop()
        {
            stopped = true;
        }

        private void SetTextureOffset()
        {
            // Floor the countdown and cap it at 1 less than the max since that digit would only display for a single
            // frame anyways
            int rounded = (int)MathF.Min(MathF.Floor(countdown), _lengthInSeconds - 1);
            sprite.TexturePosition = _positionOfLastDigit + new Point(rounded * bounds.X, 0);
        }
    }
}
