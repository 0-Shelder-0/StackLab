using System.IO;

namespace StackLab.Interfaces
{
    public interface IInterpreter<T>
    {
        string Run(Stream input, IStack<T> stack);
    }
}
