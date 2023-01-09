using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using Interfaces;

namespace TemperatureDetectorServer

{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var channel = new TcpChannel(Ports.TEMPPORT);
            ChannelServices.RegisterChannel(channel);
            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(Thermometer),Names.TEMPNAME,WellKnownObjectMode.Singleton);
            Console.WriteLine("Temperature server ready ...");
            Console.ReadLine();
        }
    }
}