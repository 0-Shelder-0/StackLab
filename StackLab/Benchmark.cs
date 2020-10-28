using System.Collections.Generic;
using System.IO;
using StackLab.Interfaces;

namespace StackLab
{
    public static class Benchmark
    {
        public static void Run(IEnumerable<string> paths,
                               IInterpreter<string> interpreter,
                               int repeatNumber,
                               Stream programOutput,
                               FileStream resultOutput)
        {
            foreach (var path in paths)
            {
                using (var input = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    var milliseconds = Measurer.Measure(input, programOutput, interpreter, repeatNumber);
                    resultOutput.StreamWriteLine($"{path}: {milliseconds} ms");
                }
            }
        }
    }
}
