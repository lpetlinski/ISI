using ISI.Configuration;
using ISI.Model;
using ISI.Model.Acceleration;
using ISI.Timer;
using ISI.Utils;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ISI.Controller
{
    /// <summary>
    /// Main simulator controller.
    /// </summary>
    public class MainController
    {
        /// <summary>
        /// City map.
        /// </summary>
        private CityMap CityMap;

        /// <summary>
        /// Canvas in which simulation is drawed.
        /// </summary>
        private Canvas Viewport;

        private CarsController carsController;

        private LightsController lightsController;

        private Action SimulationFinishedAction;

        public bool Finished
        {
            get;
            private set;
        }

        public bool Jam
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates new controller.
        /// </summary>
        /// <param name="Viewport">Canvas to draw into.</param>
        /// <param name="SimulationFinishedAction">Action to perform after simulation is finished</param>
        /// <param name="configuration">Configuration to be used in lights controller</param>
        public MainController(Canvas Viewport, Action SimulationFinishedAction, ILightsConfiguration configuration)
        {
            this.Finished = false;
            this.Jam = false;
            this.Viewport = Viewport;
            this.SimulationFinishedAction = SimulationFinishedAction;

            this.CityMap = new CityMap(Viewport != null);

            if (Viewport != null)
            {
                foreach (var building in this.CityMap.Buildings)
                {
                    Viewport.Children.Add(building.Rect);
                }

                // Uncomment this to show nodes.
                var color = new SolidColorBrush(Colors.Green);
                foreach (var node in this.CityMap.CityGraph.Nodes)
                {
                    var rect = new Rectangle();
                    rect.Width = Node.NodeSize;
                    rect.Height = Node.NodeSize;
                    Canvas.SetLeft(rect, node.Position.X);
                    Canvas.SetTop(rect, node.Position.Y);
                    rect.Fill = color;
                    Viewport.Children.Add(rect);
                }

                foreach (var light in this.CityMap.Lights)
                {
                    Viewport.Children.Add(light.Rect);
                }
            }

            this.carsController = new CarsController(this.CityMap, this.Viewport);
            this.lightsController = new LightsController(this.CityMap, carsController.Cars, configuration);
        }

        /// <summary>
        /// Updates simulation.
        /// </summary>
        public void Update()
        {
            this.lightsController.Update();
            this.carsController.UpdateCars();

            if (this.carsController.AllFinished() || this.carsController.IsJam())
            {
                this.Finished = true;
                this.Jam = this.carsController.IsJam();
                this.SimulationFinishedAction.Invoke();
            }
        }

        public IList<QueueElement> GetQueuedCarsInTimeIntervals()
        {
            return this.lightsController.CarsInQueue;
        }
    }
}
