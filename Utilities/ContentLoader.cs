using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace AnotherLib.Utilities
{
    public abstract class ContentLoader
    {
        public ContentManager contentManager;

        public ContentLoader(ContentManager content)
        {
            contentManager = content;
        }

        public Texture2D LoadTex(string path)
        {
            return contentManager.Load<Texture2D>("Textures/" + path);
        }

        public SpriteFont LoadFont(string path)
        {
            return contentManager.Load<SpriteFont>("Fonts/" + path);
        }

        public SoundEffect LoadSFX(string path)
        {
            return contentManager.Load<SoundEffect>("Sounds/" + path);
        }

        public Song LoadMusic(string name)
        {
            return contentManager.Load<Song>("Sounds/Music/" + name);
        }
    }
}
