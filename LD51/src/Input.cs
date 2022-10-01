using Microsoft.Xna.Framework.Input;

namespace LD51
{
    public class Input
    {
        private static KeyboardState previousKeyboardState = Keyboard.GetState();
        private static KeyboardState currentKeyboardState = Keyboard.GetState();

        private Input() { }

        public static void Update()
        {
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();
        }

        public static bool IsKeyPressed(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key) && previousKeyboardState.IsKeyUp(key);
        }

        public static bool IsKeyDown(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key);
        }
    }
}
