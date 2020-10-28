using System;
using System.IO;
using System.Diagnostics;
using StackLab.Interfaces;

namespace StackLab
{
    public static class Measurer
    {
        public static double Measure(Stream input,
                                       Stream output,
                                       IInterpreter<string> interpreter,
                                       int repeatNumber)
        {
            var stopwatch = new Stopwatch();
            var result = string.Empty;

            for (var i = 0; i < repeatNumber; i++)
            {
                stopwatch.Start();
                result = interpreter.Run(input, new Stack<string>());
                stopwatch.Stop();
                input.Seek(0, SeekOrigin.Begin);
            }

            output.StreamWrite(result);

            return stopwatch.Elapsed.TotalMilliseconds / repeatNumber;
        }
    }
}
