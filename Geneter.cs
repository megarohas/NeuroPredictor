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
    public static class Geneter
    {
        public static List<double> Mutation(List<double> ChromosomeIn)
        {
            List<double> Chromosome = ChromosomeIn.GetRange(0, ChromosomeIn.Count);
            Random Rand = new Random();
            int MutationCount = Rand.Next(ChromosomeIn.Count - 1);
            double MutationC = (Rand.Next(99) * 0.01);
            for (int i = 0; i < MutationCount; i++)
            {
                int C = Rand.Next(Chromosome.Count - 1);
                if (Chromosome[C] + MutationC <= 1)
                    Chromosome[C] += MutationC;
            }
            return Chromosome;
        }
        public static List<double> Cross(List<double> ChromosomeA, List<double> ChromosomeB)
        {
            Random Rand = new Random();
            int CrossPoint = Rand.Next(ChromosomeA.Count - 1 - ChromosomeA.Count / 3);

            List<double> FirstChild = ChromosomeA.GetRange(0, CrossPoint);
            List<double> SecondChild = ChromosomeB.GetRange(0, CrossPoint);

            FirstChild.AddRange(ChromosomeA.GetRange(CrossPoint, ChromosomeA.Count - CrossPoint));
            SecondChild.AddRange(ChromosomeB.GetRange(CrossPoint, ChromosomeA.Count - CrossPoint));

            /*
            double CrossC = (0.05);

            for (int j = 0; j < ChromosomeA.Count; j++)
            {
                if (FirstChild.Count < ChromosomeA.Count && !((FirstChild.FindAll(x=>(x-ChromosomeA[j] <= CrossC || ChromosomeA[j] - x <= CrossC))).Count > 0))
                {
                    FirstChild.Add(ChromosomeA[j]);
                }
                if (SecondChild.Count < ChromosomeB.Count && !((SecondChild.FindAll(x => (x - ChromosomeB[j] <= CrossC || ChromosomeB[j] - x <= CrossC))).Count > 0))
                {
                    SecondChild.Add(ChromosomeB[j]);
                }
            }
            */
            FirstChild.AddRange(SecondChild);
            return FirstChild;
        }

    }

}
