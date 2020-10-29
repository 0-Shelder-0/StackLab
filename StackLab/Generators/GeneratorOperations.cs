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
            var resultString = new StringBuilder();
            for (var i = 0; i < valuesList.Count; i++)
            {
                var levelFunc = rnd.Next(1, 5); // 1 - complex, 2-4 - simple
                if (levelFunc == 1)
                {
                    resultString.Append(i != valuesList.Count - 1
                                            ? $"{_funcDictionary[rnd.Next(6, 10)]}{valuesList[i]}{_funcDictionary[rnd.Next(1, 6)]}"
                                            : $"{_funcDictionary[rnd.Next(6, 10)]}{valuesList[i]}");
                }
                else
                {
                    resultString.Append(i != valuesList.Count - 1
                                            ? $"{valuesList[i]}{_funcDictionary[rnd.Next(1, 6)]}"
                                            : $"{valuesList[i]}");
                }
            }

            return resultString.ToString();
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
