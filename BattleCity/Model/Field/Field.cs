using BattleCity.Model.Game;
using BattleCity.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace BattleCity
{
    class Field
    {
        public Block[,] Map;
        private FieldViewer FieldViewer;

        public Block this[int Y, int X]
        {
            get => Map[Y, X];
            private set => Map[Y, X] = value;
        }
        public Block this[(int Y, int X) pos]
        {
            get => Map[pos.Y, pos.X];
            private set => Map[pos.Y, pos.X] = value;
        }


        public Field (TypeOfBlock[,] mapInfo)
        {
            Map = new Block[Controller.FieldHeight, Controller.FieldWidth];
            FieldViewer = FieldViewer.GetInstance();

            for(int i = 0; i < Controller.FieldHeight; i++)
            {
                for (int j = 0; j < Controller.FieldWidth; j++)
                {
                    AddBlock((i, j), mapInfo[i, j]);
                }
            }
        }


        #region Update mapModel & view

        public void AddBlock((int Y, int X) pos, TypeOfBlock type, Directions direction = Directions.Left)
        {
            //
            // Updating mapModel:
            //
            this[pos] = new Block(type);
            //
            // Requesting view update for one cells:
            //
            FieldViewer.Update(pos, type, direction);
        }

        public void MoveBlock(DynamicObject movingObj, (int Y, int X) toCoords, Directions direction = Directions.Left)
        {
            (int Y, int X) fromPos = movingObj.Position;
            TypeOfBlock movingObjType = this[fromPos].Type;
            //
            // Updating mapModel:
            //
            this[toCoords] = this[fromPos];
            Map[fromPos.Y, fromPos.X].TurnToEmpty();
            //
            // Requesting view update for two cells:
            //
            FieldViewer.Update(fromPos, TypeOfBlock.EmptyCell);
            FieldViewer.Update(toCoords, movingObjType, direction);
        }

        public void DeleteBlock((int Y, int X) pos)
        {
            //
            // Updating mapModel:
            //
            Map[pos.Y, pos.X].TurnToEmpty();
            //
            // Requesting view update for one cell:
            //
            FieldViewer.Update(pos, TypeOfBlock.EmptyCell);
        }

        public void RotateBlock((int Y, int X) pos, Directions direction)
        {
            TypeOfBlock blockType = this[pos].Type;
            //
            // Updating mapModel:
            //
            // (not updated)
            //
            // Requesting view update for one cell:
            //
            FieldViewer.Update(pos, blockType, direction);
        }

        #endregion
    }
}
