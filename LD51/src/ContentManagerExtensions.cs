using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace LD51
{
    public static class ContentManagerExtensions
    {
        private static string musicFolder = "music/";
        private static string soundFolder = "sound/";
        private static string textureFolder = "texture/";

        public static Song LoadMusicTrack(this ContentManager contentManager, string fileName)
        {
            return contentManager.Load<Song>(musicFolder + fileName);
        }

        public static Texture2D LoadTexture(this ContentManager contentManager, string fileName)
        {
            return contentManager.Load<Texture2D>(textureFolder + fileName);
        }

        public static SoundEffect LoadSoundEffect(this ContentManager contentManager, string fileName)
        {
            return contentManager.Load<SoundEffect>(soundFolder + fileName);
        }
    }
}
