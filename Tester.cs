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
    class Tester
    {
        public static double CorFunc(List<double> A, List<double> B)
        {
            List<string> Strings = File.ReadAllLines("generator_output.txt").ToList();
            List<double> Rander = new List<double> { };
            for (int i = 100; i < 200; i++)
                Rander.Add(Convert.ToDouble(Strings[i]));
            B = Rander;

            double Result = 0;
            if (A.Count != B.Count) return -1;
            for (int i = 0; i < A.Count; i++)
                Result += (A[i] - 50) * (B[i] - 50);
            Result = Result * Math.Pow(A.Count, -1);
            return Result;
        }
        public static double MaurerTest(List<byte> Bytes)
        {
            int L = 8;
            int V = (1 << L);
            int Q = 256;
            int K = 544;
            int MAXSAMP = Q + K;
            int i, j, b;
            List<int> table = new List<int> { };
            double sum = 0.0;

            for (i = 0; i < V; i++)
                table.Add(0);

            for (i = 0; i < Q; i++)
            {
                b = Bytes[i];
                table[b] = i % V;
            }

            for (i = Q; i < Q + K; i++)
            {
                j = i;
                b = Bytes[i];
                {
                    if (table[b] >= i)
                        sum += Math.Log((double)(i - table[b] + K - Q));
                    else
                    {
                        int a = i % 255 - table[b];
                        if (a <= 0) a += 255;
                        sum += Math.Log((double)(a));
                    }
                    table[b] = (byte)i % 255;
                }
            }
            sum = (sum / ((double)(K) * Math.Log(2.0))) - 7.1837;
            sum = Math.Abs(sum);
            return sum;
        }
        public static List<byte> DoublesToBytes(List<double> Input)
        {
            List<double> Rander = Input;

            List<byte> Bytes = new List<byte> { };
            for (int i = 0; i < Rander.Count; i++)
            {
                Bytes.AddRange(BitConverter.GetBytes(Rander[i]).ToList());
            }

            return Bytes;
        }
    }
}
