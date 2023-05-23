using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace AnotherLib.Utilities
{
    public static class StringUtils
    {
        public static string Wrap(this string text, int sentenceCharacterLimit)
        {
            List<string> createdSentences = new List<string>();
            string[] wordsArray = text.Split(" ", StringSplitOptions.None);

            string sentenceResult = "";
            for (int word = 0; word < wordsArray.Length; word++)
            {
                if (wordsArray[word].Contains("\n"))
                {
                    createdSentences.Add(sentenceResult);
                    sentenceResult = wordsArray[word] + " ";
                    continue;
                }

                if (sentenceResult.Length + wordsArray[word].Length > sentenceCharacterLimit)
                {
                    createdSentences.Add(sentenceResult);
                    sentenceResult = "\n" + wordsArray[word] + " ";
                }
                else
                {
                    sentenceResult += wordsArray[word] + " ";
                }
            }
            if (sentenceResult != "")       //Cause sometimes it doesn't fill the needed conditions to be considered something to add
                createdSentences.Add(sentenceResult);

            string finalResult = "";
            foreach (string sentencePiece in createdSentences)
                finalResult += sentencePiece;

            return finalResult;
        }

        public static string Wrap(this string text, SpriteFont font, float maxTextWidth)
        {
            List<string> createdSentences = new List<string>();
            string[] wordsArray = text.Split(" ");

            string sentenceResult = "";
            for (int word = 0; word < wordsArray.Length; word++)
            {
                if (wordsArray[word].Contains("\n"))
                {
                    createdSentences.Add(sentenceResult);
                    sentenceResult = wordsArray[word] + " ";
                    continue;
                }

                if (font.MeasureString(sentenceResult + wordsArray[word]).X > maxTextWidth)
                {
                    createdSentences.Add(sentenceResult);
                    sentenceResult = "\n" + wordsArray[word] + " ";
                }
                else
                {
                    sentenceResult += wordsArray[word] + " ";
                }
            }
            if (sentenceResult != "")       //Cause sometimes it doesn't fill the needed conditions to be considered something to add
                createdSentences.Add(sentenceResult);

            string finalResult = "";
            foreach (string sentencePiece in createdSentences)
                finalResult += sentencePiece;

            return finalResult;
        }
    }
}
