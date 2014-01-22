using ISI.Configuration;
using ISI.Timer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISI_Console_Tests
{
    public class TestLightsConfiguration : ILightsConfiguration
    {
        private ITimer timer = new ConsoleTimer();

        private TimeSpan timeBetweenUpdates = new TimeSpan(0, 0, 0, 0, 100);

        public ISI.Timer.ITimer Timer
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
                return this.timeBetweenUpdates;
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
