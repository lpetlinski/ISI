using ISI.Model.Patterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISI.Model
{
    /// <summary>
    /// Describes city as graph. Crossroads and end of roads in map edge are nodes, and reads between them are edges.
    /// </summary>
    public class CityGraph
    {
        /// <summary>
        /// Height of map.
        /// </summary>
        public static readonly double MapHeight = 600;

        /// <summary>
        /// Width of map.
        /// </summary>
        public static readonly double MapWidth = 600;

        /// <summary>
        /// List of edges - roads between nodes.
        /// </summary>
        public IList<Edge> Edges
        {
            get;
            private set;
        }

        /// <summary>
        /// List of nodes (crossroads and end of roads in map edge).
        /// </summary>
        public IList<Node> Nodes
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates graph from pattern.
        /// </summary>
        public CityGraph()
        {
            this.Edges = new List<Edge>();
            this.Nodes = new List<Node>();

            this.CreateNodesAndEdges();
            this.MarkBorderNodes();
        }

        private void CreateNodesAndEdges()
        {
            var dictionary = new Dictionary<int, Node>();

            var edgeNumber = 0;

            foreach (var pattern in CityMapPattern.GraphPattern)
            {
                var oneNode = this.GetOrCreateNode(pattern.OneNode, dictionary);
                var secondNode = this.GetOrCreateNode(pattern.SecondNode, dictionary);

                var edge = new Edge
                {
                    Number = edgeNumber++,
                    StartNode = oneNode,
                    EndNode = secondNode
                };
                oneNode.Edges.Add(edge);
                secondNode.Edges.Add(edge);
                this.Edges.Add(edge);
            }

            this.AddPositionInfoToNodes(dictionary);
        }

        private void AddPositionInfoToNodes(Dictionary<int, Node> dictionary)
        {
            foreach (var nodePosition in CityMapPattern.NodePositionPattern)
            {
                var node = dictionary[nodePosition.Number];
                node.Position = new Vector
                {
                    X = nodePosition.Position.X,
                    Y = nodePosition.Position.Y
                };
            }
        }

        private void MarkBorderNodes()
        {
            // TODO only for now border edges are those, which has only one edge
            foreach (var node in this.Nodes)
            {
                if (node.Edges.Count == 1)
                {
                    node.BorderNode = true;
                }
            }
        }

        /// <summary>
        /// Gets node with number key from given dictionary, or creates new if there isn't.
        /// </summary>
        /// <param name="key">Number of node to get from dictionary or create.</param>
        /// <param name="dictionary">Helper dictionary of nodes.</param>
        /// <returns>Node with given key.</returns>
        private Node GetOrCreateNode(int key, Dictionary<int, Node> dictionary)
        {
            Node node;
            if (!dictionary.TryGetValue(key, out node))
            {
                node = new Node
                {
                    Number = key
                };
                this.Nodes.Add(node);
                dictionary.Add(key, node);
            }
            return node;
        }
    }
}
