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



enum MemoryMode { GET, SET }
enum NeuronType { Hidden, Output }

namespace NeuroPredictor
{
    class Program
    {
        static void Main(string[] args)
        {
            //FileEditor.GenerateWeights("hidden_layer_memory.txt", 30000);
            //FileEditor.GenerateWeights("output_layer_memory.txt", 30000);
            //FileEditor.GenerateSeq();

            Console.WriteLine(RandomProvider.GetAverageMaurer(10000));
            Console.WriteLine("//////////////////////////////////////////////////");

            //Tester.TestRandomOrg();

            Network net = new Network();
            //Console.WriteLine(Network.Test(net));
            Network.Train(net);

            Console.ReadKey();
        }
    }
}