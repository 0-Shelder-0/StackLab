using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StackLab.Interfaces;

namespace StackLab.Generators
{
    public class Generator : IGenerator
    {
        public string Generate(int operationsCount)
        {
            var rnd = new Random();
            var operationsList = GenerateOperationNumbers(operationsCount, rnd);
            var stringBuilder = new StringBuilder();

            foreach (var number in operationsList)
            {
                if (number == 1)
                {
                    stringBuilder.Append($"1,{GeneratePushParameters(rnd, rnd.Next(1, 3))} ");
                }
                else
                {
                    stringBuilder.Append($"{number} ");
                }
            }

            return stringBuilder.ToString();
        }

        private IEnumerable<int> GenerateOperationNumbers(int operationsCount, Random rnd)
        {
            return Enumerable.Range(0, operationsCount)
                             .Select(value => rnd.Next(1, 5));
        }

        private string GeneratePushParameters(Random rnd, int numberParameter)
        {
            return numberParameter == 1
                       ? GenerateString(rnd)
                       : GenerateNumber(rnd).ToString();
        }

        private string GenerateString(Random rnd)
        {
            var stringBuilder = new StringBuilder();
            var length = rnd.Next(1, 10);
            for (var i = 0; i < length; i++)
            {
                stringBuilder.Append((char) rnd.Next(97, 123));
            }
            return stringBuilder.ToString();
        }

        private int GenerateNumber(Random rnd)
        {
            return rnd.Next(0, 1000000);
        }
    }
}
