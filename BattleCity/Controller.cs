using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using System.IO;
using Newtonsoft.Json;

namespace BattleCity
{
    enum ProgramStatus
    {
        Menu,
        Game,
        CLose
    }
    enum Directions
    {
        Up,
        RIght,
        Down,
        Left
    }

    class Controller
    {
        private static ProgramStatus PStatus { get; set; } = ProgramStatus.Menu;
        private static User PUser { get; set; }
        private static PlayerModel GPlayer { get; set; }
        private static IBasicGame GGame { get; set; }
        private static Menu GMenu { get; set; }
        private static bool _isServer;

        #region directory names

        private static readonly string LevelsDirectoryName = "levels";
        private static readonly string StatisticsDirectoryName = "statistics";
        private static readonly string MultiplayerLevelsDirectoryName = "multilevels";

        #endregion

        private static List<Level> GLevels = new List<Level>();
        private static List<MultiplayerLevel> GMultiplayerLevels = new List<MultiplayerLevel>();
        private static List<User> GStats = new List<User>();

        static void Main(string[] args)
        {
            ConsoleSettings();
            LoadAllLevels();
            LoadAllMultiplayerLevels();
            LoadStats();
            GMenu = new Menu((from level in GLevels select level.Name).ToArray(), (from level in GMultiplayerLevels select level.Name).ToArray(), GStats);

            GreetUser();
            
            HandleInput();

        }
        
        private static void ConsoleSettings()
        {
            (int width, int height) = (80, 25);
            
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.OutputEncoding = Encoding.Unicode;

            Console.SetWindowSize(width, height);
            Console.Clear();
        }

        private static void HandleInput()
        {
            while (PStatus != ProgramStatus.CLose)

            {
                ConsoleKey Cki = Console.ReadKey(true).Key;

                /*--------------------------------------------*/
                /*--------------------------------------------*/

                switch (PStatus)
                {
                    #region Menu case

                    case (ProgramStatus.Menu):

                        switch (GMenu.Pointer.Interface)
                        {
                            #region Menu.MainMenu

                            case (Interfaces.MainMenu):
                                switch (Cki)
                                {
                                    case ConsoleKey.Escape:
                                        PStatus = ProgramStatus.CLose;
                                        break;

                                    case ConsoleKey.Enter:
                                        switch (GMenu.Pointer.position)
                                        {
                                            case 0:
                                                GMenu.Pointer.Interface = Interfaces.Levels;
                                                GMenu.Pointer.position = 0;
                                                break;
                                            case 1:
                                                // case for MULTIPLAYER
                                                GMenu.Pointer.Interface = Interfaces.MultiPlayer;
                                                GMenu.Pointer.position = 0;
                                                break;
                                            case 2:
                                                GMenu.Pointer.Interface = Interfaces.Stats;
                                                GMenu.Pointer.position = 0;
                                                break;
                                            case 3:
                                                PStatus = ProgramStatus.CLose;
                                                break;
                                            default:
                                                break;
                                        }
                                        break;

                                    case ConsoleKey.DownArrow:
                                        GMenu.Pointer.position = (GMenu.Pointer.position + 1) % 4;
                                        break;

                                    case ConsoleKey.UpArrow:
                                        GMenu.Pointer.position = (GMenu.Pointer.position + 3) % 4;
                                        break;

                                    default:
                                        break;
                                }
                                break;

                            #endregion

                            #region Menu.Levels

                            case (Interfaces.Levels):
                                switch (Cki)
                                {
                                    case ConsoleKey.Escape:
                                        GMenu.Pointer.Interface = Interfaces.MainMenu;
                                        GMenu.Pointer.position = 0;
                                        break;

                                    case ConsoleKey.Enter:
                                        GGame = new Game(GLevels[GMenu.Pointer.position]);
                                        GMenu.Pointer.Interface = Interfaces.MainMenu;
                                        GMenu.Pointer.position = 0;
                                        PStatus = ProgramStatus.Game;
                                        GGame.StartGame();
                                        break;

                                    case ConsoleKey.DownArrow:
                                        GMenu.Pointer.position = (GMenu.Pointer.position + 1) % GMenu.MenuLevels.Count();
                                        break;

                                    case ConsoleKey.UpArrow:
                                        GMenu.Pointer.position = (GMenu.Pointer.position + (GMenu.MenuLevels.Count()-1)) % GMenu.MenuLevels.Count();
                                        break;

                                    default:
                                        break;
                                }
                                break;


                            #endregion

                            #region Menu.Multiplayer

                            case (Interfaces.MultiPlayer):
                                switch (Cki)
                                {
                                    case ConsoleKey.Escape:
                                        GMenu.Pointer.Interface = Interfaces.MainMenu;
                                        GMenu.Pointer.position = 0;
                                        break;

                                    case ConsoleKey.Enter:
                                        switch(GMenu.Pointer.position)
                                        {
                                            case 0:
                                                _isServer = true;
                                                break;
                                            case 1:
                                                _isServer = false;
                                                break;
                                        }
                                        GMenu.Pointer.Interface = Interfaces.MultiplayerLevels;
                                        GMenu.Pointer.position = 0;
                                        break;

                                    case ConsoleKey.DownArrow:
                                        GMenu.Pointer.position = (GMenu.Pointer.position + 1) % 2;
                                        break;

                                    case ConsoleKey.UpArrow:
                                        GMenu.Pointer.position = (GMenu.Pointer.position + 1) % 2;
                                        break;

                                    default: break;
                                }
                                break;

                            #endregion

                            #region Menu.MultiplayerLevels

                            case (Interfaces.MultiplayerLevels):
                                switch (Cki)
                                {
                                    case ConsoleKey.Escape:
                                        GMenu.Pointer.Interface = Interfaces.MultiPlayer;
                                        GMenu.Pointer.position = 0;
                                        break;

                                    case ConsoleKey.Enter:
                                        if (_isServer)
                                        {
                                            GGame = new ServerGame(GMultiplayerLevels[GMenu.Pointer.position]);
                                        }
                                        else
                                        {
                                            GGame = new ClientGame(GMultiplayerLevels[GMenu.Pointer.position]);
                                        }
                                        GMenu.Pointer.Interface = Interfaces.MainMenu;
                                        GMenu.Pointer.position = 0;
                                        PStatus = ProgramStatus.Game;
                                        GGame.StartGame();
                                        break;

                                    case ConsoleKey.DownArrow:
                                        GMenu.Pointer.position = (GMenu.Pointer.position + 1) % GMenu.MenuMultiplayerLevels.Count();
                                        break;

                                    case ConsoleKey.UpArrow:
                                        GMenu.Pointer.position = (GMenu.Pointer.position + (GMenu.MenuMultiplayerLevels.Count() - 1)) % GMenu.MenuMultiplayerLevels.Count();
                                        break;

                                    default: break;
                                }
                                break;

                            #endregion

                            #region Menu.Stats

                            case (Interfaces.Stats):
                                switch (Cki)
                                {
                                    case ConsoleKey.Escape:
                                        GMenu.Pointer.Interface = Interfaces.MainMenu;
                                        GMenu.Pointer.position = 0;
                                        break;
                                        
                                    case ConsoleKey.DownArrow:
                                        GMenu.Pointer.position = (GMenu.Pointer.position + 1) % GMenu.MenuStats.Count();
                                        break;

                                    case ConsoleKey.UpArrow:
                                        GMenu.Pointer.position = (GMenu.Pointer.position + (GMenu.MenuStats.Count() - 1)) % GMenu.MenuStats.Count();
                                        break;

                                    default:
                                        break;
                                }
                                break;
                        }

                        #endregion

                        

                        if (PStatus != ProgramStatus.Game)
                        {
                        GMenu.Render();
                        }

                        break;

                        
                    #endregion

                    /*--------------------------------------------*/
                    /*--------------------------------------------*/

                    #region Game case

                    case (ProgramStatus.Game):

                        if ( GGame._gameOver)
                        {
                            if ( GGame._won && !PUser.Levels.Contains(GGame.LvlName))
                            {
                                    PUser.Levels.Add(GGame.LvlName);
                                    RefreshStats();
                            }
                            
                            PStatus = ProgramStatus.Menu;
                            GGame = null;

                            ConsoleSettings();
                            GMenu.Render();

                            break;
                        }

                        switch (Cki)
                        {
                            case (ConsoleKey.Escape):

                                GGame.Quit();

                                PStatus = ProgramStatus.Menu;
                                GGame = null;

                                ConsoleSettings();
                                GMenu.Render();
                                break;
                            #region Movement
                            case (ConsoleKey.UpArrow):

                                if (GGame.Player._nextStep == null)
                                {
                                    GGame.Player._nextStep = Directions.Up;
                                }
                                
                                break;
                            case (ConsoleKey.RightArrow):

                                if (GGame.Player._nextStep == null)
                                {
                                    GGame.Player._nextStep = Directions.RIght;
                                }
                                break;
                            case (ConsoleKey.LeftArrow):

                                if (GGame.Player._nextStep == null)
                                {
                                    GGame.Player._nextStep = Directions.Left;
                                }
                                break;
                            case (ConsoleKey.DownArrow):

                                if (GGame.Player._nextStep == null)
                                {
                                    GGame.Player._nextStep = Directions.Down;
                                }
                                break;
                            #endregion

                            case (ConsoleKey.Spacebar):
                                if (!GGame.Player._nextShot)
                                {
                                    GGame.Player._nextShot = true;
                                }
                                break;
                        }

                        

                        break;

                        #endregion
                }
            }
        }

        private static void GreetUser()
        {
            Console.WriteLine(new string('\n', 3)+"Input your name please:");
            Console.CursorVisible = true;
            string userName = Console.ReadLine();
            ConsoleSettings();
            Console.WriteLine(new string('\n', 3));

            if (GStats.Any(u => u.Name == userName))
            {
                PUser = GStats.Find(u => u.Name == userName);
                Console.WriteLine($"Welcome back, {PUser.Name}!");
                Console.WriteLine($"Completed levels: {string.Join(", ", PUser.Levels)}");
            }
            else
            {
                PUser = new User(userName, new List<string> { });
                Console.WriteLine($"Nice to meet you, {PUser.Name}!");
                GStats.Add(PUser);
                RefreshStats();
            }

            Console.WriteLine("\nPress any button to start!");

            Console.ReadKey(true);
            GMenu.Render();
        }

        #region levels & stats

        private static void LoadAllLevels()
        {
            if (!Directory.Exists(LevelsDirectoryName))
            {
                return;
            }

            string [] files = Directory.GetFileSystemEntries(LevelsDirectoryName);

            for (int i = 0, length = files.Length; i < length; i++)
            {

                GLevels.Add(JsonConvert.DeserializeObject<Level>(File.ReadAllText(files[i])));
            }
        }

        private static void LoadAllMultiplayerLevels()
        {
            if (!Directory.Exists(MultiplayerLevelsDirectoryName))
            {
                return;
            }

            string [] files = Directory.GetFileSystemEntries(MultiplayerLevelsDirectoryName);

            for (int i = 0, length = files.Length; i < length; i++)
            {

                GMultiplayerLevels.Add(JsonConvert.DeserializeObject<MultiplayerLevel>(File.ReadAllText(files[i])));
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

            GStats =  JsonConvert.DeserializeObject<User[]>(fileContent).ToList();
        }

        private static void RefreshStats()
        {
            string json = JsonConvert.SerializeObject(GStats.ToArray());

            File.WriteAllText($"{StatisticsDirectoryName}\\stats.json", json);
        }

        #endregion
    }
}
