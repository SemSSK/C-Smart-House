using System;
using System.Collections.Generic;
using System.IO;
using FluentResults;
using Interfaces;

namespace AdminServer
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

        public void TurnOn() => tempViewer.TurnOn();
        public void TurnOff() => tempViewer.TurnOff();
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
        
        public void TurnOn() => humViewer.TurnOn();
        public void TurnOff() => humViewer.TurnOff();
    }
    
    public class DoorService
    {
        private IIotViewer<bool> doorViewer;

        public DoorService()
        {
            doorViewer = (IIotViewer<bool>)Activator
                .GetObject(typeof(IIotViewer<bool>),
                    RemotingUrlBuilder.GetUrl(Ports.SECUREPORT, Names.DOORNAME));
        }

        public bool GetLockState() =>
            doorViewer.GetValue();
        
        public void TurnOn() => doorViewer.TurnOn();
        public void TurnOff() => doorViewer.TurnOff();
    }
    
    public class MovementCapture
    {
        private IMovementCapture capViewer;

        public MovementCapture()
        {
            capViewer = (IMovementCapture)Activator
                .GetObject(typeof(IMovementCapture),
                    RemotingUrlBuilder.GetUrl(Ports.SECUREPORT, Names.MOVENAME));
        }

        public List<string> GetCaptures() =>
            capViewer.GetValue();
        
        public void TurnOn() => capViewer.TurnOn();
        public void TurnOff() => capViewer.TurnOff();
    }

    public class Camera
    {
        private IIotViewer<Stream> camViewer;

        public Camera()
        {
            camViewer = (IIotViewer<Stream>)Activator
                .GetObject(typeof(IIotViewer<Stream>),
                    RemotingUrlBuilder.GetUrl(Ports.SECUREPORT, Names.CAMERANAME));
        }

        public Result<Stream> GetVideos() =>
            Result.Ok(camViewer.GetValue());

        public void TurnOn() => camViewer.TurnOn();
        public void TurnOff() => camViewer.TurnOff();
    }
}