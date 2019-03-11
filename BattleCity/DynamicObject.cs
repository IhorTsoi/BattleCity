using System;
using System.Collections.Generic;
using System.Text;

namespace BattleCity
{
    abstract class DynamicObject
    {
        public virtual (int, int) Position { get; set; }
        public virtual Field Field { get; set; }
        public virtual Directions Direction { get; set; }
        public virtual Game GGame { get; set; }

        public virtual void Die() { }
    }
}
