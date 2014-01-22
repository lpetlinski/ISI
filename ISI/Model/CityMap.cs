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
        public CityMap(bool createRectangles)
        {
            this.Lights = new List<Light>();
            this.Buildings = new List<Building>();
            this.CityGraph = new CityGraph();
            CreateLights(createRectangles);
            CreateBuildings(createRectangles);
        }

        /// <summary>
        /// Create lights.
        /// </summary>
        private void CreateLights(bool createRectangles)
        {
            foreach (var node in this.CityGraph.Nodes)
            {
                if (node.Edges.Count > 1)
                {
                    foreach (var edge in node.Edges)
                    {
                        var light = new Light
                        {
                            NodeWithLight = node,
                            EdgeWithLight = edge,
                        };

                        if (createRectangles)
                        {
                            var rect = new Rectangle();
                            rect.Width = Light.LightSize;
                            rect.Height = Light.LightSize;

                            light.Rect = rect;

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
                        }
                        this.Lights.Add(light);
                    }
                }
            }
        }

        /// <summary>
        /// Creates buildings based on BuildingsPattern.
        /// </summary>
        private void CreateBuildings(bool createRectangles)
        {
            var color = new SolidColorBrush(Colors.Gray);
            foreach (var pattern in CityMapPattern.BuildingsPattern)
            {
                var building = new Building
                {
                    Height = pattern.Height,
                    Width = pattern.Width,
                    Position = pattern.Position
                };

                if (createRectangles)
                {
                    var buildingRect = new Rectangle();
                    building.Rect = buildingRect;
                    buildingRect.Height = pattern.Height;
                    buildingRect.Width = pattern.Width;
                    buildingRect.Fill = color;
                    Canvas.SetLeft(buildingRect, pattern.Position.X);
                    Canvas.SetTop(buildingRect, pattern.Position.Y);
                }

                this.Buildings.Add(building);
            }
        }
    }
}
