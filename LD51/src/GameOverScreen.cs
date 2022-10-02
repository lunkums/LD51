using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LD51
{
    public class GameOverScreen : IGoodUpdateable, IGoodDrawable
    {
        private static readonly float _layerDepth = Data.Get<int>("hudLayerDepth");
        private static readonly float _scrollSpeed = Data.Get<float>("titleScrollSpeed");

        private static Texture2D texture;

        private static Point bounds;
        private static Sprite sprite;

        private Vector2 position;
        private int direction;

        public GameOverScreen()
        {
            position = Vector2.Zero;
            direction = 1;
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

            position += Vector2.UnitY * Math.Sign(direction) * _scrollSpeed * deltaTime;

            if (!Active && !Visible) OnDisappear.Invoke();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, position * sprite.LocalScale * Sprite.GLOBAL_SCALE);
        }
    }
}
