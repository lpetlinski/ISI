using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISI.Timer
{
    public class ConsoleTimer : ITimer
    {
        private double timeDelta;

        private DateTime startTime;

        private double timeAdded = 0;

        public ConsoleTimer(double timeDelta = 50)
        {
            startTime = DateTime.Now;
            this.timeDelta = timeDelta;
        }

        public void UpdateTime()
        {
            timeAdded += timeDelta;
        }

        public DateTime GetActualTime()
        {
            return this.startTime.AddMilliseconds(timeAdded);
        }
    }
}
