using AnotherLib.Files;
using System.Collections.Generic;
using System.IO;

namespace AnotherLib
{
    public abstract class GameSettingsBase
    {
        public Dictionary<string, object> SavedGameSettings { get; }

        public void SetSetting(string settingTag, object value)
        {
            SavedGameSettings[settingTag] = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settingTag"></param>
        /// <returns></returns>
        public object? LoadGameSetting(string settingTag)
        {
            if (!File.Exists(FileWriter.SettingsPath))
                return null;

            return File.ReadAllText(FileWriter.SettingsPath);
        }
    }
}
