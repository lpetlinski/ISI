using ISI.Timer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISI.Configuration
{
    public interface ILightsConfiguration
    {
        /// <summary>
        /// Timer to get actual time from
        /// </summary>
        ITimer Timer
        {
            get;
        }

        /// <summary>
        /// Time between updates of lights
        /// </summary>
        TimeSpan TimeBetweenLightsUpdates
        {
            get;
        }

        /// <summary>
        /// True to use algorith to increase flow performance
        /// </summary>
        bool UseAlgorithm
        {
            get;
        }

        /// <summary>
        /// Number of miliseconds to be added in each time update - used only if algorithm is used
        /// </summary>
        int DeltaTimeUpdate
        {
            get;
        }
    }
}
