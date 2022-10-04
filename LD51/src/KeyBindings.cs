using Microsoft.Xna.Framework.Input;
using System;

namespace LD51
{
    public class KeyBindings
    {
        static KeyBindings()
        {
            Up = GetKey("bindingsUp");
            Down = GetKey("bindingsDown");
            Left = GetKey("bindingsLeft");
            Right = GetKey("bindingsRight");
            Select = GetKey("bindingsSelect");
            Exit = GetKey("bindingsExit");
            VolumeUp = GetKey("bindingsVolumeUp");
            VolumeDown = GetKey("bindingsVolumeDown");
            ShowFps = GetKey("bindingsShowFps");
        }

        public static Keys Up { get; }
        public static Keys Down { get; }
        public static Keys Left { get; }
        public static Keys Right { get; }
        public static Keys Select { get; }
        public static Keys Exit { get; }
        public static Keys VolumeUp { get; }
        public static Keys VolumeDown { get; }
        public static Keys ShowFps { get; }

        // Try to get the key from its string name, otherwise, try to get it from its integer value
        private static Keys GetKey(string keyName)
        {
            string val = Data.GetRaw(keyName);
            return Enum.TryParse(val, true, out Keys key) ? key : (Keys)int.Parse(val);
        }
    }
}
