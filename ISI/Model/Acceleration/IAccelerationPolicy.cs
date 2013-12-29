using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ISI.Model.Acceleration
{
    public interface IAccelerationPolicy
    {
        double MaxSpeed
        {
            get;
        }

        double AccelerationSpeed
        {
            get;
        }

        Color CarColor
        {
            get;
        }
    }
}
