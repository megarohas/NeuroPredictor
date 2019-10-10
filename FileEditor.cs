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
    public static class FileEditor
    {
        public static void GenerateWeights(string path, int Size)
        {
            List<double> Numbers = new List<double> { };
            List<string> Strings = new List<string> { };
            double Buf = 0;
            for (int i = 0; i < Size; i++)
            {
                Buf = RandomProvider.GetRandom();
                Numbers.Add(Buf * 0.001);
                Strings.Add((Buf * 0.001).ToString());
            }
            File.WriteAllLines(path, Strings.ToArray());
        }
        public static void GenerateSeq()
        {
            List<string> Strings = new List<string> { };
            double Buf = 0;
            for (int i = 0; i < 200; i++)
            {
                Buf = RandomProvider.GetRandom();
                Strings.Add((Buf).ToString());
            }

            File.WriteAllLines("generator_output.txt", Strings.ToArray());
        }
    }
}
