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
        // Constants :
        private static readonly string[] SymbolsPlayer = new string[] { "◄","▲","►","▼" };
        private static readonly string[] SymbolsNPC = new string[] { "←","↑","→","↓" };

        // Properties :
        public TypeOfBlock Type { get; set; }
        public string Symbol { get; set; }
        private sbyte _health;
        public sbyte Health
        {
            get { return _health; }
            set
            {
                if (value <= 0)
                {
                    TurnToEmpty();
                }
                else
                {
                    _health = value;
                }
            }
        }
        public DynamicObject Model { get; set; }
        public ConsoleColor BgColor { get; set; }
        public ConsoleColor FgColor { get; set; }


        // Constructors :

        public Block(TypeOfBlock type, DynamicObject model = null, Directions? direction = null)
        {
            Type = type;

            switch (type)
            {
                case TypeOfBlock.Player:
                    Model = model;
                    Symbol = (direction != null) ? 
                        Block.GetSymbol(direction, type)
                        : "▲";
                    _health = -1;
                    BgColor = ConsoleColor.DarkGray;
                    FgColor = ConsoleColor.Black;
                    break;
                case TypeOfBlock.NPC:
                    Model = model;
                    Symbol = (direction != null) ?
                        Block.GetSymbol(direction, type)
                        : "↓";
                    _health = -1;
                    BgColor = ConsoleColor.DarkGray;
                    FgColor = ConsoleColor.Black;
                    break;
                case TypeOfBlock.Bullet:
                    Model = model;
                    Symbol = "ᴏ";
                    _health = -1;
                    BgColor = ConsoleColor.DarkGray;
                    FgColor = ConsoleColor.DarkRed;
                    break;
                case TypeOfBlock.EmptyCell:
                    Model = model;
                    Symbol = " ";
                    _health = -1;
                    BgColor = ConsoleColor.DarkGray;
                    FgColor = ConsoleColor.DarkGray;
                    break;
                case TypeOfBlock.BrickWall:
                    Model = model;
                    Symbol = "□";
                    _health = 2;
                    BgColor = ConsoleColor.DarkBlue;
                    FgColor = ConsoleColor.DarkGray;
                    break;
                case TypeOfBlock.Wall:
                    Model = model;
                    Symbol = "#";
                    _health = -1;
                    BgColor = ConsoleColor.Black;
                    FgColor = ConsoleColor.Black;
                    break;
                default:
                    throw new Exception("\nNot implemented type of Block\n");
            }
        }
        

        // Methods :

        public void TurnToEmpty ()
        {
            Type = TypeOfBlock.EmptyCell;
            Model = null;
            Symbol = " ";
            _health = -1;
            BgColor = ConsoleColor.DarkGray;
            FgColor = ConsoleColor.DarkGray;
        }

        public void Rotate (Directions? direction, TypeOfBlock type)
        {
            if (type == TypeOfBlock.Player)
            {
                Symbol = SymbolsPlayer[(int)direction];
            }
            else
            {
                Symbol = SymbolsNPC[(int)direction];
            }
        }

        private static string GetSymbol (Directions? direction, TypeOfBlock type)
        {
            if (type == TypeOfBlock.Player)
            {
                return SymbolsPlayer[(int)direction];
            }
            else
            {
                return SymbolsNPC[(int)direction];
            }
        }
    }
}
