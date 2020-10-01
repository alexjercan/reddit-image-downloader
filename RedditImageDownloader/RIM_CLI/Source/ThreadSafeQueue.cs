using System.Collections.Generic;
using System.Threading;

namespace RIM_CLI
{
    public class ThreadSafeQueue<T>
    {
        private readonly Queue<T> _elements;

        private readonly Semaphore _empty;
        private readonly Mutex _mutex = new Mutex();

        public ThreadSafeQueue(int maxSize)
        {
            _empty = new Semaphore(0, maxSize);
            _elements = new Queue<T>(maxSize);
        }
        
        public void Add(T element)
        {
            _mutex.WaitOne();
            _elements.Enqueue(element);
            _mutex.ReleaseMutex();
            _empty.Release();
        }

        public T Get()
        {
            _empty.WaitOne();
            _mutex.WaitOne();
            var result = _elements.Dequeue();
            _mutex.ReleaseMutex();

            return result;
        }
    }
}