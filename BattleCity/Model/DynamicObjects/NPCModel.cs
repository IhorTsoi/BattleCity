using System;
using System.Collections.Generic;
using System.Text;
using BattleCity.Model.Game;

namespace BattleCity
{
    class NPCModel : DynamicObject
    {
        // Properties:
        public PlayerModel Player { get; set; }
        private byte _health = 3;
        public byte Health
        {
            get { return _health; }
            set
            {
                if (value <= 0)
                {
                    Die();
                }
                else
                {
                    _health = value;
                }
            }
        }
        private bool _nextShoot = false;
        private bool _goingRoundTheObstacle = false;
        private int GetXDistance() => Math.Abs((Position.X - Player.Position.X));
        private int GetYDistance() => Math.Abs((Position.Y - Player.Position.Y));
        
        // Constructors:
        public NPCModel() { }
        //
        public NPCModel((int Y, int X) position, Field field, PlayerModel player, IGame game)
        {
            Position = position;
            Direction = Directions.Down;
            Field = field;
            Player = player;
            GGame = game;
            Field.Map[position.Y, position.X].Model = this;
        }

        // Public methods:
        public virtual void AIMove()
        {
            if (_goingRoundTheObstacle)
            {
                FinishMove(Direction);
                _goingRoundTheObstacle = false;
                return;
            }
            
            if (!PlayerIsOnLine(out Directions direction))
            {
                // if player is NOT on the same line
                MoveOnLine();
            }
            else
            {
                // player on the same line, checking direction
                //
                if (Direction != direction)
                {
                    RotateSelf(direction);
                }
                _nextShoot = true;
                
            }
        }

        public void AIShoot()
        {
            if (_nextShoot)
            {
                Shoot();
                _nextShoot = false;
            }
            return;
        }


        // Secondary shooting methods:
        private void Shoot()
        {
            (int Y, int X) = GetPosition(Position, Direction);

            switch (Field[Y, X].Type)
            {
                case TypeOfBlock.EmptyCell:
                    Field[Y, X] = new Block(TypeOfBlock.Bullet, model: null, direction: Direction);
                    GGame.Bullets.Add(new Bullet(( Y, X ), Field, Direction, GGame));
                    break;

                case TypeOfBlock.Player:
                    ((PlayerModel)Field.Map[Y, X].Model).Health--;
                    break;

                case TypeOfBlock.NPC:
                    ((NPCModel)Field.Map[Y, X].Model).Health--;
                    break;

                case TypeOfBlock.Bullet:
                    ((Bullet)Field.Map[Y, X].Model).Die();
                    break;

                case TypeOfBlock.BrickWall:
                    Field.Map[Y, X].Health--;
                    break;

                default:
                    // never change this line !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    break;
            }
        }


        // Secondary moving methods:
        private void MoveOnLine()
        {
            int _YDistance = GetYDistance(), _XDistance = GetXDistance();
            Queue<Directions> _nextSteps = new Queue<Directions>();
            //
            // defining the priority of directions
            //
            if (_YDistance < _XDistance)
            {
                _nextSteps.Enqueue((Position.Y > Player.Position.Y) ? Directions.Up : Directions.Down);
                if (Position.X > Player.Position.X)
                {
                    _nextSteps.Enqueue(Directions.Left);
                    _nextSteps.Enqueue(Directions.Right);
                }
                else
                {
                    _nextSteps.Enqueue(Directions.Right);
                    _nextSteps.Enqueue(Directions.Left);
                }
            }
            else
            {
                _nextSteps.Enqueue((Position.X > Player.Position.X) ? Directions.Left : Directions.Right);
                if (Position.Y > Player.Position.Y)
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
            //
            //
            //
            StepWithPriority(_nextSteps);
        }

        private void StepWithPriority(Queue<Directions> direction)
        {
            if (direction.Count == 0) return;
            //
            //
            //
            (int Y, int X) = GetPosition(Position, direction.Peek());
            //
            switch (Field[Y, X].Type)
            {
                case TypeOfBlock.EmptyCell:
                    if (Direction != direction.Peek())
                    {
                        RotateSelf(direction.Dequeue());
                        _goingRoundTheObstacle = true;
                    }
                    else
                    {
                        Field[Y, X] = Field[Position.Y, Position.X];
                        Field.Map[Position.Y, Position.X].TurnToEmpty();
                        Position = (Y, X);
                    }
                    return;

                default:
                    // if any obstacle
                    direction.Dequeue();
                    StepWithPriority(direction);
                    break;
            }            
        }

        private void FinishMove(Directions direction)
        {
            (int Y, int X) = GetPosition(Position, direction);

            switch (Field[Y, X].Type)
            {
                case TypeOfBlock.EmptyCell:
                        Field[Y, X] = Field[Position.Y, Position.X];
                        Field.Map[Position.Y, Position.X].TurnToEmpty();
                        Position = (Y, X);
                    return;

                default:
                    // if obstacle
                    _goingRoundTheObstacle = false;
                    AIMove();
                    return;
            }
        }

        private void RotateSelf(Directions direction)
        {
            Field.Map[ Position.Y, Position.X ].Rotate( direction, type: TypeOfBlock.NPC );
            Direction = direction;
        }


        // Private methods:
        private bool PlayerIsOnLine(out Directions direction)
        {
            if (Player.Position.Y == Position.Y)
            {
                direction = (Player.Position.X > Position.X) ? Directions.Right : Directions.Left;
                return true;
            }
            else if (Player.Position.X == Position.X)
            {
                direction = (Player.Position.Y > Position.Y) ? Directions.Down : Directions.Up;
                return true;
            }
            else
            {
                // is never used
                direction = Directions.Up;
                return false;
            }
        }

        public override void Die()
        {
            Field.Map[Position.Y, Position.X].TurnToEmpty();
            GGame.NPCs.Remove(this);
        }
    }
}
