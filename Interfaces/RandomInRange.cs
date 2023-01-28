using System;

namespace Interfaces
{
    public class RandomInRange
    {
        public static double Generate(double min, double max)
            => new System.Random().NextDouble() * (max - min) + min;
    }
}