using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;

namespace LD51
{
    public class Audio
    {
        private static Dictionary<string, SoundEffect> soundEffects = new Dictionary<string, SoundEffect>();
        private static Rand rand = new Rand();

        private static int volume = 7;

        public static void AddSoundEffect(string effectName, SoundEffect effect)
        {
            soundEffects.Add(effectName, effect);
        }

        public static void Play(string effectName)
        {
            if (soundEffects.TryGetValue(effectName, out SoundEffect effect))
            {
                effect.Play(volume / 10f, 0f, 0f);
            }
        }

        public static void PlayRandom(string[] effectNames)
        {
            Play(effectNames[rand.NextInt(0, effectNames.Length)]);
        }

        public static void IncreaseVolume()
        {
            if (volume > 9) return;

            volume++;
            Play("click");
        }

        public static void DecreaseVolume()
        {
            if (volume < 1) return;

            volume--;
            Play("click");
        }
    }
}
