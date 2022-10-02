using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace LD51
{
    public class GameOverScreen : IGoodUpdateable, IGoodDrawable
    {
        private static readonly float _layerDepth = Data.Get<int>("hudLayerDepth");
        private static readonly float _scrollSpeed = Data.Get<float>("titleScrollSpeed");
        private static readonly float _scrollDelay = Data.Get<float>("gameOverScrollDelay");

        private static Texture2D texture;

        private static Point bounds;
        private static Sprite sprite;

        private Vector2 position;
        private int direction;
        private float scrollCountdown;

        public GameOverScreen()
        {
            position = Vector2.Zero;
            direction = 1;
            scrollCountdown = 0;
            OnDisappear += () => { };
            OnDeactivate += () => { };
        }

        public event Action OnDisappear;
        public event Action OnDeactivate;

        public bool Visible => position.Y < 0;
        public bool FullyVisible => position.Y <= -128;
        public bool Active
        {
            get => direction < 0;
            set
            {
                direction = value ? -1 : 1;
                if (!Active) OnDeactivate.Invoke();
                else scrollCountdown = _scrollDelay;
            }
        }

        public static Texture2D Texture
        {
            get => texture;
            set
            {
                texture = value;
                bounds = new Point(128, 128);
                sprite = new Sprite(Texture, bounds, Color.White, _layerDepth, 1 / 2f);
            }
        }

        public void Update(float deltaTime)
        {
            if ((Active && FullyVisible) || (!Active && !Visible)) return;

            scrollCountdown = MathF.Max(scrollCountdown - deltaTime, -0.1f);

            if (scrollCountdown > 0) return;

            position += Vector2.UnitY * Math.Sign(direction) * _scrollSpeed * deltaTime;

            if (!Active && !Visible) OnDisappear.Invoke();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, position * sprite.LocalScale * Sprite.GLOBAL_SCALE);
        }
    }
}
