using System;
using System.Security.Cryptography;
using Interfaces;

namespace MovementCaptureDevice
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            while (true)
            {
                new SecurityServer.DatabaseAccessor().AddMoCap(DateTime.Now.ToString());
                Console.WriteLine("Movement captured");
                System.Threading.Thread.Sleep(Convert.ToInt32(Math.Floor(RandomInRange.Generate(10000, 30000))));
            }
        }
    }
}