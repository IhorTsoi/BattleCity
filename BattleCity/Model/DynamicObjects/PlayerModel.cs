using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using BattleCity.Model.Game;

namespace BattleCity
{
    class PlayerModel : DynamicObject
    {
        // Properties
        public Directions? NextStep = null;
        public bool NextShoot = false;
        public byte Health {
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
        private byte _health = 5;

        // Events
        public event Action DieEvent;

        // Constructor
        public PlayerModel((int, int) position, Field field, IGame game)
        {
            Position = position;
            Field = field;
            Direction = Directions.Up;
            GGame = game;
            Field.Map[position.Item1, position.Item2].Model = this;
        }

        // Methods
        public Directions? MoveHero()
        {
            if (NextStep == null) return NextStep;
            //
            if (Direction != NextStep)
            {
                Rotate();
            }
            else
            {
                Step();
            }
            //
            Directions? _nextStepCopy = NextStep;
            NextStep = null;
            //
            return _nextStepCopy;
        } 
        private void Rotate()
        {
            Field.Map[ Position.Y, Position.X ].Rotate( NextStep, TypeOfBlock.Player );
            Direction = (Directions)NextStep;
        }
        private void Step()
        {
            (int Y, int X) = GetPosition( Position, NextStep );

            if ( Field[Y, X].Type == TypeOfBlock.EmptyCell )
            {
                Field[Y, X] = Field[ Position.Y, Position.X ];
                Field.Map[ Position.Y, Position.X ].TurnToEmpty();

                Position = (Y, X);
            }
        }
        //
        public bool Shoot() {
            //
            if (!NextShoot) return false;
            //
            (int Y, int X) = GetPosition(Position, Direction);
            //
            switch (Field[Y, X].Type)
            {
                case TypeOfBlock.EmptyCell:
                    Field[Y, X] = new Block( TypeOfBlock.Bullet, model: null, direction: Direction );
                    GGame.Bullets.Add( 
                        new Bullet(
                            position: (Y, X),
                            field: Field,
                            direction: Direction,
                            game: GGame) 
                        );
                    break;
                //
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
                    // never change this line !!!!!!!!!!!!!
                    break;
            }
            // 
            NextShoot = false;
            return true;
        }
        //
        public override void Die()
        {
            Field.Map[Position.Y, Position.X].TurnToEmpty();
            DieEvent();
        }
    }
}
