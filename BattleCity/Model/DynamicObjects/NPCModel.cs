using System;
using System.Collections.Generic;
using System.Text;
using BattleCity.Model.Game;
using BattleCity.Model.Components;
using System.IO;

namespace BattleCity
{
    class NPCModel : DynamicObject
    {
        // Properties:
        protected PlayerModel Player { get; set; }
        private bool _nextShoot = false;
        private readonly RollStack<Directions> path = new RollStack<Directions>(length: 2);

        // Constructors:

        public NPCModel((int Y, int X) position, Field field, PlayerModel player, IGame game)
            : base(health: 3, position, field)
        {
            Player = player;
            Game = game;
            Field.Map[position.Y, position.X].Model = this;
        }



        // PATH-FINDING ALGORITHM:

        
        public virtual void AIMove()
        {
            if( PlayerIsOnLine(out Directions direction) )
            {
                if (Direction != direction)
                {
                    RotateSelf(direction);
                }
                _nextShoot = true;
                //
                path.MakeEmpty();
                return;
            }

            if (path.IsEmpty())
            {
                FindPath();
            }
            else
            {
                Step();
            }
        }


        private void Step()
        {
            Directions direction = path.Peek();
            (int Y, int X) = GetPosition(Position, direction);
            //
            if ( Field[Y,X].Type == TypeOfBlock.EmptyCell )
            {
                if (Direction != direction)
                {
                    RotateSelf(direction);
                }
                else
                {
                    Field.MoveBlock(
                        movingObj: this,
                        toCoords: (Y, X),
                        Direction);
                    Position = (Y, X);
                    path.Pop();
                }
            }
        }
        //
        private void FindPath()
        {
            bool isReachable = LaunchWave(out int[,] liField);

            if (!isReachable)
            {
                return;
            }

            RecoverPath( liField );
        }


        private bool LaunchWave(out int[,] liField)
        {
            // QUEUE, FIELD, bool FOUND:
            Queue<(int Y, int X)> cellsToVisit = new Queue<(int, int)>();
            liField = CreateLiField(Field.Map);
            bool FOUND = false;

            // adding current position to QUEUE:
            cellsToVisit.Enqueue(Position);

            // WAVE:
            while (cellsToVisit.Count != 0 && !FOUND)
            {
                (int Y, int X) currentPos = cellsToVisit.Dequeue();
                List<(int Y, int X)> positionsOrtogonal = GenerateOrtogonal(currentPos);

                foreach(var pos in positionsOrtogonal)
                {
                    if (liField[pos.Y, pos.X] == -1)
                    {
                        liField[pos.Y, pos.X] = liField[currentPos.Y, currentPos.X] + 1;
                        cellsToVisit.Enqueue(pos);
                    }
                    else if (liField[pos.Y, pos.X] == -3)
                    {
                        liField[pos.Y, pos.X] = liField[currentPos.Y, currentPos.X] + 1;
                        FOUND = true;
                        break;
                    }
                }
            }

            /*
             * LOGGING
             * 
            File.AppendAllLines("log.txt", new string[] { "" });
            StringBuilder log = new StringBuilder(900);
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 60; j++)
                {
                    log.Append(liField[i, j] + " ,");
                }
            }
            File.AppendAllText("log.txt", log.ToString());
            */

            return FOUND;
        }
        //
        private static List<(int Y, int X)> GenerateOrtogonal((int Y, int X) pos)
        {
            List<(int Y, int X)> positionsOrtogonal = new List<(int Y, int X)>(4);


            (int Y, int X) tempPos;
            // left
            tempPos = pos;
            tempPos.X--;
            if (tempPos.X >= 0)
            {
                positionsOrtogonal.Add(tempPos);
            }
            // top
            tempPos = pos;
            tempPos.Y--;
            if (tempPos.Y >= 0)
            {
                positionsOrtogonal.Add(tempPos);
            }
            // right
            tempPos = pos;
            tempPos.X++;
            if (tempPos.X < Controller.FieldWidth)
            {
                positionsOrtogonal.Add(tempPos);
            }
            // down
            tempPos = pos;
            tempPos.Y++;
            if (tempPos.Y < Controller.FieldHeight)
            {
                positionsOrtogonal.Add(tempPos);
            }


            return positionsOrtogonal;
        }
        //
        private void RecoverPath(int[,] liField)
        {
            (int Y, int X) position = Player.Position;

            while (position != Position)
            {
                (int Y, int X) nextPosition = FindDecrementValue(position, liField);
                path.Push(GetDirectionFromTo(nextPosition, position));
                position = nextPosition;
            }
        }
        //
        private static Directions GetDirectionFromTo((int Y, int X) nextPosition, (int Y, int X) position)
        {
            int diff;
            if (nextPosition.X != position.X)
            {
                diff = nextPosition.X - position.X;
                return diff < 0 ? Directions.Right : Directions.Left;
            }
            else
            {
                diff = nextPosition.Y - position.Y;
                return diff < 0 ? Directions.Down : Directions.Up;
            }
        }
        //
        private static (int Y, int X) FindDecrementValue((int Y, int X) position, int[,] liField)
        {
            List<(int Y, int X)> coordsOrtogonal = GenerateOrtogonal(position);

            foreach (var coords in coordsOrtogonal)
            {
                if (liField[position.Y, position.X] - liField[coords.Y, coords.X] == 1)
                {
                    return coords;
                }
            }

            throw new Exception("This situation will never happen!");
        }
        //
        private int[,] CreateLiField(Block[,] map)
        {
            int[,] liField = new int[Controller.FieldHeight, Controller.FieldWidth];
            //
            for (int Y = 0; Y < Controller.FieldHeight; Y++)
            {
                for (int X = 0; X < Controller.FieldWidth; X++)
                {
                    (int, int) pos = (Y, X);

                    if (pos == Position)
                    {
                        liField[Y, X] = 0;
                    }
                    else if (pos == Player.Position)
                    {
                        liField[Y, X] = -3;
                    }
                    else if (map[Y,X].Type == TypeOfBlock.EmptyCell)
                    {
                        liField[Y, X] = -1;
                    }
                    else
                    {
                        liField[Y, X] = -2;
                    }
                }
            }
            
            /*
             * LOGGING
             * 
            StringBuilder log = new StringBuilder(900);
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 60; j++)
                {
                    log.Append(liField[i, j] + " ,");
                }
            }
            File.WriteAllText("log.txt", log.ToString());
            */
            
            return liField;
        }

        

        #region Previous algorithm of path choosing
        /*
        
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


        private int GetXDistance() => Math.Abs((Position.X - Player.Position.X));
        private int GetYDistance() => Math.Abs((Position.Y - Player.Position.Y));
        private bool _goingRoundTheObstacle = false;

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

        */
        #endregion



        // Shooting:
        public void AIShoot()
        {
            if (_nextShoot)
            {
                Shoot();
                _nextShoot = false;
            }
            return;
        }

        private void Shoot()
        {
            (int Y, int X) nextPosition = GetPosition(Position, Direction);

            Bullet.CreateBullet(nextPosition, Direction, damageVal: 1, Game);
        }



        // Private methods:
        private void RotateSelf(Directions direction)
        {
            Field.RotateBlock(Position, direction);
            Direction = direction;
        }

        private bool PlayerIsOnLine(out Directions direction)
        {
            direction = default(Directions);
            //
            if (Player.Position.Y == Position.Y)
            {
                direction = (Player.Position.X > Position.X) ? Directions.Right : Directions.Left;
            }
            else if (Player.Position.X == Position.X)
            {
                direction = (Player.Position.Y > Position.Y) ? Directions.Down : Directions.Up;
            }
            else
            {
                return false;
            }
            //
            return LineIsShootable(direction);
        }

        private bool LineIsShootable(Directions direction)
        {
            (int diffY, int diffX) = (0, 0); 
            if (direction == Directions.Left || direction == Directions.Right)
            {
                diffX = (int)direction - 1; // 0-1 = -1, 2-1 = 1
            }
            else
            {
                diffY = (int)direction - 2; // 1-2 = -1, 3-2 = 1
            }
            //
            for (int Y = Position.Y, X = Position.X; (Y,X) != Player.Position; Y += diffY, X += diffX)
            {
                if (Field[Y,X].Type == TypeOfBlock.Wall)
                {
                    return false;
                }
            }
            //
            return true;
        }

        public override void Die()
        {
            Field.DeleteBlock(Position);
            Game.NPCs.Remove(this);
        }
    }
}
