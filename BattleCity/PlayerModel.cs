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

        public PlayerModel((int, int) position, Field field, IBasicGame game)
        {
            this.Position = position;
            this.Field = field;
            this.Direction = Directions.Up;
            this.GGame = game;
            this.Field.map[position.Item1, position.Item2].Model = this;

        }

        #endregion

        public Directions? MoveHero()
        {
            if (_nextStep == null)
            {
                return _nextStep;
            }

            if (this.Direction != _nextStep)
            {
                this.Field.map[this.Position.Y, this.Position.X].Rotate(_nextStep, TypeOfBlock.Player);
                this.Direction = (Directions)_nextStep;
            }
            else
            {
                (int, int) nextPosition = _GetPosition(this.Position, _nextStep);

                if (this.Field.map[nextPosition.Item1, nextPosition.Item2].Type == TypeOfBlock.EmptyCell)
                {

                    this.Field.map[nextPosition.Item1, nextPosition.Item2] = this.Field.map[this.Position.Y, this.Position.X];
                    this.Field.map[this.Position.Y, this.Position.X].TurnToEmpty();

                    this.Position = nextPosition;
                }
            }
            Directions? _nextStepCopy = _nextStep;
            _nextStep = null;
            return _nextStepCopy;
        }

        public bool Shoot() {
            if (!_nextShot)
            {
                return false;
            }

            (int, int) nextPosition = _GetPosition(this.Position, this.Direction);
            //////////////////////

            switch (this.Field.map[nextPosition.Item1, nextPosition.Item2].Type)
            {
                case TypeOfBlock.EmptyCell:
                    this.Field.map[nextPosition.Item1, nextPosition.Item2] = new Block(TypeOfBlock.Bullet, model: null, direction: this.Direction);
                    this.GGame.Bullets.Add(new Bullet(nextPosition, this.Field, this.Direction, this.GGame));
                    break;
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

            ///////////////
            _nextShot = false;
            return true;
        }

        public override void Die()
        {
            this.GGame.Quit();
        }
    }
}
