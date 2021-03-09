using System.Collections.Generic;
using BenchmarkDotNet.Running;
using Bogus;

namespace SortedListOrDictionary
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<DatastructuresFindBenchmark>();
        }
    }
}