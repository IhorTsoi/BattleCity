using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using BattleCity.Views;

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
        private const string FieldViewDirectoryName = "fieldView";

        private const string StatisticsFileName = "stats.json";
        private const string FieldViewFileName = "fieldViewConfig.json";

        public const int FieldWidth = 60;
        public const int FieldHeight = 15;

        #endregion

        public static void InitializeData()
        {
            LoadAllLevels();
            LoadAllMultiplayerLevels();
            LoadStats();
            InitializeFieldView();

            Menu = new Menu((from level in GameLevels select level.Name).ToArray(), (from level in GameMLevels select level.Name).ToArray(), Statistics);
        }
        private static void InitializeFieldView()
        {
            if (!Directory.Exists(FieldViewDirectoryName))
            {
                return;
            }

            string fileContent = File.ReadAllText($"{FieldViewDirectoryName}\\{FieldViewFileName}");

            // for console app:
            FieldViewerConsole.Initialize(
                JsonConvert.DeserializeObject<Dictionary<TypeOfBlock, BlockViewConsole>>(fileContent));

            // for GUI app:
            //FieldViewGUI.Initialize(
            //     JsonConvert.DeserializeObject<Dictionary<TypeOfBlock, BlockViewGUI>>(fileContent));
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

                GameLevels.Add(JsonConvert.DeserializeObject<Level>(File.ReadAllText(files[i])));
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

            string fileContent = File.ReadAllText($"{StatisticsDirectoryName}\\{StatisticsFileName}");

            Statistics = JsonConvert.DeserializeObject<User[]>(fileContent).ToList();
        }


        public static void RefreshStats()
        {
            string json = JsonConvert.SerializeObject(Statistics.ToArray());

            File.WriteAllText($"{StatisticsDirectoryName}\\{StatisticsFileName}", json);
        }
    }
}
