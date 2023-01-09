using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using Interfaces;

namespace HumidityDetectorServer
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var channel = new TcpChannel(Ports.HUMIDITYPORT);
            ChannelServices.RegisterChannel(channel);
            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(HumidityDetector),Names.HUMIDIDYNAME,WellKnownObjectMode.Singleton);
            Console.WriteLine("Humidity detector server ready ...");
            Console.ReadLine();
        }
    }
}