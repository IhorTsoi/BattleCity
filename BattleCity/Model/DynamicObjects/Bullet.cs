using System;
using System.Collections.Generic;
using System.Text;
using BattleCity.Model.Game;

namespace BattleCity
{
    class Bullet : DynamicObject
    {
        #region Constructors

        public Bullet() { }

        public Bullet ((int, int) position, Field field, Directions direction, IGame game)
        {
            this.Position = position;
            this.Field = field;
            this.Direction = direction;
            this.GGame = game;
            this.Field.Map[position.Item1, position.Item2].Model = this;

        }

        #endregion

        public void MoveBullet ()
        {
            (int Y, int X) nextPosition = GetPosition(position: this.Position, direction: this.Direction);


            switch (this.Field[ nextPosition.Y, nextPosition.X ].Type)
            {
                case TypeOfBlock.EmptyCell:
                    this.Field[nextPosition.Y, nextPosition.X] = this.Field[this.Position.Y, this.Position.X];
                    this.Field.Map[this.Position.Y, this.Position.X].TurnToEmpty();

                    this.Position = nextPosition;
                    return;
                // EmptyCell is the only forwarding block that DOES NOT KILL the bullet.
                // "return" above is VITAL !
                    
                case TypeOfBlock.Player:
                    ((PlayerModel)this.Field.Map[nextPosition.Y, nextPosition.X].Model).Health--;
                    break;

                case TypeOfBlock.NPC:
                    ((NPCModel)this.Field.Map[nextPosition.Y, nextPosition.X].Model).Health--;
                    break;

                case TypeOfBlock.Bullet:
                    ((Bullet)this.Field.Map[nextPosition.Y, nextPosition.X].Model).Die();
                    break;

                case TypeOfBlock.BrickWall:
                    this.Field.Map[nextPosition.Y, nextPosition.X].Health--;
                    break;
                default:
                    break;
            }
            
            this.Die();
        }

        public override void Die ()
        {
            this.Field.Map[this.Position.Y, this.Position.X].TurnToEmpty();
            this.GGame.Bullets.Remove(this);
        }
    }
}
