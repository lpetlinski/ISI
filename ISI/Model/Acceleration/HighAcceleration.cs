using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ISI.Model.Acceleration
{
    public class HighAcceleration : IAccelerationPolicy
    {
        public double MaxSpeed
        {
            get
            {
                return 2;
            }
        }

        public double AccelerationSpeed
        {
            get
            {
                return 0.2;
            }
        }


        public System.Windows.Media.Color CarColor
        {
            get
            {
                return Colors.Cyan;
            }
        }
    }
}
