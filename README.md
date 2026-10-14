# NeuroPredictor

A console experiment that trains a small feed-forward neural network with a
genetic algorithm on a pseudo-random number sequence, and measures the
randomness of sequences with Maurer's universal statistical test.

## What it does

1. **Baseline.** Generates 10,000 sequences of 100 pseudo-random numbers
   (0–99, `System.Random`), runs a simplified Maurer's universal test on the
   800 bytes of each sequence (L = 8, Q = 256, K = 544) and prints the average
   deviation of the test statistic from its expected value, 7.1837.
2. **Network.** 100 inputs → 300 hidden neurons → 100 outputs; each neuron
   outputs `floor(weighted sum mod 100)`. The input is the first 100 numbers
   of `generator_output.txt`; the initial weights are read from
   `hidden_layer_memory.txt` and `output_layer_memory.txt` (30,000 values
   each).
3. **Training.** A genetic algorithm whose chromosome is the full vector of
   60,000 weights. Each generation adds the output of the crossover operator
   and three mutants to the population, scores every individual, keeps the
   two with the lowest score and prints those scores (12,200 generations).
   The score is the absolute covariance (centred at 50) between the network's
   100 outputs and the next 100 numbers of the sequence (lines 101–200 of
   `generator_output.txt`). A variant that scored the output with the Maurer
   statistic is left commented out in the code.

   Note: because of a bug in `Geneter.Cross`, the crossover returns copies of
   the parents instead of mixing them, so in practice only mutation explores
   new weights.

`FileEditor` has helpers (commented out in `Main`) to regenerate the weight
files and a new 200-number sequence.

## Tech

- C# 6, .NET Framework 4.6.1, console application
- No external libraries; network, genetic operators and the Maurer test are
  implemented from scratch

## Build and run

There is no solution file: open `NeuroPredictor.csproj` in Visual Studio
(2015 or later) and run it. The data files are read from the working
directory; copies are in `bin/Debug`, which is where Visual Studio starts the
app.

The weight files use a comma as the decimal separator (`0,099`) and are
parsed with the current culture, so run it under a comma-decimal locale (for
example Russian); otherwise the weights are misread.

## Context

University project, Omsk State University, 2018 (published on GitHub in
2019).
