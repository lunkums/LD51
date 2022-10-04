using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace LD51
{
    public class GameOverScreen : IGoodUpdateable, IGoodDrawable
    {
        private static readonly float _layerDepth = Data.Get<float>("screenLayerDepth");
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
            OnFullyAppear += () => { };
        }

        public event Action OnDisappear;
        public event Action OnFullyAppear;

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

        public bool Active
        {
            get => direction < 0;
            set => direction = value ? -1 : 1;
        }

        private bool Visible => position.Y < 0;
        private bool FullyVisible => position.Y <= -128;

        public void Update(float deltaTime)
        {
            if ((Active && FullyVisible) || (!Active && !Visible)) return;

            position += Vector2.UnitY * Math.Sign(direction) * _scrollSpeed * deltaTime;

            // The guard clause should prevent these from being invoked more than once
            if (FullyVisible) OnFullyAppear.Invoke();
            else if (!Visible) OnDisappear.Invoke();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, position * sprite.LocalScale * Sprite.GLOBAL_SCALE);
        }
    }
}
