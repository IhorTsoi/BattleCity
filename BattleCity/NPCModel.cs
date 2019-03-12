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
            (bool horizontal, bool wMore, bool vertical, bool hMore) _PlayerOnLine = CheckForPlayerOnLine();
            // (bool horizontal, bool wMore, bool vertical, bool hMore)
            if (!(_PlayerOnLine.horizontal || _PlayerOnLine.vertical))
            {
                this._Move();
            }
            else
            {
                if (_PlayerOnLine.vertical)
                {
                    if (_PlayerOnLine.hMore)
                    {
                        if (this.Direction != Directions.Down)
                        {
                            this.Field.map[this.Position.Item1, this.Position.Item2].Rotate(Directions.Down, TypeOfBlock.NPC);
                            this.Direction = Directions.Down;
                        }
                    }
                    else
                    {
                        if (this.Direction != Directions.Up)
                        {
                            this.Field.map[this.Position.Item1, this.Position.Item2].Rotate(Directions.Up, TypeOfBlock.NPC);
                            this.Direction = Directions.Up;
                        }
                    }
                }
                else if (_PlayerOnLine.horizontal)
                {
                    if (_PlayerOnLine.wMore)
                    {
                        if (this.Direction != Directions.RIght)
                        {
                            this.Field.map[this.Position.Item1, this.Position.Item2].Rotate(Directions.RIght, TypeOfBlock.NPC);
                            this.Direction = Directions.RIght;
                        }
                    }
                    else
                    {
                        if (this.Direction != Directions.Left)
                        {
                            this.Field.map[this.Position.Item1, this.Position.Item2].Rotate(Directions.Left, TypeOfBlock.NPC);
                            this.Direction = Directions.Left;
                        }
                    }
                }
            }

            //code

        }
        public void AIShoot()
        {
            (bool horizontal, bool wMore, bool vertical, bool hMore) _PlayerOnLine = CheckForPlayerOnLine();

            if (_PlayerOnLine.vertical)
            {
                if (_PlayerOnLine.hMore)
                {
                    if (this.Direction == Directions.Down)
                    {
                        _Shoot();
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    if (this.Direction == Directions.Up)
                    {
                        _Shoot();
                    }
                    else
                    {
                        return;
                    }
                }
            }
            else if (_PlayerOnLine.horizontal)
            {
                if (_PlayerOnLine.wMore)
                {
                    if (this.Direction == Directions.RIght)
                    {
                        _Shoot();
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    if (this.Direction == Directions.Left)
                    {
                        _Shoot();
                    }
                    else
                    {
                        return;
                    }
                }
            }

            

            return;
        }

        private void _Shoot()
        {
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
        }

        private void _Move()
        {
            (int, int, Directions) _nextStep = (this.Position.Item1,this.Position.Item2, this.Direction);

            if (Math.Abs((this.Position.Item1 - this.Player.Position.Item1)) < Math.Abs((this.Position.Item2 - this.Player.Position.Item2)))
            {
                if (this.Position.Item1 > this.Player.Position.Item1)
                {
                    _nextStep = (this.Position.Item1 - 1, this.Position.Item2, Directions.Up);
                }
                else if (this.Position.Item1 < this.Player.Position.Item1)
                {
                    _nextStep = (this.Position.Item1 + 1, this.Position.Item2, Directions.Down);
                }

            }
            else
            {
                if (this.Position.Item2 > this.Player.Position.Item2)
                {
                    _nextStep = (this.Position.Item1, this.Position.Item2 - 1, Directions.Left);
                }
                else if (this.Position.Item2 < this.Player.Position.Item2)
                {
                    _nextStep = (this.Position.Item1, this.Position.Item2 + 1, Directions.RIght);
                }
            }

            if (this.Direction != _nextStep.Item3)
            {
                this.Field.map[this.Position.Item1, this.Position.Item2].Rotate(_nextStep.Item3, TypeOfBlock.NPC);
                this.Direction = _nextStep.Item3;
            }
            else
            {
                if (this.Field.map[_nextStep.Item1, _nextStep.Item2].Type == TypeOfBlock.EmptyCell)
                {

                    this.Field.map[_nextStep.Item1, _nextStep.Item2] = this.Field.map[this.Position.Item1, this.Position.Item2];
                    this.Field.map[this.Position.Item1, this.Position.Item2].TurnToEmpty();

                    this.Position = (_nextStep.Item1, _nextStep.Item2);
                }
            }

            return;
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


        private (bool, bool, bool, bool) CheckForPlayerOnLine()
        {
            (bool horizontal, bool wMore, bool vertical, bool hMore) res = (false, false, false, false);

            if (this.Player.Position.Item1 == this.Position.Item1)
            {
                res.horizontal = true;
                if (this.Player.Position.Item2 > this.Position.Item2)
                {
                    res.wMore = true;
                }
            }
            else if (this.Player.Position.Item2 == this.Position.Item2)
            {
                res.vertical = true;
                if (this.Player.Position.Item1 > this.Position.Item1)
                {
                    res.hMore = true;
                }
            }

            return res;
        }
        public override void Die()
        {
            this.Field.map[this.Position.Item1, this.Position.Item2].TurnToEmpty();
            this.GGame.NPCs.Remove(this);
        }
    }
}
