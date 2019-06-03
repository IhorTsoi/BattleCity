using System;
using System.Collections.Generic;
using System.Text;

namespace BattleCity.Model.DynamicObjects
{
    class BrickWall : DamagableObject
    {
        public BrickWall((int Y, int X) position, Field field)
            : base(health: 2, position, field)
        { }

        public override void Die()
        {
            Field.DeleteBlock(Position);
        }
    }
}
