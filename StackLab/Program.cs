using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using StackLab.Generators;
using StackLab.Interfaces;
using StackLab.Interpreters;

namespace StackLab
{
    static class Program
    {
        private static void Main()
        {
            var testsFile = new FilePath($"C:/Users/{Environment.UserName}/Desktop/Tests", "test.txt");
            var resultFile = new FilePath($"C:/Users/{Environment.UserName}/Desktop/Results", "result.txt");
            RunInterpreter(testsFile, resultFile, 5, 1000, new Generator(), new Interpreter());
            // RunInterpreter(testsFile, resultFile, 5, 100, new GeneratorOperations(), new InterpreterOperations());
        }

        private static void RunInterpreter(FilePath testFilePath,
                                           FilePath resultFilePath,
                                           int count,
                                           int step,
                                           IGenerator generator,
                                           IInterpreter<string> interpreter)
        {
            Directory.CreateDirectory(testFilePath.DirectoryPath);
            Directory.CreateDirectory(resultFilePath.DirectoryPath);
            var itemCountList = GetItemCountList(count, step, 3);
            var paths = GenerationResultWriter.WriteResults(testFilePath, generator, itemCountList);

            using (var resultOutput = new FileStream(resultFilePath.FullPath, FileMode.Create, FileAccess.Write))
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
            return Enumerable.Range(1, count)
                             .SelectMany(item => Enumerable.Repeat(item, repeatNumber))
                             .Select(item => item * step);
        }
    }
}
