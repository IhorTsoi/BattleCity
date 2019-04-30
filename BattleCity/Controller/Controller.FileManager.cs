using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System.Linq;

namespace BattleCity
{
    static partial class Controller
    {
        private static List<Level> GameLevels = new List<Level>();
        private static List<MultiplayerLevel> GameMLevels = new List<MultiplayerLevel>();
        private static List<User> Statistics = new List<User>();

        #region directory names

        private const string LevelsDirectoryName = "levels";
        private const string StatisticsDirectoryName = "statistics";
        private const string MultiplayerLevelsDirectoryName = "multilevels";

        #endregion

        public static void InitializeData()
        {
            LoadAllLevels();
            LoadAllMultiplayerLevels();
            LoadStats();

            Menu = new Menu((from level in GameLevels select level.Name).ToArray(), (from level in GameMLevels select level.Name).ToArray(), Statistics);
        }
        private static void LoadAllLevels()
        {
            if (!Directory.Exists(LevelsDirectoryName))
            {
                return;
            }

            string[] files = Directory.GetFileSystemEntries(LevelsDirectoryName);

            for (int i = 0, length = files.Length; i < length; i++)
            {

                Controller.GameLevels.Add(JsonConvert.DeserializeObject<Level>(File.ReadAllText(files[i])));
            }
        }
        private static void LoadAllMultiplayerLevels()
        {
            if (!Directory.Exists(MultiplayerLevelsDirectoryName))
            {
                return;
            }

            string[] files = Directory.GetFileSystemEntries(MultiplayerLevelsDirectoryName);

            for (int i = 0, length = files.Length; i < length; i++)
            {

                GameMLevels.Add(JsonConvert.DeserializeObject<MultiplayerLevel>(File.ReadAllText(files[i])));
            }
        }
        private static void LoadStats()
        {
            if (!Directory.Exists(StatisticsDirectoryName))
            {
                return;
            }

            string file = "stats.json";
            string fileContent = File.ReadAllText(StatisticsDirectoryName + "\\" + file);

            Statistics = JsonConvert.DeserializeObject<User[]>(fileContent).ToList();
        }


        public static void RefreshStats()
        {
            string json = JsonConvert.SerializeObject(Statistics.ToArray());

            File.WriteAllText($"{StatisticsDirectoryName}\\stats.json", json);
        }
    }
}
