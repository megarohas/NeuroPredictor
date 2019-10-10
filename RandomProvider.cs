using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml;
using static System.Math;
using static System.Console;

namespace NeuroPredictor
{
    public static class RandomProvider
    {
        private static int Seed = Environment.TickCount;
        private static ThreadLocal<Random> RandomWrapper = new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref Seed)));
        public static double GetRandom()
        {
            return RandomWrapper.Value.Next(100);
        }
        public static List<double> GetRandomList(int Length)
        {
            List<double> Result = new List<double>();
            for (int i = 0; i < Length; i++)
                Result.Add(Convert.ToDouble(RandomProvider.GetRandom()));
            return Result;
        }
        public static double GetAverageMaurer(int MaxCount)
        {
            double Result = 0;
            do
            {
                Result = 0;
                for (int i = 0; i < MaxCount; i++)
                {
                    Result += Tester.MaurerTest(Tester.DoublesToBytes(RandomProvider.GetRandomList(100)));
                }
            } while (Double.IsInfinity(Result));
            return Result / MaxCount;
        }
    }
}
