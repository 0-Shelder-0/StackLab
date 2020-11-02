using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StackLab.Interfaces;

namespace StackLab.Generators
{
    public class GeneratorOperations : IGenerator
    {
        private readonly Dictionary<int, string> _funcDictionary = new Dictionary<int, string>
        {
            [1] = "+",
            [2] = "-",
            [3] = "*",
            [4] = ":",
            [5] = "^",
            [6] = "ln",
            [7] = "cos",
            [8] = "sin",
            [9] = "sqrt",
        };

        public string Generate(int operationsCount)
        {
            var rnd = new Random();
            var valuesList = GenerateValues(rnd, operationsCount);
            var firstString = GenerateOperationsString(rnd, valuesList);
            var variablesValues = GenerateVariablesValues(rnd, valuesList);
            var resultString = new StringBuilder();

            resultString.Append($"{firstString}\n");

            foreach (var value in variablesValues)
            {
                resultString.Append($"{value}\n");
            }

            return resultString.ToString();
        }

        private List<string> GenerateValues(Random rnd, int operationCount)
        {
            var valuesList = new List<string>();

            for (var i = 0; i < operationCount; i++)
            {
                var numberValue = rnd.Next(1, 3);
                valuesList.Add(numberValue == 1
                                   ? ((char) rnd.Next(97, 123)).ToString()
                                   : rnd.Next(1, 100).ToString());
            }

            return valuesList;
        }

        private string GenerateOperationsString(Random rnd, List<string> valuesList)
        {
            var strBuilder = new StringBuilder();
            for (var i = 0; i < valuesList.Count; i++)
            {
                var levelFunc = rnd.Next(1, 5); // 1 - complex, 2-4 - simple

                var mathOperation = _funcDictionary[rnd.Next(6, 10)];
                var arOperation = _funcDictionary[rnd.Next(1, 6)];
                if (levelFunc == 1)
                {
                    strBuilder.Append(i != valuesList.Count - 1
                                          ? $"{mathOperation}({valuesList[i]}){arOperation}"
                                          : $"{mathOperation}({valuesList[i]})");
                }
                else
                {
                    strBuilder.Append(i != valuesList.Count - 1
                                          ? $"{valuesList[i]}{arOperation}"
                                          : $"{valuesList[i]}");
                }
            }
            var resultString = AddBrackets(strBuilder.ToString(), 0, 1);
            resultString = AddBrackets(resultString, resultString.Length - 1, -1);
            return resultString;
        }

        private string AddBrackets(string line, int start, int step)
        {
            if (step == 0)
            {
                return line;
            }
            var stack = new Stack<char>();
            var firstB = step > 0 ? '(' : ')';
            var secondB = step > 0 ? ')' : '(';
            for (var i = start; i < line.Length && i >= 0; i += step)
            {
                if (line[i].Equals(firstB))
                {
                    stack.Push(line[i]);
                }
                else if (line[i].Equals(secondB))
                {
                    stack.Pop();
                }
            }
            var count = 0;
            while (!stack.IsEmpty())
            {
                count++;
                stack.Pop();
            }
            return step > 0
                       ? line.PadRight(line.Length + count, secondB)
                       : line.PadLeft(line.Length + count, secondB);
        }

        private List<string> GenerateVariablesValues(Random rnd, List<string> valuesList)
        {
            var variables = new HashSet<string>();
            var stringBuilder = new StringBuilder();
            var resultList = new List<string>();

            foreach (var value in valuesList.Where(value => !int.TryParse(value, out _)))
            {
                variables.Add(value);
            }

            foreach (var variable in variables)
            {
                stringBuilder.Append($"{variable}={rnd.Next(1, 100)}");
                resultList.Add(stringBuilder.ToString());
                stringBuilder.Clear();
            }

            return resultList;
        }
    }
}
