using System;
using FluentResults;
using Interfaces;

namespace Visitor
{
    public class TemperatureViewer
    {
        private IIotViewer<double> tempViewer;

        public TemperatureViewer()
        {
            tempViewer = (IIotViewer<double>)Activator
                .GetObject(typeof(IIotViewer<double>),
                    RemotingUrlBuilder.GetUrl(Ports.TEMPPORT, Names.TEMPNAME));
        }

        public Result<double> GetTemperature() =>
            Result.Ok(tempViewer.GetValue());
    }

    public class HumidityViewer
    {
        private IIotViewer<double> humViewer;

        public HumidityViewer()
        {
            humViewer = (IIotViewer<double>)Activator
                .GetObject(typeof(IIotViewer<double>),
                    RemotingUrlBuilder.GetUrl(Ports.HUMIDITYPORT, Names.HUMIDIDYNAME));
        }

        public Result<double> GetHumidity() =>
            Result.Ok(humViewer.GetValue());
    }
}