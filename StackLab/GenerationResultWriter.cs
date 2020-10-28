using System.Collections.Generic;
using System.IO;
using StackLab.Interfaces;

namespace StackLab
{
    public static class GenerationResultWriter
    {
        public static IEnumerable<string> WriteResults(string path,
                                                       string filePrefix,
                                                       IGenerator generator,
                                                       IEnumerable<int> itemCountList)
        {
            var numberFile = 0;
            foreach (var item in itemCountList)
            {
                numberFile++;
                using (var stream =
                    new FileStream(path + $"{filePrefix}_{numberFile}.txt", FileMode.Create, FileAccess.Write))
                {
                    stream.StreamWrite(generator.Generate(item));
                }
                yield return path + $"{filePrefix}_{numberFile}.txt";
            }
        }
    }
}
