using System;
using System.Collections.Generic;
using System.Text;

namespace BattleCity
{
    abstract class DamagableObject
    {
        private int health;
        public (int Y, int X) Position { get; set; }
        public Field Field { get; set; }


        protected DamagableObject(int health, (int Y, int X) position, Field field)
        {
            this.health = health;
            Position = position;
            Field = field;
        }


        public void GetDamaged(int damageValue = 1)
        {
            health -= damageValue;
            if (health <= 0)
            {
                Die();
            }
        }
        public abstract void Die(); 
    }
}
