using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace LD51
{
    public class Countdown
    {
        public event Action OnCountdownEnd;

        private static Texture2D texture;
        private static Point bounds;
        private static Sprite sprite;

        private readonly Point positionOfLastDigit = new Point(128, 8);
        private readonly float lengthInSeconds;

        private float countdown;

        public Countdown(float lengthInSeconds)
        {
            this.lengthInSeconds = lengthInSeconds;
            countdown = lengthInSeconds;
            OnCountdownEnd += () => { };
        }

        public static Texture2D Texture
        {
            get => texture;
            set
            {
                texture = value;
                bounds = new Point(8, 8);
                sprite = new Sprite(texture, bounds, Color.White, 1 / 2f)
                {
                    TexturePosition = new Point(200, 8)
                };
            }
        }

        public void Update(float deltaTime)
        {
            countdown -= deltaTime;

            if (countdown < 0)
            {
                countdown = lengthInSeconds;
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
            countdown = lengthInSeconds;
        }

        private void SetTextureOffset()
        {
            // Floor the countdown and cap it at 1 less than the max since that digit would only display for a single
            // frame anyways
            int rounded = Math.Min((int)Math.Floor(countdown), (int)lengthInSeconds - 1);
            sprite.TexturePosition = positionOfLastDigit + new Point(rounded * bounds.X, 0);
        }
    }
}
