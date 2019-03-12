using System;
using System.Collections.Generic;
using System.Text;

namespace BattleCity
{
    class Field
    {
        public Field (TypeOfBlock[,] mapInfo)
        {
            this.map = new Block[15,60];
            for(int i = 0; i < 15;i++)
            {
                for (int j = 0; j < 60; j++)
                {
                    this.map[i, j] = new Block(mapInfo[i,j]);
                }
            }
        }

        public Block[,] map;

        private string[,] _previousMap = new string[15, 60];

        public void Render()
        {
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 60; j++)
                {
                    if (this.map[i, j].Symbol != this._previousMap[i, j])
                    {
                        Console.BackgroundColor = this.map[i, j].BgColor;
                        Console.ForegroundColor = this.map[i, j].FgColor;

                        Console.SetCursorPosition(left: j, top: i);
                        Console.Write(this.map[i, j].Symbol);

                        _previousMap[i, j] = this.map[i, j].Symbol;
                    }
                }
            }
        }

        public void _firstRender()
        {
            
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 60; j++)
                {
                    Console.BackgroundColor = this.map[i, j].BgColor;
                    Console.ForegroundColor = this.map[i, j].FgColor;

                    Console.Write(this.map[i, j].Symbol);
                    _previousMap[i, j] = this.map[i, j].Symbol;
                    
                }

                Console.Write('\n');
            }
        }
    }
}
