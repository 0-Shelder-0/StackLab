using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using StackLab.Generators;
using StackLab.Interfaces;
using StackLab.Interpreters;

namespace StackLab
{
    static class Program
    {
        private static void Main()
        {
            var path = $"C:/Users/{Environment.UserName}/Desktop/";
            // RunInterpreter(path, 15, "input", new Generator(), new Interpreter());
            RunInterpreter(path, 1, "operations", new GeneratorOperations(), new InterpreterOperations());
        }

        private static void RunInterpreter(string path,
                                           int count,
                                           string prefix,
                                           IGenerator generator,
                                           IInterpreter<string> interpreter)
        {
            var itemCountList = GetItemCountList(count, 10);
            var paths = GenerationResultWriter.WriteResults(path, prefix, generator, itemCountList);

            using (var resultOutput = new FileStream(path + "result.txt", FileMode.Create, FileAccess.Write))
            {
                Benchmark.Run(paths, interpreter, 5, Console.OpenStandardOutput(), resultOutput);
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
