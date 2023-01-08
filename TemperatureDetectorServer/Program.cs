using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

namespace TemperatureDetectorServer

{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var channel = new TcpChannel(3001);
            ChannelServices.RegisterChannel(channel);
            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(Thermometer),"thermometer",WellKnownObjectMode.Singleton);
            Console.WriteLine("Temperature server ready ...");
            Console.ReadLine();
        }
    }
}