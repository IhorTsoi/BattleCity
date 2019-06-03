using System;
using System.Collections.Generic;
using System.Text;
using BattleCity.Model.Game;

namespace BattleCity
{
    class Bullet : DynamicObject
    {
        private int damageValue;

        private Bullet ((int Y, int X) position, Field field, Directions direction, IGame game, int damageVal)
            : base(health: 1, position, field)
        {
            damageValue = damageVal;
            Direction = direction;
            Game = game;
            Field.Map[position.Y, position.X].Model = this;
        }


        public static void CreateBullet((int Y, int X) position, Directions direction, int damageVal, IGame game)
        {
            Field field = game.Field;
            Block block = field[position.Y, position.X];
            //
            if (block.Type == TypeOfBlock.EmptyCell)
            {
                field.AddBlock(
                    position, TypeOfBlock.Bullet);
                //
                game.Bullets.Add(
                    new Bullet(position, field, direction, game, damageVal));
            }
            else if(block.Model == null)
            {
                //
                // only non-destructable objects 
                // don't have model
                //
                return;
            }
            else
            {
                block.Model.GetDamaged(damageVal);
            }
        }


        public void MoveBullet ()
        {
            (int Y, int X) nextPosition = GetPosition(Position, Direction);
            //
            Block block = Field[nextPosition];
            TypeOfBlock type = block.Type;
            DamagableObject model = block.Model;
            //
            //
            if (type == TypeOfBlock.EmptyCell)
            {
                Field.MoveBlock(
                    movingObj: this,
                    nextPosition
                    );
                Position = nextPosition;
            }
            else if(model == null)
            {
                //
                // only non-destructable objects 
                // don't have model
                //
                Die();
            }
            else
            {
                model.GetDamaged(damageValue);
                Die();
            }
        }


        public override void Die ()
        {
            Field.DeleteBlock(Position);
            Game.Bullets.Remove(this);
        }
    }
}
