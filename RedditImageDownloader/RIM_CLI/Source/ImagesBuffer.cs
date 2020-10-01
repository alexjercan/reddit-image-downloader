using System.Collections.Generic;
using System.Threading;

namespace RIM_CLI
{
    public class ImagesBuffer
    {
        private readonly Queue<Image> _images;

        private readonly Semaphore _empty;
        private readonly Mutex _mutex = new Mutex();

        public ImagesBuffer(int maxSize)
        {
            _empty = new Semaphore(0, maxSize);
            _images = new Queue<Image>(maxSize);
        }
        
        public void Add(Image image)
        {
            _mutex.WaitOne();
            _images.Enqueue(image);
            _mutex.ReleaseMutex();
            _empty.Release();
        }

        public Image Get()
        {
            _empty.WaitOne();
            _mutex.WaitOne();
            var result = _images.Dequeue();
            _mutex.ReleaseMutex();

            return result;
        }
    }
}