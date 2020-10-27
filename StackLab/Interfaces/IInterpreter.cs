using System.IO;

namespace StackLab.Interfaces
{
    public interface IInterpreter<T>
    {
        void Run(Stream input, Stream output, IStack<T> stack);
    }
}
