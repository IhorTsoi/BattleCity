using System;
using System.Collections.Generic;
using System.Linq;
using BattleCity.Model.Game;
using BattleCity.Views;

namespace BattleCity
{
    enum ProgramStatus
    {
        Menu,
        Game
    }
    enum Directions
    {
        Left,
        Up,
        Right,
        Down
    }

    static partial class Controller
    {
        private static ProgramStatus ProgramStatus { get; set; } = ProgramStatus.Menu;
        private static User ProgramUser { get; set; }
        private static bool _isServer;

        private static IGame Game { get; set; }
        private static Menu Menu { get; set; }

        static void Main(string[] args)
        {
            #region creating fieldviewconfig.json

            /*
            Dictionary<TypeOfBlock, BlockViewConsole> blocksView = new Dictionary<TypeOfBlock, BlockViewConsole>
            {
                [TypeOfBlock.Player] = new BlockViewConsole(
                symbols: new char[] { '◄', '▲', '►', '▼' },
                backGroundColor: ConsoleColor.DarkGray,
                foreGroundColor: ConsoleColor.Black),
                //
                [TypeOfBlock.NPC] = new BlockViewConsole(
                symbols: new char[] { '←', '↑', '→', '↓' },
                backGroundColor: ConsoleColor.DarkGray,
                foreGroundColor: ConsoleColor.Black),
                //
                [TypeOfBlock.Bullet] = new BlockViewConsole(
                symbols: new char[] { 'ᴏ' },
                backGroundColor: ConsoleColor.DarkGray,
                foreGroundColor: ConsoleColor.DarkRed),
                //
                [TypeOfBlock.EmptyCell] = new BlockViewConsole(
                symbols: new char[] { ' ' },
                backGroundColor: ConsoleColor.DarkGray,
                foreGroundColor: ConsoleColor.DarkGray),
                //
                [TypeOfBlock.BrickWall] = new BlockViewConsole(
                symbols: new char[] { '□' },
                backGroundColor: ConsoleColor.DarkBlue,
                foreGroundColor: ConsoleColor.DarkGray),
                //
                [TypeOfBlock.Wall] = new BlockViewConsole(
                symbols: new char[] { '#' },
                backGroundColor: ConsoleColor.Black,
                foreGroundColor: ConsoleColor.Black),
            };
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(blocksView);
            System.IO.File.WriteAllText($"{FieldViewDirectoryName}\\{FieldViewFileName}", json);
            */

            #endregion


            View.ConsoleSettings();
            InitializeData();
            GreetUser();

            while (HandleInput(OnKeyPress())) ;
        }

        private static ConsoleKey OnKeyPress() => Console.ReadKey(true).Key;

        private static bool HandleInput(ConsoleKey Cki)
        {
            switch (ProgramStatus)
            {
                #region Menu case

                case (ProgramStatus.Menu):

                    switch (Menu.Pointer.Interface)
                    {
                        #region Menu.MainMenu

                        case (MenuInterfaces.MainMenu):
                            switch (Cki)
                            {
                                case ConsoleKey.Escape:
                                    return false;

                                case ConsoleKey.Enter:
                                    switch (Menu.Pointer.position)
                                    {
                                        case 0: // LEVELS
                                            Menu.Pointer.Interface = MenuInterfaces.Levels;
                                            Menu.Pointer.position = 0;
                                            break;
                                        case 1: // MULTIPLAYER
                                            Menu.Pointer.Interface = MenuInterfaces.MultiPlayer;
                                            Menu.Pointer.position = 0;
                                            break;
                                        case 2:// STATISTICS
                                            Menu.Pointer.Interface = MenuInterfaces.Stats;
                                            Menu.Pointer.position = 0;
                                            break;
                                        case 3:// EXIT
                                            return false;
                                        default:
                                            throw new Exception("Not implemented Menu.Pointer.Position");
                                    }
                                    break;

                                case ConsoleKey.DownArrow:
                                case ConsoleKey.UpArrow:
                                    Menu.SetPointerPosition(cki: Cki);
                                    break;

                                default:
                                    break;
                            }
                            break;

                        #endregion

                        #region Menu.Levels

                        case (MenuInterfaces.Levels):
                            switch (Cki)
                            {
                                case ConsoleKey.Escape:
                                    Menu.Pointer.Interface = MenuInterfaces.MainMenu;
                                    Menu.Pointer.position = 0;
                                    break;

                                case ConsoleKey.Enter: // STARTS THE GAME
                                    Game = new OnePlayerGame(GameLevels[Menu.Pointer.position]);
                                    ProgramStatus = ProgramStatus.Game;
                                    //
                                    Menu.Pointer.Interface = MenuInterfaces.MainMenu;
                                    Menu.Pointer.position = 0;
                                    //
                                    Game.StartGame();
                                    return true;

                                case ConsoleKey.DownArrow:
                                case ConsoleKey.UpArrow:
                                    Menu.SetPointerPosition(cki: Cki);
                                    break;

                                default:
                                    break;
                            }
                            break;


                        #endregion

                        #region Menu.Multiplayer

                        case (MenuInterfaces.MultiPlayer):
                            switch (Cki)
                            {
                                case ConsoleKey.Escape:
                                    Menu.Pointer.Interface = MenuInterfaces.MainMenu;
                                    Menu.Pointer.position = 0;
                                    break;

                                case ConsoleKey.Enter:// SERVER / CLIENT
                                    _isServer = (Menu.Pointer.position == 0) ? true : false;
                                    Menu.Pointer.Interface = MenuInterfaces.MultiplayerLevels;
                                    Menu.Pointer.position = 0;
                                    break;

                                case ConsoleKey.DownArrow:
                                case ConsoleKey.UpArrow:
                                    Menu.SetPointerPosition(cki: Cki);
                                    break;

                                default: break;
                            }
                            break;

                        #endregion

                        #region Menu.MultiplayerLevels

                        case (MenuInterfaces.MultiplayerLevels):
                            switch (Cki)
                            {
                                case ConsoleKey.Escape:
                                    Menu.Pointer.Interface = MenuInterfaces.MultiPlayer;
                                    Menu.Pointer.position = 0;
                                    break;

                                case ConsoleKey.Enter: // STARTS THE GAME
                                    Game = _isServer ?
                                        (IGame)new ServerGame(GameMLevels[Menu.Pointer.position]) :
                                        (IGame)new ClientGame(GameMLevels[Menu.Pointer.position]);
                                    ProgramStatus = ProgramStatus.Game;
                                    //
                                    Menu.Pointer.Interface = MenuInterfaces.MainMenu;
                                    Menu.Pointer.position = 0;
                                    //
                                    Game.StartGame();
                                    return true;

                                case ConsoleKey.DownArrow:
                                case ConsoleKey.UpArrow:
                                    Menu.SetPointerPosition(cki: Cki);
                                    break;

                                default: break;
                            }
                            break;

                        #endregion

                        #region Menu.Stats

                        case (MenuInterfaces.Stats):
                            switch (Cki)
                            {
                                case ConsoleKey.Escape:
                                    Menu.Pointer.Interface = MenuInterfaces.MainMenu;
                                    Menu.Pointer.position = 0;
                                    break;
                                        
                                case ConsoleKey.DownArrow:
                                case ConsoleKey.UpArrow:
                                    Menu.SetPointerPosition(cki: Cki);
                                    break;

                                default:
                                    break;
                            }
                            break;
                    }

                    #endregion
                        
                    Menu.Render();

                    break;

                        
                #endregion

                #region Game case

                case (ProgramStatus.Game):

                    if ( Game.GameOver )
                    {
                        if ( Game.Won && !ProgramUser.Levels.Contains(Game.LvlName))
                        {
                            ProgramUser.Levels.Add(Game.LvlName);
                            RefreshStats();
                        }
                            
                        ProgramStatus = ProgramStatus.Menu;
                        Game = null;

                        View.ConsoleSettings();
                        Menu.Render();

                        break;
                    }

                    switch (Cki)
                    {
                        case (ConsoleKey.Escape):
                            Game.Quit();
                            break;

                        case (ConsoleKey.LeftArrow):
                        case (ConsoleKey.UpArrow):
                        case (ConsoleKey.RightArrow):
                        case (ConsoleKey.DownArrow):
                            Game.Player.NextStep = Game.Player.NextStep ?? (Directions)((int)Cki - 37); 
                            // LeftArrow = 37, Directions.Left = 0
                            break;

                        case (ConsoleKey.Spacebar):
                            if (!Game.Player.NextShoot)
                            {
                                Game.Player.NextShoot = true;
                            }
                            break;
                    }

                        

                    break;

                    #endregion
            }

            return true;
        }

        private static void GreetUser()
        {
            Console.WriteLine(new string('\n', 3)+"Input your name please:");
            Console.CursorVisible = true;
            string userName = Console.ReadLine();
            View.ConsoleSettings();
            Console.WriteLine(new string('\n', 3));

            if (Statistics.Any(u => u.Name == userName))
            {
                ProgramUser = Statistics.Find(u => u.Name == userName);
                Console.WriteLine($"Welcome back, {ProgramUser.Name}!");
                Console.WriteLine($"Completed levels: {string.Join(", ", ProgramUser.Levels)}");
            }
            else
            {
                ProgramUser = new User(userName, new List<string> { });
                Console.WriteLine($"Nice to meet you, {ProgramUser.Name}!");
                Statistics.Add(ProgramUser);
                RefreshStats();
            }

            Console.WriteLine("\nPress any button to start!");

            Console.ReadKey(true);
            Menu.Render();
        }
    }
}
