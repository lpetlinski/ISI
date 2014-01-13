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
        private const double LightFromBorder = 5;

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
        public IList<Building> Buildings
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
            this.Buildings = new List<Building>();
            this.CityGraph = new CityGraph();
            CreateLights();
            CreateBuildings();
        }

        /// <summary>
        /// Create lights.
        /// </summary>
        private void CreateLights()
        {
            foreach (var node in this.CityGraph.Nodes)
            {
                if(node.Edges.Count > 1){
                    foreach (var edge in node.Edges)
                    {
                        var rect = new Rectangle();
                        var light = new Light
                        {
                            NodeWithLight = node,
                            EdgeWithLight = edge,
                            Rect = rect
                        };

                        rect.Width = Light.LightSize;
                        rect.Height = Light.LightSize;

                        var vector = edge.GetDirectionFromNode(node);

                        if (vector.X > 0.9)
                        {
                            // right
                            Canvas.SetLeft(rect, node.Position.X + Node.NodeSize + LightFromBorder);
                            Canvas.SetTop(rect, node.Position.Y - LightFromBorder - Light.LightSize);
                            light.State = LightState.Green;
                        }
                        else if (vector.X < -0.9)
                        {
                            // left
                            Canvas.SetLeft(rect, node.Position.X - LightFromBorder - Light.LightSize);
                            Canvas.SetTop(rect, node.Position.Y + Node.NodeSize + LightFromBorder);
                            light.State = LightState.Green;
                        }
                        else if (vector.Y > 0.9)
                        {
                            // bottom
                            Canvas.SetLeft(rect, node.Position.X + Node.NodeSize + LightFromBorder);
                            Canvas.SetTop(rect, node.Position.Y + Node.NodeSize + LightFromBorder);
                            light.State = LightState.Red;
                        }
                        else
                        {
                            // top
                            Canvas.SetLeft(rect, node.Position.X - LightFromBorder - Light.LightSize);
                            Canvas.SetTop(rect, node.Position.Y - LightFromBorder - Light.LightSize);
                            light.State = LightState.Red;
                        }
                        this.Lights.Add(light);
                    }
                }
            }
        }

        /// <summary>
        /// Creates buildings based on BuildingsPattern.
        /// </summary>
        private void CreateBuildings()
        {
            var color = new SolidColorBrush(Colors.Gray);
            foreach (var pattern in CityMapPattern.BuildingsPattern)
            {
                var buildingRect = new Rectangle();
                buildingRect.Height = pattern.Height;
                buildingRect.Width = pattern.Width;
                buildingRect.Fill = color;
                Canvas.SetLeft(buildingRect, pattern.Position.X);
                Canvas.SetTop(buildingRect, pattern.Position.Y);

                var building = new Building
                {
                    Height = pattern.Height,
                    Width = pattern.Width,
                    Position = pattern.Position,
                    Rect = buildingRect
                };

                this.Buildings.Add(building);
            }
        }
    }
}
