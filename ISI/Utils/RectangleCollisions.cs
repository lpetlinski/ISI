using ISI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;

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

        /// <summary>
        /// Returns distance of which given square overlaps given rectangle.
        /// </summary>
        /// <param name="squarePosition">The position of left top square corner</param>
        /// <param name="squareLength">The lenght of square</param>
        /// <param name="building">The rectangle</param>
        /// <returns>Dostance of which given square overlaps rectancle.</returns>
        public static double GetSquareWithBuildingCollision(Vector squarePosition, double squareLength, Building building)
        {
            var innerCorners = CreateCorners(squarePosition, squareLength);

            if (CheckAtLeastTwoCornersInBuilding(innerCorners, building))
            {
                var left = building.Position.X;
                var top = building.Position.Y;
                var right = building.Position.X + building.Width;
                var bottom = building.Position.Y + building.Height;
                foreach (var corner in innerCorners)
                {
                    if (corner.X > left && corner.Y > top && corner.Y < bottom && corner.X - left < squareLength)
                    {
                        return corner.X - left;
                    }

                    if (corner.X < right && corner.Y > top && corner.Y < bottom && right - corner.X < squareLength)
                    {
                        return right - corner.X;
                    }

                    if (corner.Y > top && corner.X > left && corner.X < right && corner.Y - top < squareLength)
                    {
                        return corner.Y - top;
                    }

                    if (corner.Y < bottom && corner.X > left && corner.X < right && bottom - corner.Y < squareLength)
                    {
                        return bottom - corner.Y;
                    }
                }
            }
            return 0;
        }

        private static bool CheckAtLeastTwoCornersInBuilding(List<Vector> corners, Building building)
        {
            var left = building.Position.X;
            var top = building.Position.Y;
            var right = building.Position.X + building.Width;
            var bottom = building.Position.Y + building.Height;
            var numberOfCorners = 0;
            foreach (var corner in corners)
            {
                if (corner.X > left && corner.X < right && corner.Y > top && corner.Y < bottom)
                {
                    numberOfCorners++;
                }
            }
            return numberOfCorners >= 2;
        }
    }
}
