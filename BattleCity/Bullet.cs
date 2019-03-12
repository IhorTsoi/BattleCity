using System;
using System.Collections.Generic;
using System.Text;

namespace BattleCity
{
    class Bullet : DynamicObject
    {
        #region Constructors

        public Bullet() { }

        public Bullet ((int, int) position, Field field, Directions direction, Game game)
        {
            this.Position = position;
            this.Field = field;
            this.Direction = direction;
            this.GGame = game;
            this.Field.map[position.Item1, position.Item2].Model = this;

        }

        #endregion

        public void MoveBullet ()
        {
            (int Y, int X) nextPosition = _GetPosition(position: this.Position, direction: this.Direction);


            switch (this.Field.map[ nextPosition.Y, nextPosition.X ].Type)
            {
                case TypeOfBlock.EmptyCell:
                    this.Field.map[nextPosition.Y, nextPosition.X] = this.Field.map[this.Position.Y, this.Position.X];
                    this.Field.map[this.Position.Y, this.Position.X].TurnToEmpty();

                    this.Position = nextPosition;
                    return;
                // EmptyCell is the only forwarding block that DOES NOT KILL the bullet.
                // "return" above is VITAL !
                    
                case TypeOfBlock.Player:
                    ((PlayerModel)this.Field.map[nextPosition.Y, nextPosition.X].Model).Health--;
                    break;

                case TypeOfBlock.NPC:
                    ((NPCModel)this.Field.map[nextPosition.Y, nextPosition.X].Model).Health--;
                    break;

                case TypeOfBlock.Bullet:
                    ((Bullet)this.Field.map[nextPosition.Y, nextPosition.X].Model).Die();
                    break;

                case TypeOfBlock.BrickWall:
                    this.Field.map[nextPosition.Y, nextPosition.X].Health--;
                    break;
                default:
                    break;
            }
            
            this.Die();
        }

        public override void Die ()
        {
            this.Field.map[this.Position.Y, this.Position.X].TurnToEmpty();
            this.GGame.Bullets.Remove(this);
        }
    }
}
