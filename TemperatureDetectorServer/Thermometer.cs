using System;
using FluentResults;
using Interfaces;

namespace TemperatureDetectorServer
{
    public class Thermometer : MarshalByRefObject ,IIotViewer<double>
    {
        public bool state = true;
        public double temperature = 24.2;

        public void TurnOn()
        {
            state = false;
        }

        public void TurnOff()
        {
            state = true;
        }

        public double GetValue() =>
            temperature;
    }
}