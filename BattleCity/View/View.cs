using System;
using System.Collections.Generic;
using System.Text;

namespace BattleCity
{
    static class View
    {
        const int width = 80;
        const int height = 25;
        private const string WonMessage = "\tYOU WON! PRESS ANY BUTTON!";
        private const string LostMessage = "\tGAME OVER. PRESS ANY BUTTON.";

        public static void ConsoleSettings()
        {
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.OutputEncoding = Encoding.Unicode;

            Console.SetWindowSize(width, height);
            Console.Clear();
        }

        public static void PrintGameOver(bool won)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.Clear();
            Console.WriteLine(new string('\n', 7) + (won ?
                                                        WonMessage :
                                                        LostMessage));
        }
    }
}
