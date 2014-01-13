using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISI.Model
{
    /// <summary>
    /// Class describing vector in 2 dimensional space
    /// </summary>
    public class Vector
    {
        /// <summary>
        /// X dimension of vector.
        /// </summary>
        public double X;

        /// <summary>
        /// Y dimension of vector.
        /// </summary>
        public double Y;

        public void Normalize()
        {
            var length = this.Length();
            this.X /= length;
            this.Y /= length;
        }

        public double Length()
        {
            return Math.Sqrt(Math.Pow(this.X, 2) + Math.Pow(this.Y, 2));
        }

        public void RotateLeft()
        {
            var tmp = this.X;
            this.X = this.Y;
            this.Y = -tmp;
        }
    }
}
