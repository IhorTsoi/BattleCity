using System;
using System.Collections.Generic;
using System.Timers;

namespace BattleCity.Model.Game.Components
{
    class GameTimer
    {
        // Timer instance :
        private Timer Timer { get; set; }


        // Constructor :
        public GameTimer(ElapsedEventHandler loop_function, int interval = 180, bool autoReset = true)
        {
            Timer = new Timer(interval: interval);
            Timer.Elapsed += loop_function;
            Timer.AutoReset = autoReset;
        }


        // Methods :
        public void StartTimer()
        {
            Timer.Enabled = true;
        }

        public void StopTimer()
        {
            Timer.Stop();
            Timer.Dispose();
        }
    }
}
