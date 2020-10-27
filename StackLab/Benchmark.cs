using System.Collections.Generic;
using System.IO;

namespace StackLab
{
    public class Benchmark
    {
        public static void Run(IEnumerable<string> paths)
        {
            var measurer = new Measurer();
            foreach (var path in paths)
            {
                using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    measurer.Measure();
                }
            }
        }
    }
}
