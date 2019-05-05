using System;
using System.Collections.Generic;
using System.Text;

namespace BattleCity.Model.Game.Components
{
    struct GameState
    {
        // Properties:
        private bool State;
        private bool Frozen;


        // Getters:
        public bool GetState() => State;
        public bool Died() => Frozen;
        
        
        // Methods:
        public void Win() => State = !Frozen;
        public void Lose()
        {
            State = false;
            Frozen = true;
        }
    }
}
