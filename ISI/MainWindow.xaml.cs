using ISI.Controller;
using ISI.Model;
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
        private Timer timer;

        private MainController mainController;

        private bool stopped;

        public MainWindow()
        {
            InitializeComponent();
            mainController = new MainController(this.ViewPort);

            this.timer = new Timer();
            timer.Interval = 50;
            timer.AutoReset = true;
            timer.Elapsed += (obj, data) =>
            {
                if (!stopped)
                {
                    mainController.Update();
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
