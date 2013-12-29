using ISI.Model.Patterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ISI.Model
{
    /// <summary>
    /// Class describing map of city, where car moves.
    /// </summary>
    public class CityMap
    {
        /// <summary>
        /// List of lights.
        /// </summary>
        public IList<Light> Lights
        {
            get;
            private set;
        }

        /// <summary>
        /// List of buildings (generally they are only gray rectangles)
        /// </summary>
        public IList<Rectangle> Buildings
        {
            get;
            private set;
        }

        /// <summary>
        /// Graph of roads, and crossroads.
        /// </summary>
        public CityGraph CityGraph
        {
            get;
            set;
        }

        /// <summary>
        /// Creates new city map.
        /// </summary>
        public CityMap()
        {
            this.Lights = new List<Light>();
            this.Buildings = new List<Rectangle>();
            CreateLights();
            CreateBuildings();
            this.CityGraph = new CityGraph();
        }

        /// <summary>
        /// Create lights.
        /// </summary>
        private void CreateLights()
        {
        }

        /// <summary>
        /// Creates buildings based on BuildingsPattern.
        /// </summary>
        private void CreateBuildings()
        {
            var color = new SolidColorBrush(Colors.Gray);
            foreach (var pattern in CityMapPattern.BuildingsPattern)
            {
                var building = new Rectangle();
                building.Height = pattern.Height;
                building.Width = pattern.Width;
                building.Fill = color;
                Canvas.SetLeft(building, pattern.Position.X);
                Canvas.SetTop(building, pattern.Position.Y);
                this.Buildings.Add(building);
            }
        }
    }
}
