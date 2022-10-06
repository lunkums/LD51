using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;

namespace LD51
{
    public class Audio
    {
        private const int MaxVolume = 10;

        private static Dictionary<string, Song> musicTracks;
        private static Dictionary<string, SoundEffect> soundEffects;
        private static Rand rand;
        private static int volume;

        static Audio()
        {
            musicTracks = new Dictionary<string, Song>();
            soundEffects = new Dictionary<string, SoundEffect>();
            rand = new Rand();
            volume = 7;

            MediaPlayer.Volume = Level;
        }

        private static float Level => volume / (float)MaxVolume;

        public static void AddMusicTrack(string trackName, Song track)
        {
            musicTracks.Add(trackName, track);
        }

        public static void AddSoundEffect(string effectName, SoundEffect effect)
        {
            soundEffects.Add(effectName, effect);
        }

        public static void PlayMusicTrack(string trackName)
        {
            if (musicTracks.TryGetValue(trackName, out Song track))
            {
                MediaPlayer.Play(track);
            }
        }

        public static void PlayEffect(string effectName)
        {
            if (soundEffects.TryGetValue(effectName, out SoundEffect effect))
            {
                effect.Play(Level, 0f, 0f);
            }
        }

        public static void PlayRandomEffect(string[] effectNames)
        {
            PlayEffect(effectNames[rand.NextInt(0, effectNames.Length)]);
        }

        public static void IncreaseVolume()
        {
            if (volume == MaxVolume) return;

            volume++;
            MediaPlayer.Volume = Level;
            PlayEffect("click");
        }

        public static void DecreaseVolume()
        {
            if (volume == 0) return;

            volume--;
            MediaPlayer.Volume = Level;
            PlayEffect("click");
        }

        public static void StopMusic()
        {
            MediaPlayer.Stop();
        }
    }
}
