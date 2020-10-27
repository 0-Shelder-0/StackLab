using System.Collections.Generic;
using System.IO;
using StackLab.Interfaces;

namespace StackLab
{
    public static class Benchmark
    {
        public static void Run(IEnumerable<string> paths,
                               IInterpreter<string> interpreter,
                               Stream programOutput,
                               FileStream resultOutput)
        {
            foreach (var path in paths)
            {
                using (var input = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    var timeSpan = Measurer.Measure(input, programOutput, interpreter);
                    resultOutput.StreamWriteLine($"{path}: {timeSpan}");
                }
            }
        }
    }
}
