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
        
        #endregion

        #region Constructors

        public NPCModel() { }

        public NPCModel((int, int) position,Field field , PlayerModel player, Game game)
        {
            this.Position = position;
            this.Direction = Directions.Down;
            this.Field = field;
            this.Player = player;
            this.GGame = game;
            this.Field.map[position.Item1, position.Item2].Model = this;

        }

        #endregion

        /*
         todo:
         1. if the player is online:
            - if the path contains a wall:
                - go up/down;

            - if the path contains a bullet && the bullet goes towards:
                - go up/down;

            -else:
                - return;

            COMING SOON:
            - if the path contains an NPC:
                - return;

         2. if the player is not online:
            - if the shortest path to the Player contains the wall:
                - go by the second path
                - if the second path to the Player contains the wall:
                    - go the opposite direction
                    - if the opposite direction to the Player contains the wall:
                        - go the second opposite direction
        */

        public void AIMove()
        {
            (bool horizontal, bool wMore, bool vertical, bool hMore) _PlayerOnLine = _CheckForPlayerOnLine();

            if (!(_PlayerOnLine.horizontal || _PlayerOnLine.vertical))
            {
                this._Move();
            }
            else
            {
                if (_PlayerOnLine.vertical)
                {
                    if (_PlayerOnLine.hMore && this.Direction != Directions.Down)
                    {
                        this._RotateSelf(Directions.Down);
                    }
                    else if (!_PlayerOnLine.hMore && this.Direction != Directions.Up)
                    {
                        this._RotateSelf(Directions.Up);
                    }
                }
                else if (_PlayerOnLine.horizontal)
                {
                    if (_PlayerOnLine.wMore && this.Direction != Directions.RIght)
                    {
                        this._RotateSelf(Directions.RIght);
                    }
                    else if (!_PlayerOnLine.wMore && this.Direction != Directions.Left)
                    {
                        this._RotateSelf(Directions.Left);
                    }
                }
            }
        }

        public void AIShoot()
        {
            (bool horizontal, bool wMore, bool vertical, bool hMore) _PlayerOnLine = _CheckForPlayerOnLine();


            if (_PlayerOnLine.vertical)
            {
                if (_PlayerOnLine.hMore && this.Direction == Directions.Down 
                    || !_PlayerOnLine.hMore && this.Direction == Directions.Up)
                {
                    _Shoot();
                }
            }
            else if (_PlayerOnLine.horizontal)
            {
                if (_PlayerOnLine.wMore && this.Direction == Directions.RIght
                    || !_PlayerOnLine.wMore && this.Direction == Directions.Left)
                {
                    _Shoot();
                }
            }
            
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

        private void _Move()
        {
            (int, int, Directions) _nextStep = (this.Position.Y, this.Position.X, this.Direction);

            // defining the shortest STRAIGHT path to the Player
            if (Math.Abs((this.Position.Y - this.Player.Position.Item1)) < Math.Abs((this.Position.X - this.Player.Position.Item2)))
            {
                if (this.Position.Y > this.Player.Position.Item1)
                {
                    _nextStep = (this.Position.Y - 1, this.Position.X, Directions.Up);
                }
                else if (this.Position.Y < this.Player.Position.Item1)
                {
                    _nextStep = (this.Position.Y + 1, this.Position.X, Directions.Down);
                }
            }
            else
            {
                if (this.Position.X > this.Player.Position.Item2)
                {
                    _nextStep = (this.Position.Y, this.Position.X - 1, Directions.Left);
                }
                else if (this.Position.X < this.Player.Position.Item2)
                {
                    _nextStep = (this.Position.Y, this.Position.X + 1, Directions.RIght);
                }
            }

            // rotate or move
            if (this.Direction != _nextStep.Item3)
            {
                this._RotateSelf(_nextStep.Item3);
            }
            else
            {
                if (this.Field.map[_nextStep.Item1, _nextStep.Item2].Type == TypeOfBlock.EmptyCell)
                {

                    this.Field.map[_nextStep.Item1, _nextStep.Item2] = this.Field.map[this.Position.Y, this.Position.X];
                    this.Field.map[this.Position.Y, this.Position.X].TurnToEmpty();

                    this.Position = (_nextStep.Item1, _nextStep.Item2);
                }
            }

            return;
        }
        
        private void _RotateSelf(Directions direction)
        {
            this.Field.map[ this.Position.Y, this.Position.X ].Rotate( direction: direction, type: TypeOfBlock.NPC );
            this.Direction = direction;
        }
        
        private (bool, bool, bool, bool) _CheckForPlayerOnLine()
        {
            (bool horizontal, bool wMore, bool vertical, bool hMore) res = (false, false, false, false);

            if (this.Player.Position.Item1 == this.Position.Y)
            {
                res.horizontal = true;
                if (this.Player.Position.Item2 > this.Position.X)
                {
                    res.wMore = true;
                }
            }
            else if (this.Player.Position.Item2 == this.Position.X)
            {
                res.vertical = true;
                if (this.Player.Position.Item1 > this.Position.Y)
                {
                    res.hMore = true;
                }
            }

            return res;
        }



        public override void Die()
        {
            this.Field.map[this.Position.Y, this.Position.X].TurnToEmpty();
            this.GGame.NPCs.Remove(this);
        }
    }
}
