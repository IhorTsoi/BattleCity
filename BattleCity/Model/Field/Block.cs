using System;
using System.Collections.Generic;
using System.Text;

namespace BattleCity
{
    enum TypeOfBlock
    {
        Player,
        NPC,
        Bullet,
        EmptyCell,
        BrickWall,
        Wall,
        SecondPLayer
    }

    struct Block
    {
        // Properties :
        public TypeOfBlock Type { get; set; }
        public DamagableObject Model { get; set; }


        public Block(TypeOfBlock type)
        {
            Type = type;
            Model = null;
        }
        

        public void TurnToEmpty ()
        {
            Type = TypeOfBlock.EmptyCell;
            Model = null;
        }


        #region Old stuff

        // Constants :
        //public string Symbol { get; set; }
        //public ConsoleColor BgColor { get; set; }
        //public ConsoleColor FgColor { get; set; }
        //public Directions Direction { get; private set; }

        //private static readonly string[] SymbolsPlayer = new string[] { "◄","▲","►","▼" };
        //private static readonly string[] SymbolsNPC = new string[] { "←","↑","→","↓" };
        //         case TypeOfBlock.Player:
        //            Model = model;
        //            Symbol = SymbolsPlayer[(int)direction];
        //            _health = -1;
        //            BgColor = ConsoleColor.DarkGray;
        //            FgColor = ConsoleColor.Black;
        //            break;
        //        case TypeOfBlock.NPC:
        //            Model = model;
        //            Symbol = SymbolsNPC[(int)direction];
        //            _health = -1;
        //            BgColor = ConsoleColor.DarkGray;
        //            FgColor = ConsoleColor.Black;
        //            break;
        //        case TypeOfBlock.Bullet:
        //            Model = model;
        //            Symbol = "ᴏ";
        //            _health = -1;
        //            BgColor = ConsoleColor.DarkGray;
        //            FgColor = ConsoleColor.DarkRed;
        //            break;
        //        case TypeOfBlock.EmptyCell:
        //            Model = model;
        //            Symbol = " ";
        //            _health = -1;
        //            BgColor = ConsoleColor.DarkGray;
        //            FgColor = ConsoleColor.DarkGray;
        //            break;
        //        case TypeOfBlock.BrickWall:
        //            Model = model;
        //            Symbol = "□";
        //            _health = 2;
        //            BgColor = ConsoleColor.DarkBlue;
        //            FgColor = ConsoleColor.DarkGray;
        //            break;
        //        case TypeOfBlock.Wall:
        //            Model = model;
        //            Symbol = "#";
        //            _health = -1;
        //            BgColor = ConsoleColor.Black;
        //            FgColor = ConsoleColor.Black;
        //            break;

        #endregion
    }
}
