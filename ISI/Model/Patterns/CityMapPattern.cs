using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISI.Model.Patterns
{
    public static class CityMapPattern
    {
        /// <summary>
        /// Pattern from which buildings are created.
        /// </summary>
        public static IList<Building> BuildingsPattern = new List<Building>
        {
            new Building(185, 185, 0, 0),
            new Building(185, 170, 215, 0),
            new Building(185, 185, 415, 0),
            new Building(170, 185, 0, 215),
            new Building(170, 170, 215, 215),
            new Building(170, 185, 415, 215),
            new Building(185, 185, 0, 415),
            new Building(185, 170, 215, 415),
            new Building(185, 185, 415, 415)
        };

        /*
         * Graph nodes numbers:
         *    1  2
         *    |  |
         * 3--4--5--6
         *    |  |
         * 7--8--9--10
         *    |  |
         *   11  12
         */

        /// <summary>
        /// Pattern to build graph from. This is a list of edges between nodes.
        /// </summary>
        public static readonly IList<EdgePattern> GraphPattern = new List<EdgePattern>
        {
            new EdgePattern(1,4),
            new EdgePattern(2,5),
            new EdgePattern(3,4),
            new EdgePattern(4,5),
            new EdgePattern(5,6),
            new EdgePattern(4,8),
            new EdgePattern(5,9),
            new EdgePattern(7,8),
            new EdgePattern(8,9),
            new EdgePattern(9,10),
            new EdgePattern(8,11),
            new EdgePattern(9,12)
        };

        /// <summary>
        /// Pattern to set node position on map.
        /// </summary>
        public static readonly IList<NodePattern> NodePositionPattern = new List<NodePattern>
        {
            new NodePattern(1, 185, 0),
            new NodePattern(2, 385, 0),
            new NodePattern(3, 0, 185),
            new NodePattern(4, 185, 185),
            new NodePattern(5, 385, 185),
            new NodePattern(6, 570, 185),
            new NodePattern(7, 0, 385),
            new NodePattern(8, 185, 385),
            new NodePattern(9, 385, 385),
            new NodePattern(10, 570, 385),
            new NodePattern(11, 185, 570),
            new NodePattern(12, 385, 570)
        };

        /// <summary>
        /// Helper class desribing building patterns.
        /// </summary>
        public class Building
        {
            /// <summary>
            /// Left top corner position.
            /// </summary>
            public Vector Position
            {
                get;
                set;
            }

            /// <summary>
            /// Width of building.
            /// </summary>
            public double Width
            {
                get;
                set;
            }

            /// <summary>
            /// Height of building.
            /// </summary>
            public double Height
            {
                get;
                set;
            }

            /// <summary>
            /// Creates new building.
            /// </summary>
            /// <param name="height">Height of building</param>
            /// <param name="width">Width of building</param>
            /// <param name="x">X dimension of position.</param>
            /// <param name="y">Y dimension of position.</param>
            public Building(double height, double width, double x, double y)
            {
                this.Position = new Vector
                {
                    X = x,
                    Y = y
                };
                this.Width = width;
                this.Height = height;
            }
        }

        /// <summary>
        /// Helper class for creating pattern.
        /// </summary>
        public class EdgePattern
        {
            /// <summary>
            /// Key of first node in edge.
            /// </summary>
            public int OneNode
            {
                get;
                set;
            }

            /// <summary>
            /// Key of second node in edge.
            /// </summary>
            public int SecondNode
            {
                get;
                set;
            }

            /// <summary>
            /// Creates new helper object.
            /// </summary>
            /// <param name="oneNode">Key of first node in edge.</param>
            /// <param name="secondNode">Key of second node in edge.</param>
            public EdgePattern(int oneNode, int secondNode)
            {
                this.OneNode = oneNode;
                this.SecondNode = secondNode;
            }
        }

        public class NodePattern
        {
            public int Number
            {
                get;
                set;
            }

            public Vector Position
            {
                get;
                set;
            }

            public NodePattern(int number, double x, double y)
            {
                this.Number = number;
                this.Position = new Vector
                {
                    X = x,
                    Y = y
                };
            }
        }
    }
}
