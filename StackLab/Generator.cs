using System;
using System.Collections.Generic;
using System.Text;
using StackLab.Interfaces;

namespace StackLab
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
                    stringBuilder.Append($"1,{GeneratePushParameters(rnd, rnd.Next(1, 2))} ");
                }
                else
                {
                    stringBuilder.Append($"{number} ");
                }
            }
            
            return stringBuilder.ToString();
        }

        private List<int> GenerateOperationNumbers(int operationsCount, Random rnd)
        {
            var operationsList = new List<int>();
            for (var i = 0; i < operationsCount; i++)
            {
                operationsList.Add(rnd.Next(1, 5));
            }
            return operationsList;
        }

        private string GeneratePushParameters(Random rnd, int numberParameter)
        {
            if (numberParameter == 1)
            {
                return GenerateString(rnd);
            }
            else
            {
                return GenerateNumber(rnd).ToString();
            }
        }

        private string GenerateString(Random rnd)
        {
            var stringBuilder = new StringBuilder();
            var length = rnd.Next(1, 10);
            for (var i = 0; i < length; i++)
            {
                stringBuilder.Append((char) rnd.Next(48, 126));
            }
            return stringBuilder.ToString();
        }

        private int GenerateNumber(Random rnd)
        {
            return rnd.Next(0, int.MaxValue);
        }
    }
}
