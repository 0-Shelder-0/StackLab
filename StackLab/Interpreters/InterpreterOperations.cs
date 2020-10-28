using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using StackLab.Interfaces;

namespace StackLab.Interpreters
{
    public class InterpreterOperations : IInterpreter<string>
    {
        public string Run(Stream input, IStack<string> stack)
        {
            var str = GetPostfixRecord(input, stack);
            var record = GetPostfixRecord(input, stack);
            return null;
        }

        private string GetPostfixRecord(Stream input, IStack<string> stack)
        {
            var stringBuilder = new StringBuilder();
            var bytes = new byte[input.Length];
            input.Read(bytes);
            var strings = GetCommands(bytes);
            var expr = strings.FirstOrDefault();
            var list = Regex.Split(expr, @"\d+|ln|-");
            return null;
        }

        private static string[] GetCommands(byte[] bytes)
        {
            return Encoding.Default
                           .GetString(bytes)
                           .Split()
                           .Where(x => x.Length > 0)
                           .ToArray();
        }
    }
}
