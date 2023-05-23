using AnotherLib.Drawing;
using Microsoft.Xna.Framework;
using System;

namespace AnotherLib
{
    public class GameData : Game
    {
        /// <summary>
        /// The posiiton in which audio instances adjust to. Used for tracked sound effect instances.
        /// </summary>
        public static Vector2 AudioPosition;
        public static Vector2 MouseScreenPosition;
        public static Vector2 MouseWorldPosition;
        public static GameDrawInfo CurrentGameDrawInfo;
        public static float MusicVolume = 1f;
        public static float SoundEffectVolume = 1f;
        public static Random random;
        public static Vector2 mouseScreenDivision;
        public static bool MouseOverUI;
        public static bool DebugMode;
    }
}