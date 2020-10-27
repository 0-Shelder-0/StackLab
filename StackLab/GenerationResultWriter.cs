using System.Collections.Generic;
using System.IO;
using StackLab.Interfaces;

namespace StackLab
{
    public static class GenerationResultWriter
    {
        public static IEnumerable<string> WriteResults(string path,
                                                       IGenerator generator,
                                                       IEnumerable<int> itemCountList)
        {
            var numberFile = 0;
            foreach (var item in itemCountList)
            {
                numberFile++;
                using (var stream =
                    new FileStream(path + $"input_{numberFile}.txt", FileMode.Create, FileAccess.Write))
                {
                    stream.StreamWrite(generator.Generate(item));
                }
                yield return path + $"input_{numberFile}.txt";
            }
        }
    }
}
