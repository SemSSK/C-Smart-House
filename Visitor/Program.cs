using System;

namespace Visitor
{
    internal class Program
    {
        public static void Main(string[] args)
        {
           
            var tempViewer = new TemperatureViewer();
            var humViewer = new HumidityViewer();
            
            Console.WriteLine("temperature is : {0} c°",tempViewer.GetTemperature().Value);
            Console.WriteLine("humididty is : {0} g.m³",humViewer.GetHumidity().Value);
        }
    }
}