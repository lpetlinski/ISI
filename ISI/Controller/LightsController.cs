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

        private DateTime lastTimeUpdate = DateTime.Now;

        private static readonly TimeSpan lightTimeUpdateInterval = new TimeSpan(0, 0, 1);

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
                else if (now - light.LastChangeDate >= light.StateTime[light.State])
                {
                    light.LastChangeDate = now;
                    light.State = StateMachine[light.State];
                }
            }

            if (DateTime.Now - lastTimeUpdate > lightTimeUpdateInterval)
            {
                lastTimeUpdate = DateTime.Now;
                this.UpdateLightTimes();
            }
        }

        private void UpdateLightTimes()
        {
            // TODO create some algorithm here
            foreach (var node in this.cityMap.CityGraph.Nodes)
            {
                this.UpdateLightTime(node, 100, true, true);
            }
        }

        /// <summary>
        /// Updates times of lights which are connected to given node. It also updates opposite lights (ex. if you add 500 ms to horizontal green lights, it also updates vertical red lights by the same ammount)
        /// </summary>
        /// <param name="node">Node to update lights on</param>
        /// <param name="timeToAdd">Time in ms to add (an be negative)</param>
        /// <param name="green">True if you want to update green light time, red otherwise</param>
        /// <param name="horizontal">True if you want to update horizontal light time, vertical otherwise.</param>
        private void UpdateLightTime(Node node, int timeToAdd, bool green, bool horizontal){
            IList<Light> horizontalLights;
            IList<Light> verticalLights;

            this.GetVerticalAndHorizontalLightsFromNode(node, out horizontalLights, out verticalLights);

            IList<Light> lightsToChange;
            IList<Light> otherLights;

            LightState stateToChange;
            LightState otherState;

            if (horizontal)
            {
                lightsToChange = horizontalLights;
                otherLights = verticalLights;
            }
            else
            {
                lightsToChange = verticalLights;
                otherLights = horizontalLights;
            }

            if (green)
            {
                stateToChange = LightState.Green;
                otherState = LightState.Red;
            }
            else
            {
                stateToChange = LightState.Red;
                otherState = LightState.Green;
            }

            foreach (var light in lightsToChange)
            {
                light.StateTime[stateToChange] = light.StateTime[stateToChange].Add(new TimeSpan(0, 0, 0, 0, timeToAdd));
            }

            foreach (var light in otherLights)
            {
                light.StateTime[otherState] = light.StateTime[otherState].Add(new TimeSpan(0, 0, 0, 0, timeToAdd));
            }
        }

        private void AddTime(Light light, LightState state, int timeToAdd)
        {
            if (timeToAdd >= 0)
            {
                light.StateTime[state] = light.StateTime[state].Add(new TimeSpan(0, 0, 0, 0, timeToAdd));
            }
            else
            {
                light.StateTime[state] = light.StateTime[state].Subtract(new TimeSpan(0, 0, 0, 0, -timeToAdd));
            }
        }

        private void GetVerticalAndHorizontalLightsFromNode(Node node, out IList<Light> horizontalLights, out IList<Light> verticalLights)
        {
            var lights = this.cityMap.Lights.Where<Light>(l => l.NodeWithLight == node).ToList();

            var tmp = horizontalLights = lights.Where<Light>(l => Math.Abs(l.EdgeWithLight.GetDirectionFromNode(l.NodeWithLight).Y) < 0.1).ToList();
            verticalLights = lights.Where<Light>(l => !tmp.Contains(l)).ToList();
        }
    }
}
