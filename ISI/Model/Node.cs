using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISI.Model
{
    /// <summary>
    /// Class describing Node in graph.
    /// </summary>
    public class Node
    {
        /// <summary>
        /// Size of node.
        /// </summary>
        public static readonly double NodeSize = 30;

        /// <summary>
        /// Number of node - working as id. Unique within graph.
        /// </summary>
        public int Number
        {
            get;
            set;
        }

        /// <summary>
        /// List of edges coming out from given node.
        /// </summary>
        public IList<Edge> Edges
        {
            get;
            set;
        }

        /// <summary>
        /// Position of node on map.
        /// </summary>
        public Vector Position
        {
            get;
            set;
        }

        /// <summary>
        /// Indicates whether node is border node - on map edge.
        /// </summary>
        public bool BorderNode
        {
            get;
            set;
        }

        /// <summary>
        /// Creates new node.
        /// </summary>
        public Node()
        {
            this.Edges = new List<Edge>();
        }
    }
}
