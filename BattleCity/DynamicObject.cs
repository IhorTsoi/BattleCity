using System;
using System.Collections.Generic;
using System.Text;

namespace BattleCity
{
    abstract class DynamicObject
    {
        public (int, int) Position { get; set; }
        public Field Field { get; set; }
        public Directions Direction { get; set; }
        public Game GGame { get; set; }

        public virtual void Die() { }
    }
}
