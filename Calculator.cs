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
    public static class Calculator
    {
        public static List<double> ReturnGeneratorOutput()
        {
            List<string> Strings = File.ReadAllLines("generator_output.txt").ToList();

            List<double> Rander = new List<double> { };

            for (int i = 0; i < 100; i++)
            {
                Rander.Add(Convert.ToDouble(Convert.ToInt32(Strings[i])%100));
            }

            return Rander;
        }
    }
}
