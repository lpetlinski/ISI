using ISI.Model.Acceleration;
using ISI.Utils;
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
    /// Class describing car.
    /// </summary>
   public class Car
    {
       /// <summary>
       /// The length of a car (square).
       /// </summary>
       public static readonly double CarLength = 10;

       /// <summary>
       /// Rectangle of car to draw on map.
       /// </summary>
       public Rectangle DrawingRect
       {
           get;
           private set;
       }

       /// <summary>
       /// Position of top left corner of a car.
       /// </summary>
       public Vector Position
       {
           get;
           private set;
       }

       /// <summary>
       /// Actual speed of a car.
       /// </summary>
       public double Speed
       {
           get;
           set;
       }

       /// <summary>
       /// Acceleration policy of a car.
       /// </summary>
       public IAccelerationPolicy AccelerationPolicy
       {
           get;
           private set;
       }

       public Edge ActualRoad
       {
           get;
           set;
       }

       public Edge NextRoad
       {
           get;
           set;
       }

       public Node LastNode
       {
           get;
           set;
       }

       public Node Destination
       {
           get;
           set;
       }

       /// <summary>
       /// Creates new car and sets it in point (0,0).
       /// </summary>
       public Car(IAccelerationPolicy accelerationPolicy)
       {
           this.DrawingRect = new Rectangle();
           this.AccelerationPolicy = accelerationPolicy;
           this.DrawingRect.Width = CarLength;
           this.DrawingRect.Height = CarLength;
           this.DrawingRect.Fill = new SolidColorBrush(this.AccelerationPolicy.CarColor);
           this.Position = new Vector();
           this.MoveTo(0, 0);
       }

       /// <summary>
       /// Moves car to given point.
       /// </summary>
       /// <param name="x">X dimension of point to move car to</param>
       /// <param name="y">Y dimension of point to move car to</param>
       public void MoveTo(double x, double y){
           this.Position.X = x;
           this.Position.Y = y;
           this.UpdatePosition();
       }

       /// <summary>
       /// Moves car by given distance.
       /// </summary>
       /// <param name="x">X dimension of distance to move car by</param>
       /// <param name="y">Y dimension of distance to move car by</param>
       public void MoveBy(double x, double y)
       {
           this.Position.X += x;
           this.Position.Y += y;
           this.UpdatePosition();
       }

       /// <summary>
       /// Checks if there is a collision between this car, and given car.
       /// </summary>
       /// <param name="anotherCar">Car to check collision with.</param>
       /// <returns>True if there is collision between cars.</returns>
       public bool CheckCollision(Car anotherCar)
       {
           return RectangleCollisions.CheckSquaresCollision(this.Position, CarLength, anotherCar.Position, CarLength);
       }

       /// <summary>
       /// Updates position of cars rectangle.
       /// </summary>
       private void UpdatePosition()
       {
           if (!DrawingRect.Dispatcher.HasShutdownStarted)
           {
               try
               {
                   DrawingRect.Dispatcher.Invoke(() =>
                   {
                       Canvas.SetLeft(this.DrawingRect, this.Position.X);
                       Canvas.SetTop(this.DrawingRect, this.Position.Y);
                   });
               }
               catch (TaskCanceledException)
               {
                   // Do nothing.
               }
           }
       }
    }
}
