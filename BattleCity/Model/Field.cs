﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BattleCity
{
    class Field
    {
        public Block[,] Map;
        private string[,] _PreviousMap = new string[15, 60];

        public Block this[int Y, int X]
        {
            get => this.Map[Y, X];
            set => this.Map[Y, X] = value;
        }

        public Field (TypeOfBlock[,] mapInfo)
        {
            this.Map = new Block[15,60];
            for(int i = 0; i < 15;i++)
            {
                for (int j = 0; j < 60; j++)
                {
                    this.Map[i, j] = new Block(mapInfo[i,j]);
                }
            }
        }


        private void RenderIteration(int i, int j)
        {
            if (this.Map[i, j].Symbol != this._PreviousMap[i, j])
            {
                Console.BackgroundColor = this.Map[i, j].BgColor;
                Console.ForegroundColor = this.Map[i, j].FgColor;

                Console.SetCursorPosition(left: j, top: i);
                Console.Write(this.Map[i, j].Symbol);

                _PreviousMap[i, j] = this.Map[i, j].Symbol;
            }
        }

        public void RenderCommon(bool first = false)
        {
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 60; j++)
                {
                    if (first)
                        FirstRenderIteration(i, j);
                    else
                        RenderIteration(i, j);
                }
                if (first)
                    Console.Write('\n');
            }
        }

        public void FirstRenderIteration(int i, int j)
        {
            Console.BackgroundColor = this.Map[i, j].BgColor;
            Console.ForegroundColor = this.Map[i, j].FgColor;

            Console.Write(this.Map[i, j].Symbol);
            _PreviousMap[i, j] = this.Map[i, j].Symbol;
        }
    }
}