using System.IO;
using System.Text;

namespace StackLab
{
    public static class StreamExtension
    {
        public static void StreamWrite(this Stream stream, string value)
        {
            stream.Write(Encoding.Default.GetBytes($"{value}"));
        }

        public static void StreamWriteLine(this Stream stream, string value)
        {
            stream.Write(Encoding.Default.GetBytes($"{value}\r\n"));
        }
    }
}
