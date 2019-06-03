using System;
using BattleCity.Model.Game;

namespace BattleCity
{
    abstract class DynamicObject : DamagableObject
    {
        public Directions Direction { get; set; } = Directions.Left;
        public IGame Game { get; set; }

        protected DynamicObject(int health, (int Y, int X) position, Field field) 
            : base(health, position, field)
        { }

        protected static (int, int) GetPosition((int Y, int X) position, Directions? direction)
        {
            (int Y, int X) res = ( position.Y, position.X );

            switch (direction)
            {
                // 0 <= res.X <= 59
                // 0 <= res.Y <= 14
                case Directions.Left:   //0
                case Directions.Right:  //2
                    res.X = (res.X + (int)direction + (Controller.FieldWidth - 1)) % Controller.FieldWidth;
                    break;
                case Directions.Up:     //1
                case Directions.Down:   //3
                    res.Y = (res.Y + (int)direction + (Controller.FieldHeight - 2)) % Controller.FieldHeight;
                    break;
                default: throw new NotImplementedException("Direction type not implemented");
            }

            return res;
        }
    }
}
