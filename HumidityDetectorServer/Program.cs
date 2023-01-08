using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

namespace HumidityDetectorServer
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var channel = new TcpChannel(3002);
            ChannelServices.RegisterChannel(channel);
            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(HumidityDetector),"humidity",WellKnownObjectMode.Singleton);
            Console.WriteLine("Humidity detector server ready ...");
            Console.ReadLine();
        }
    }
}