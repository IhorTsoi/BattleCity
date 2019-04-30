using System;
using System.Collections.Generic;
using System.Timers;

namespace BattleCity.Model.Game.Components
{
    class GameTimer
    {
        // timer instance
        private Timer Timer { get; set; }
        // timer properties
        private int Interval { get; set; }
        // timer callback
        private ElapsedEventHandler Elapsed { get; set; }

        // constructor
        public GameTimer(ElapsedEventHandler loop_function, int interval = 180)
        {
            Elapsed = loop_function;
            Interval = interval;
        }

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
