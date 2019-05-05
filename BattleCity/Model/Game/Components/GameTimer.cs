using System;
using System.Collections.Generic;
using System.Timers;

namespace BattleCity.Model.Game.Components
{
    class GameTimer
    {
        // Timer instance :
        private Timer Timer { get; set; }

        // Timer properties :
        private int Interval { get; set; }
        private ElapsedEventHandler Elapsed { get; set; }


        // Constructor :
        public GameTimer(ElapsedEventHandler loop_function, int interval = 180)
        {
            Elapsed = loop_function;
            Interval = interval;
        }


        // Methods :
        public void StartTimer()
        {
            Timer = new Timer(interval: Interval);
            Timer.Elapsed += Elapsed;
            Timer.AutoReset = true;
            Timer.Enabled = true;
        }

        public void StopTimer()
        {
            Timer.Stop();
            Timer.Dispose();
        }
    }
}
