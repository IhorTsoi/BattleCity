using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace BattleCity
{
    class PlayerModel : DynamicObject
    {
        #region Fields & Properties

        public Directions? _nextStep = null;
        public bool _nextShot = false;

        private byte _health = 5;
        public byte Health {
            get { return _health; }
            set
            {
                if (value == 0)
                {
                    this.Die();
                }
                else
                {
                    _health = value;
                }
            }
        }


        #endregion

        #region Constructors

        public PlayerModel() { }

        public PlayerModel((int, int) position, Field field, Game game)
        {
            this.Position = position;
            this.Field = field;
            this.Direction = Directions.Up;
            this.GGame = game;
            this.Field.map[position.Item1, position.Item2].Model = this;

        }

        #endregion

        public void MoveHero()
        {
            if (_nextStep == null)
            {
                return;
            }

            if (this.Direction != _nextStep)
            {
                this.Field.map[this.Position.Item1, this.Position.Item2].Rotate(_nextStep, TypeOfBlock.Player);
                this.Direction = (Directions)_nextStep;
            }
            else
            {
                (int, int) nextPosition = GetPosition(this.Position, _nextStep);

                if (this.Field.map[nextPosition.Item1, nextPosition.Item2].Type == TypeOfBlock.EmptyCell)
                {

                    this.Field.map[nextPosition.Item1, nextPosition.Item2] = this.Field.map[this.Position.Item1, this.Position.Item2];
                    this.Field.map[this.Position.Item1, this.Position.Item2].TurnToEmpty();

                    this.Position = nextPosition;
                }
            }

            _nextStep = null;
            return;
        }

        public void Shoot() {
            if (!_nextShot)
            {
                return;
            }

            (int, int) nextPosition = GetPosition(this.Position, this.Direction);

            if (this.Field.map[nextPosition.Item1, nextPosition.Item2].Type == TypeOfBlock.EmptyCell)
            {
                this.Field.map[nextPosition.Item1, nextPosition.Item2] = new Block(TypeOfBlock.Bullet, model: null, direction: this.Direction);
                this.GGame.Bullets.Add(new Bullet(nextPosition, this.Field, this.Direction, this.GGame));
            }
            else if (this.Field.map[nextPosition.Item1, nextPosition.Item2].Type != TypeOfBlock.Wall)
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
            }

            _nextShot = !_nextShot;
            return;
        }

        public override void Die()
        {
            this.GGame.StopTimer();
            this.GGame.gameOver = true;
        }

        private (int, int) GetPosition ((int,int) position, Directions? direction)
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
