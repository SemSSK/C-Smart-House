using System;
using FluentResults;
using Interfaces;

namespace HumidityDetectorServer
{
    public class HumidityDetector : MarshalByRefObject, IIotViewer<double>
    {
        bool state = true;
        private double min = 40.0;
        private double max = 45;
        
        public void TurnOn()
        {
            state = true;
        }

        public void TurnOff()
        {
            state = false;
        }

        public double GetValue() =>
            RandomInRange.Generate(min,max);
    }
}