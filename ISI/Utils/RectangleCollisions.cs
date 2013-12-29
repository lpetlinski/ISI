using ISI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISI.Utils
{
    public class RectangleCollisions
    {
        /// <summary>
        /// Checks whenever two squares collides.
        /// </summary>
        /// <param name="positionFirst">Position of first square</param>
        /// <param name="sideLengthFirst">Side length of first square</param>
        /// <param name="positionSecond">Position of second square.</param>
        /// <param name="sideLengthSecond">Side length of second square</param>
        /// <returns></returns>
        public static bool CheckSquaresCollision(Vector positionFirst, double sideLengthFirst, Vector positionSecond, double sideLengthSecond)
        {
            var secondRectCorners = CreateCorners(positionSecond, sideLengthSecond);

            foreach (var corner in secondRectCorners)
            {
                if (corner.X >= positionFirst.X && corner.X <= positionFirst.X + sideLengthFirst && corner.Y >= positionFirst.Y && corner.Y <= positionFirst.Y + sideLengthFirst)
                {
                    return true;
                }
            }
            return false;
        }

        private static List<Vector> CreateCorners(Vector position, double length)
        {
            var corners = new List<Vector>();
            corners.Add(new Vector
            {
                X = position.X,
                Y = position.Y
            });
            corners.Add(new Vector
            {
                X = position.X + length,
                Y = position.Y
            });
            corners.Add(new Vector
            {
                X = position.X,
                Y = position.Y + length
            });
            corners.Add(new Vector
            {
                X = position.X + length,
                Y = position.Y + length
            });
            return corners;
        }

        public static bool CheckSquaresIncluding(Vector positionOuter, double sideLengthOuter, Vector positionInner, double sideLenghtInner)
        {
            var innerCorners = CreateCorners(positionInner, sideLenghtInner);
            foreach (var corner in innerCorners)
            {
                if (corner.X < positionOuter.X || corner.X > positionOuter.X + sideLengthOuter || corner.Y < positionOuter.Y || corner.Y > positionOuter.Y + sideLengthOuter)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
