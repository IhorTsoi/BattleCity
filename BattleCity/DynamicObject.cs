using System;
using System.Collections.Generic;
using System.Text;

namespace BattleCity
{
    abstract class DynamicObject
    {
        public (int Y, int X) Position { get; set; }
        public Field Field { get; set; }
        public Directions Direction { get; set; }
        public IBasicGame GGame { get; set; }

        public virtual void Die() { }

        protected static (int, int) _GetPosition((int, int) position, Directions? direction)
        {
            (int, int) res = ( position.Item1, position.Item2);

            switch (direction)
            {
                case Directions.Up:
                    res.Item1--;
                    break;
                case Directions.RIght:
                    res.Item2++;
                    break;
                case Directions.Down:
                    res.Item1++;
                    break;
                case Directions.Left:
                    res.Item2--;
                    break;
                default:
                    break;
            }

            if (res.Item1 == -1)
            {
                res.Item1 = 14;
            }
            else if (res.Item1 == 15)
            {
                res.Item1 = 0;
            }
            else if (res.Item2 == -1)
            {
                res.Item2 = 59;
            }
            else if (res.Item2 == 60)
            {
                res.Item2 = 0;
            }
            return res;
        }
    }
}
