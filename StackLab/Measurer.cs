using System.Diagnostics;
using System.IO;
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
            var result = interpreter.Run(input, new Stack<string>());

            for (var i = 0; i < repeatNumber; i++)
            {
                stopwatch.Start();
                result = interpreter.Run(input, new Stack<string>());
                stopwatch.Stop();
                input.Seek(0, SeekOrigin.Begin);
            }
            output.StreamWriteLine(result);

            return stopwatch.Elapsed.TotalMilliseconds / repeatNumber;
        }
    }
}
