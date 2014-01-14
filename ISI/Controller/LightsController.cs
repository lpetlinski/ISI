using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Model;

namespace ISI.Controller
{
    public class LightsController
    {
        private Dictionary<LightState, LightState> StateMachine = new Dictionary<LightState, LightState>
        {
            {LightState.Green, LightState.Yellow},
            {LightState.Yellow, LightState.Red},
            {LightState.Red, LightState.YellowStart},
            {LightState.YellowStart, LightState.Green}
        };

        private Dictionary<LightState, TimeSpan> StateTime = new Dictionary<LightState, TimeSpan>
        {
            {LightState.Green, new TimeSpan(0, 0, 0, 5)},
            {LightState.Yellow, new TimeSpan(0, 0, 0, 1)},
            {LightState.Red, new TimeSpan(0, 0, 0, 5)},
            {LightState.YellowStart, new TimeSpan(0, 0, 0, 1)}
        };

        private CityMap cityMap;

        public LightsController(CityMap map)
        {
            this.cityMap = map;
        }

        public void Update()
        {
            var now = DateTime.Now;
            foreach (var light in cityMap.Lights)
            {
                if (light.LastChangeDate == null)
                {
                    light.LastChangeDate = now;
                }
                else if (now - light.LastChangeDate >= StateTime[light.State])
                {
                    light.LastChangeDate = now;
                    light.State = StateMachine[light.State];
                }
            }
        }
    }
}
