using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;

namespace LD51
{
    public class Audio
    {
        private static Dictionary<string, SoundEffect> soundEffects = new Dictionary<string, SoundEffect>();

        public static void AddSoundEffect(string effectName, SoundEffect effect)
        {
            soundEffects.Add(effectName, effect);
        }

        public static void Play(string effectName)
        {
            if (soundEffects.TryGetValue(effectName, out SoundEffect effect))
            {
                effect.Play();
            }
        }
    }
}
