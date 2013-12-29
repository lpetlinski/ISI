using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISI.Model
{
    /// <summary>
    /// Class describing edge in graph.
    /// </summary>
    public class Edge
    {
        /// <summary>
        /// Number of edge - working as id. Unique within graph.
        /// </summary>
        public int Number
        {
            get;
            set;
        }

        /// <summary>
        /// Fist node.
        /// </summary>
        public Node StartNode
        {
            get;
            set;
        }

        /// <summary>
        /// Second node.
        /// </summary>
        public Node EndNode
        {
            get;
            set;
        }

        /// <summary>
        /// Returns direction vercotr of edge from given node. Notice, that this node should be one of edge nodes.
        /// </summary>
        /// <param name="node">Start node to get direction vector from.</param>
        /// <returns>The direction normalized vector starting in given node.</returns>
        public Vector GetDirectionFromNode(Node node)
        {
            if (node != StartNode && node != EndNode)
            {
                throw new ArgumentException("Node should belong to edge!");
            }
            var anotherNode = StartNode == node ? EndNode : StartNode;

            var result = new Vector
            {
                X = anotherNode.Position.X - node.Position.X,
                Y = anotherNode.Position.Y - node.Position.Y
            };

            result.Normalize();

            return result;
        }

        public Node GetAnotherNode(Node node)
        {
            if (node != StartNode && node != EndNode)
            {
                throw new ArgumentException("Node should belong to edge!");
            }

            return StartNode == node ? EndNode : StartNode;
        }
    }
}
