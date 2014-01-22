using ISI.Configuration;
using ISI.Controller;
using ISI.Timer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISI_Console_Tests
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new TestLightsConfiguration();
            var mainController = new MainController(null, () =>
            {
            }, config);

            var iteration = 0;

            while (!mainController.Finished && !mainController.Jam)
            {
                if (iteration % 10 == 0)
                {
                    System.Console.WriteLine("iteracja: {0}", iteration);
                }
                mainController.Update();
                ((ConsoleTimer)config.Timer).UpdateTime();
                iteration++;
            }

            if (mainController.Jam)
            {
                System.Console.WriteLine("-------------KOREK-------------");
            }

            StreamWriter writer = new StreamWriter("write.txt", false);
            writer.WriteLine(iteration);
            foreach (var item in mainController.GetQueuedCarsInTimeIntervals())
            {
                writer.WriteLine("{0}\t{1}", item.CarsInQueue, item.CarsInSimulation);
            }
            writer.Close();
        }
    }
}
