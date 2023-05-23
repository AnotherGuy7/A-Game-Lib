using AnotherLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace AnotherLib.Utilities
{
    public class DebugTools
    {
        private static readonly Vector2 baseDebugTextPosition = new Vector2(4f);
        private const float baseDebugTextScale = 0.3f;
        private static List<DebugText> debugTextList = new List<DebugText>();

        public struct DebugText
        {
            public string text;
            public Color textColor;
            public int lifeTime;
        }

        public static void AddDebugText(object text, int lifeTime = 1, Color color = default)
        {
            DebugText debugText = new DebugText();
            debugText.text = text.ToString();
            debugText.textColor = color;
            debugText.lifeTime = lifeTime;
            debugTextList.Add(debugText);
        }

        public static void AddDebugText(string text, int lifeTime = 1, Color color = default)
        {
            DebugText debugText = new DebugText();
            debugText.text = text;
            debugText.textColor = color;
            debugText.lifeTime = lifeTime;
            debugTextList.Add(debugText);
        }

        public static void DrawDebugText(SpriteBatch spriteBatch, SpriteFont font)
        {
            if (!GameData.DebugMode)
                return;

            for (int i = 0; i < debugTextList.Count; i++)
            {
                DebugText debugText = debugTextList[i];
                debugText.lifeTime--;

                Vector2 textPosition = baseDebugTextPosition + new Vector2(0f, 10f * i);
                spriteBatch.DrawString(font, debugText.text, textPosition, debugText.textColor, 0f, Vector2.Zero, baseDebugTextScale, SpriteEffects.None, 0f);

                if (debugText.lifeTime <= 0)
                {
                    debugTextList.RemoveAt(i);
                    i--;
                }
                else
                    debugTextList[i] = debugText;
            }
        }
    }
}
