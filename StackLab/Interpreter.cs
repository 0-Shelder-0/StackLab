using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using StackLab.Interfaces;

namespace StackLab
{
    public class Interpreter
    {
        private readonly Dictionary<string, Func<IStack<string>, string, string>> _dictionaryFunc =
            new Dictionary<string, Func<IStack<string>, string, string>>
            {
                ["1"] = (stack, command) =>
                        {
                            var value = command.Split(',').LastOrDefault();
                            stack.Push(value);
                            return $"Push: {value}";
                        },
                ["2"] = (stack, command) => stack.Pop(),
                ["3"] = (stack, command) => stack.Top(),
                ["4"] = (stack, command) => stack.IsEmpty().ToString(),
                ["5"] = (stack, command) => stack.Print()
            };

        public void Run(string program, IStack<string> stack, Stream output)
        {
            var commands = program.Split().Where(x => x.Length > 0).ToArray();
            for (var i = 1; i <= commands.Length; i++)
            {
                var operation = commands[i].Split(',').FirstOrDefault();
                if (!_dictionaryFunc.ContainsKey(operation))
                {
                    WriteText(output, $"Line {i} undexpected token: {commands[i]}");
                    break;
                }
                WriteText(output, _dictionaryFunc[operation](stack, commands[i]));
            }
        }

        private static void WriteText(Stream stream, string str)
        {
            stream.Write(Encoding.Default.GetBytes($"{str}\r\n"));
        }
    }
}
