﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ISI.Model
{
    public enum LightState
    {
        Green,
        Yellow,
        Red,
        YellowStart
    }

    public class Light
    {
        public static double LightSize = 10;

        private static Dictionary<LightState, bool> StateStopMap = new Dictionary<LightState, bool>
        {
            { LightState.Green, false},
            { LightState.Yellow, true},
            { LightState.YellowStart, true},
            { LightState.Red, true}
        };

        private static Dictionary<LightState, SolidColorBrush> StateColorMap = new Dictionary<LightState, SolidColorBrush>
        {
            { LightState.Green, new SolidColorBrush(Colors.Green)},
            { LightState.Yellow, new SolidColorBrush(Colors.Yellow)},
            { LightState.YellowStart, new SolidColorBrush(Colors.Yellow)},
            { LightState.Red, new SolidColorBrush(Colors.Red)}
        };

        public readonly Dictionary<LightState, TimeSpan> StateTime = new Dictionary<LightState, TimeSpan>
        {
            {LightState.Green, new TimeSpan(0, 0, 0, 5)},
            {LightState.Yellow, new TimeSpan(0, 0, 0, 1)},
            {LightState.Red, new TimeSpan(0, 0, 0, 5)},
            {LightState.YellowStart, new TimeSpan(0, 0, 0, 1)}
        };

        public Edge EdgeWithLight
        {
            get;
            set;
        }

        public Node NodeWithLight
        {
            get;
            set;
        }

        public Rectangle Rect
        {
            get;
            set;
        }

        public bool Stop
        {
            get;
            private set;
        }

        public DateTime LastChangeDate
        {
            get;
            set;
        }

        private LightState innerState;

        public LightState State
        {
            get{
                return this.innerState;
            }
            set
            {
                this.innerState = value;
                this.Stop = StateStopMap[value];
                this.ChangeColor();
            }
        }

        private void ChangeColor()
        {
            if (Rect != null && Rect.Dispatcher != null && !Rect.Dispatcher.HasShutdownStarted)
            {
                try
                {
                    Rect.Dispatcher.Invoke(() =>
                    {
                        this.Rect.Fill = StateColorMap[innerState];
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
