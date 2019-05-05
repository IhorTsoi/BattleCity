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
            Position = position;
            Field = field;
            Direction = direction;
            GGame = game;
            Field.Map[position.Item1, position.Item2].Model = this;

        }

        #endregion

        public void MoveBullet ()
        {
            (int Y, int X) nextPosition = GetPosition(position: Position, direction: Direction);


            switch (Field[ nextPosition.Y, nextPosition.X ].Type)
            {
                case TypeOfBlock.EmptyCell:
                    Field[nextPosition.Y, nextPosition.X] = Field[Position.Y, Position.X];
                    Field.Map[Position.Y, Position.X].TurnToEmpty();

                    Position = nextPosition;
                    return;
                // EmptyCell is the only forwarding block that DOES NOT KILL the bullet.
                // "return" above is VITAL !
                    
                case TypeOfBlock.Player:
                    ((PlayerModel)Field.Map[nextPosition.Y, nextPosition.X].Model).Health--;
                    break;

                case TypeOfBlock.NPC:
                    ((NPCModel)Field.Map[nextPosition.Y, nextPosition.X].Model).Health--;
                    break;

                case TypeOfBlock.Bullet:
                    ((Bullet)Field.Map[nextPosition.Y, nextPosition.X].Model).Die();
                    break;

                case TypeOfBlock.BrickWall:
                    Field.Map[nextPosition.Y, nextPosition.X].Health--;
                    break;
                default:
                    break;
            }
            
            Die();
        }

        public override void Die ()
        {
            Field.Map[Position.Y, Position.X].TurnToEmpty();
            GGame.Bullets.Remove(this);
        }
    }
}
