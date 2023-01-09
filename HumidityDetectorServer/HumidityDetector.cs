using System;
using FluentResults;
using Interfaces;

namespace HumidityDetectorServer
{
    public class HumidityDetector : MarshalByRefObject, IIotViewer<double>
    {
        public bool state = true;
        public double value = 43.5;
        
        public void TurnOn()
        {
            state = true;
        }

        public void TurnOff()
        {
            state = false;
        }

        public double GetValue() =>
            value;
    }
}