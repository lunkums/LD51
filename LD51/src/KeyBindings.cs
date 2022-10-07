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
            MusicVolumeUp = GetKey("bindingsMusicVolumeUp");
            MusicVolumeDown = GetKey("bindingsMusicVolumeDown");
            SoundEffectsVolumeUp = GetKey("bindingsSoundEffectsVolumeUp");
            SoundEffectsVolumeDown = GetKey("bindingsSoundEffectsVolumeDown");
            ShowFps = GetKey("bindingsShowFps");
        }

        public static Keys Up { get; }
        public static Keys Down { get; }
        public static Keys Left { get; }
        public static Keys Right { get; }
        public static Keys Select { get; }
        public static Keys Exit { get; }
        public static Keys MusicVolumeUp { get; }
        public static Keys MusicVolumeDown { get; }
        public static Keys SoundEffectsVolumeUp { get; }
        public static Keys SoundEffectsVolumeDown { get; }
        public static Keys ShowFps { get; }

        // Try to get the key from its string name, otherwise, try to get it from its integer value
        private static Keys GetKey(string keyName)
        {
            string val = Data.GetRaw(keyName);
            return Enum.TryParse(val, true, out Keys key) ? key : (Keys)int.Parse(val);
        }
    }
}
