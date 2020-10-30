using System.Collections.Generic;
using System.IO;
using StackLab.Interfaces;

namespace StackLab
{
    public static class GenerationResultWriter
    {
        public static IEnumerable<FilePath> WriteResults(FilePath resultFilePath,
                                                         IGenerator generator,
                                                         IEnumerable<int> itemCountList)
        {
            var result = new List<FilePath>();
            var numberFile = 0;
            foreach (var item in itemCountList)
            {
                numberFile++;
                var filePath = new FilePath(resultFilePath.DirectoryPath, $"{numberFile}_{resultFilePath.Name}");
                using (var stream = new FileStream(filePath.FullPath, FileMode.Create, FileAccess.Write))
                {
                    stream.StreamWrite(generator.Generate(item));
                }
                result.Add(filePath);
            }
            return result;
        }
    }
}
