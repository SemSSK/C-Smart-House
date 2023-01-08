using FluentResults;
using Interfaces;

namespace TemperatureDetectorServer
{
    public class Thermometer : IIotManager, IIotViewer<double>
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

        public Result<double> GetValue() =>
            state ? Result.Ok(temperature) : Result.Fail("Temperature detector off");
    }
}