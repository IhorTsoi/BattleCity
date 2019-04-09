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
        #region Fields & Properties

        public TypeOfBlock Type { get; set; }
        public string Symbol { get; set; }
        private sbyte _health;
        public sbyte Health
        {
            get { return _health; }
            set
            {
                if (value == 0)
                {
                    this.TurnToEmpty();
                }
                else
                {
                    this._health = value;
                }
            }
        }
        public DynamicObject Model { get; set; }
        public ConsoleColor BgColor { get; set; }
        public ConsoleColor FgColor { get; set; }

        private static readonly string[] SymbolsPlayer = new string[] { "▲","►","▼","◄" };
        private static readonly string[] SymbolsNPC = new string[] { "↑","→","↓", "←" };

        #endregion

        #region Constructors

        public Block(TypeOfBlock Type, DynamicObject model = null, Directions? direction = null)
        {
            this.Type = Type;

            switch (Type)
            {
                case TypeOfBlock.Player:
                    this.Model = model;
                    this.Symbol = (direction != null) ? 
                        Block.GetSymbol(direction, Type)
                        : "▲";
                    this._health = -1;
                    this.BgColor = ConsoleColor.DarkGray;//White;
                    this.FgColor = ConsoleColor.Black;
                    break;
                case TypeOfBlock.NPC:
                    this.Model = model;
                    this.Symbol = (direction != null) ?
                        Block.GetSymbol(direction, Type)
                        : "↓";
                    this._health = -1;
                    this.BgColor = ConsoleColor.DarkGray;//White;
                    this.FgColor = ConsoleColor.Black;
                    break;
                case TypeOfBlock.Bullet:
                    this.Model = model;
                    this.Symbol = "ᴏ";
                    this._health = -1;
                    this.BgColor = ConsoleColor.DarkGray;//White;
                    this.FgColor = ConsoleColor.DarkRed;
                    break;
                case TypeOfBlock.EmptyCell:
                    this.Model = model;
                    this.Symbol = " ";
                    this._health = -1;
                    this.BgColor = ConsoleColor.DarkGray;//White;
                    this.FgColor = ConsoleColor.DarkGray;//White;
                    break;
                case TypeOfBlock.BrickWall:
                    this.Model = model;
                    this.Symbol = "□";
                    this._health = 2;
                    this.BgColor = ConsoleColor.DarkBlue;
                    this.FgColor = ConsoleColor.DarkGray;
                    break;
                case TypeOfBlock.Wall:
                    this.Model = model;
                    this.Symbol = "#";
                    this._health = -1;
                    this.BgColor = ConsoleColor.Black;
                    this.FgColor = ConsoleColor.Black;
                    break;
                default:
                    throw new Exception("\nno such Type.\n");
            }
        }

        #endregion

        #region Methods

        public void TurnToEmpty ()
        {
            this.Type = TypeOfBlock.EmptyCell;
            this.Model = null;
            this.Symbol = " ";
            this._health = -1;
            this.BgColor = ConsoleColor.DarkGray;//White;
            this.FgColor = ConsoleColor.DarkGray;//White;
        }

        public void Rotate (Directions? direction, TypeOfBlock type)
        {
            if (type == TypeOfBlock.Player)
            {
                this.Symbol = SymbolsPlayer[(int)direction];
            }
            else
            {
                this.Symbol = SymbolsNPC[(int)direction];
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
        #endregion
    }
}
