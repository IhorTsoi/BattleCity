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
        public Directions? NextStep { get; set; }
        public bool NextShoot { get; set; } = false;


        // Events
        public event Action DieEvent;


        // Constructor
        public PlayerModel((int Y, int X) position, Field field, IGame game)
            :base(health: 8, position, field)
        {
            Game = game;
            Field.Map[position.Y, position.X].Model = this;
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
            Field.RotateBlock(Position, (Directions)NextStep);
            Direction = (Directions)NextStep;
        }
        private void Step()
        {
            (int Y, int X) = GetPosition( Position, NextStep );

            if ( Field[Y, X].Type == TypeOfBlock.EmptyCell )
            {
                Field.MoveBlock(
                    movingObj: this,
                    toCoords: (Y, X),
                    Direction);
                Position = (Y, X);
            }
        }
        //
        public bool Shoot() {
            if (!NextShoot)
                return false;
            //
            (int Y, int X) nextPosition = GetPosition(Position, Direction);
            
            Bullet.CreateBullet(nextPosition, Direction, damageVal: 1, Game);
            // 
            NextShoot = false;
            return true;
        }
        //
        public override void Die()
        {
            Field.DeleteBlock(Position);
            DieEvent();
        }
    }
}
