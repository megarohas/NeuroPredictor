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
    class Neuron
    {
        public Neuron(double[] inputs, double[] weights, NeuronType type)
        {
            _type = type;
            _weights = weights;
            _inputs = inputs;
        }
        private NeuronType _type;
        private double[] _weights;
        private double[] _inputs;
        public double[] Weights { get { return _weights; } set { _weights = value; } }
        public double[] Inputs { get { return _inputs; } set { _inputs = value; } }
        public double Out { get { return Activator(_inputs, _weights); } }
        private double Activator(double[] i, double[] w)//преобразования
        {
            double sum = 0;
            for (int l = 0; l < i.Length; ++l)
                sum += i[l] * w[l];//линейные
            //return sum;
            return Math.Floor(sum % 100);
            //return Pow(1 + Exp(-sum), 1);//нелинейные
        }
    }
    abstract class Layer
    {//type используется для связи с одноимённым полю слоя файлом памяти
        public double[,] Weights = new double[0, 0];
        protected Layer(int non, int nopn, NeuronType nt, string type)
        {//увидите это в WeightInitialize
            numofneurons = non;
            numofprevneurons = nopn;
            Neuron[] Neurons = new Neuron[non];
            Weights = WeightInitialize(MemoryMode.GET, type);
            for (int i = 0; i < non; ++i)
            {
                double[] temp_weights = new double[nopn];
                for (int j = 0; j < nopn; ++j)
                    temp_weights[j] = Weights[i, j];
                Neurons[i] = new Neuron(null, temp_weights, nt);//про подачу null на входы ниже
            }
            _neurons = Neurons;
        }
        protected int numofneurons;//число нейронов текущего слоя
        protected int numofprevneurons;//число нейронов предыдущего слоя

        Neuron[] _neurons;
        public Neuron[] Neurons { get { return _neurons; } set { _neurons = value; } }
        public double[] Data//я подал null на входы нейронов, так как
        {//сначала нужно будет преобразовать информацию
            set//(видео, изображения, etc.)
            {//а загружать input'ы нейронов слоя надо не сразу,
                for (int i = 0; i < Neurons.Length; ++i)
                    Neurons[i].Inputs = value;
            }//а только после вычисления выходов предыдущего слоя
        }
        public double[,] WeightInitialize(MemoryMode mm, string type)
        {
            double[,] _weights = new double[numofneurons, numofprevneurons];
            //WriteLine($"{type} weights are being initialized...");
            switch (mm)
            {
                case MemoryMode.GET:
                    List<string> Strings = File.ReadAllLines($"{type}_memory.txt").ToList();
                    int Count = 0;
                    for (int l = 0; l < _weights.GetLength(0); ++l)
                        for (int k = 0; k < _weights.GetLength(1); ++k)
                        {
                            //double buf = RandomProvider.GetRandom();
                            // _weights[l, k] = buf * 0.001;
                            _weights[l, k] = Convert.ToDouble(Strings[Count]);
                            Count++;
                        }
                    break;
                case MemoryMode.SET:
                    for (int l = 0; l < Neurons.Length; ++l)
                        for (int k = 0; k < numofprevneurons; ++k) { }
                            //memory_el.ChildNodes.Item(k + numofprevneurons * l).InnerText = Neurons[l].Weights[k].ToString();
                    break;
            }
            //memory_doc.Save($"{type}_memory.xml");
            //WriteLine($"{type} weights have been initialized...");
            return _weights;
        }
        abstract public void Recognize(Network net, Layer nextLayer);//для прямых проходов
    }
    class HiddenLayer : Layer
    {
        public HiddenLayer(int non, int nopn, NeuronType nt, string type) : base(non, nopn, nt, type) { }
        public override void Recognize(Network net, Layer nextLayer)
        {
            List<double> Rander = Calculator.ReturnGeneratorOutput();
            Neuron[] NeuronsT = new Neuron[Neurons.Length];

            for (int i = 0; i < Neurons.Length; ++i)
            {
                double[] temp_weights = new double[Neurons.Length];
                for (int j = 0; j < 100; ++j)
                    temp_weights[j] = Weights[i, j];
                Neurons[i] = new Neuron(Rander.ToArray(), temp_weights, NeuronType.Hidden);//про подачу null на входы ниже
            }

            double[] hidden_out = new double[Neurons.Length];
            for (int i = 0; i < Neurons.Length; ++i)
                hidden_out[i] = Neurons[i].Out;
            nextLayer.Data = hidden_out;
        }
    }
    class OutputLayer : Layer
    {
        public OutputLayer(int non, int nopn, NeuronType nt, string type) : base(non, nopn, nt, type) { }
        public override void Recognize(Network net, Layer nextLayer)
        {
            Neuron[] NeuronsT = new Neuron[Neurons.Length];

            for (int i = 0; i < 100; ++i)
            {
                double[] temp_weights = new double[300];
                for (int j = 0; j < 300; ++j)
                    temp_weights[j] = Weights[i, j];
                Neurons[i] = new Neuron(Neurons[i].Inputs, temp_weights, NeuronType.Output);//про подачу null на входы ниже
            }

            for (int i = 0; i < Neurons.Length; ++i)
                net.fact[i] = Neurons[i].Out;
        }
    }
    public class InputLayer
    {
        public List<double> Trainset
        {
            get
            {

                List<double> Rander = Calculator.ReturnGeneratorOutput();


                List<byte> Bytes = new List<byte> { };
                for (int i = 0; i < Rander.Count; i++)
                    Bytes.AddRange(BitConverter.GetBytes(Rander[i]).ToList());

                Tester.MaurerTest(Bytes);

                return Rander;
            }
        }
    }
    class Network
    {
        //все слои сети
        public InputLayer input_layer = new InputLayer();
        public HiddenLayer hidden_layer = new HiddenLayer(300, 100, NeuronType.Hidden, nameof(hidden_layer));
        public OutputLayer output_layer = new OutputLayer(100, 300, NeuronType.Output, nameof(output_layer));
        //массив для хранения выхода сети
        public double[] fact = new double[100];
        //тестирование сети
        public static double Test(Network net)
        {
            List<double> Numbers = new List<double> { };
            net.hidden_layer.Data = net.input_layer.Trainset.ToArray();
            net.hidden_layer.Recognize(null, net.output_layer);
            net.output_layer.Recognize(net, null);
            for (int j = 0; j < net.fact.Length; ++j)
                Numbers.Add(net.fact[j]);

            Tester.CorFunc(Numbers, Numbers);

            List<byte> Bytes = new List<byte> { };
            for (int i = 0; i < Numbers.Count; i++)
                Bytes.AddRange(BitConverter.GetBytes(Numbers[i]).ToList());

            return Tester.MaurerTest(Bytes);
        }
        public static double Test(Network net, string mode)
        {
            List<double> Numbers = new List<double> { };
            net.hidden_layer.Recognize(net, net.output_layer);
            net.output_layer.Recognize(net, null);
            for (int j = 0; j < net.fact.Length; ++j)
                Numbers.Add(net.fact[j]);

            List<byte> Bytes = new List<byte> { };
            for (int i = 0; i < Numbers.Count; i++)
                Bytes.AddRange(BitConverter.GetBytes(Numbers[i]).ToList());

            return Math.Abs(Tester.CorFunc(Numbers, Numbers));
            // return Tester.MaurerTest(Bytes);
        }
        public static double TrainWithWeights(Network net, List<double> Weights)
        {
            double[,] _weights_hl = net.hidden_layer.Weights;
            double[,] _weights_ol = net.output_layer.Weights;
            List<double> ChromosomeS = Weights.GetRange(0, Weights.Count / 2);
            List<double> ChromosomeE = Weights.GetRange(Weights.Count / 2, Weights.Count / 2);
            //Chromosome.AddRange();
            int CountS = 0;
            for (int i = 0; i < 300; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    _weights_hl[i, j] = ChromosomeS[CountS];
                    CountS++;
                }
            }

            int CountE = 0;
            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 300; j++)
                {
                    _weights_ol[i, j] = ChromosomeE[CountE];
                    CountE++;
                }
            }

            net.hidden_layer.Weights = _weights_hl;
            net.output_layer.Weights = _weights_ol;

            //Console.WriteLine(Test(net, "without_weight_init"));
            return Test(net, "without_weight_init");
        }
        public static void Train(Network net)
        {
            double[,] _weights_hl = net.hidden_layer.Weights;
            List<double> Chromosome = new List<double> { };

            for (int i = 0; i < 300; i++)
                for (int j = 0; j < 100; j++)
                    Chromosome.Add(_weights_hl[i, j]);

            double[,] _weights_ol = net.output_layer.Weights;
            for (int i = 0; i < 100; i++)
                for (int j = 0; j < 300; j++)
                    Chromosome.Add(_weights_ol[i, j]);

            List<List<double>> Population = new List<List<double>> { };
            Population.Add(Chromosome);
            Population.Add(Geneter.Mutation(Chromosome));

            List<double> Buf = new List<double> { 0 };
            int cc = 0;
            while (cc < 12200)
            {
                List<double> CrossRes = Geneter.Cross(Population[0], Population[1]);
                Population.Add(CrossRes.GetRange(0, CrossRes.Count / 2));

                Population.Add(CrossRes.GetRange(CrossRes.Count / 2 - 1, CrossRes.Count / 2));
                Population.Add(Geneter.Mutation(Population[1]));
                Population.Add(Geneter.Mutation(Population[2]));
                Population.Add(Geneter.Mutation(Population[3]));

                Dictionary<List<double>, double> BestOfTheBestInivids = new Dictionary<List<double>, double> { };
                List<double> MaurerVals = new List<double> { };

                for (int i = 0; i < Population.Count; i++)
                {
                    double MaurerVal = TrainWithWeights(net, Population[i]);
                    BestOfTheBestInivids.Add(Population[i], MaurerVal);
                    MaurerVals.Add(MaurerVal);
                }

                MaurerVals.Sort();
                //MaurerVals.Reverse();

                while (MaurerVals.Count > 2)
                    MaurerVals.RemoveAt(MaurerVals.Count - 1);

                for (int i = 0; i < MaurerVals.Count; i++)
                    Console.WriteLine(MaurerVals[i]);

                List<List<double>> Population2 = new List<List<double>> { };

                for (int i = 0; i < Population.Count; i++)
                    if (MaurerVals.Contains(BestOfTheBestInivids[Population[i]])) Population2.Add(Population[i]);

                Population = Population2;
                cc++;
            }
        }
    }
}
