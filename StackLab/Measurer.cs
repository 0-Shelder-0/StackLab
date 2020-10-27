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
            var stack = new Stack<string>();

            stopwatch.Start();
            interpreter.Run(input, output, stack);
            stopwatch.Stop();

            return stopwatch.Elapsed;
        }
    }
}
