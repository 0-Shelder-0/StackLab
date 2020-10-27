using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using StackLab.Interfaces;

namespace StackLab.Interpreters
{
    public class Interpreter : IInterpreter<string>
    {
        public void Run(Stream input, Stream output, IStack<string> stack)
        {
            var bytes = new byte[input.Length];
            input.Read(bytes);
            var commands = GetCommands(bytes);
            for (var i = 0; i < commands.Length; i++)
            {
                var operation = commands[i].Split(',').FirstOrDefault();
                if (!_dictionaryFunc.ContainsKey(operation))
                {
                    output.StreamWriteLine($"Line {i + 1} undexpected token: {commands[i]}");
                    break;
                }
                output.StreamWriteLine(_dictionaryFunc[operation](stack, commands[i]));
            }
        }

        private readonly Dictionary<string, Func<IStack<string>, string, string>> _dictionaryFunc =
            new Dictionary<string, Func<IStack<string>, string, string>>
            {
                ["1"] = (stack, command) =>
                        {
                            var value = command.Split(',').LastOrDefault();
                            stack.Push(value);
                            return $"Push: {value}";
                        },
                ["2"] = (stack, command) => $"Pop: {stack.Pop()}",
                ["3"] = (stack, command) => $"Top: {stack.Top()}",
                ["4"] = (stack, command) => $"IsEmpty: {stack.IsEmpty().ToString()}",
                ["5"] = (stack, command) => $"Print: {stack.Print()}"
            };

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
