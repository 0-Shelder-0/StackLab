using System;
using System.IO;
using System.Diagnostics;
using StackLab.Interfaces;

namespace StackLab
{
    public static class Measurer
    {
        public static TimeSpan Measure(Stream input, Stream output, IInterpreter<string> interpreter)
        {
            var stopwatch = new Stopwatch();
            var result = string.Empty;

            stopwatch.Start();
            for (var i = 0; i < 3; i++)
            {
                result = interpreter.Run(input, new Stack<string>());
            }
            stopwatch.Stop();

            output.StreamWrite(result);

            return stopwatch.Elapsed / 3;
        }
    }
}
