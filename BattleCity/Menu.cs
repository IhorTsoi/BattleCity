using System;
using System.Collections.Generic;
using System.Text;

namespace BattleCity
{
        enum Interfaces
        {
            MainMenu,
            Levels,
            MultiPlayer,
            MultiplayerLevels,
            Stats
        }

    class Menu
    {
        private static string[] MainMenuOptions = new string[] 
        {
            "1-PLAYER",
            "2-PLAYER",
            "STATS",
            "QUIT"
        };
        private static string[] MultiPlayerOptions = new string[]
        {
            "SERVER",
            "CLIENT"
        };
        public (Interfaces Interface, int position) Pointer = (Interfaces.MainMenu, 0);
        public string[] MenuLevels;
        public string[] MenuMultiplayerLevels;
        public List<User> MenuStats;

        public Menu() { }

        public Menu(string[] menuLevels, string[] menuMultiplayerLevels, List<User> stats)
        {
            this.MenuLevels = menuLevels;
            this.MenuStats = stats;
            this.MenuMultiplayerLevels = menuMultiplayerLevels;
        }

        public void Render ()
        {
            Console.Clear();

            switch (Pointer.Interface)
            {
                case Interfaces.MainMenu:
                    WriteTitle("BATTLE CITY");
                    WriteMenu(arr: Menu.MainMenuOptions, position: Pointer.position);
                    break;
                case Interfaces.Levels:
                    WriteTitle("LEVELS");
                    WriteMenu(arr: this.MenuLevels, position: Pointer.position);
                    break;
                case Interfaces.MultiPlayer:
                    WriteTitle("CHOOSE MODE");
                    WriteMenu(arr: Menu.MultiPlayerOptions, position: Pointer.position);
                    break;
                case Interfaces.MultiplayerLevels:
                    WriteTitle("LEVELS FOR 2 PLAYERS");
                    WriteMenu(arr: this.MenuMultiplayerLevels, position: Pointer.position);
                    break;
                case Interfaces.Stats:
                    WriteTitle("STATS");
                    WriteMenu(list: this.MenuStats, position: Pointer.position);
                    break;
                
                default: throw new Exception("error in menu");
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
