using ISI.Model;
using ISI.Model.Acceleration;
using ISI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace ISI.Controller
{
    public class CarsController
    {
        /// <summary>
        /// TODO Maybe do something with this.
        /// 
        /// Number of cars in simulation.
        /// </summary>
        private static readonly int CarsCount = 40;

        /// <summary>
        /// Static dictionary with acceleration policies.
        /// </summary>
        private static readonly Dictionary<int, IAccelerationPolicy> AccelerationPolicies = new Dictionary<int, IAccelerationPolicy>
        {
            {0, new SlowAcceleration()},
            {1, new HighAcceleration()}
        };

        /// <summary>
        /// List of cars.
        /// </summary>
        private IList<Car> Cars;

        /// <summary>
        /// City map.
        /// </summary>
        private CityMap CityMap;

        /// <summary>
        /// Canvas in which cars are drawed.
        /// </summary>
        private Canvas Viewport;

        /// <summary>
        /// Creates new cars controller.
        /// </summary>
        /// <param name="cityMap">City map.</param>
        /// <param name="viewport">Canvas in which cars are drawed.</param>
        public CarsController(CityMap cityMap, Canvas viewport)
        {
            this.Cars = new List<Car>();
            this.CityMap = cityMap;
            this.Viewport = viewport;
        }

        /// <summary>
        /// Updates cars positions.
        /// </summary>
        public void UpdateCars()
        {
            foreach (var car in this.Cars.ToList())
            {
                if (car.Speed < car.AccelerationPolicy.MaxSpeed)
                {
                    car.Speed += car.AccelerationPolicy.AccelerationSpeed;
                }

                var vector = car.ActualRoad.GetDirectionFromNode(car.LastNode);
                if (this.HasToTurnRight(car, vector) || this.HasToTurnLeft(car, vector))
                {
                    var anotherNode = car.ActualRoad.GetAnotherNode(car.LastNode);
                    car.LastNode = anotherNode;
                    car.ActualRoad = car.NextRoad;
                    car.NextRoad = this.FindNextRoad(car);
                    vector = car.ActualRoad.GetDirectionFromNode(car.LastNode);
                }
                car.MoveBy(vector.X * car.Speed, vector.Y * car.Speed);

                if (this.CheckCarCollision(car) || this.CheckCarLight(car))
                {
                    car.MoveBy(-vector.X * car.Speed, -vector.Y * car.Speed);
                    car.Speed = 0;
                }

                this.RepairCarPosition(car);
            }

            if (!this.Viewport.Dispatcher.HasShutdownStarted && this.Cars.Count < CarsCount)
            {
                try
                {
                    this.Viewport.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                    {
                        this.AddCars();
                    }));
                }
                catch (TaskCanceledException)
                {
                    // Do nothing.
                }
            }
        }

        /// <summary>
        /// Checks whenever given car collides with another cars.
        /// </summary>
        /// <param name="car">Car to check collision with another cars.</param>
        /// <returns>True if car collides with another cars.</returns>
        private bool CheckCarCollision(Car car)
        {
            foreach (var anotherCar in this.Cars.ToList())
            {
                if (anotherCar != car && car.CheckCollision(anotherCar))
                {
                    return true;
                }
            }
            return false;
        }

        private bool CheckCarLight(Car car)
        {
            var node = car.ActualRoad.GetAnotherNode(car.LastNode);
            if (RectangleCollisions.CheckSquaresCollision(node.Position, Node.NodeSize, car.Position, Car.CarLength))
            {
                var light = this.CityMap.Lights.FirstOrDefault<Light>(l => l.NodeWithLight == node && l.EdgeWithLight == car.ActualRoad);
                if (light != null)
                {
                    return light.Stop;
                }
            }
            return false;
        }

        /// <summary>
        /// Adds cars if there are less than should be.
        /// </summary>
        private void AddCars()
        {
            if (this.Cars.Count < CarsCount)
            {
                var rand = new Random();
                var destinationNodes = this.CityMap.CityGraph.Nodes.Where(n => n.BorderNode).ToList();
                for (int i = 0; i < CarsCount - this.Cars.Count; i++)
                {
                    var emptyStartPoints = this.GetEmptyStartPoints(destinationNodes);
                    if (emptyStartPoints.Count == 0)
                    {
                        // Stop adding, when there are no statring nodes left without a car.
                        break;
                    }
                    var car = new Car(AccelerationPolicies[rand.Next(0, AccelerationPolicies.Keys.Count)]);

                    var start = emptyStartPoints[rand.Next(0, emptyStartPoints.Count)];

                    // Start cannot be same as destination point.
                    var tmpDestination = destinationNodes.ToList();
                    tmpDestination.Remove(start);
                    var destination = tmpDestination[rand.Next(0, tmpDestination.Count)];

                    car.Destination = destination;
                    car.LastNode = start;
                    car.ActualRoad = start.Edges[0];
                    car.NextRoad = this.FindNextRoad(car);

                    var startPosition = this.ComputeStartPosition(start);
                    car.MoveTo(startPosition.X, startPosition.Y);

                    Cars.Add(car);
                    Viewport.Children.Add(car.DrawingRect);
                }
            }
        }

        private Vector ComputeStartPosition(Node startNode)
        {
            var result = new Vector
            {
                X = startNode.Position.X,
                Y = startNode.Position.Y
            };
            var edge = startNode.Edges[0];
            var direction = edge.GetDirectionFromNode(startNode);

            // those weird comparisions are because of operation on doubles,
            // Which means, that there is no guarantee, that property will equal to 1 :(
            if (direction.X > direction.Y && direction.X > 0.9)
            {
                // left side
                result.Y += Node.NodeSize * 2 / 3;
            }
            else if (direction.X > direction.Y && direction.Y < -0.9)
            {
                // bottom side
                result.Y += Node.NodeSize * 2 / 3;
                result.X += Node.NodeSize * 2 / 3;
            }
            else if (direction.X < direction.Y && direction.X < -0.9)
            {
                // right side
                result.X += Node.NodeSize * 2 / 3;
            }
            // top side is ok.
            return result;
        }

        /// <summary>
        /// Finds next road. If there is road from nearest node to destination, then this road will be returned. Otherwise random.
        /// </summary>
        /// <param name="car">Car to find another road for.</param>
        /// <returns>Next road.</returns>
        private Edge FindNextRoad(Car car)
        {
            var anotherNode = car.ActualRoad.GetAnotherNode(car.LastNode);
            var edges = anotherNode.Edges.ToList();
            edges.Remove(car.ActualRoad);
            if (edges.Count == 0)
            {
                return car.ActualRoad;
            }

            foreach (var edge in edges)
            {
                if (edge.StartNode == car.Destination || edge.EndNode == car.Destination)
                {
                    return edge;
                }
            }
            var rand = new Random();
            return edges[rand.Next(0, edges.Count)];
        }

        private bool HasToTurnRight(Car car, Vector actualVector)
        {
            var anotherNode = car.ActualRoad.GetAnotherNode(car.LastNode);

            if (RectangleCollisions.CheckSquaresIncluding(anotherNode.Position, Node.NodeSize, car.Position, Car.CarLength))
            {
                var nextVector = car.NextRoad.GetDirectionFromNode(anotherNode);
                if (actualVector.X * nextVector.Y - actualVector.Y * nextVector.X > 0)
                {
                    return true;
                }
            }

            return false;
        }

        private bool HasToTurnLeft(Car car, Vector actualVector)
        {
            var anotherNode = car.ActualRoad.GetAnotherNode(car.LastNode);

            var position = new Vector
            {
                X = anotherNode.Position.X + actualVector.X * (Node.NodeSize*2/3),
                Y = anotherNode.Position.Y + actualVector.Y * (Node.NodeSize*2/3)
            };
            if (RectangleCollisions.CheckSquaresIncluding(position, Node.NodeSize, car.Position, Car.CarLength))
            {
                var nextVector = car.NextRoad.GetDirectionFromNode(anotherNode);
                if (actualVector.X * nextVector.Y - actualVector.Y * nextVector.X < 0)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns list of starting nodes, where is no car.
        /// </summary>
        /// <param name="destinationNodes">List of starting nodes.</param>
        /// <returns>List of starting nodes without car within.</returns>
        private List<Node> GetEmptyStartPoints(List<Node> destinationNodes)
        {
            return destinationNodes.Where(n => !this.Cars.Any(c => RectangleCollisions.CheckSquaresCollision(n.Position, Node.NodeSize, c.Position, Car.CarLength))).ToList();
        }

        private void RepairCarPosition(Car car)
        {
            foreach (var building in CityMap.Buildings)
            {
                var distance = RectangleCollisions.GetSquareWithBuildingCollision(car.Position, Car.CarLength, building);
                if (distance > 0)
                {
                    var vector = car.ActualRoad.GetDirectionFromNode(car.LastNode);
                    vector.RotateLeft();
                    car.MoveBy(vector.X * distance, vector.Y * distance);
                }
            }
        }
    }
}
