using FluentResults;
using Interfaces;

namespace HumidityDetectorServer
{
    public class HumidityDetector : IIotManager, IIotViewer<double>
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

        public Result<double> GetValue() =>
            state ? Result.Ok(value) : Result.Fail("Humidity Detector Off");
    }
}