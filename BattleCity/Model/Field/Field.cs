using System;
using System.Collections.Generic;
using System.Text;

namespace BattleCity
{
    class Field
    {
        public Block[,] Map;
        private string[,] _PreviousMap = new string[Controller.FieldHeight, Controller.FieldWidth];

        public Block this[int Y, int X]
        {
            get => Map[Y, X];
            set => Map[Y, X] = value;
        }

        public Field (TypeOfBlock[,] mapInfo)
        {
            Map = new Block[Controller.FieldHeight, Controller.FieldWidth];
            for(int i = 0; i < Controller.FieldHeight; i++)
            {
                for (int j = 0; j < Controller.FieldWidth; j++)
                {
                    Map[i, j] = new Block(mapInfo[i,j]);
                }
            }
        }


        private void RenderIteration(int i, int j)
        {
            if (Map[i, j].Symbol != _PreviousMap[i, j])
            {
                Console.BackgroundColor = Map[i, j].BgColor;
                Console.ForegroundColor = Map[i, j].FgColor;

                Console.SetCursorPosition(left: j, top: i);
                Console.Write(Map[i, j].Symbol);

                _PreviousMap[i, j] = Map[i, j].Symbol;
            }
        }

        public void RenderCommon(bool firstRender = false)
        {
            for (int i = 0; i < Controller.FieldHeight; i++)
            {
                for (int j = 0; j < Controller.FieldWidth; j++)
                {
                    if (firstRender)
                        FirstRenderIteration(i, j);
                    else
                        RenderIteration(i, j);
                }
                if (firstRender)
                    Console.Write('\n');
            }
        }

        public void FirstRenderIteration(int i, int j)
        {
            Console.BackgroundColor = Map[i, j].BgColor;
            Console.ForegroundColor = Map[i, j].FgColor;

            Console.Write(Map[i, j].Symbol);
            _PreviousMap[i, j] = Map[i, j].Symbol;
        }
    }
}
