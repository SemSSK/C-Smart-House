using System;
using FluentResults;
using Interfaces;

namespace TemperatureDetectorServer
{
    public class Thermometer : MarshalByRefObject ,IIotViewer<double>
    {
        public bool state = true;
        private double min = 20.0;
        private double max = 25.0;

        public void TurnOn()
        {
            state = false;
        }

        public void TurnOff()
        {
            state = true;
        }

        public double GetValue() =>
            RandomInRange.Generate(min,max);
    }
}