using ISI.Timer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISI.Configuration
{
    public class DefaultLightsConfiguration : ILightsConfiguration
    {
        private Timer.ITimer timer = new SimulationTimer();

        private TimeSpan timeBetweenUpdates = new TimeSpan(0, 0, 0, 0, 100);

        public Timer.ITimer Timer
        {
            get
            {
                return this.timer;
            }
        }

        public TimeSpan TimeBetweenLightsUpdates
        {
            get
            {
                return timeBetweenUpdates;
            }
        }

        public bool UseAlgorithm
        {
            get
            {
                return true;
            }
        }

        public int DeltaTimeUpdate
        {
            get
            {
                return 400;
            }
        }
    }
}
