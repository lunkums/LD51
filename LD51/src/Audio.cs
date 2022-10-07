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
        private static int musicVolume;
        private static int soundEffectVolume;

        static Audio()
        {
            musicTracks = new Dictionary<string, Song>();
            soundEffects = new Dictionary<string, SoundEffect>();
            rand = new Rand();
            musicVolume = 3;
            soundEffectVolume = 7;

            MediaPlayer.Volume = MusicLevel;
        }

        private static float MusicLevel => musicVolume / (float)MaxVolume;
        private static float SoundEffectLevel => soundEffectVolume / (float)MaxVolume;

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
                effect.Play(SoundEffectLevel, 0f, 0f);
            }
        }

        public static void PlayRandomEffect(string[] effectNames)
        {
            PlayEffect(effectNames[rand.NextInt(0, effectNames.Length)]);
        }

        public static void IncreaseSoundEffectVolume()
        {
            if (soundEffectVolume == MaxVolume) return;

            soundEffectVolume++;
            PlayEffect("click");
        }

        public static void DecreaseSoundEffectVolume()
        {
            if (soundEffectVolume == 0) return;

            soundEffectVolume--;
            PlayEffect("click");
        }

        public static void IncreaseMusicVolume()
        {
            if (musicVolume == MaxVolume) return;

            musicVolume++;
            MediaPlayer.Volume = MusicLevel;
        }

        public static void DecreaseMusicVolume()
        {
            if (musicVolume == 0) return;

            musicVolume--;
            MediaPlayer.Volume = MusicLevel;
        }

        public static void StopMusic()
        {
            MediaPlayer.Stop();
        }
    }
}
