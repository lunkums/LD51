using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace LD51
{
    public class Input
    {
        private static KeyboardState previousKeyboardState = Keyboard.GetState();
        private static KeyboardState currentKeyboardState = Keyboard.GetState();

        private static MouseState previousMouseState = Mouse.GetState();
        private static MouseState currentMouseState = Mouse.GetState();

        private static bool gameInFocus = true;

        private Input() { }

        public static Vector2 MouseWorldPosition => new Vector2(currentMouseState.X, -currentMouseState.Y);

        public static void Update(bool gameInFocus)
        {
            Input.gameInFocus = gameInFocus;

            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();
        }

        public static bool IsKeyPressed(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key) && previousKeyboardState.IsKeyUp(key);
        }

        public static bool IsKeyDown(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key);
        }

        public static bool LeftMousePressed()
        {
            return gameInFocus && currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released;
        }

        public static bool RightMousePressed()
        {
            return gameInFocus && currentMouseState.RightButton == ButtonState.Pressed && previousMouseState.RightButton == ButtonState.Released;
        }
    }
}
