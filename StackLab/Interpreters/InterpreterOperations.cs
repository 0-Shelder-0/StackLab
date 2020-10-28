using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using StackLab.Interfaces;

namespace StackLab.Interpreters
{
    public class InterpreterOperations : IInterpreter<string>
    {
        private readonly Dictionary<string, int> _operationPriority = new Dictionary<string, int>
        {
            ["+"] = 1,
            ["-"] = 1,
            ["*"] = 2,
            [":"] = 2,
            ["^"] = 3,
            ["ln"] = 3,
            ["sin"] = 3,
            ["cos"] = 3,
            ["sqrt"] = 3
        };
        private readonly Dictionary<string, Func<double, double, double>> _arithmeticOperations =
            new Dictionary<string, Func<double, double, double>>
            {
                ["+"] = (y, x) => x + y,
                ["-"] = (y, x) => x - y,
                ["*"] = (y, x) => x * y,
                [":"] = (y, x) => x / y,
                ["^"] = (y, x) => Math.Pow(x, y)
            };
        private readonly Dictionary<string, Func<double, double>> _mathsOperations =
            new Dictionary<string, Func<double, double>>
            {
                ["ln"] = Math.Log,
                ["sin"] = Math.Sin,
                ["cos"] = Math.Cos,
                ["sqrt"] = Math.Sqrt
            };
        private readonly List<string> _values = new List<string>
        {
            @"\d+",
            @"\w"
        };

        public string Run(Stream input, IStack<string> stack)
        {
            var tokens = GetPostfixRecord(input, stack, _operationPriority.Keys.Concat(_values));
            var result = ComputeExpression(stack, tokens);
            return result;
        }

        private List<string> GetPostfixRecord(Stream input,
                                              IStack<string> stack,
                                              IEnumerable<string> exprContentTokens)
        {
            var bytes = new byte[input.Length];
            input.Read(bytes);
            var program = GetCommands(bytes);
            var expr = program.FirstOrDefault();
            var tokens = Regex.Matches(expr, GetPattern(exprContentTokens))
                              .Select(match => match.Value)
                              .ToList();
            SubstitutionNumbers(program.Skip(1), tokens);
            var postfix = SortingYardAlgorithm(tokens, stack);

            return postfix;
        }

        private string ComputeExpression(IStack<string> stack, IEnumerable<string> tokens)
        {
            foreach (var token in tokens)
            {
                if (Regex.IsMatch(token, @"\d+"))
                {
                    stack.Push(token);
                }
                else if (_arithmeticOperations.ContainsKey(token))
                {
                    var value = _arithmeticOperations[token](StackPop(stack), StackPop(stack));
                    stack.Push(value.ToString(CultureInfo.InvariantCulture));
                }
                else if (_mathsOperations.ContainsKey(token))
                {
                    var value = _mathsOperations[token](StackPop(stack));
                    stack.Push(value.ToString(CultureInfo.InvariantCulture));
                }
                else
                {
                    throw new Exception($"Unexpected token: {token}");
                }
            }
            return stack.Pop();
        }

        private static double StackPop(IStack<string> stack)
        {
            return double.Parse(stack.Pop(), CultureInfo.InvariantCulture);
        }

        private List<string> SortingYardAlgorithm(IEnumerable<string> tokens,
                                                  IStack<string> stack)
        {
            var postfixList = new List<string>();
            foreach (var token in tokens)
            {
                if (Regex.IsMatch(token, @"\d+"))
                {
                    postfixList.Add(token);
                }
                if (_operationPriority.ContainsKey(token))
                {
                    while (!stack.IsEmpty() &&
                           _operationPriority.ContainsKey(stack.Top()) &&
                           _operationPriority[stack.Top()] >= _operationPriority[token])
                    {
                        postfixList.Add(stack.Pop());
                    }
                    stack.Push(token);
                }
            }
            while (!stack.IsEmpty())
            {
                postfixList.Add(stack.Pop());
            }
            return postfixList;
        }

        private void SubstitutionNumbers(IEnumerable<string> variables,
                                         List<string> tokens)
        {
            var dict = new Dictionary<string, int>();
            foreach (var variable in variables.Where(line => line.Length > 0))
            {
                var line = variable.Split('=')
                                   .Where(item => item.Length > 0)
                                   .ToList();
                if (line.Count == 2 && Regex.IsMatch(line[0], @"\w+") && Regex.IsMatch(line[1], @"\d+"))
                {
                    dict[line[0]] = int.Parse(line[1]);
                }
            }
            for (var i = 0; i < tokens.Count; i++)
            {
                if (dict.ContainsKey(tokens[i]))
                {
                    tokens[i] = dict[tokens[i]].ToString();
                }
            }
        }

        private string GetPattern(IEnumerable<string> tokens)
        {
            var pattern = new StringBuilder();
            foreach (var token in tokens)
            {
                pattern.Append(token.Length == 1
                                   ? '\\' + token + '|'
                                   : token + '|');
            }
            return pattern.ToString().TrimEnd('|');
        }

        private string[] GetCommands(byte[] bytes)
        {
            return Encoding.Default
                           .GetString(bytes)
                           .Split()
                           .Where(x => x.Length > 0)
                           .ToArray();
        }
    }
}
