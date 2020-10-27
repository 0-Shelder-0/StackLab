namespace StackLab.Interfaces
{
    public interface IStack<T>
    {
        void Push(T value);
        T Pop();
        T Top();
        bool IsEmpty();
        string Print();
    }
}
