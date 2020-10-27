using System;
using System.IO;
using System.Text;
using StackLab.Interpreters;

namespace StackLab
{
    static class Program
    {
        private static void Main()
        {
            var interpreter = new Interpreter();
            var m = new MemoryStream(Encoding.Default.GetBytes("1,4 1,3 3 4 5 2 2"));
            interpreter.Run(m, Console.OpenStandardOutput(), new Stack<string>());
        }
    }
}
