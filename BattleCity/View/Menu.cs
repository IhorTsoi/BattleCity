using System;
using System.Collections.Generic;
using System.Text;

namespace BattleCity
{
    enum MenuInterfaces
    {
        MainMenu,
        Levels,
        MultiPlayer,
        MultiplayerLevels,
        Stats
    }

    class Menu
    {
        private static readonly string[] MenuMainMenu = new string[] 
        {
            "1-PLAYER",
            "2-PLAYER",
            "STATS",
            "QUIT"
        };
        private static readonly string[] MenuMultiplayer = new string[]
        {
            "SERVER",
            "CLIENT"
        };
        public string[] MenuLevels;
        public string[] MenuMultiplayerLevels;
        public List<User> MenuStats;

        public (MenuInterfaces Interface, int position) Pointer = (MenuInterfaces.MainMenu, 0);

        public Menu(string[] menuLevels, string[] menuMultiplayerLevels, List<User> stats)
        {
            MenuLevels = menuLevels;
            MenuStats = stats;
            MenuMultiplayerLevels = menuMultiplayerLevels;
        }


        public void Render()
        {
            Console.Clear();

            switch (Pointer.Interface)
            {
                case MenuInterfaces.MainMenu:
                    WriteTitle("BATTLE CITY");
                    WriteMenu(arr: Menu.MenuMainMenu, position: Pointer.position);
                    break;
                case MenuInterfaces.Levels:
                    WriteTitle("LEVELS");
                    WriteMenu(arr: MenuLevels, position: Pointer.position);
                    break;
                case MenuInterfaces.MultiPlayer:
                    WriteTitle("CHOOSE MODE");
                    WriteMenu(arr: Menu.MenuMultiplayer, position: Pointer.position);
                    break;
                case MenuInterfaces.MultiplayerLevels:
                    WriteTitle("LEVELS FOR 2 PLAYERS");
                    WriteMenu(arr: MenuMultiplayerLevels, position: Pointer.position);
                    break;
                case MenuInterfaces.Stats:
                    WriteTitle("STATS");
                    WriteMenu(list: MenuStats, position: Pointer.position);
                    break;
                
                default: throw new Exception("Not implemented Pointer.Interface");
            }
        }
        public void SetPointerPosition(ConsoleKey cki)
        {
            int _optionsCount;
            switch (Pointer.Interface)
            {
                case MenuInterfaces.MainMenu:
                    _optionsCount = MenuMainMenu.Length;
                    break;
                case MenuInterfaces.Levels:
                    _optionsCount = MenuLevels.Length;
                    break;
                case MenuInterfaces.MultiPlayer:
                    _optionsCount = MenuMultiplayer.Length;
                    break;
                case MenuInterfaces.MultiplayerLevels:
                    _optionsCount = MenuMultiplayerLevels.Length;
                    break;
                case MenuInterfaces.Stats:
                    _optionsCount = MenuStats.Count;
                    break;
                default:
                    throw new Exception("Not implemented MenuInterface");
            }

            switch (cki)
            {
                case ConsoleKey.DownArrow:
                    Pointer.position = (Pointer.position + 1) % _optionsCount;
                    break;

                case ConsoleKey.UpArrow:
                    Pointer.position = (Pointer.position + _optionsCount - 1) % _optionsCount;
                    break;
            }
        }

        private static void WriteTitle(string title)
        {
            Console.WriteLine(new string('\n', 5));
            Console.WriteLine(new string(' ', 15) + title);
            Console.WriteLine(new string('\n', 3));
        }
        private static void WriteMenu(string[] arr, int position)
        {
            for (int i = 0, length = arr.Length; i < length; i++)
            {
                string str = new string(' ', 15) + arr[i];

                if (i == position)
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine(str + "\n");
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                }
                else
                {
                    Console.WriteLine(str + "\n");
                }
            }
        }
        private static void WriteMenu(List<User> list, int position)
        {
            for (int i = 0, length = list.Count; i < length; i++)
            {
                string str = new string(' ', 15) + list[i].Name + "\n" + new string(' ', 18) + string.Join(", ", list[i].Levels);

                if (i == position)
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine(str + "\n");
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                }
                else
                {
                    Console.WriteLine(str + "\n");
                }
            }
        }
    }
}
