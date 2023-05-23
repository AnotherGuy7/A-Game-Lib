using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;

namespace AnotherLib.Utilities
{
    public class MusicPlayer
    {
        public const int AmountOfMusic = 9;
        public const int None = -1;
        public const int TitleMusic = 0;
        public const int LobbyMusic = 1;
        public const int DungeonTheme = 2;
        public const int DungeonTheme2 = 3;
        public const int Tenebrais_Opening = 4;
        public const int Tenebrais_Battle = 5;
        public const int Tenebrais_Battle_2 = 6;
        public const int Tenebrais_Battle_3 = 7;
        public const int Tenebrais_Battle_Break = 8;
        public static Song[] gameMusic;

        public const int Special_Stage2 = 0;
        public const int Special_Stage3 = 1;
        public const int Special_LowHealth = 2;
        public const int Special_OffenseChallenge = 3;

        public static SoundEffectInstance stage2Addition;
        public static SoundEffect[] enemyGameSongAdditions;
        public static SoundEffect[] specialGameSongAdditions;
        public static int[] songMeasureCount;

        public static int[] activeGameSongAdditions;

        private bool fadingOut = false;
        private int fadeOutDuration = 0;
        private int fadeOutTimer = 0;
        private int nextSongIndex = 0;
        private int activeSongIndex = 0;
        private int lastTimePlayed = 0;
        private int amountOfPlays = 0;

        private bool fadingIn = false;
        private int fadeInDuration = 0;
        private int fadeInTimer = 0;

        private static MusicPlayer musicPlayer;

        public MusicPlayer()
        {
            musicPlayer = this;
        }

        /// <summary>
        /// Switches the current music with another using the same timestamp the previous music had.
        /// </summary>
        public static void SwitchMusicInto(int songIndex)
        {
            TimeSpan currentTimestamp = MediaPlayer.PlayPosition;

            MediaPlayer.Play(gameMusic[songIndex], currentTimestamp);
            musicPlayer.activeSongIndex = songIndex;
        }

        public static void FadeOutInto(int songIndex, int fadeOutTime, int fadeInTime = 180)
        {
            musicPlayer.nextSongIndex = songIndex;
            musicPlayer.fadingOut = true;
            musicPlayer.fadeOutDuration = fadeOutTime;
            musicPlayer.fadeInDuration = fadeInTime;
        }

        public void FadeIn()
        {
            if (fadeInTimer < fadeInDuration)
                fadeInTimer++;

            MediaPlayer.Volume = ((float)fadeInTimer / (float)fadeInDuration) * GameData.MusicVolume;

            if (fadeInTimer >= fadeInDuration)
            {
                fadingIn = false;
                fadeInTimer = 0;
                fadeInDuration = 0;
            }
        }

        public void FadeOut()
        {
            if (fadeOutTimer < fadeOutDuration)
                fadeOutTimer++;

            MediaPlayer.Volume = (((float)fadeOutDuration - (float)fadeOutTimer) / (float)fadeOutDuration) * GameData.MusicVolume;

            if (fadeOutTimer >= fadeOutDuration)
            {
                fadingOut = false;
                fadingIn = true;
                fadeOutTimer = 0;
                fadeOutDuration = 0;
                activeSongIndex = nextSongIndex;

                if (nextSongIndex == None)
                    MediaPlayer.Stop();
                else
                    MediaPlayer.Play(gameMusic[activeSongIndex]);
            }
        }

        public void Update()
        {
            if (fadingOut)
                FadeOut();

            if (fadingIn)
                FadeIn();

            if (!fadingOut && !fadingIn)
                MediaPlayer.Volume = GameData.MusicVolume;

            if ((MediaPlayer.PlayPosition.Seconds % 4) - 1 == 0 && lastTimePlayed != MediaPlayer.PlayPosition.Seconds && MediaPlayer.PlayPosition.Milliseconds == 500)
            {
                amountOfPlays++;
                PlaySongAdditions();
                lastTimePlayed = MediaPlayer.PlayPosition.Seconds;
                if (amountOfPlays >= 8)
                    amountOfPlays = 0;
            }

            if (MediaPlayer.Volume <= 0.01f)
                return;

            if (MediaPlayer.State == MediaState.Playing)
                return;

            if (activeSongIndex == None)
                return;

            MediaPlayer.Play(gameMusic[activeSongIndex]);
            //if (activeSongIndex == DungeonTheme)
                //stage2Addition.Play();
        }

        public void AddSongAddition(int enemyType)
        {
            for (int i = 0; i < activeGameSongAdditions.Length; i++)
            {
                if (activeGameSongAdditions[i] == enemyType)
                    return;
            }

            for (int i = 0; i < activeGameSongAdditions.Length; i++)
            {
                if (activeGameSongAdditions[i] == -1)
                {
                    activeGameSongAdditions[i] = enemyType;
                    break;
                }
            }
        }

        public void AddSpecialSongAddition(int id)
        {
            for (int i = 0; i < activeGameSongAdditions.Length; i++)
            {
                if (activeGameSongAdditions[i] == -1)
                {
                    activeGameSongAdditions[i] = enemyGameSongAdditions.Length + id;
                    break;
                }
            }
        }

        public void ClearSongAdditions()
        {
            for (int i = 0; i < activeGameSongAdditions.Length; i++)
                activeGameSongAdditions[i] = -1;
        }

        private void PlaySongAdditions()
        {
            for (int s = 0; s < activeGameSongAdditions.Length; s++)
            {
                if (activeGameSongAdditions[s] == -1)
                    continue;

                if (amountOfPlays % songMeasureCount[activeGameSongAdditions[s]] == 0)
                {
                    float musicVolume = 0f;
                    if (GameData.MusicVolume > 0f)
                        musicVolume = Math.Clamp(GameData.MusicVolume - 0.3f, 0.05f, 0.7f);

                    if (activeGameSongAdditions[s] < enemyGameSongAdditions.Length)     //Silly but it works
                        enemyGameSongAdditions[activeGameSongAdditions[s]].Play(musicVolume, 0f, 0f);
                    else
                        specialGameSongAdditions[activeGameSongAdditions[s] - enemyGameSongAdditions.Length].Play(musicVolume, 0f, 0f);
                    //UI.ChatUI.AddMessage("Debug", "Played " + activeGameSongAdditions[s] + " at " + MediaPlayer.PlayPosition.Seconds);
                }
            }
        }
    }
}