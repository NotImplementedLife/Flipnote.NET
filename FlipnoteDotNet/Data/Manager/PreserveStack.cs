using System.Collections;
using System.Collections.Generic;

namespace FlipnoteDotNet.Data.Manager
{
    public class PreserveStack<T>
    {
        private readonly Stack<T> Stack = new Stack<T>();
        private readonly Stack<T> RemovedStack = new Stack<T>();


        public void Clear()
        {
            Stack.Clear();
            RemovedStack.Clear();
        }
        public void Push(T item)
        {
            RemovedStack.Clear();
            Stack.Push(item);
        }

        public T Pop()
        {
            var item = Stack.Pop();
            RemovedStack.Push(item);
            return item;
        }

        public T Peek() => Stack.Peek();

        public T UnPop()
        {
            var item = RemovedStack.Pop();
            Stack.Push(item);
            return item;
        }

        public T PeekRemoved() => RemovedStack.Peek();

        public bool CanPop => Stack.Count > 0;
        public bool CanUnPop => RemovedStack.Count > 0;
    }
}
