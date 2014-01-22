using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISI.Timer
{
    public class SimulationTimer : ITimer
    {
        public DateTime GetActualTime()
        {
            return DateTime.Now;
        }
    }
}
