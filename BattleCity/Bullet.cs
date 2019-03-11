using System;
using System.Collections.Generic;
using System.Text;

namespace BattleCity
{
    class Bullet : DynamicObject
    {
        #region Fields & Properties

        public override (int, int) Position { get; set; }
        public override Field Field { get; set; }
        public override Directions Direction { get; set; }
        public override Game GGame { get; set; }


        #endregion

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
            (int, int) nextPosition = GetPosition(position: this.Position, direction: this.Direction);

            // if
            if (this.Field.map[nextPosition.Item1, nextPosition.Item2].Type == TypeOfBlock.EmptyCell)
            {
                this.Field.map[nextPosition.Item1, nextPosition.Item2] = this.Field.map[this.Position.Item1, this.Position.Item2];
                this.Field.map[this.Position.Item1, this.Position.Item2].TurnToEmpty();

                this.Position = nextPosition;
            }
            else
            {
                switch (this.Field.map[nextPosition.Item1, nextPosition.Item2].Type)
                {
                    case TypeOfBlock.Player:
                        ((PlayerModel)this.Field.map[nextPosition.Item1, nextPosition.Item2].Model).Health--;
                        break;
                    case TypeOfBlock.NPC:
                        ((NPCModel)this.Field.map[nextPosition.Item1, nextPosition.Item2].Model).Health--;
                        break;
                    case TypeOfBlock.Bullet:
                        ((Bullet)this.Field.map[nextPosition.Item1, nextPosition.Item2].Model).Die();
                        break;
                    case TypeOfBlock.BrickWall:
                        this.Field.map[nextPosition.Item1, nextPosition.Item2].Health--;
                        break;
                    default:
                        break;
                }
                this.Die();
            }
        }

        public override void Die ()
        {
            this.Field.map[this.Position.Item1, this.Position.Item2].TurnToEmpty();
            this.GGame.Bullets.Remove(this);
        }

        private (int, int) GetPosition((int, int) position, Directions? direction)
        {
            (int, int) res = (0 + position.Item1, 0 + position.Item2);

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
