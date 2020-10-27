using System.Text;
using StackLab.Interfaces;

namespace StackLab
{
    public class Stack<T> : IStack<T>
    {
        private StackItem<T> _head;
        private StackItem<T> _tail;
        private int _count;

        public Stack(T elem)
        {
            _head = _tail = new StackItem<T> {Value = elem};
            _count = 1;
        }

        public Stack()
        {
            _count = 0;
        }

        public void Push(T item)
        {
            if (_count++ == 0)
            {
                _head = _tail = new StackItem<T> {Value = item};
            }
            else
            {
                var currentItem = new StackItem<T> {Value = item, Previous = _tail};
                _tail = currentItem;
            }
        }

        public T Pop()
        {
            if (_count == 0)
            {
                return default;
            }
            var result = _tail.Value;
            _tail = _tail.Previous;
            _count--;
            if (_count == 0)
            {
                _head = _tail = null;
            }
            return result;
        }

        public T Top()
        {
            return _tail.Value;
        }

        public bool IsEmpty()
        {
            return _count == 0;
        }

        public string Print()
        {
            if (_count == 0)
            {
                return string.Empty;
            }
            var str = new StringBuilder();
            var current = _tail;
            while (current != _head)
            {
                str.Append($"{current.Value.ToString()} ");
                current = current.Previous;
            }
            str.Append($"{current.Value.ToString()}");
            return str.ToString();
        }
    }

    public class StackItem<T>
    {
        public T Value { get; set; }
        public StackItem<T> Previous { get; set; }
    }
}
