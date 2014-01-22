using ISI.Configuration;
using ISI.Controller;
using ISI.Model;
using ISI.Timer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ISI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private System.Timers.Timer timer;

        private MainController mainController;

        private bool stopped;

        private int iteration = 0;

        public MainWindow()
        {
            InitializeComponent();
            mainController = new MainController(this.ViewPort, () => this.Close(), new DefaultLightsConfiguration());

            this.timer = new System.Timers.Timer();
            timer.Interval = 50;
            timer.AutoReset = true;
            timer.Elapsed += (obj, data) =>
            {
                if (!stopped)
                {
                    if (!mainController.Finished)
                    {
                        if (this.iteration % 10 == 0)
                        {
                            System.Console.WriteLine("Iteracja {0}", this.iteration);
                        }
                        mainController.Update();
                        this.iteration++;
                    }
                }
            };
            timer.Start();
        }

        private void Window_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.timer.Stop();
        }

        private void Window_KeyUp_1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                this.stopped = !stopped;
            }
        }
    }
}
