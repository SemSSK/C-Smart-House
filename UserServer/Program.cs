using System;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using Interfaces;

namespace UserServer
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var channel = new TcpChannel(Ports.USERPORT);
            ChannelServices.RegisterChannel(channel);
            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(AccountsManager), Names.ACCOUNTSNAME,WellKnownObjectMode.Singleton);
            Console.WriteLine("User Server is ready ...");
            Console.Read();
        }
    }
}