using System;
using System.IO;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using Interfaces;

namespace SecurityServer
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var channel = new TcpChannel(Ports.SECUREPORT);
            ChannelServices.RegisterChannel(channel);
            
            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(CameraService),Names.CAMERANAME,WellKnownObjectMode.Singleton);
            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(DoorService),Names.DOORNAME,WellKnownObjectMode.Singleton);
            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(MovementCaptureService),Names.MOVENAME,WellKnownObjectMode.Singleton);

            var cam = new CameraService();
            var file = cam.GetValue();
            string contents;
            using (var sr = new StreamReader(file))
            {
                contents = sr.ReadToEnd();
            }
            Console.WriteLine("Security Server Ready ...");
            Console.ReadLine();
        }
    }
}