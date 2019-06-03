using System;
using System.Collections.Generic;
using System.Text;

namespace BattleCity.Views
{
    class BlockViewConsole
    {
        public char[] Symbols { get; set; }
        //
        public ConsoleColor BackGroundColor { get; set; }
        public ConsoleColor ForeGroundColor { get; set; }


        public BlockViewConsole(char[] symbols, ConsoleColor backGroundColor, ConsoleColor foreGroundColor)
        {
            Symbols = symbols;
            BackGroundColor = backGroundColor;
            ForeGroundColor = foreGroundColor;
        }
    }
}
