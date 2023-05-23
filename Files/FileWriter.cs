using AnotherLib.Utilities;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;

namespace AnotherLib.Files
{
    public class FileWriter
    {
        public static string GameFileFolderPath { get; }
        public static string ErrorsLogsPath = GameFileFolderPath + "Error Logs\\";
        public static readonly string SettingsPath = GameFileFolderPath + "GameSettings.txt";
        public static string CustomErrorPath { get; } = string.Empty;
        public static string CustomSettingsPath { get; } = string.Empty;

        public void SaveTexture2DAsPNG(Texture2D texture, string savePath)
        {
            string path = GameFileFolderPath + savePath + ".png";
            if (!File.Exists(path))
                Directory.CreateDirectory(Path.GetDirectoryName(path));

            FileStream fileStream = File.OpenWrite(path);
            StreamWriter writer = new StreamWriter(fileStream);
            texture.SaveAsPng(fileStream, texture.Width, texture.Height);
            writer.Close();
            fileStream.Close();
        }

        public void CreateLogs(Exception exception)
        {
            string path = ErrorsLogsPath + "Log_" + DateTime.Now.Month + "_" + DateTime.Today.Day + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + ".txt";
            if (CustomErrorPath != string.Empty)
                path = CustomErrorPath + "Log_" + DateTime.Now.Month + "_" + DateTime.Today.Day + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + ".txt";

            Directory.CreateDirectory(Path.GetDirectoryName(path));

            FileStream fileStream = File.OpenWrite(path);
            StreamWriter writer = new StreamWriter(fileStream);
            writer.WriteLine(exception.Message);
            writer.WriteLine(exception.StackTrace);
            writer.WriteLine(exception.Source);
            writer.Close();
            fileStream.Close();
        }

        public void SaveFileTo(string path, string fileName, string contents)
        {
            string fullPath = path + fileName;
            if (!File.Exists(fullPath))
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            File.WriteAllText(fullPath, contents);
        }
    }
}
