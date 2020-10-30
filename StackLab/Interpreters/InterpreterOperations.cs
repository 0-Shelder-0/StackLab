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
            ["ln"] = 4,
            ["sin"] = 4,
            ["cos"] = 4,
            ["sqrt"] = 4
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
        private readonly List<string> _exprContent = new List<string>
        {
            "(",
            ")",
            @"\d+",
            @"[a-zA-Z]+"
        };

        public string Run(Stream input, IStack<string> stack)
        {
            try
            {
                var tokens = GetPostfixRecord(input, stack, _operationPriority.Keys.Concat(_exprContent));
                var result = ComputeExpression(stack, tokens);
                return result;
            }
            catch (Exception exception)
            {
                return exception.Message;
            }
        }

        private IEnumerable<string> GetPostfixRecord(Stream input,
                                                     IStack<string> stack,
                                                     IEnumerable<string> exprContentTokens)
        {
            var bytes = new byte[input.Length];
            input.Read(bytes);
            var program = GetCommands(bytes);
            var expression = program.FirstOrDefault();
            var tokens = Regex.Matches(expression, GetPattern(exprContentTokens))
                              .Select(match => match.Value);
            var record = SubstituteVariables(program.Skip(1), tokens);
            var postfixRecord = SortingYardAlgorithm(record, stack);
            return postfixRecord;
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
                    ArithmeticOperationActions(stack, token);
                }
                else if (_mathsOperations.ContainsKey(token))
                {
                    MathsOperationActions(stack, token);
                }
                else
                {
                    throw new Exception($"Unexpected token: {token}");
                }
            }
            return stack.Pop();
        }

        private void ArithmeticOperationActions(IStack<string> stack, string token)
        {
            var firstArg = StackPop(stack);
            var secondArg = StackPop(stack);
            var value = _arithmeticOperations[token](firstArg, secondArg);
            stack.Push(ToString(value));
            IsCorrectResult(new[] {firstArg, secondArg}, value, token);
        }

        private void MathsOperationActions(IStack<string> stack, string token)
        {
            var arg = StackPop(stack);
            var value = _mathsOperations[token](arg);
            stack.Push(ToString(value));
            IsCorrectResult(new[] {arg}, value, token);
        }

        private void IsCorrectResult(double[] args, double value, string token)
        {
            if (double.IsNaN(value))
            {
                switch (args.Length)
                {
                    case 1:
                        throw new Exception($"Unexpected operation: {token}{ToString(args[0])}");
                    case 2:
                        throw new Exception($"Unexpected operation: {ToString(args[1])}{token}{ToString(args[0])}");
                }
            }
        }

        private IEnumerable<string> SortingYardAlgorithm(IEnumerable<string> tokens, IStack<string> stack)
        {
            var postfixRecord = new List<string>();
            foreach (var token in tokens)
            {
                if (Regex.IsMatch(token, @"\d+"))
                {
                    postfixRecord.Add(token);
                }
                else if (token.Equals("("))
                {
                    stack.Push(token);
                }
                else if (token.Equals(")"))
                {
                    ClosingBracketActions(stack, postfixRecord);
                }
                else if (_operationPriority.ContainsKey(token))
                {
                    OperatorActions(stack, token, postfixRecord);
                }
                else
                {
                    throw new Exception($"Unexpected token: {token}");
                }
            }
            while (!stack.IsEmpty())
            {
                postfixRecord.Add(stack.Pop());
            }
            return postfixRecord;
        }

        private void ClosingBracketActions(IStack<string> stack, List<string> postfixRecord)
        {
            var top = stack.Pop();
            while (!stack.IsEmpty() && !top.Equals("("))
            {
                postfixRecord.Add(top);
                top = stack.Pop();
            }
            if (!top.Equals("("))
            {
                throw new Exception("Missing closing bracket");
            }
        }

        private void OperatorActions(IStack<string> stack, string token, List<string> postfixRecord)
        {
            while (!stack.IsEmpty() &&
                   !stack.Top().Equals("(") &&
                   _operationPriority.ContainsKey(stack.Top()) &&
                   _operationPriority[stack.Top()] >= _operationPriority[token])
            {
                postfixRecord.Add(stack.Pop());
            }
            stack.Push(token);
        }

        private IEnumerable<string> SubstituteVariables(IEnumerable<string> variables, IEnumerable<string> tokens)
        {
            var dict = new Dictionary<string, double>();
            foreach (var variable in variables.Where(line => line.Length > 0))
            {
                var line = variable.Split('=', ' ', '\t')
                                   .Where(item => item.Length > 0)
                                   .ToList();
                if (line.Count == 2 &&
                    Regex.IsMatch(line[0], @"^[a-zA-Z]+$") &&
                    Regex.IsMatch(line[1], @"^\d+$"))
                {
                    dict[line[0]] = double.Parse(line[1]);
                }
            }
            foreach (var token in tokens)
            {
                if (dict.ContainsKey(token))
                {
                    yield return ToString(dict[token]);
                }
                else
                {
                    yield return token;
                }
            }
        }

        private string GetPattern(IEnumerable<string> tokens)
        {
            var pattern = new StringBuilder();
            foreach (var token in tokens)
            {
                pattern.Append(token.Length == 1
                                   ? $"\\{token}|"
                                   : $"{token}|");
            }
            return pattern.ToString().TrimEnd('|');
        }

        private static string ToString(double value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        private static double StackPop(IStack<string> stack)
        {
            return double.Parse(stack.Pop(), CultureInfo.InvariantCulture);
        }

        private string[] GetCommands(byte[] bytes)
        {
            return Encoding.Default
                           .GetString(bytes)
                           .Split('\r', '\n')
                           .Where(line => line.Length > 0)
                           .ToArray();
        }
    }
}
