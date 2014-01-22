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
            new Building(128, 128, 0, 0),
            new Building(128, 128, 0, 158),
            new Building(30, 126, 0, 285),
            new Building(127, 128, 0, 314),
            new Building(128, 128, 0, 472),
            new Building(128, 127, 158, 0),
            new Building(126, 30, 285, 0),
            new Building(128, 127, 315, 0),
            new Building(127, 127, 158, 158),
            new Building(127, 127, 158, 315),
            new Building(128, 127, 158, 472),
            new Building(126, 30, 285, 474),
            new Building(128, 127, 315, 472),
            new Building(127, 127, 315, 158),
            new Building(127, 127, 315, 315),
            new Building(128, 128, 472, 0),
            new Building(128, 128, 472, 158),
            new Building(30, 126, 474, 285),
            new Building(127, 128, 472, 314),
            new Building(128, 128, 472, 472),
        };

        /*
         * Graph nodes numbers:
         *     1       2
         *     |       |
         * 3---4---5---6---7
         *     |   |   |
         *     8---9---10
         *     |   |   |
         * 11--12--13--14--15
         *     |       |
         *    16       17
         */

        /// <summary>
        /// Pattern to build graph from. This is a list of edges between nodes.
        /// </summary>
        public static readonly IList<EdgePattern> GraphPattern = new List<EdgePattern>
        {
            new EdgePattern(1,4),
            new EdgePattern(2,6),
            new EdgePattern(3,4),
            new EdgePattern(4,5),
            new EdgePattern(5,6),
            new EdgePattern(6,7),
            new EdgePattern(4,8),
            new EdgePattern(5,9),
            new EdgePattern(6,10),
            new EdgePattern(8,9),
            new EdgePattern(9,10),
            new EdgePattern(8,12),
            new EdgePattern(9,13),
            new EdgePattern(10,14),
            new EdgePattern(11,12),
            new EdgePattern(12,13),
            new EdgePattern(13,14),
            new EdgePattern(14,15),
            new EdgePattern(12,16),
            new EdgePattern(14,17),
        };

        /// <summary>
        /// Pattern to set node position on map.
        /// </summary>
        public static readonly IList<NodePattern> NodePositionPattern = new List<NodePattern>
        {
            new NodePattern(1, 128, 0),
            new NodePattern(2, 442, 0),
            new NodePattern(3, 0, 128),
            new NodePattern(4, 128, 128),
            new NodePattern(5, 285, 128),
            new NodePattern(6, 442, 128),
            new NodePattern(7, 570, 128),
            new NodePattern(8, 128, 285),
            new NodePattern(9, 285, 285),
            new NodePattern(10, 442, 285),
            new NodePattern(11, 0, 442),
            new NodePattern(12, 128, 442),
            new NodePattern(13, 285, 442),
            new NodePattern(14, 442, 442),
            new NodePattern(15, 570, 442),
            new NodePattern(16, 128, 570),
            new NodePattern(17, 442, 570)
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
