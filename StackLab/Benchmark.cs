using System.Collections.Generic;
using System.IO;
using StackLab.Interfaces;

namespace StackLab
{
    public static class Benchmark
    {
        public static void Run(IEnumerable<FilePath> paths,
                               IInterpreter<string> interpreter,
                               int repeatNumber,
                               Stream programOutput,
                               FileStream resultOutput)
        {
            foreach (var path in paths)
            {
                using (var input = new FileStream(path.FullPath, FileMode.Open, FileAccess.Read))
                {
                    var milliseconds = Measurer.Measure(input, programOutput, interpreter, repeatNumber);
                    resultOutput.StreamWriteLine($"{path.Name}: {milliseconds} ms");
                }
            }
        }
    }
}
