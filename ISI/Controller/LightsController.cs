using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Model;
using ISI.Timer;
using ISI.Configuration;

namespace ISI.Controller
{
    public struct QueueElement
    {
        public int CarsInQueue
        {
            get;
            set;
        }

        public int CarsInSimulation
        {
            get;
            set;
        }
    }

    public class LightsController
    {
        public List<QueueElement> CarsInQueue
        {
            get;
            private set;
        }

        private Dictionary<LightState, LightState> StateMachine = new Dictionary<LightState, LightState>
        {
            {LightState.Green, LightState.Yellow},
            {LightState.Yellow, LightState.Red},
            {LightState.Red, LightState.YellowStart},
            {LightState.YellowStart, LightState.Green}
        };

        private DateTime lastTimeUpdate;

        private CityMap cityMap;

        private IList<Car> cars;

        private ILightsConfiguration configuration;

        public LightsController(CityMap map, IList<Car> cars, ILightsConfiguration configuration)
        {
            this.cityMap = map;
            this.cars = cars;
            this.CarsInQueue = new List<QueueElement>();
            this.configuration = configuration;
            lastTimeUpdate = this.configuration.Timer.GetActualTime();
        }

        public void Update()
        {
            var now = this.configuration.Timer.GetActualTime();
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

            if (this.configuration.UseAlgorithm && now - lastTimeUpdate > this.configuration.TimeBetweenLightsUpdates)
            {
                lastTimeUpdate = now;
                this.UpdateLightTimes();
            }
            else if(now - lastTimeUpdate > this.configuration.TimeBetweenLightsUpdates)
            {
                // count queue on lights if not using algorithm
                var queued = 0;
                foreach (var node in this.cityMap.CityGraph.Nodes.Where(n => this.cityMap.Lights.Any(l => l.NodeWithLight == n)))
                {
                    IList<Light> horizontalLights;
                    IList<Light> verticalLights;
                    this.GetVerticalAndHorizontalLightsFromNode(node, out horizontalLights, out verticalLights);

                    var horizontalQueue = this.GetQueueOnLights(horizontalLights);
                    var verticalQueue = this.GetQueueOnLights(verticalLights);
                    queued += horizontalQueue + verticalQueue;
                }

                this.CarsInQueue.Add(new QueueElement()
                {
                    CarsInQueue = queued,
                    CarsInSimulation = this.cars.Count(c => !c.IsFinished)
                });
            }
        }

        private void UpdateLightTimes()
        {
            var queued = 0;
            foreach (var node in this.cityMap.CityGraph.Nodes.Where(n => this.cityMap.Lights.Any(l => l.NodeWithLight == n)))
            {
                IList<Light> horizontalLights;
                IList<Light> verticalLights;

                this.GetVerticalAndHorizontalLightsFromNode(node, out horizontalLights, out verticalLights);

                var horizontalQueue = this.GetQueueOnLights(horizontalLights)+1;
                var verticalQueue = this.GetQueueOnLights(verticalLights)+1;

                var horizontalTime = horizontalLights[0].StateTime[LightState.Green].TotalMilliseconds;
                var horizontalRedTime = horizontalLights[0].StateTime[LightState.Red].TotalMilliseconds;
                var verticalTime = verticalLights[0].StateTime[LightState.Green].TotalMilliseconds;
                var verticalRedTime = verticalLights[0].StateTime[LightState.Red].TotalMilliseconds;

                double anotherTime;
                bool horizontal;

                queued += horizontalQueue + verticalQueue-2;

                if (verticalQueue / horizontalQueue < verticalTime / horizontalTime)
                {
                    anotherTime = horizontalRedTime;
                    horizontal = true;
                }
                else
                {
                    anotherTime = verticalRedTime;
                    horizontal = false;
                }

                // At least it should have 1 second.
                if (anotherTime > 1000)
                {
                    this.UpdateLightTime(this.configuration.DeltaTimeUpdate, true, horizontal, horizontalLights, verticalLights);
                    this.UpdateLightTime(-this.configuration.DeltaTimeUpdate, false, horizontal, horizontalLights, verticalLights);
                }
            }

            this.CarsInQueue.Add(new QueueElement()
            {
                CarsInQueue = queued,
                CarsInSimulation = this.cars.Count(c => !c.IsFinished)
            });
        }

        /// <summary>
        /// Updates times of lights which are connected to given node. It also updates opposite lights (ex. if you add 500 ms to horizontal green lights, it also updates vertical red lights by the same ammount)
        /// </summary>
        /// <param name="timeToAdd">Time in ms to add (an be negative)</param>
        /// <param name="green">True if you want to update green light time, red otherwise</param>
        /// <param name="horizontal">True if you want to update horizontal light time, vertical otherwise.</param>
        /// <param name="horizontalLights">List of horizontal lights</param>
        /// <param name="verticalLights">List of vertical lights</param>
        private void UpdateLightTime(int timeToAdd, bool green, bool horizontal, IList<Light> horizontalLights, IList<Light> verticalLights)
        {
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

        private int GetQueueOnLights(IList<Light> lights)
        {
            var result = 0;

            foreach (var light in lights)
            {
                // TODO sprawdzać, czy one faktycznie stoja na światłach
                result += this.cars.Count(c => c.ActualRoad == light.EdgeWithLight && c.LastNode != light.NodeWithLight && c.Speed < 0.0001);
            }

            return result;
        }
    }
}
