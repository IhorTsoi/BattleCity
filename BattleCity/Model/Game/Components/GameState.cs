using System;
using System.Collections.Generic;
using System.Text;

namespace BattleCity.Model.Game.Components
{
    struct GameState
    {
        private bool State;
        private bool Frozen;
        //
        public void Win() => State = !Frozen;
        //
        public void Lose()
        {
            State = false;
            Frozen = true;
        }
        //
        public bool GetState() => State;
        //
        public bool Died() => Frozen;
    }
}
