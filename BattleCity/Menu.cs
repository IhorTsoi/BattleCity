using System;
using System.Collections.Generic;
using System.Text;

namespace BattleCity
{
        enum Interfaces
        {
            MainMenu,
            Levels,
            Stats
        }

    class Menu
    {
        private static string[] MainMenuOptions = new string[] 
        {
            "LEVELS",
            "STATS",
            "QUIT"
        };

        public (Interfaces Interface, int position) Pointer = (Interfaces.MainMenu, 0);
        public string[] MenuLevels;
        public List<User> MenuStats;

        public Menu() { }

        public Menu(string[] menuLevels, List<User> stats)
        {
            this.MenuLevels = menuLevels;
            this.MenuStats = stats;
        }

        public void Render ()
        {
            Console.Clear();

            switch (Pointer.Interface)
            {
                case Interfaces.MainMenu:
                    Console.WriteLine(new string('\n', 5));
                    Console.WriteLine(new string(' ', 15) + "BATTLE CITY");
                    Console.WriteLine(new string('\n', 3));
                    for (int i = 0, length = Menu.MainMenuOptions.Length; i < length; i++)
                    {
                        string str = new string(' ', 15) + Menu.MainMenuOptions[i];

                        if (i == Pointer.position)
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
                    break;
                case Interfaces.Levels:
                    Console.WriteLine(new string('\n', 5));
                    Console.WriteLine(new string(' ', 15) + "LEVELS");
                    Console.WriteLine(new string('\n', 3));
                    for (int i = 0, length = this.MenuLevels.Length; i < length; i++)
                    {
                        string str = new string(' ', 15) + this.MenuLevels[i];

                        if (i == Pointer.position)
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
                    break;
                case Interfaces.Stats:
                    Console.WriteLine(new string('\n', 5));
                    Console.WriteLine(new string(' ', 15) + "STATS");
                    Console.WriteLine(new string('\n', 3));
                    for (int i = 0, length = this.MenuStats.Count; i < length; i++)
                    {
                        string str = new string(' ', 15) + this.MenuStats[i].Name + "\n" + new string(' ', 18) + string.Join(", ",this.MenuStats[i].Levels);

                        if (i == Pointer.position)
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
                    break;

                default:
                    throw new Exception("error in menu");
            }
        }
    }
}
