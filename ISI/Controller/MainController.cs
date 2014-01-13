using ISI.Model;
using ISI.Model.Acceleration;
using ISI.Utils;
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

        /// <summary>
        /// Creates new controller.
        /// </summary>
        /// <param name="Viewport">Canvas to draw into.</param>
        public MainController(Canvas Viewport)
        {
            this.Viewport = Viewport;

            this.CityMap = new CityMap();
            foreach (var building in this.CityMap.Buildings)
            {
                Viewport.Children.Add(building);
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

            this.carsController = new CarsController(this.CityMap, this.Viewport);
            
        }

        /// <summary>
        /// Updates simulation.
        /// </summary>
        public void Update()
        {
            this.carsController.UpdateCars();
        }
    }
}
