using System;
using System.Collections.Generic;
using System.Text;

namespace BattleCity
{
    class NPCModel : DynamicObject
    {
        #region Fields & Properties

        public PlayerModel Player { get; set; }
        private byte _health = 3;
        public byte Health
        {
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
        private bool _nextShoot = false;
        private bool _goingRoundTheObstacle = false;
        private int GetXDistance() => Math.Abs((this.Position.X - this.Player.Position.X));
        private int GetYDistance() => Math.Abs((this.Position.Y - this.Player.Position.Y));

        #endregion

        #region Constructors

        public NPCModel() { }

        public NPCModel((int, int) position, Field field, PlayerModel player, IBasicGame game)
        {
            this.Position = position;
            this.Direction = Directions.Down;
            this.Field = field;
            this.Player = player;
            this.GGame = game;
            this.Field.map[position.Item1, position.Item2].Model = this;
        }

        #endregion

        public void AIMove()
        {
            if (this._goingRoundTheObstacle)
            {
                this._FinishMove(this.Direction);
                this._goingRoundTheObstacle = false;
                return;
            }
            
            if (!_PlayerIsOnLine(out Directions direction))
            {
                this._MoveOnLine();
            }
            else
            {
                if (this.Direction != direction)
                {
                    this._RotateSelf(direction);
                    _nextShoot = true;
                }
                else
                {
                    _nextShoot = true;
                }
            }
        }

        public void AIShoot()
        {
            if (_nextShoot)
            {
                this._Shoot();
                _nextShoot = false;
            }
            return;
        }

        private void _Shoot()
        {
            (int Y, int X) nextPosition = _GetPosition(this.Position, this.Direction);

            switch (this.Field.map[nextPosition.Y, nextPosition.X].Type)
            {
                case TypeOfBlock.EmptyCell:
                    this.Field.map[nextPosition.Y, nextPosition.X] = new Block(TypeOfBlock.Bullet, model: null, direction: this.Direction);
                    this.GGame.Bullets.Add(new Bullet(nextPosition, this.Field, this.Direction, this.GGame));
                    break;

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
        }

        private void _MoveOnLine()
        {
            int _YDistance = GetYDistance(), _XDistance = GetXDistance();
            Queue<Directions> _nextSteps = new Queue<Directions>();

            // defining the priority of directions
            if (_YDistance < _XDistance)
            {
                _nextSteps.Enqueue((this.Position.Y > this.Player.Position.Y) ? Directions.Up : Directions.Down);
                if (this.Position.X > this.Player.Position.X)
                {
                    _nextSteps.Enqueue(Directions.Left);
                    _nextSteps.Enqueue(Directions.RIght);
                }
                else
                {
                    _nextSteps.Enqueue(Directions.RIght);
                    _nextSteps.Enqueue(Directions.Left);
                }
            }
            else
            {
                _nextSteps.Enqueue((this.Position.X > this.Player.Position.X) ? Directions.Left : Directions.RIght);
                if (this.Position.Y > this.Player.Position.Y)
                {
                    _nextSteps.Enqueue(Directions.Up);
                    _nextSteps.Enqueue(Directions.Down);
                }
                else
                {
                    _nextSteps.Enqueue(Directions.Down);
                    _nextSteps.Enqueue(Directions.Up);
                }
            }
            _nextSteps.Enqueue((Directions)(((int)_nextSteps.Peek() + 2) % 4));
            
            this._StepWithPriority(_nextSteps);

            return;
        }

        



        private void _StepWithPriority(Queue<Directions> direction)
        {
            if (direction.Count == 0)
            {
                return;
            }
            else
            {
                int _y = this.Position.Y,
                    _x = this.Position.X;

                switch (direction.Peek())
                {
                    case Directions.Up:
                        _y--;
                        break;
                    case Directions.RIght:
                        _x++;
                        break;
                    case Directions.Down:
                        _y++;
                        break;
                    case Directions.Left:
                        _x--;
                        break;
                    default:
                        break;
                }

                switch (this.Field.map[_y, _x].Type)
                {
                    // if empty
                    case TypeOfBlock.EmptyCell:
                        if (this.Direction != direction.Peek())
                        {
                            this._RotateSelf(direction.Dequeue());
                            this._goingRoundTheObstacle = true;
                        }
                        else
                        {
                            this.Field.map[_y, _x] = this.Field.map[this.Position.Y, this.Position.X];
                            this.Field.map[this.Position.Y, this.Position.X].TurnToEmpty();
                            this.Position = (_y, _x);
                        }
                        return;

                    // if obstacle
                    default:
                        direction.Dequeue();
                        this._StepWithPriority(direction);
                        break;
                }
            }
        }

        private void _FinishMove(Directions direction)
        {
            int _y = this.Position.Y,
                _x = this.Position.X;

            switch (direction)
            {
                case Directions.Up:
                    _y--;
                    break;
                case Directions.RIght:
                    _x++;
                    break;
                case Directions.Down:
                    _y++;
                    break;
                case Directions.Left:
                    _x--;
                    break;
                default:
                    break;
            }

            switch (this.Field.map[_y, _x].Type)
            {
                // if empty
                case TypeOfBlock.EmptyCell:
                        this.Field.map[_y, _x] = this.Field.map[this.Position.Y, this.Position.X];
                        this.Field.map[this.Position.Y, this.Position.X].TurnToEmpty();
                        this.Position = (_y, _x);
                    return;

                // if obstacle
                default:
                    this._goingRoundTheObstacle = false;
                    this.AIMove();
                    return;
            }
        }

        private void _RotateSelf(Directions direction)
        {
            this.Field.map[ this.Position.Y, this.Position.X ].Rotate( direction: direction, type: TypeOfBlock.NPC );
            this.Direction = direction;
        }
        private bool _PlayerIsOnLine(out Directions direction)
        {
            if (this.Player.Position.Y == this.Position.Y)
            {
                direction = (this.Player.Position.X > this.Position.X) ? Directions.RIght : Directions.Left;
                return true;
            }
            else if (this.Player.Position.X == this.Position.X)
            {
                direction = (this.Player.Position.Y > this.Position.Y) ? Directions.Down : Directions.Up;
                return true;
            }
            else
            {
                direction = Directions.Up;
                return false;
            }
        }



        public override void Die()
        {
            this.Field.map[this.Position.Y, this.Position.X].TurnToEmpty();
            this.GGame.NPCs.Remove(this);
        }
    }
}
