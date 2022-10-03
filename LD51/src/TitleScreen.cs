using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace LD51
{
    public class TitleScreen : IGoodUpdateable, IGoodDrawable
    {
        private static readonly float _layerDepth = Data.Get<float>("hudLayerDepth");
        private static readonly float _scrollSpeed = Data.Get<float>("titleScrollSpeed");

        private static Texture2D texture;

        private static Point bounds;
        private static Sprite sprite;

        private Vector2 position;
        private bool scrolling;

        public TitleScreen()
        {
            position = new Vector2(0, -128);
            scrolling = false;
            OnDisappear += () => { };
        }

        public event Action OnDisappear;

        private bool Visible => position.Y < 0;

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
            if (!scrolling || !Visible) return;

            position += _scrollSpeed * Vector2.UnitY * deltaTime;

            if (!Visible) OnDisappear.Invoke();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch,position * sprite.LocalScale * Sprite.GLOBAL_SCALE);
        }

        public void ScrollUp()
        {
            scrolling = true;
        }
    }
}
