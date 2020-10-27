using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using StackLab.Generators;
using StackLab.Interpreters;

namespace StackLab
{
    static class Program
    {
        private static void Main()
        {
            const int count = 15;
            var path = $"C:/Users/{Environment.UserName}/Desktop/";
            var itemCountList = GetItemCountList(count, 1000);
            var paths = GenerationResultWriter.WriteResults(path, new Generator(), itemCountList);
            using (var resultOutput = new FileStream(path + "result.txt", FileMode.Create, FileAccess.Write))
            {
                Benchmark.Run(paths, new Interpreter(), Console.OpenStandardOutput(), resultOutput);
            }
        }

        private static IEnumerable<int> GetItemCountList(int count, int step)
        {
            return Enumerable.Range(1, count)
                             .Select(item => item * step);
        }

        private static IEnumerable<int> GetItemCountList(int count, int step, int repeatNumber)
        {
            return Enumerable.Range(1, count * repeatNumber)
                             .SelectMany(item => Enumerable.Repeat(item, repeatNumber))
                             .Select(item => item * step);
        }
    }
}
