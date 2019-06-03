using System;
using System.Collections.Generic;
using System.Text;

namespace BattleCity.Views
{
    struct BlockState
    {
        public (int Y, int X) Position { get; private set; }
        public Directions Direction { get; private set; }
        public TypeOfBlock Type { get; private set; }

        public BlockState((int Y, int X) position, TypeOfBlock type, Directions direction)
        {
            Position = position;
            Type = type;
            Direction = direction;
        }
    }
}
